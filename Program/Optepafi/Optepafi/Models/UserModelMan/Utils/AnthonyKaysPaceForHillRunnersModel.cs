using System;

namespace Optepafi.Models.UserModelMan.Utils;

public static class AnthonyKaysPaceForHillRunnersModel
{
    public static double GetPace(double gradient, bool criticalGradientsExist)
    {
        if (gradient > 0.3152)
            return criticalGradientsExist 
                ? UphillQuadratic(gradient) 
                : UphillLinear(gradient);
        if (gradient < -0.2617)
            return criticalGradientsExist 
                ? DownhillQuadratic(gradient) 
                : DownhillLinear(gradient);
        return Quartic(gradient);
    }

    public static double GetMinimalPace()
        => Quartic(-0.0885);
    
    private static double Quartic(double m)
        => 0.1707 + 0.5656 * m + 3.2209 * Math.Pow(m,2) - 0.3211 * Math.Pow(m, 3) - 4.3635 * Math.Pow(m,4);
    
    private static double UphillQuadratic(double m)
        => 0.0314 + 1.7544 * m + 0.3162 * Math.Pow(m, 2);

    private static double UphillLinear(double m)
        => 1.9538 * m;
    
    private static double DownhillQuadratic(double m)
        => 0.1151 + 0.0061 * m + 1.6802 * Math.Pow(m, 2);
    
    private static double DownhillLinear(double m)
        => -0.8732 * m;

}