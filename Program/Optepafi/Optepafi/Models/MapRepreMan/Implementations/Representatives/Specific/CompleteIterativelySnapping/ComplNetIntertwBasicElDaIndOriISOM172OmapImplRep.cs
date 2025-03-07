using System;
using System.Linq;
using System.Threading;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapRepresentatives;
using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.MapRepreMan.Configurations;
using Optepafi.Models.MapRepreMan.Graphs.Specific.CompleteNetIntertwining;
using Optepafi.Models.MapRepreMan.Implementations.Specific.CompleteIterativelySnapping;
using Optepafi.Models.MapRepreMan.Utils;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.MapRepreMan.Implementations.Representatives.Specific.CompleteIterativelySnapping;

public class CompleteNetIntertwiningBasicElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationRep :
    ElevDataIndepImplementationRep<Orienteering_ISOM_2017_2, OmapMap, OmapMap, CompleteNetIntertwiningBasicElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation, CompleteNetIntertwiningMapRepreConfiguration, ICompleteNetIntertwiningBasicGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex, ICompleteNetIntertwiningBasicGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge, Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>
{
    public static CompleteNetIntertwiningBasicElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationRep Instance { get; } = new();
    private CompleteNetIntertwiningBasicElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationRep(){ }
    public override Orienteering_ISOM_2017_2 UsedTemplate { get; } = Orienteering_ISOM_2017_2.Instance;
    public override IMapFormat<OmapMap> UsedMapFormat { get; } = OmapMapRepresentative.Instance;

    public override CompleteNetIntertwiningBasicElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation CreateImplementation(
        Orienteering_ISOM_2017_2 template, OmapMap map, CompleteNetIntertwiningMapRepreConfiguration configuration,
        IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken)
    {
        var vertexBuilders = GraphCreator.Instance.Create(map, configuration, progress, cancellationToken);
        RadiallySearchableKdTree<CompleteNetIntertwiningBasicElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation.EdgesEditableVertex> vertices = new (vertexBuilders.Select(vb => vb.BuildBasic()), v => (v.Attributes.Position.XPos, v.Attributes.Position.YPos));
        foreach (var vertexBuilder in vertexBuilders) vertexBuilder.ConnectBasicAfterBuild(); 
        return new CompleteNetIntertwiningBasicElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation(vertices, map.Scale);
    }
}