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
/// For converting canvas coordinates to Avalonia point we must at first convert values of coordinates from metric system to value of device independent pixels (dip).
/// For this operation we use converting class <see cref="MicrometersToDipConverter"/>.
/// </summary>
public class CanvasCoordinateToAvaloniaPointConverter : IValueConverter
{
    private static MicrometersToDipConverter _microToDipConverter = new ();
    /// <inheritdoc cref="IValueConverter.Convert"/>
    /// <remarks>
    /// Converts <c>CanvasCoordinate</c> to Avalonia <c>Point</c>. 
    /// </remarks>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is CanvasCoordinate mapCoordinate)
            return new Point((double)_microToDipConverter.Convert(mapCoordinate.LeftPos), (double)_microToDipConverter.Convert(mapCoordinate.TopPos));
        return new BindingNotification(new InvalidOperationException("The value must be a map coordinate of type MapCoordinate."));
    }

    /// <inheritdoc cref="IValueConverter.ConvertBack"/>
    /// <remarks>
    /// Converts Avalonia <c>Point</c> back to <c>CanvasCoordinate</c>.
    /// </remarks>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Point point)
            return new CanvasCoordinate((int)_microToDipConverter.ConvertBack(point.X), (int)_microToDipConverter.ConvertBack(point.Y));
        return new BindingNotification(new InvalidOperationException("The value must be a point of type Avalonia.Point."));
    }
    /// <summary>
    /// Method for more convenient converting where only the value to be converted must be provided.
    /// </summary>
    /// <param name="value">CanvasCoordinates value to be converted to Avalonia Point.</param>
    /// <returns>Corresponding Avalonia Point.</returns>
    public object Convert(object? value)
    {
        return Convert(value, typeof(Point), null, CultureInfo.CurrentCulture);
    }
    /// <summary>
    /// Method for more convenient backward conversion where only the value to be converted must be provided.
    /// </summary>
    /// <param name="value">Avalonia Point to be converted back to CanvasCoordinates.</param>
    /// <returns>Corresponding CanvasCoordinates.</returns>
    public object ConvertBack(object? value)
    {
        return ConvertBack(value, typeof(CanvasCoordinate), null, CultureInfo.CurrentCulture);
    }
}