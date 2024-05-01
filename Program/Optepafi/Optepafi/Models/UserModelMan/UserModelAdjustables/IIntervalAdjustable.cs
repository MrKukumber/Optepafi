namespace Optepafi.Models.UserModelMan.UserModelAdjustables;

public interface IIntervalAdjustable<TValue>
{
    public TValue StartValue { get; set; } 
    public TValue EndValue { get; set; }
}