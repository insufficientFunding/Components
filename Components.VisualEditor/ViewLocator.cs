using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Components.VisualEditor.ViewModels;
using System;
namespace Components.VisualEditor;

public class ViewLocator : IDataTemplate
{
    public Control Build (object? data)
    {
        if (data is null)
            return new TextBlock { Text = "No data: " + data };
        
        var name = data.GetType ().FullName!.Replace ("ViewModel", "View");
        var type = Type.GetType (name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance (type)!;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match (object? data)
    {
        return data is ViewModelBase;
    }
}
