// See https://aka.ms/new-console-template for more information

using ConsoleApp1;

public class Program
{
    public static void Main(string[] args)
    {
        IItem item = new Item();
        IRepresentant<IItem> representant = new Representant();
        GenericMethod(item.Repre.CastItem(item), representant.CastMyself());
        
        
    }
    
    public static void GenericMethod<TItem, TRepresenentant>(TItem item, TRepresenentant repre) where TItem : IItem where TRepresenentant : IRepresentant<TItem>
    {
        IItemUser<IItem> itemUser = new ItemUser();
        item.VisitForUsing(itemUser);
    }
}