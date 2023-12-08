namespace Serialization.Writer;

public sealed partial class StringCreator
{
    internal const string ElementTemplate = """
                                            {name}: {
                                            """;

    internal const string AttributeTemplate = """
                                              {name}: {content}
                                              """;

    public string GetElement (string name) => ElementTemplate
        .Replace ("{name}", name);

    public StringCreator WriteElement (string name)
    {
        _builder.AppendLine (GetElement (name));
        return this;
    }

    public StringCreator WriteEndElement ()
    {
        _builder.AppendLine ("}");
        return this;
    }
    
    public string GetAttribute (string name, string content) => AttributeTemplate
        .Replace ("{name}", name)
        .Replace ("{content}", content);

    public StringCreator WriteAttribute (string name, string content)
    {
        _builder.AppendLine (GetAttribute(name, content));
        return this;
    }

    public StringCreator WriteNewLine ()
    {
        _builder.Append (Environment.NewLine);
        return this;
    }
}
