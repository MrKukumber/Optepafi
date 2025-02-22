namespace ConsoleApp1;

public class Item : IItem
{
    public IRepresentant<IItem> Repre { get; } = Representant.Instance;

    public void VisitForUsing(IItemUser<IItem> itemUser)
    {
        itemUser.UseItem(this);
    }
}