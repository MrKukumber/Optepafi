namespace Optepafi.Models.MapRepreMan;

public abstract class MapRepre
{
    public IMapRepreRep<MapRepre> MapRepreRep { get; }

    protected MapRepre(IMapRepreRep<MapRepre> mapRepreRep)
    {
        MapRepreRep = mapRepreRep;
    }
}