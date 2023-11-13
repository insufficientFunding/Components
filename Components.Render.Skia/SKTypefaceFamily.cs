using Components.Enums;
using SkiaSharp;

namespace Components.Render.Skia;

public struct SKTypefaceFamily
{
    public SKTypeface? ExtraLight { get; set; }
    public SKTypeface? Light { get; set; }
    public SKTypeface? Regular { get; set; }
    public SKTypeface? SemiBold { get; set; }
    public SKTypeface? Bold { get; set; }
}

public static class SKTypefaceFamilyExtensions
{
    public static SKTypeface? GetSKTypeface (this SKTypefaceFamily? family, FontWeight weight)
    {
        if (family is null)
            return null;

        string weightName = weight.ToString ();
        string [] fontWeights = Enum.GetNames (typeof (FontWeight));

        foreach (string familyWeight in fontWeights)
        {
            if (familyWeight == weightName)
                return (SKTypeface?)family.GetType ().GetProperty (familyWeight)!.GetValue (family);
        }

        return null;
    }
}
