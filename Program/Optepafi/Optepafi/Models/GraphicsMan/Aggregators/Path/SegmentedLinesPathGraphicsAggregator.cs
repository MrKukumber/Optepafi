using System.Collections.Generic;
using System.Threading;
using Optepafi.Models.GraphicsMan.Collectors;
using Optepafi.Models.GraphicsMan.Objects.Path;
using Optepafi.Models.SearchingAlgorithmMan.Paths.Specific;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels.Functionalities;
using Optepafi.Models.Utils.Shapes.Segments;

namespace Optepafi.Models.GraphicsMan.Aggregators.Path;

//TODO: comment
public class SegmentedLinesPathGraphicsAggregator<TVertexAttributes, TEdgeAttributes> : IPathGraphicsAggregator<SegmentedLinesPath<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    public static SegmentedLinesPathGraphicsAggregator<TVertexAttributes, TEdgeAttributes> Instance { get; } = new();
    private SegmentedLinesPathGraphicsAggregator(){}
    
    public void AggregateGraphics(SegmentedLinesPath<TVertexAttributes, TEdgeAttributes> path, IComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel,
        IGraphicObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken)
    {
        if (userModel is IPositionRetrieving<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> positionRetrievingUserModel)
            foreach (var line in path.Lines)
            {
                if (line.Count < 2) continue;
                var startPoint = positionRetrievingUserModel.RetrievePosition(line[0]);
                List<Segment> segments = [];
                for (var i = 1; i < line.Count; i++) segments.Add(new LineSegment(positionRetrievingUserModel.RetrievePosition(line[i])));
                collectorForAggregatedObjects.Add(new SegmentedLineObject( new Utils.Shapes.Path(startPoint, segments)));
            }
    }
}