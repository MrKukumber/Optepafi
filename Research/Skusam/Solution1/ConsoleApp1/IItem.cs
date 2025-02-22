namespace ConsoleApp1;


public interface IItem
{
    public IRepresentant<IItem> Repre { get; }
    public void VisitForUsing(IItemUser<IItem> itemUser);
}