using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DynamicData;
using Optepafi.Models.Graphics;
using Optepafi.Models.Graphics.Objects;
using Optepafi.Models.Graphics.Sources;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.ReportMan.Reports;
using Optepafi.Models.ReportMan.Reports.Path;
using Optepafi.Models.ReportSubMan.Aggregators;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.ReportMan.Aggregators.Path;

public class SmileyFacePathReportAggregator<TVertexAttributes, TEdgeAttributes> : IPathReportAggregator<SmileyFacePath<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    public static SmileyFacePathReportAggregator<TVertexAttributes, TEdgeAttributes> Instance { get; } = new();
    private SmileyFacePathReportAggregator(){}

    public IPathReport AggregateReport(SmileyFacePath<TVertexAttributes, TEdgeAttributes> path, 
        IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel,
        CancellationToken? cancellationToken = null)
    {
        CollectingGraphicsSource collectingGraphicsSource = new();
        var graphicsAggregationTask = Task.Run(() =>
            GraphicsSubManager<TVertexAttributes, TEdgeAttributes>.Instance.AggregatePathGraphics(path, userModel,
                collectingGraphicsSource.Collector, cancellationToken));
        int horizontallySquishedFacesCount = 0;
        int verticallySquishedFacesCount = 0;
        int notSquishedFacesCount = 0;
        foreach (var (legStart, legFinish) in path.Path)
        {
            float width = int.Abs(legFinish.XPos - legStart.XPos);
            float height = int.Abs(legFinish.YPos - legStart.YPos);
            if ( width/height > 1.05)
                verticallySquishedFacesCount++;
            else if (width/height < 0.95)
                horizontallySquishedFacesCount++;
            else notSquishedFacesCount++;
        }
        _ = graphicsAggregationTask.Result;
        return new SmileyFacePathReport(collectingGraphicsSource, horizontallySquishedFacesCount, verticallySquishedFacesCount, notSquishedFacesCount);
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