using System.Xml.Linq;
namespace Components.Xml.Features;

internal class FeatureSwitcher : IFeatureSwitcher
{
    private readonly Dictionary<string, XElement> enabledFeatures = new Dictionary<string, XElement> ();

    public void EnableFeatureCandidate (string key, XElement element)
    {
        enabledFeatures.Add (key, element);
    }

    public bool IsFeatureEnabled (string key)
    {
        return enabledFeatures.ContainsKey (key);
    }

    public bool IsFeatureEnabled (string key, out XElement? source)
    {
        return enabledFeatures.TryGetValue (key, out source);
    }
}
