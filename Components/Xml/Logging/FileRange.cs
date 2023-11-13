namespace Components.Xml.Logging;

internal struct FileRange
{
    public FileRange (int startLine, int startCol, int endLine, int endCol)
    {
        StartLine = startLine;
        StartCol = startCol;
        EndLine = endLine;
        EndCol = endCol;
    }

    public int StartLine { get; }
    public int StartCol { get; }
    public int EndLine { get; }
    public int EndCol { get; }
}
