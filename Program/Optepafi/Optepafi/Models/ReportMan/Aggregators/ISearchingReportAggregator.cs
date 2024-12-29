using System.Threading;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.ReportMan.Reports;
using Optepafi.Models.SearchingAlgorithmMan.SearchingStates;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.Models.UserModelMan.UserModels.Functionalities;

namespace Optepafi.Models.ReportMan.Aggregators;

/// <summary>
/// Represents aggregator of reports for searching states of specific type. The main task of aggregator is to assemble corresponding searching report and return it.
/// 
/// Algorithms can not by default extract information from vertex and edge attributes. They are therefore able to provide searching state which contains only non processed vertex/edge attributes.  
/// For this reason searching report aggregator will also receive corresponding user model, which may be able to extract necessary information from attributes contained in searching state.  
/// The word "may" is important one. No one will ensure, that user model is able to deliver some service. Aggregator should be prepared for user model not containing some functionality and simply not include information dependent on it in aggregated report.  
/// If generated searching state report should contain some graphics that is going to be shown to user, <see cref="GraphicsSubManager{TVertexAttributes,TEdgeAttributes}"/> can be used for acquiring of appropriate graphics.  
/// </summary>
/// <typeparam name="TSearchingState">Type of searching states for which report is aggregated.</typeparam>
/// <typeparam name="TVertexAttributes">Type of vertex attributes which searching state can contain and user model can use for computing.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes which searching state can contain and user model can use for computing.</typeparam>
public interface ISearchingReportAggregator<in TSearchingState, TVertexAttributes, TEdgeAttributes> : IReportAggregator
    where TSearchingState : ISearchingState<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    /// <summary>
    /// Aggregates report of provided searching state.
    /// 
    /// It can ask inserted user model for some information extraction service on vertex/edge attributes. Although user model is not obligated to provide such service. For more on this topi see <see cref="ISearchingReportAggregator{TSearchingState,TVertexAttributes,TEdgeAttributes}"/>.  
    /// For incidental aggregation of searching states graphics for report <see cref="GraphicsSubManager{TVertexAttributes,TEdgeAttributes}"/> can be used.  
    /// </summary>
    /// <param name="searchingState">Searching state on which aggregation of report is based.</param>
    /// <param name="userModel">User model which can be asked for some information extraction.</param>
    /// <param name="cancellationToken">Cancellation token for cancelling aggregation.</param>
    /// <returns>Aggregated searching report.</returns>
    ISearchingReport AggregateReport(TSearchingState searchingState, IUserModel< ITemplate<TVertexAttributes, TEdgeAttributes>> userModel, CancellationToken? cancellationToken = null);
}