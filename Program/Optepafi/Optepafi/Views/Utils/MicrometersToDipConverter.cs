using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Optepafi.Views.Utils;

public class MicrometersToDipConverter : IValueConverter
{
    private const double StandardDipPerInch = 96;
    private const int MicrometersPerInch = 25400;
    private const double MicrometersPerDip = MicrometersPerInch / StandardDipPerInch;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int micrometers)
        {
            double dip = micrometers / MicrometersPerDip;
            return dip;
        }
        throw new InvalidOperationException("The value must be a int representing micrometers.");
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double dip)
        {
            int micrometers = (int) (dip * MicrometersPerDip);
            return micrometers;
        }
        throw new InvalidOperationException("The value must be a double representing DIP.");
    }
}