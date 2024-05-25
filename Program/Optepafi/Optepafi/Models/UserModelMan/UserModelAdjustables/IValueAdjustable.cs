namespace Optepafi.Models.UserModelMan.UserModelAdjustables;

/// <summary>
/// Represents one value adjustable property. It should be used in those cases in which it is possible to set one definite value for some user model property.
/// </summary>
/// <typeparam name="TValue">Type of value returned by adjustable prop.</typeparam>
public interface IValueAdjustable<TValue> : IUserModelAdjustable
{
    public TValue Value { get; set; }
}