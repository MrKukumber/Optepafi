namespace Optepafi.Models.Utils;

public interface IConfigItemGenericVisitor<TOut>
{
    TOut AcceptGeneric<TConfigItem>(TConfigItem item) where TConfigItem : IConfigItem;
}