﻿namespace Components.IO.Xml.Flatten;

internal interface IRootFlattenable<T> : IAutoRotateRoot
{
    IEnumerable<T> Flatten (FlattenContext context);
}
