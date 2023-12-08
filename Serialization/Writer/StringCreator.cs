using System.Text;
namespace Serialization.Writer;

public sealed partial class StringCreator
{
    private readonly StringBuilder _builder;

    public StringCreator ()
    {
        _builder = new StringBuilder ();
    }

    public StringCreator Append (string value)
    {
        _builder.Append (value);

        return this;
    }

    public StringCreator Replace (string oldValue, string newValue)
    {
        _builder.Replace (oldValue, newValue);

        return this;
    }
    public StringBuilder AppendIndented (string value)
    {
        foreach (var line in value.TrimEnd ().Split ('\n'))
            if (!string.IsNullOrWhiteSpace (line))
                _builder.AppendLine ($"\t{line}");
        
        return _builder;
    }

    public override string ToString ()
    {
        return _builder.ToString ();
    }
}
