﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Avalonia.Rendering;
using Avalonia.VisualTree;
namespace Components.VisualEditor.Services;

internal static class StorageService
{
    public static FilePickerFileType Json { get; } = new FilePickerFileType ("Json")
    {
        Patterns = new []
        {
            "*.json",
        },
        
        AppleUniformTypeIdentifiers = new []
        {
            "public.json",
        },
        
        MimeTypes = new []
        {
            "application/json",
        },
    };

    public static IStorageProvider? GetStorageProvider ()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: { } window })
        {
            return window.StorageProvider;
        }

        if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime { MainView: { } mainView })
        {
            IRenderRoot? visualRoot = mainView.GetVisualRoot ();
            if (visualRoot is TopLevel topLevel)
            {
                return topLevel.StorageProvider;
            }
        }

        return null;
    }
}
