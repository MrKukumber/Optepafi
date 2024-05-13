using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.MapMan.Maps;

public class GeoReferenceContainingOMAP : OMAP, IGeoReferencedMap
{
    //TODO: implement
    public GeoCoordinate RepresentativeLocation { get; }
}