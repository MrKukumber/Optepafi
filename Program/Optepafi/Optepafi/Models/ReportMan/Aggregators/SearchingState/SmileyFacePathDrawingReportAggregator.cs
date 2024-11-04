using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using DynamicData;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.GraphicsMan.Collectors;
using Optepafi.Models.GraphicsMan.Objects;
using Optepafi.Models.GraphicsMan.Sources;
using Optepafi.Models.ReportMan.Reports;
using Optepafi.Models.ReportMan.Reports.SearchingState;
using Optepafi.Models.SearchingAlgorithmMan.SearchingStates.Specific;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.Models.UserModelMan.UserModels.Functionalities;

namespace Optepafi.Models.ReportMan.Aggregators.SearchingState;

/// <summary>
/// Singleton class which aggregates report for <see cref="SmileyFacePathDrawingState{TVertexAttributes,TEdgeAttributes}"/> searching state type.
/// 
/// For more information on searching report aggregators see <see cref="ISearchingReportAggregator{TSearchingState,TVertexAttributes,TEdgeAttributes}"/>.
/// </summary>
/// <typeparam name="TVertexAttributes">Type of vertex attributes which searching state can contain and user model can use for computing.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes which searching state can contain and user model can use for computing.</typeparam>
public class SmileyFacePathDrawingReportAggregator<TVertexAttributes, TEdgeAttributes> : ISearchingReportAggregator<SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    public static SmileyFacePathDrawingReportAggregator<TVertexAttributes, TEdgeAttributes> Instance { get; } = new();
    private SmileyFacePathDrawingReportAggregator(){}
    
    /// <inheritdoc cref="ISearchingReportAggregator{TSearchingState,TVertexAttributes,TEdgeAttributes}.AggregateReport"/>
    /// <remarks>
    /// Creates collecting graphic source to which collector graphics of searching state will be collected. Returns report which includes created graphics and indication of most recently drawn object.
    /// </remarks>
    public ISearchingReport AggregateReport(SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes> searchingState, IComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel, CancellationToken? cancellationToken = null)
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