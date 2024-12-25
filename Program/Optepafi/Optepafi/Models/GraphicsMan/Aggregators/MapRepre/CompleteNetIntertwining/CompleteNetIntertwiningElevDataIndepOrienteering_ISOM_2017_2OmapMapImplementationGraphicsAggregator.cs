using System.Collections.Generic;
using System.Threading;
using Optepafi.Models.GraphicsMan.Collectors;
using Optepafi.Models.GraphicsMan.Objects.MapRepre.CompleteNetIntertwiningMapRepre;
using Optepafi.Models.MapRepreMan.Implementations.Specific.CompleteIterativelySnapping;
using Optepafi.Models.TemplateMan.Templates;
using Optepafi.Models.Utils;

namespace Optepafi.Models.GraphicsMan.Aggregators.MapRepre.CompleteNetIntertwining;

public class CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationGraphicsAggregator : IMapRepreGraphicsAggregator<CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation>
{
    public static CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationGraphicsAggregator Instance { get; } = new();
    private CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationGraphicsAggregator(){}
    public void AggregateGraphics(CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation implementation,
        IGraphicObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken)
    {
        HashSet<(MapCoordinates, MapCoordinates)> foundEdgeCoordinates = new HashSet<(MapCoordinates, MapCoordinates)>();
        // foreach (var vertex in implementation.SearchableVertices.FindInEuclideanDistanceFrom((826435, 165544), 50_000)) // for debugging
        foreach (var vertex in implementation)
        {
            collectorForAggregatedObjects.Add(new VertexObject(vertex.Attributes.Position));
            foreach (var edge in vertex.GetEdges())
                if (!foundEdgeCoordinates.Contains((edge.To.Attributes.Position, vertex.Attributes.Position)))
                {
                    collectorForAggregatedObjects.Add(new EdgeObject(vertex.Attributes.Position, edge.To.Attributes.Position, edge.Attributes));
                    foundEdgeCoordinates.Add((vertex.Attributes.Position, edge.To.Attributes.Position));
                }
        }
    }
}