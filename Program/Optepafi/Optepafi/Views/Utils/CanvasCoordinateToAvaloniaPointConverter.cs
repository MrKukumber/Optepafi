using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Optepafi.Models.MapMan;
using Optepafi.ViewModels.DataViewModels;

namespace Optepafi.Views.Utils;

public class CanvasCoordinateToAvaloniaPointConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is CanvasCoordinate mapCoordinate)
        {
            MicrometersToDipConverter microToDipConverter = new MicrometersToDipConverter();
            return new Point((double)microToDipConverter.Convert(mapCoordinate.LeftPos), (double)microToDipConverter.Convert(-mapCoordinate.BottomPos));
        }
        return new BindingNotification(new InvalidOperationException("The value must be a map coordinate of type MapCoordinate."));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Point point)
        {
            MicrometersToDipConverter microToDipConverter = new MicrometersToDipConverter();
            return new CanvasCoordinate((int)microToDipConverter.ConvertBack(point.X), (int)microToDipConverter.ConvertBack(-point.Y));
        }
        return new BindingNotification(new InvalidOperationException("The value must be a point of type Avalonia.Point."));
    }
}