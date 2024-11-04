namespace Optepafi.Models.Utils.Configurations;

//TODO: comment
public interface IConfigItemGenericVisitor<TOut>
{
    TOut AcceptGeneric<TConfigItem>(TConfigItem item) where TConfigItem : IConfigItem;
}