namespace Optepafi.Models.UserModelMan.UserModelAdjustables;

/// <summary>
/// Represents interval adjustable property. The lower and upper bound of the interval can be set.
/// 
/// Its mean of use is in cases when is not 100% clear what value should some parameter have and because of that interval is more usable option.  
/// These adjustable properties can then be set more precisely by methods like relevance feedback.  
/// </summary>
/// <typeparam name="TValue">Type of value returned by adjustable prop.</typeparam>
public interface IIntervalAdjustable<TValue>
{
    public TValue StartValue { get; set; } 
    public TValue EndValue { get; set; }
}