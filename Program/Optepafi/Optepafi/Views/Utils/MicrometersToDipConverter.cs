using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data;
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
        return new BindingNotification(new InvalidOperationException("The value must be a int representing micrometers."));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double dip)
        {
            int micrometers = (int) (dip * MicrometersPerDip);
            return micrometers;
        }
        return new BindingNotification(new InvalidOperationException("The value must be a double representing DIP."));
    }

    public object? Convert(object? value)
    {
        return Convert(value, typeof(float), null, CultureInfo.CurrentCulture);
    }
    public object? ConvertBack(object? value)
    {
        return ConvertBack(value, typeof(Int32), null, CultureInfo.CurrentCulture);
    }
}