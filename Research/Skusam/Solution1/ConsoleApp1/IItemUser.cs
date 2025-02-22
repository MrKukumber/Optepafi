namespace ConsoleApp1;

public interface IItemUser<out TItem> where TItem : IItem
{
    public void UseItem<TItem>(TItem item) where TItem : IItem
    {
        int a = 10;
    }
}