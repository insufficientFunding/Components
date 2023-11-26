using Components.Interfaces;
using Components.Interfaces.Render;
using Components.Interfaces.TypeDescription;
using Components.Render.Drawing;
using Components.Render.Drawing.DrawingContext;
using Components.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Components.IntegrationTest;

public static class TestHelper
{
    public static string GetSolutionPath ()
    {
        string? projectPath = Path.GetDirectoryName (Assembly.GetExecutingAssembly ().Location);
        if (projectPath is null)
            throw new Exception ("Could not get project parent.");

        DirectoryInfo? parent = Directory.GetParent (projectPath!);

        return parent!.Parent?.Parent?.Parent?.FullName!;
    }

    #region Svg
    public static void RenderAllToSvg (HashSet<IComponentDescription> componentDescriptions, string path)
    {
        IComponentService componentService = Program.Services.GetRequiredService<IComponentService> ();

        foreach (IComponentDescription description in componentDescriptions)
        {
            IPositionalComponent? component = componentService.CreateComponent (description.Metadata.Name);
            if (component is null)
                throw new Exception ($"Failed to create component: {description.Metadata.Name}.");

            string targetPath = $"{path}/{description.Metadata.Group}";

            Directory.CreateDirectory (targetPath);

            RenderToSvg (component, description, targetPath);
        }
    }

    public static void RenderToSvg (IPositionalComponent component, IComponentDescription description, string path)
    {
        using FileStream? fileStream = File.Open (path + $"/{component.Name}.svg", FileMode.OpenOrCreate, FileAccess.ReadWrite);

        using SvgDrawingContext context = new SvgDrawingContext (description.Metadata.Size, description.Metadata.Size, fileStream);

        ILayoutContext layoutContext = new LayoutContext ();

        foreach (IRenderDescription renderDescription in description.RenderDescriptions)
        {
            if (renderDescription.Conditions.IsMet (component, description))
                renderDescription.Render (context, layoutContext, component.Layout);
        }

    }
    #endregion

    #region Serialize
    public static void SerializeAllToJson (HashSet<IComponentDescription> componentDescriptions, string path)
    {
        IComponentService componentService = Program.Services.GetRequiredService<IComponentService> ();

        foreach (IComponentDescription description in componentDescriptions)
        {
            IPositionalComponent? component = componentService.CreateComponent (description.Metadata.Name);
            if (component is null)
                throw new Exception ($"Failed to create component: {description.Metadata.Name}.");

            string targetPath = $"{path}/{description.Metadata.Group}";

            Directory.CreateDirectory (targetPath);

            SerializeToJson (component, description, targetPath);
        }
    }

    public static void SerializeToJson (IPositionalComponent component, IComponentDescription description, string path)
    {
        using FileStream? fileStream = File.Open (path + $"/{component.Name}.json", FileMode.Create, FileAccess.ReadWrite);

        JsonSerializer.Serialize (fileStream, description, new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter () },
            UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement,
            PreferredObjectCreationHandling = JsonObjectCreationHandling.Populate,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
/*
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            NullValueHandling = NullValueHandling.Include,
            Converters = { new StringEnumConverter(), },
        };

        string converted = JsonConvert.SerializeObject (description, settings);

        using TextWriter textWriter = new StreamWriter (fileStream);

        textWriter.Write (converted);

        textWriter.Close();
*/

    }
    #endregion
}
