namespace ConsoleApp1;

public class Representant : IRepresentant<Item>
{
    public Item? CastItem(IItem item)
    {
        if (item is Item i) return i;
        return null;
    }
    static public Representant Instance { get; } = new();
}