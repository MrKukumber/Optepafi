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

public class SmileyFacePathGraphicsAggregator<TVertexAttributes, TEdgeAttributes> : IPathGraphicsAggregator<SmileyFacePath<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    public static SmileyFacePathGraphicsAggregator<TVertexAttributes, TEdgeAttributes> Instance { get; } = new();
    private SmileyFacePathGraphicsAggregator(){}
    public void AggregateGraphics(SmileyFacePath<TVertexAttributes, TEdgeAttributes> path, 
        IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel, 
        IGraphicsObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken)
    {
        foreach (var (legStart, legFinish) in path.Path)
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