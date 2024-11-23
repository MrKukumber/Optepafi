using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.Configurations;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.Graphs.Representatives;
using Optepafi.Models.MapRepreMan.Graphs.Representatives.Specific;
using Optepafi.Models.MapRepreMan.Implementations.Representatives;
using Optepafi.Models.MapRepreMan.Implementations.Representatives.Specific.CompleteIterativelySnapping;
using Optepafi.Models.MapRepreMan.MapRepres.Specific;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan.MapRepres.Representatives.Specific;

//TODO: comment
public class CompleteSnappingMapRepreRep : MapRepreRepresentative<ICompleteSnappingMapRepre, CompleteSnappingMapRepreConfiguration>
{
    public static CompleteSnappingMapRepreRep Instance { get; } = new();
    private CompleteSnappingMapRepreRep() { }
    public override string MapRepreName { get; } = "Complete, iteratively snapping map representation";

    public override IImplementationIndicator<ITemplate, IMap, ICompleteSnappingMapRepre>[]
        ImplementationIndicators { get; } = [CompleteSnappingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationRep.Instance];

    public override IGraphRepresentative<IGraph<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> GetCorrespondingGraphRepresentative<TVertexAttributes, TEdgeAttributes>() => CompleteSnappingGraphRep<TVertexAttributes, TEdgeAttributes>.Instance;
    protected override CompleteSnappingMapRepreConfiguration DefaultConfiguration { get; } = new CompleteSnappingMapRepreConfiguration();
}