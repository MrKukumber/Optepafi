using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Optepafi.Views.Utils;

/// <summary>
/// Converter from metric system to corresponding value of device independent pixels (dip).
///
/// It uses standard size of dip equal to 1/96 of an inch. It takes input value in micrometers and converts it to corresponding count of dip.  
/// </summary>
public class MicrometersToDipConverter : IValueConverter
{
    public static MicrometersToDipConverter Instance { get;} = new MicrometersToDipConverter();

    private const double StandardDipPerInch = 96;
    private const int MicrometersPerInch = 25400;
    private const double MicrometersPerDip = MicrometersPerInch / StandardDipPerInch;

    /// <inheritdoc cref="IValueConverter.Convert"/>
    /// <remarks>
    /// Converts micrometers to corresponding value of dip.
    /// </remarks>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int micrometers)
        {
            double dip = micrometers / MicrometersPerDip;
            return dip;
        }
        return new BindingNotification(new InvalidOperationException("The value must be a int representing micrometers."));
    }

    /// <inheritdoc cref="IValueConverter.ConvertBack"/>
    /// <remarks>
    /// Converts value of dip back to micrometers.
    /// </remarks>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double dip)
        {
            int micrometers = (int) (dip * MicrometersPerDip);
            return micrometers;
        }
        return new BindingNotification(new InvalidOperationException("The value must be a double representing DIP."));
    }

    /// <summary>
    /// Method for more convenient converting where only the value to be converted must be provided.
    /// </summary>
    /// <param name="value">Value in micrometers to be converted to value of dip.</param>
    /// <returns>Corresponding value of dip.</returns>
    public double Convert(int value)
    {
        return (double) Convert(value, typeof(float), null, CultureInfo.CurrentCulture);
    }
    /// <summary>
    /// Method for more convenient backward conversion where only the value to be converted must be provided.
    /// </summary>
    /// <param name="value">Value of dip to be converted back to micrometers.</param>
    /// <returns>Corresponding value in micrometers.</returns>
    public int ConvertBack(double value)
    {
        return (int) ConvertBack(value, typeof(Int32), null, CultureInfo.CurrentCulture);
    }
}