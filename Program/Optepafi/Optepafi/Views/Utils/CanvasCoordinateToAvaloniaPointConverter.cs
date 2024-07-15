using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Optepafi.ViewModels.Data;

namespace Optepafi.Views.Utils;

/// <summary>
/// Converter of <c>CanvasCoordinate</c> to Avalonia <c>Point</c>.
///
/// For converting canvas coordinates to Avalonia point we must at first convert values of coordinates from metric system to value of device independent pixels (dip). For this operation we use converting class <see cref="MicrometersToDipConverter"/>.
/// </summary>
public class CanvasCoordinateToAvaloniaPointConverter : IValueConverter
{
    /// <inheritdoc cref="IValueConverter.Convert"/>
    /// <remarks>
    /// Converts <c>CanvasCoordinate</c> to Avalonia <c>Point</c>. 
    /// </remarks>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is CanvasCoordinate mapCoordinate)
        {
            MicrometersToDipConverter microToDipConverter = new MicrometersToDipConverter();
            return new Point((double)microToDipConverter.Convert(mapCoordinate.LeftPos), (double)microToDipConverter.Convert(-mapCoordinate.BottomPos));
        }
        return new BindingNotification(new InvalidOperationException("The value must be a map coordinate of type MapCoordinate."));
    }

    /// <inheritdoc cref="IValueConverter.ConvertBack"/>
    /// <remarks>
    /// Converts Avalonia <c>Point</c> back to <c>CanvasCoordinate</c>.
    /// </remarks>
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