namespace ConsoleApp1;

public interface IRepresentant<out TItem> where TItem : IItem
{
    TItem? CastItem(IItem item);

    IRepresentant<TItem> CastMyself()
    {
        return this;
    }
}