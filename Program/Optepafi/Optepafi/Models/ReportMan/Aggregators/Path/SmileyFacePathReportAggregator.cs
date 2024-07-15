using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DynamicData;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.GraphicsMan.Objects;
using Optepafi.Models.GraphicsMan.Sources;
using Optepafi.Models.ReportMan.Reports;
using Optepafi.Models.ReportMan.Reports.Path;
using Optepafi.Models.SearchingAlgorithmMan.Paths.Specific;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.ReportMan.Aggregators.Path;

/// <summary>
/// Singleton class which aggregates report for <see cref="SmileyFacePath{TVertexAttributes,TEdgeAttributes}"/> path type.
/// 
/// For more information on searching report aggregators see <see cref="IPathReportAggregator{TPath,TVertexAttributes,TEdgeAttributes}"/>.
/// </summary>
/// <typeparam name="TVertexAttributes">Type of vertex attributes which path can contain and user model can use for computing.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes which path can contain and user model can use for computing.</typeparam>
public class SmileyFacePathReportAggregator<TVertexAttributes, TEdgeAttributes> : IPathReportAggregator<SmileyFacePath<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    public static SmileyFacePathReportAggregator<TVertexAttributes, TEdgeAttributes> Instance { get; } = new();
    private SmileyFacePathReportAggregator(){}

    
    /// <inheritdoc cref="IPathReportAggregator{TPath,TVertexAttributes,TEdgeAttributes}.AggregateReport"/>
    /// <remarks>
    /// Creates collecting graphic source to which collector graphics of provided path will be collected.  
    /// Then it counts number of horizontally, vertically and not at all squished drawings.  
    /// Then it assembles all these objects into resulting report and returns it.  
    /// </remarks>
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
        foreach (var (legStart, legFinish) in path.PathSegments)
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


    /// <summary>
    /// Graphic source implementation that is able to provide collector by which its graphic objects are collected.
    /// 
    /// For more information on graphics sources see <see cref="IGraphicsSource"/>.  
    /// </summary>
    private class CollectingGraphicsSource : IGraphicsSource
    {
        /// <inheritdoc cref="IGraphicsSource.GraphicObjects"/>
        public SourceList<IGraphicObject> GraphicObjects { get; } = new SourceList<IGraphicObject>();
        /// <summary>
        /// Collector by which graphic objects of this class can be collected.
        /// </summary>
        public IGraphicObjectCollector Collector => new GraphicObjectCollector(GraphicObjects);
        
        /// <summary>
        /// Collector implementation which appends added graphic objects directly to the <c>GraphicObjects</c> source list.
        /// </summary>
        private class GraphicObjectCollector : IGraphicObjectCollector
        {
            
            /// <summary>
            /// Graphics sources source list to which added objects shall be appended.
            /// </summary>
            private SourceList<IGraphicObject> _graphicObjectSourceList;
            public GraphicObjectCollector(SourceList<IGraphicObject> graphicObjectSourceList)
            {
                _graphicObjectSourceList = graphicObjectSourceList;
            }
            
            /// <inheritdoc cref="IGraphicObjectCollector.Add"/>
            /// <remarks>
            /// Appends added objects directly to provided source list.
            /// </remarks>
            public void Add(IGraphicObject graphicObject)
            {
                _graphicObjectSourceList.Add(graphicObject);    
            }
            
            /// <inheritdoc cref="IGraphicObjectCollector.AddRange"/>
            /// <remarks>
            /// Appends added object ranges directly to provided source list.
            /// </remarks>
            public void AddRange(IEnumerable<IGraphicObject> graphicObjects)
            {
                _graphicObjectSourceList.AddRange(graphicObjects);
            }
        }
    }
}