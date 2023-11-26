using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Components.Avalonia.IntegrationTest.ViewModels;
using Components.Avalonia.IntegrationTest.Views;

namespace Components.Avalonia.IntegrationTest;

public partial class App : Application
{
    public override void Initialize ()
    {
        AvaloniaXamlLoader.Load (this);
    }

    public override void OnFrameworkInitializationCompleted ()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel (),
            };
        }

        base.OnFrameworkInitializationCompleted ();
    }
}
