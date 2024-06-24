using ReactiveUI;

namespace Optepafi.ViewModels.DataViewModels;


public abstract class DataViewModel<TData> : ViewModelBase
    where TData : notnull
{
    protected abstract TData Data { get; }
    
    public override bool Equals(object? obj)
    {
        if (obj is DataViewModel<TData> correctObj)
            return correctObj.Data.Equals(Data); 
        return false;
    }

    public override int GetHashCode()
    {
        return Data.GetHashCode();
    }

    public static bool operator ==(DataViewModel<TData>? operand1, DataViewModel<TData>? operand2)
    {
        if (operand1 is null)
        {
            if (operand2 is null) return true;
            return false;
        }
        return operand1.Equals(operand2);
    }

    public static bool operator !=(DataViewModel<TData>? operand1, DataViewModel<TData>? operand2)
    {
        return !(operand1 == operand2);
    }
}