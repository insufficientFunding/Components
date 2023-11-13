using Components.Enums;
namespace Components.Render.Drawing.Extensions;

public static class TextAlignmentExtensions
{
    /// <summary>
    ///     Flips the given <see cref="alignment"/> if required.
    /// </summary>
    /// <param name="alignment">The alignment to flip.</param>
    /// <param name="flipType">The flip type.</param>
    /// <returns>A flipped <see cref="TextAlignment"/>.</returns>
    public static TextAlignment Flip (this TextAlignment alignment, FlipType flipType)
    {
        TextAlignment tempAlignment = alignment;

        if ((flipType & FlipType.Horizontal) == FlipType.Horizontal)
        {
            switch (alignment)
            {
                case TextAlignment.BottomLeft:
                    tempAlignment = TextAlignment.BottomRight;
                    break;
                case TextAlignment.BottomRight:
                    tempAlignment = TextAlignment.BottomLeft;
                    break;
                case TextAlignment.CenterLeft:
                    tempAlignment = TextAlignment.CenterRight;
                    break;
                case TextAlignment.CenterRight:
                    tempAlignment = TextAlignment.CenterLeft;
                    break;
                case TextAlignment.TopLeft:
                    tempAlignment = TextAlignment.TopRight;
                    break;
                case TextAlignment.TopRight:
                    tempAlignment = TextAlignment.TopLeft;
                    break;
            }
        }
        if ((flipType & FlipType.Vertical) == FlipType.Vertical)
        {
            switch (alignment)
            {
                case TextAlignment.BottomCenter:
                    tempAlignment = TextAlignment.TopCenter;
                    break;
                case TextAlignment.BottomLeft:
                    tempAlignment = TextAlignment.TopLeft;
                    break;
                case TextAlignment.BottomRight:
                    tempAlignment = TextAlignment.TopRight;
                    break;
                case TextAlignment.TopCenter:
                    tempAlignment = TextAlignment.BottomCenter;
                    break;
                case TextAlignment.TopLeft:
                    tempAlignment = TextAlignment.BottomLeft;
                    break;
                case TextAlignment.TopRight:
                    tempAlignment = TextAlignment.BottomRight;
                    break;
            }
        }

        return tempAlignment;
    }
}
