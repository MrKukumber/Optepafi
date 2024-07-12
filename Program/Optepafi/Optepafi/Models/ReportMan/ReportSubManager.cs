using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Optepafi.Models.ReportMan.Aggregators;
using Optepafi.Models.ReportMan.Aggregators.Path;
using Optepafi.Models.ReportMan.Aggregators.SearchingState;
using Optepafi.Models.ReportMan.Reports;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.Models.SearchingAlgorithmMan.SearchingStates;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.ReportMan;

/// <summary>
/// Singleton class used for sub-managing of reports aggregation. It is main channel between report aggregation mechanisms and other managers/constructs of Model.
/// It provides supporting methods for correct calling of report aggregators.
/// Its functionality is similar to <see cref="ReportManager"/> singleton class which is intended for user from applications logic (ModelViews/ViewModels).
/// Main difference in way of use of these classes is that, this class has two generic parameters (representing used vertex and edge attributes in aggregation of report) and in provided methods request for specific properties of inserted object types.
/// This is the reason why this class is intended for use from Model constructs. It is property of these constructs they know functionality of their data already. So the check for their functionality again would be redundant. They also work with specific type of vertex/edge attributes so they are able to provide them.
///
/// For more information about process of reports aggregation see <see cref="ReportManager"/>.
/// </summary>
/// <typeparam name="TVertexAttributes">Type of vertex attributes used in aggregation.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes used in aggregation.</typeparam>
public class ReportSubManager<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    public static ReportSubManager<TVertexAttributes, TEdgeAttributes> Instance { get; } = new();
    private ReportSubManager(){}
    
    /// <summary>
    /// Collection of aggregators for specific path types. It is searched through when path report is to be aggregated.
    /// </summary>
    public ISet<IReportAggregator> PathReportAggregators { get; } =
        ImmutableHashSet.Create<IReportAggregator>(SmileyFacePathReportAggregator<TVertexAttributes,TEdgeAttributes>.Instance);

    /// <summary>
    /// Collection of aggregators for specific searching state types. It is searched through when searching state report is to be aggregated.
    /// </summary>
    public ISet<IReportAggregator> SearchingReportAggregators { get; } =
        ImmutableHashSet.Create<IReportAggregator>(SmileyFacePathDrawingReportAggregator<TVertexAttributes, TEdgeAttributes>.Instance);
    
    /// <summary>
    /// Method for aggregating of path reports. It accepts path which aggregation is based on and also requests user model that can be used for computing of some values for aggregator.
    /// No specific functionality is forced upon provided user model. More about usage of user models in <see cref="IPathReportAggregator{TPath,TVertexAttributes,TEdgeAttributes}"/>.
    /// It runs through <c>PathReportAggregators</c> and looks for appropriate path report aggregator by pattern-matching their generic parameter <c>TPath</c> with type of provided path.
    /// When such aggregator is found, its aggregating method is called.
    /// </summary>
    /// <param name="path">Path which report is to be aggregated.</param>
    /// <param name="userModel">User model used in process of aggregation.</param>
    /// <param name="cancellationToken">Cancellation token for cancelling aggregation.</param>
    /// <typeparam name="TPath">Type of path which report is to be aggregated. It is used for finding appropriate aggregator.</typeparam>
    /// <returns>Aggregated path report. Null, if no suitable aggregator is found.</returns>
    public IPathReport? AggregatePathReport<TPath>(
        TPath path, IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel, CancellationToken? cancellationToken)
        where TPath : IPath<TVertexAttributes, TEdgeAttributes> 
    {
        foreach (var pathReportAggregator in PathReportAggregators)
        {
            if (pathReportAggregator is IPathReportAggregator<TPath, TVertexAttributes, TEdgeAttributes> pra)
            {
                return pra.AggregateReport(path, userModel, cancellationToken);
            }
        }
        return null;
    }
    
    /// <summary>
    /// Method for aggregating of searching reports. It accepts searching state which aggregation is based on and also requests user model that can be used for computing of some values for aggregator.
    /// No specific functionality is forced upon provided user model. More about usage of user models in <see cref="IPathReportAggregator{TPath,TVertexAttributes,TEdgeAttributes}"/>.
    /// It runs through <c>SearchingReportAggregators</c> and looks for appropriate searching report aggregator by pattern-matching their generic parameter <c>TSearchingState</c> with type of provided searching state.
    /// When such aggregator is found, its aggregating method is called.
    /// </summary>
    /// <param name="searchingState">Searching state which report is to be aggregated.</param>
    /// <param name="userModel">User model that can be used in process of aggregation.</param>
    /// <typeparam name="TSearchingState">Type of searching state which report is to be aggregated. It is used for finding appropriate aggregator.</typeparam>
    /// <returns>Aggregated searching state report. Null, if no suitable aggregator is found.</returns>
    public ISearchingReport? AggregateSearchingReport<TSearchingState>(
        TSearchingState searchingState, IComputingUserModel<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel)
        where TSearchingState : ISearchingState<TVertexAttributes, TEdgeAttributes> 
    {
        foreach (var searchingReportAggregator in SearchingReportAggregators)
        {
            if (searchingReportAggregator is ISearchingReportAggregator<TSearchingState, TVertexAttributes, TEdgeAttributes> sra)
            {
                return sra.AggregateReport(searchingState, userModel);
            }
        }
        return null;
    }
}

// public class ReportSubManager
// {
    // public static ReportSubManager Instance { get; } = new();
    // private ReportSubManager(){}

    // public ISet<IReportAggregator> PathReportAggregators { get; } =
        // ImmutableHashSet.Create<IReportAggregator>();

    // public ISet<IReportAggregator> SearchingReportAggregators { get; } =
        // ImmutableHashSet.Create<IReportAggregator>();

    // public IPathReport? CreatePathReport<TPath>(TPath path)
        // where TPath : IPath
    // {
        // foreach (var pathReportAggregator in PathReportAggregators)
        // {
            // if (pathReportAggregator is IPathReportAggregator<TPath> pra)
            // {
                // return pra.AggregateReport(path);
            // }
        // }
        // return null;
    // }


    // public ISearchingReport? CreateSearchingReport<TSearchingState>(TSearchingState searchingState)
        // where TSearchingState : ISearchingState
    // {
        // foreach (var searchingReportAggregator in SearchingReportAggregators)
        // {
            // if (searchingReportAggregator is ISearchingReportAggregator<TSearchingState> sra)
            // {
                // return sra.AggregateReport(searchingState);
            // }
        // }
        // return null;
    // }
    
// }