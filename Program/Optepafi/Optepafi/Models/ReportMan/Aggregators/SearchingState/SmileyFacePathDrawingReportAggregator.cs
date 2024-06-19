using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using DynamicData;
using Optepafi.Models.Graphics;
using Optepafi.Models.Graphics.Objects;
using Optepafi.Models.Graphics.Sources;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.ReportMan.Reports;
using Optepafi.Models.ReportMan.Reports.SearchingState;
using Optepafi.Models.ReportSubMan.Aggregators;
using Optepafi.Models.SearchingAlgorithmMan.SearchingStates;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.ModelViews.PathFinding.Utils;

namespace Optepafi.Models.ReportMan.Aggregators.SearchingState;

public class SmileyFacePathDrawingReportAggregator<TVertexAttributes, TEdgeAttributes> : ISearchingReportAggregator<SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    public static SmileyFacePathDrawingReportAggregator<TVertexAttributes, TEdgeAttributes> Instance { get; } = new();
    private SmileyFacePathDrawingReportAggregator(){}
    public ISearchingReport AggregateReport(SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes> searchingState, IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel, CancellationToken? cancellationToken = null)
    {
        CollectingGraphicsSource collectingGraphicsSource = new();
        _ = GraphicsSubManager<TVertexAttributes, TEdgeAttributes>.Instance.AggregateSearchingStateGraphics(searchingState,
            userModel, collectingGraphicsSource.Collector, cancellationToken);
        return new SmileyFacePathDrawingReport(collectingGraphicsSource,
            searchingState.LastAddedObject.drawnObject switch
            {
                SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes>.SmileyFaceObject.LeftEye =>
                    SmileyFacePathDrawingReport.SmileyFaceObject.LeftEye,
                SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes>.SmileyFaceObject.RightEye =>
                    SmileyFacePathDrawingReport.SmileyFaceObject.RightEye,
                SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes>.SmileyFaceObject.Nose =>
                    SmileyFacePathDrawingReport.SmileyFaceObject.Nose,
                SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes>.SmileyFaceObject.Mouth =>
                    SmileyFacePathDrawingReport.SmileyFaceObject.Mouth,
                _ => throw new InvalidEnumArgumentException()
            }, searchingState.LastAddedObject.associatedLegsOrder);
    }
    
    private class CollectingGraphicsSource : IGraphicsSource
    {
        public SourceList<IGraphicObject> GraphicObjects { get; } = new SourceList<IGraphicObject>();
        public IGraphicsObjectCollector Collector => new GraphicsObjectCollector(GraphicObjects);
        private class GraphicsObjectCollector : IGraphicsObjectCollector
        {
            private SourceList<IGraphicObject> _graphicObjectSourceList;
            public GraphicsObjectCollector(SourceList<IGraphicObject> graphicObjectSourceList)
            {
                _graphicObjectSourceList = graphicObjectSourceList;
            }
            public void Add<TGraphicObject>(TGraphicObject graphicObject) where TGraphicObject : IGraphicObject
            {
                _graphicObjectSourceList.Add(graphicObject);    
            }

            public void AddRange(IEnumerable<IGraphicObject> graphicObjects)
            {
                _graphicObjectSourceList.AddRange(graphicObjects);
            }
        }
    }
}