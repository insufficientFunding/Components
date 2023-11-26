using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Components.VisualEditor.ViewModels;
using Components.VisualEditor.Views;

namespace Components.VisualEditor;

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
