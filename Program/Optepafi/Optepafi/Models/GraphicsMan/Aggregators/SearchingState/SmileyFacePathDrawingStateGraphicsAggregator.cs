using System.Threading;
using Optepafi.Models.Graphics.Objects.Path;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.SearchingAlgorithmMan.SearchingStates;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.Graphics.GraphicsAggregators.SearchingState;

public class SmileyFacePathDrawingStateGraphicsAggregator<TVertexAttributes, TEdgeAttributes> : 
    ISearchingStateGraphicsAggregator<SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes>,TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    public static SmileyFacePathDrawingStateGraphicsAggregator<TVertexAttributes, TEdgeAttributes> Instance { get; } = new();
    private SmileyFacePathDrawingStateGraphicsAggregator(){}
    public void AggregateGraphics(SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes> searchingState, IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel,
        IGraphicsObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken)
    {
        foreach (var ((legStart, legFinish), smileyFaceObjects) in searchingState.DrawnObjects)
        {
            int width = int.Abs(legFinish.XPos - legStart.XPos);
            int height = int.Abs(legFinish.YPos - legStart.YPos);
            MapCoordinate leftBottomVertex = new MapCoordinate(
                int.Min(legStart.XPos, legFinish.XPos),
                int.Min(legStart.YPos, legFinish.YPos));
            foreach (var smileyFaceObject in smileyFaceObjects)
            {
                switch (smileyFaceObject)
                {
                    case SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes>.SmileyFaceObject.LeftEye: 
                        collectorForAggregatedObjects.Add(new SmileyFaceEyeObject( 
                            new MapCoordinate((int)(leftBottomVertex.XPos + 0.25 * width), (int)(leftBottomVertex.YPos + 0.8 *height)), 
                            (int)(width * 0.1), (int)(height * 0.1)));
                        break;
                    case SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes>.SmileyFaceObject.RightEye: 
                        collectorForAggregatedObjects.Add(new SmileyFaceEyeObject(
                            new MapCoordinate((int)(leftBottomVertex.XPos + 0.75 * width), (int)(leftBottomVertex.YPos + 0.8 * height)),
                            (int)(width * 0.1), (int)(height * 0.1)));
                        break;
                    case SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes>.SmileyFaceObject.Nose:
                        collectorForAggregatedObjects.Add(new SmileyFaceNoseObject( 
                            new MapCoordinate((int)(leftBottomVertex.XPos + 0.5 * width), (int)(leftBottomVertex.YPos + 0.5 * height)), 
                            (int)(width * 0.1), (int)(height * 0.1)));
                        break;
                    case SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes>.SmileyFaceObject.Mouth:
                        collectorForAggregatedObjects.Add(new SmileyFaceMouthObject(
                            new MapCoordinate((int)(leftBottomVertex.XPos + 0.15 * width), (int)(leftBottomVertex.YPos + 0.15 * height)),
                            new MapCoordinate((int)(leftBottomVertex.XPos + 0.35 * width), (int)(leftBottomVertex.YPos - 0.15 * height)),
                            new MapCoordinate((int)(leftBottomVertex.XPos + 0.65 * width), (int)(leftBottomVertex.YPos - 0.15 * height)),
                            new MapCoordinate((int)(leftBottomVertex.XPos + 0.85 * width), (int)(leftBottomVertex.YPos + 0.15 * height))));
                        break;
                }
            }
        }
    }
}