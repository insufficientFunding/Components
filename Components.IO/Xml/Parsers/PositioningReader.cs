namespace Components.IO.Xml.Parsers;

public class PositioningReader : TextReader
{
    private StringReader internalReader;
    private int pos = 0;

    public int CharPos
    {
        get
        {
            return pos;
        }
    }

    public PositioningReader (StringReader inner)
    {
        internalReader = inner;
    }

    public override int Peek ()
    {
        return internalReader.Peek ();
    }

    public override int Read ()
    {
        int c = internalReader.Read ();

        if (c >= 0)
            AdvancePosition ((char)c);

        return c;
    }

    private void AdvancePosition (char c)
    {
        pos++;
    }
}
