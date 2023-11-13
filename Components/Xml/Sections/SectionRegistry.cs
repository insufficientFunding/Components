namespace Components.Xml.Sections;

internal class SectionRegistry : ISectionRegistry
{
    private readonly Dictionary<Type, object> _sections = new Dictionary<Type, object> ();

    public void RegisterSection<T> (T section)
    {
        if (section != null)
            _sections [typeof (T)] = section;
    }

    public T GetSection<T> ()
    {
        if (_sections.TryGetValue (typeof (T), out object? sectionValue))
            return (T)sectionValue;

        throw new ArgumentException ($"No section of type {typeof (T)} was registered.");
    }
}
