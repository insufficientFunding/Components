using Avalonia.Platform.Storage;
using Components.VisualEditor.Logging;
using Components.VisualEditor.Models;
using Components.VisualEditor.Serialization.Converters;
using Components.VisualEditor.Serialization.Converters.Path;
using Components.VisualEditor.Serialization.Converters.Properties;
using Components.VisualEditor.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonSerializer = System.Text.Json.JsonSerializer;
namespace Components.VisualEditor.Services;

public class FileService : IFileService
{
    private readonly ILogger<FileService> _logger;
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        WriteIndented = true,
        AllowTrailingCommas = true,
        PreferredObjectCreationHandling = JsonObjectCreationHandling.Populate,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
        IgnoreReadOnlyProperties = true,
        MaxDepth = 1000,
        Converters =
        {
            new MetadataConverter (),
            new EditorRenderCommandConverter (),
            new RenderGroupConverter (),
            new RenderCommandConverter (),
            new RenderPathConverter (),
            new ComponentPropertyConverter(),
            new EditorConverter(),
        }
    };

    public FileService (ILogger<FileService> logger)
    {
        _logger = logger;
    }

    public async Task SaveComponentAsync (IEditor? editor)
    {
        if (editor is null)
            return;

        IStorageProvider? storageProvider = StorageService.GetStorageProvider ();
        if (storageProvider is null)
            return;

        IStorageFile? file = await storageProvider.SaveFilePickerAsync (new FilePickerSaveOptions
        {
            Title = "Save Component",
            FileTypeChoices = [StorageService.Json],
            SuggestedFileName = Path.GetFileNameWithoutExtension (editor.Metadata.Name),
            DefaultExtension = "json",
            ShowOverwritePrompt = true,
        });

        if (file is null)
            return;

        try
        {
            string json = JsonSerializer.Serialize (editor, _jsonSerializerOptions);
            await using Stream stream = await file.OpenWriteAsync ();
            await using StreamWriter writer = new StreamWriter (stream, Encoding.UTF8);
            await writer.WriteAsync (json);
        }
        catch (Exception ex)
        {
            _logger.LogError (ex.Message);
            throw;
        }

    }

    public async Task<IEditor?> OpenComponentAsync ()
    {
        IStorageProvider? storageProvider = StorageService.GetStorageProvider ();
        if (storageProvider is null)
            return _logger.LogInformationReturnDefault<IEditor?> ("No storage provider found");

        IReadOnlyList<IStorageFile> result = await storageProvider.OpenFilePickerAsync (new FilePickerOpenOptions
        {
            Title = "Open Component",
            FileTypeFilter = [StorageService.Json],
            AllowMultiple = false,
        });

        IStorageFile? file = result.FirstOrDefault ();
        if (file is null)
            return _logger.LogInformationReturnDefault<IEditor?> ("No file selected");

        try
        {
            await using Stream stream = await file.OpenReadAsync ();
            using StreamReader reader = new StreamReader (stream, Encoding.UTF8);

            string json = await reader.ReadToEndAsync ();

            IEditor? editor = JsonSerializer.Deserialize<EditorViewModel> (json, _jsonSerializerOptions);

            return editor;
        }
        catch (Exception ex)
        {
            _logger.LogError (ex.Message);
            throw;
        }
    }
}
