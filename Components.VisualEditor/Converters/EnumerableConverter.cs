using Avalonia.Data.Converters;
using Components.VisualEditor.Parsers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
namespace Components.VisualEditor.Converters;

public class EnumerableConverter : IValueConverter
{

    public object? Convert (object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not IEnumerable<object> enumerable)
            return value;

        if (parameter is not string parameterString)
            return value;

        string [] parameters = parameterString.Split (":", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        if (parameters.Length == 0)
            return value;
        
        var first = parameters.ElementAtOrDefault (1);

        switch (parameters [0].ToLowerInvariant ())
        {
            case "skip":
                var skip = first.ParseInt (0);
                
                return enumerable.Skip (skip);
            
            case "at":
                var at = first.ParseInt (0);
                
                return enumerable.ElementAtOrDefault (at);
            
            default:
                return value;
        }
    }
    public object? ConvertBack (object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}
