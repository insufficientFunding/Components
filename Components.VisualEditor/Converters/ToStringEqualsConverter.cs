using Avalonia.Data.Converters;
using System;
using System.Globalization;
namespace Components.VisualEditor.Converters;

public class ToStringEqualsConverter : IValueConverter
{

    public object? Convert (object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value?.ToString () == parameter?.ToString ())
            return true;
        
        return false;
    }
    public object? ConvertBack (object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException ();
    }
}
