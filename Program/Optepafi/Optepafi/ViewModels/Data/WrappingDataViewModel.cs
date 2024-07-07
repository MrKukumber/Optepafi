namespace Optepafi.ViewModels.Data;


/// <summary>
/// Abstract class which represents wrapping <c>DataViewModel</c>. That means it is very closely coupled with some data instance of specific type.
/// This data can be retrieved backwardly. Additionally equality operators are overriden in such way that two instances of
/// <c>WrappingDataViewModel</c> are equal when their coupled data are equal.
/// 
/// For these reasons this class is suitable to be implemented by view models of data by which ViewModel communicates backwardly with ModelView.
/// It is enough for data which need to be only displayed in View to have ViewModel which implements only <see cref="DataViewModel"/>.
/// </summary>
/// <typeparam name="TData">Type of coupled data. Provided data cant be nullable.</typeparam>
public abstract class WrappingDataViewModel<TData> : DataViewModel
    where TData : notnull
{
    /// <summary>
    /// Reference to coupled data to this ViewModel.
    /// </summary>
    protected abstract TData Data { get; }
    
    /// <inheritdoc cref="object.Equals(object?)"/>
    /// <remarks>
    /// Two wrapping data view models are equal when their coupled data are equal.
    /// </remarks>
    public override bool Equals(object? obj)
    {
        if (obj is WrappingDataViewModel<TData> correctObj)
            return correctObj.Data.Equals(Data); 
        return false;
    }

    /// <inheritdoc cref="object.GetHashCode"/>
    /// <remarks>
    /// Hash code of wrapping data view model is set to be hash code of its coupled data.
    /// </remarks>
    public override int GetHashCode()
    {
        return Data.GetHashCode();
    }

    public static bool operator ==(WrappingDataViewModel<TData>? operand1, WrappingDataViewModel<TData>? operand2)
    {
        if (operand1 is null)
        {
            if (operand2 is null) return true;
            return false;
        }
        return operand1.Equals(operand2);
    }
    
    public static bool operator !=(WrappingDataViewModel<TData>? operand1, WrappingDataViewModel<TData>? operand2)
    {
        return !(operand1 == operand2);
    }
}