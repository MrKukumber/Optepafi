using System.Threading;
using Optepafi.Models.GraphicsMan.Objects.Path;
using Optepafi.Models.MapMan;
using Optepafi.Models.SearchingAlgorithmMan.SearchingStates.Specific;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.GraphicsMan.Aggregators.SearchingState;

/// <summary>
/// Singleton class representing aggregator of graphic objects for <see cref="SmileyFacePathDrawingState{TVertexAttributes,TEdgeAttributes}"/> searching state type.
/// 
/// For more info on searching state graphics aggregators see <see cref="ISearchingStateGraphicsAggregator{TSearchingState,TVertexAttributes,TEdgeAttributes}"/>.  
/// </summary>
/// <typeparam name="TVertexAttributes">Type of vertex attributes which searching state can contain and user model can use for computing.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes which searching state can contain and user model can use for computing.</typeparam>
public class SmileyFacePathDrawingStateGraphicsAggregator<TVertexAttributes, TEdgeAttributes> : 
    ISearchingStateGraphicsAggregator<SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes>,TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    public static SmileyFacePathDrawingStateGraphicsAggregator<TVertexAttributes, TEdgeAttributes> Instance { get; } = new();
    private SmileyFacePathDrawingStateGraphicsAggregator(){}
    
    /// <inheritdoc cref="ISearchingStateGraphicsAggregator{TSearchingState,TVertexAttributes,TEdgeAttributes}.AggregateGraphics"/>
    /// <remarks>
    /// <c>SmileyFacePathDrawingState</c> provides information about which parts of drawing were already drawn.  
    /// Aggregator will create graphic object for each such part and submit it into collector.   
    /// </remarks>
    public void AggregateGraphics(SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes> searchingState, IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel,
        IGraphicObjectCollector collectorForAggregatedObjects, CancellationToken? cancellationToken)
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