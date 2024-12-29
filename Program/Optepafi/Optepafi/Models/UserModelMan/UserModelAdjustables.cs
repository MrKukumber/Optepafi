using System.Text.Json.Serialization;

namespace Optepafi.Models.UserModelMan;

/// <summary>
/// Main interface representing base contract for adjustable property of user model.
/// 
/// It defines basic identifiers for adjustable property.  
/// Adjustable prop. can be used for (surprisingly) adjusting of user model computations of edge weights for path finding algorithms.  
/// </summary>
public interface IUserModelAdjustable
{
    public string Name { get; }
    public string Caption { get; }
}

/// <summary>
/// Represents one value adjustable property.
/// 
/// It should be used in those cases in which it is possible to set one definite value for some user model property.  
/// </summary>
/// <typeparam name="TValue">Type of value returned by adjustable prop.</typeparam>
public interface IValueAdjustable<TValue> : IUserModelAdjustable
{
    public string? Unit { get; }
    public TValue Value { get; set; }
}
/// <summary>
/// Represents interval adjustable property. The lower and upper bound of the interval can be set.
/// 
/// Its mean of use is in cases when is not 100% clear what value should some parameter have and because of that interval is more usable option.  
/// These adjustable properties can then be set more precisely by methods like relevance feedback.  
/// </summary>
/// <typeparam name="TValue">Type of value returned by adjustable prop.</typeparam>
public interface IIntervalAdjustable<TValue> : IUserModelAdjustable
{
    public string? Unit { get; }
    public float StartValue { get; set; }
    public float EndValue { get; set; }
}

/// <summary>
/// Represents one float value adjustable property.
/// 
/// It should be used in those cases in which it is possible to set one definite value for some user model property.  
/// </summary>
public class FloatValueAdjustable(string name, float value, string? unit, string? caption = null) : IValueAdjustable<float>
{
    [JsonIgnore]
    public string Name { get; } = name;
    [JsonIgnore]
    public string Caption { get; } = caption is null ? name : caption;
    [JsonIgnore]
    public string? Unit { get; } = unit;

    public float Value { get; set; } = value;
}

/// <summary>
/// Represents interval adjustable property. The lower and upper float bound of the interval can be set.
/// 
/// Its mean of use is in cases when is not 100% clear what value should some parameter have and because of that interval is more usable option.  
/// These adjustable properties can then be set more precisely by methods like relevance feedback.  
/// </summary>
public class FloatIntervalAdjustable(string name,  (float, float) interval, string? unit, string? caption = null) : IIntervalAdjustable<float>
{
    [JsonIgnore]
    public string Name { get; } = name;
    [JsonIgnore]
    public string Caption { get; } = caption is null ? name : caption;
    [JsonIgnore]
    public string? Unit { get; } = unit;

    public float StartValue { get; set; } = interval.Item1;
    public float EndValue { get; set; } = interval.Item2;
}

/// <summary>
/// Represents one value bounded float adjustable property. Value of this adjustable should not fall out from interval defined by min and max value.
/// 
/// It should be used in those cases in which it is possible to set one definite value for some user model property.  
/// </summary>
public class BoundedFloatValueAdjustable(string name, float value, string? unit, float min, float max, string? caption = null) : IValueAdjustable<float>
{
    [JsonIgnore]
    public string Name { get; } = name;
    [JsonIgnore]
    public string Caption { get; } = caption is null ? name : caption;
    [JsonIgnore]
    public string? Unit { get; } = unit;

    [JsonIgnore]
    public float Min { get; } = min;
    [JsonIgnore]
    public float Max { get; } = max;
    
    public float Value { get; set; } = value;
    
}