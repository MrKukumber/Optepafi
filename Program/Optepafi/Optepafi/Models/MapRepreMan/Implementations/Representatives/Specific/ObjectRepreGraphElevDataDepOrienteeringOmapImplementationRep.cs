using System;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapFormats;
using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.MapRepreMan.MapRepres.GraphImplementations;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.MapRepreMan.MapRepreImplementationReps.SpecificImplementationReps;

public class ObjectRepreGraphElevDataDepOrienteeringOmapImplementationRep : 
    ElevDataDepImplementationRep<Orienteering_ISOM_2017_2, OMAP, GeoReferenceContainingOMAP, ObjectRepreGraphOrienteeringOmapImplementation, Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>
{
    public static ObjectRepreGraphElevDataDepOrienteeringOmapImplementationRep Instance { get; } = new();
    private ObjectRepreGraphElevDataDepOrienteeringOmapImplementationRep() { }
    public override Orienteering_ISOM_2017_2 UsedTemplate { get; } = Orienteering_ISOM_2017_2.Instance;
    public override IMapFormat<OMAP> UsedMapFormat { get; } = OMAPRepresentative.Instance;
    
    public override ObjectRepreGraphOrienteeringOmapImplementation ConstructMapRepre(Orienteering_ISOM_2017_2 template, GeoReferenceContainingOMAP map, IElevData elevData,
        IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken)
    {
        //TODO: implement
        throw new NotImplementedException();
    }
}