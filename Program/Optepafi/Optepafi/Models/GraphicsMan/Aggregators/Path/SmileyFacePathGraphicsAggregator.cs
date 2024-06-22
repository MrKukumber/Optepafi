using System.Threading;
using Optepafi.Models.Graphics.Objects.Path;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;
using ReactiveUI;

namespace Optepafi.Models.Graphics.GraphicsAggregators.Path;

/// <summary>
/// Singleton class representing aggregator of graphic objects for <see cref="SmileyFacePath{TVertexAttributes,TEdgeAttributes}"/> path type.
/// For more info on path graphics aggregators see <see cref="IPathGraphicsAggregator{TPath,TVertexAttributes,TEdgeAttributes}"/>.
/// </summary>
/// <typeparam name="TVertexAttributes">Type of vertex attributes which path can contain and user model can use for computing.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes which path can contain and user model can use for computing.</typeparam>
public class SmileyFacePathGraphicsAggregator<TVertexAttributes, TEdgeAttributes> : IPathGraphicsAggregator<SmileyFacePath<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    public static SmileyFacePathGraphicsAggregator<TVertexAttributes, TEdgeAttributes> Instance { get; } = new();
    private SmileyFacePathGraphicsAggregator(){}
    
    /// <inheritdoc cref="IPathGraphicsAggregator{TPath,TVertexAttributes,TEdgeAttributes}.AggregateGraphics"/>
    /// <remarks>
    /// <c>SmileyFacePath</c> provides set of legs for which smiley face drawing was created.
    /// Aggregator will create graphic objects for each such leg and submit them into collector.
    /// </remarks>
    public void AggregateGraphics(SmileyFacePath<TVertexAttributes, TEdgeAttributes> path, 
        IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel, 
        IGraphicObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken)
    {
        foreach (var (legStart, legFinish) in path.PathSegments)
        {
            int width = int.Abs(legFinish.XPos - legStart.XPos);
            int height = int.Abs(legFinish.YPos - legStart.YPos);
            MapCoordinate leftBottomVertex = new MapCoordinate(
                int.Min(legStart.XPos, legFinish.XPos),
                int.Min(legStart.YPos, legFinish.YPos));
            collectorForAggregatedObjects.Add(new SmileyFaceEyeObject(
                new MapCoordinate((int)(leftBottomVertex.XPos + 0.25 * width), (int)(leftBottomVertex.YPos + 0.8 *height)), 
                (int)(width * 0.1), (int)(height * 0.1)));
            collectorForAggregatedObjects.Add(new SmileyFaceEyeObject(
                new MapCoordinate((int)(leftBottomVertex.XPos + 0.75 * width), (int)(leftBottomVertex.YPos + 0.8 * height)),
                (int)(width * 0.1), (int)(height * 0.1)));
            collectorForAggregatedObjects.Add(new SmileyFaceNoseObject(
                new MapCoordinate((int)(leftBottomVertex.XPos + 0.5 * width), (int)(leftBottomVertex.YPos + 0.5 * height)), 
                (int)(width * 0.1), (int)(height * 0.1)));
            collectorForAggregatedObjects.Add(new SmileyFaceMouthObject(
                new MapCoordinate((int)(leftBottomVertex.XPos + 0.15 * width), (int)(leftBottomVertex.YPos + 0.15 * height)),
                new MapCoordinate((int)(leftBottomVertex.XPos + 0.35 * width), (int)(leftBottomVertex.YPos - 0.15 * height)),
                new MapCoordinate((int)(leftBottomVertex.XPos + 0.65 * width), (int)(leftBottomVertex.YPos - 0.15 * height)),
                new MapCoordinate((int)(leftBottomVertex.XPos + 0.85 * width), (int)(leftBottomVertex.YPos + 0.15 * height))));
        }
    }
}