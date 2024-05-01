namespace Optepafi.Models.UserModelMan.UserModelAdjustables;

public interface IValueAdjustable<TValue> : IUserModelAdjustable
{
    public TValue Value { get; set; }
}