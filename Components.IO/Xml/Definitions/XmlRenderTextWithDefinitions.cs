﻿using Components.Base.Enums;
using Components.IO.Xml.Flatten;
using Components.IO.Xml.Render;
using Components.Render.Drawing.RenderCommands;
using Components.Render.Text;
using Components.Render.TypeDescription;
using Components.Render.TypeDescription.Conditions;
namespace Components.IO.Xml.Definitions;

internal class XmlRenderTextWithDefinitions : XmlRenderText
{
    public new ConditionalCollection<TextAlignment> Alignment { get; internal protected set; } = null!;

    public new ConditionalCollection<FontWeight> Weight { get; internal protected set; } = null!;

    public new ConditionalCollection<TextRotation> Rotation { get; internal protected set; } = null!;

    public override IEnumerable<Conditional<IRenderCommand>> Flatten (FlattenContext context)
    {
        foreach (Conditional<ComponentPoint>? location in Position.Flatten (context))
        {
            foreach (Conditional<TextAlignment>? alignment in Alignment)
            {
                foreach (Conditional<FontWeight>? weight in Weight)
                {
                    foreach (Conditional<TextRotation>? rotation in Rotation)
                    {
                        RenderText? command = new RenderText (
                            location.Value,
                            alignment.Value,
                            weight.Value,
                            rotation.Value,
                            TextRuns);

                        IConditionTreeItem? conditions = ConditionTreeBuilder.And (new [] { location.Conditions, alignment.Conditions, rotation.Conditions });

                        yield return new Conditional<IRenderCommand> (command, conditions);
                    }
                }
            }
        }
    }
}
