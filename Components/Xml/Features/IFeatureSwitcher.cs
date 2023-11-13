namespace Components.Xml.Features;

internal interface IFeatureSwitcher
{
    /// <summary>
    ///     Checks if a feature is enabled.
    /// </summary>
    /// <param name="featureKey">The key of the feature to check.</param>
    /// <returns>True if the feature is enabled, false otherwise.</returns>
    bool IsFeatureEnabled (string featureKey);
}
