namespace Components.Xml.Sections;

internal interface ISectionRegistry
{
    void RegisterSection<T> (T section);

    T GetSection<T> ();
}
