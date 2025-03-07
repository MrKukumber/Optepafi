using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Optepafi.Models.GraphicsMan.Objects.Map;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapRepresentatives;
using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.MapRepreMan.Configurations;
using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.MapRepreMan.Graphs.Specific.CompleteNetIntertwining;
using Optepafi.Models.MapRepreMan.Implementations.Specific.CompleteIterativelySnapping;
using Optepafi.Models.MapRepreMan.Utils;
using Optepafi.Models.TemplateMan.Templates;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Shapes.Segments;

namespace Optepafi.Models.MapRepreMan.Implementations.Representatives.Specific.CompleteIterativelySnapping;

/// <summary>
/// Represents and creates net intertwining graph implementation of OMAP map representation which uses <see cref="Orienteering_ISOM_2017_2"/>
/// tempalte and does not require elevation data for its creation. It does not include any elevation information in the graph at all.
/// </summary>
public class CompleteNetIntertwiningPredecessorRememberingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationRep : 
    ElevDataIndepImplementationRep<Orienteering_ISOM_2017_2, OmapMap, OmapMap, CompleteNetIntertwiningPredecessorRememberingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation, CompleteNetIntertwiningMapRepreConfiguration, ICompleteNetIntertwiningPredecessorRememberingGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex, ICompleteNetIntertwiningPredecessorRememberingGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge, Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>
{
    public static CompleteNetIntertwiningPredecessorRememberingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationRep Instance { get; } = new();
    private CompleteNetIntertwiningPredecessorRememberingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationRep(){ }

    public override Orienteering_ISOM_2017_2 UsedTemplate { get; } = Orienteering_ISOM_2017_2.Instance;
    public override IMapFormat<OmapMap> UsedMapFormat { get; } = OmapMapRepresentative.Instance;

    public override CompleteNetIntertwiningPredecessorRememberingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation CreateImplementation(Orienteering_ISOM_2017_2 template, OmapMap map,
        CompleteNetIntertwiningMapRepreConfiguration configuration, IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken)
    {
        var vertexBuilders = GraphCreator.Instance.Create(map, configuration, progress, cancellationToken);
        RadiallySearchableKdTree<CompleteNetIntertwiningPredecessorRememberingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation.EdgesEditableVertex> vertices = new (vertexBuilders.Select(vb => vb.BuildPredecessorRemembering()), v => (v.Attributes.Position.XPos, v.Attributes.Position.YPos));
        foreach (var vertexBuilder in vertexBuilders) vertexBuilder.ConnectPredecessorRememberingAfterBuild(); 
        return new CompleteNetIntertwiningPredecessorRememberingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation(vertices, map.Scale);
    }

}

