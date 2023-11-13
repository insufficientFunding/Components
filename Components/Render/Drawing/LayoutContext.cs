using Components.Interfaces.Render;
namespace Components.Render.Drawing;

public class LayoutContext : ILayoutContext
{
    private readonly Func<string, string?> formattedVariable;

    public LayoutContext (Func<string, string?> formattedVariable)
    {
        this.formattedVariable = formattedVariable;
    }

    public LayoutContext ()
    {
        formattedVariable = (s) => s;
    }

    public string? GetFormattedVariable (string variableName)
    {
        return formattedVariable (variableName);
    }
}
