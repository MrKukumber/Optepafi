using System;
using System.Threading;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapFormats;
using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.MapRepreMan.Configurations;
using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.MapRepreMan.Implementations.Representatives.Specific.CompleteIterativelySnapping;

public class CompleteSnappingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationRep : 
    ElevDataIndepImplementationRep<Orienteering_ISOM_2017_2, OmapMap, OmapMap, ICompleteSnappingGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>, CompleteSnappingMapRepreConfiguration, Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>
{
    public static CompleteSnappingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationRep Instance { get; } = new();
    private CompleteSnappingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationRep(){ }

    public override Orienteering_ISOM_2017_2 UsedTemplate { get; } = Orienteering_ISOM_2017_2.Instance;
    public override IMapFormat<OmapMap> UsedMapFormat { get; } = OmapMapRepresentative.Instance;

    public override ICompleteSnappingGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes> ConstructMapRepre(Orienteering_ISOM_2017_2 template, OmapMap map,
        CompleteSnappingMapRepreConfiguration configuration, IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken)
    {
        // throw new NotImplementedException();
    }
}