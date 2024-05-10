using System;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapFormats;
using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.MapRepreMan.MapRepres.MapRepreImplementations.ObjectRepres;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.MapRepreMan.MapRepreImplementationReps;

public class ObjectRepreElevDepOrienteeringOmapImplementationRep : 
    MapRepreElevDataDepImplementationRep<Orienteering_ISOM_2017_2, OMAP, GeoReferenceContainingOMAP, ObjectRepreOrienteeringOmapImplementation, Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>
{
    public static ObjectRepreElevDepOrienteeringOmapImplementationRep Instance { get; } = new();
    private ObjectRepreElevDepOrienteeringOmapImplementationRep() { }
    public override Orienteering_ISOM_2017_2 UsedTemplate { get; } = Orienteering_ISOM_2017_2.Instance;
    public override IMapFormat<OMAP> UsedMapFormat { get; } = OMAPFormat.Instance;

    public override ObjectRepreOrienteeringOmapImplementation? ConstructMapRepre(Orienteering_ISOM_2017_2 template, GeoReferenceContainingOMAP map, IElevData elevData,
        IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken)
    {
        //TDOD: implement
        throw new NotImplementedException();
    }
}