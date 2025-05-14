using System.Collections.Generic;
using System.Threading;
using Optepafi.Models.GraphicsMan.Collectors;
using Optepafi.Models.GraphicsMan.Objects.MapRepre;
using Optepafi.Models.MapRepreMan.Graphs.Specific.CompleteNetIntertwining;
using Optepafi.Models.MapRepreMan.Implementations.Specific.CompleteIterativelySnapping;
using Optepafi.Models.MapRepreMan.Utils;
using Optepafi.Models.TemplateMan.Templates;
using Optepafi.Models.Utils;

namespace Optepafi.Models.GraphicsMan.Aggregators.MapRepre.CompleteNetIntertwining;

public class CompleteNetIntertwiningBasicElevDataDepOrienteering_ISOM_2017_2OmapMapImplementationGraphicsAggregator 
    : IMapRepreGraphicsAggregator<CompleteNetIntertwiningBasicElevDataDepOrienteering_ISOM_2017_2OmapMapImplementation>
{
    public static CompleteNetIntertwiningBasicElevDataDepOrienteering_ISOM_2017_2OmapMapImplementationGraphicsAggregator Instance { get; } = new();
    private CompleteNetIntertwiningBasicElevDataDepOrienteering_ISOM_2017_2OmapMapImplementationGraphicsAggregator(){}
    public void AggregateGraphics(
        CompleteNetIntertwiningBasicElevDataDepOrienteering_ISOM_2017_2OmapMapImplementation implementation,
        IGraphicObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken)
    {
        HashSet<(MapCoordinates, MapCoordinates)> foundEdgeCoordinates = new HashSet<(MapCoordinates, MapCoordinates)>();
        // foreach (var vertex in ((IRadiallySearchableDataStruct<ICompleteNetIntertwiningBasicGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex>)implementation.SearchableVertices).FindInEuclideanDistanceFrom((-22740, -39020), 5000)) // for debugging
        foreach (var vertex in implementation)
        {
            collectorForAggregatedObjects.Add(new VertexObject(vertex.Attributes.Position, vertex.Attributes.Elevation));
            foreach (var edge in vertex.GetEdges())
                if (!foundEdgeCoordinates.Contains((edge.To.Attributes.Position, vertex.Attributes.Position)))
                {
                    collectorForAggregatedObjects.Add(new EdgeObject(vertex.Attributes.Position, edge.To.Attributes.Position, edge.Attributes));
                    foundEdgeCoordinates.Add((vertex.Attributes.Position, edge.To.Attributes.Position));
                }
        }
    }
}