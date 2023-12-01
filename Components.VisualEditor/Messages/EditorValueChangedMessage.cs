using CommunityToolkit.Mvvm.Messaging.Messages;
namespace Components.VisualEditor.Messages;

public class EditorValueChangedMessage : ValueChangedMessage<string>
{
    public EditorValueChangedMessage (string propertyName) : base (propertyName)
    { }
}
