using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Optepafi.Models.MapMan;

namespace Optepafi.Views.Utils;

public class MapCoordinateToAvaloniaPointConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is MapCoordinate mapCoordinate)
        {
            MicrometersToDipConverter microToDipConverter = new MicrometersToDipConverter();
            return new Point((double)microToDipConverter.Convert(mapCoordinate.XPos), (double)microToDipConverter.Convert(-mapCoordinate.YPos));
        }
        return new BindingNotification(new InvalidOperationException("The value must be a map coordinate of type MapCoordinate."));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Point point)
        {
            MicrometersToDipConverter microToDipConverter = new MicrometersToDipConverter();
            return new MapCoordinate((int)microToDipConverter.ConvertBack(point.X), (int)microToDipConverter.ConvertBack(-point.Y));
        }
        return new BindingNotification(new InvalidOperationException("The value must be a point of type Avalonia.Point."));
    }
}