using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Components.VisualEditor.Logging;
using Components.VisualEditor.Services;
using Components.VisualEditor.ViewModels;
using Components.VisualEditor.Views;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Components.VisualEditor;

public partial class App : Application
{
    private IServiceProvider? _serviceProvider;
    
    public override void Initialize ()
    {
        AvaloniaXamlLoader.Load (this);
    }

    public override void OnFrameworkInitializationCompleted ()
    {
        BuildServiceProvider ();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = _serviceProvider?.GetRequiredService<MainWindowViewModel> (),
            };
        }

        base.OnFrameworkInitializationCompleted ();
    }

    private void BuildServiceProvider ()
    {
        IServiceCollection serviceCollection = new ServiceCollection ();
        
        serviceCollection.AddSingleton<MainWindowViewModel> ();

        serviceCollection.AddSingleton<IFileService, FileService> ();

        serviceCollection.AddLogging (x => x.SetupLogging (true, false));
        
        _serviceProvider = serviceCollection.BuildServiceProvider ();
    }
}
