using System.Collections.Generic;
using System.Threading;
using Optepafi.Models.GraphicsMan.Collectors;
using Optepafi.Models.GraphicsMan.Objects.MapRepre;
using Optepafi.Models.MapRepreMan.Implementations.Specific.CompleteIterativelySnapping;
using Optepafi.Models.MapRepreMan.Utils;
using Optepafi.Models.TemplateMan.Templates;
using Optepafi.Models.Utils;

namespace Optepafi.Models.GraphicsMan.Aggregators.MapRepre.CompleteNetIntertwining;

public class CompleteNetIntertwiningPredecessorRememberingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationGraphicsAggregator : IMapRepreGraphicsAggregator<CompleteNetIntertwiningPredecessorRememberingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation>
{
    public static CompleteNetIntertwiningPredecessorRememberingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationGraphicsAggregator Instance { get; } = new();
    private CompleteNetIntertwiningPredecessorRememberingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationGraphicsAggregator(){}
    public void AggregateGraphics(CompleteNetIntertwiningPredecessorRememberingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation implementation,
        IGraphicObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken)
    {
        HashSet<(MapCoordinates, MapCoordinates)> foundEdgeCoordinates = new HashSet<(MapCoordinates, MapCoordinates)>();
        // foreach (var vertex in ((IRadiallySearchableDataStruct<CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation.EdgesEditableVertex>)implementation.SearchableVertices).FindInEuclideanDistanceFrom((-3841, 8942), 5000)) // for debugging
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