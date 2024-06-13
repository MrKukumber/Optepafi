using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Optepafi.Models.ReportMan.Aggregators;
using Optepafi.Models.ReportSubMan.Aggregators;
using Optepafi.Models.SearchingAlgorithmMan.Paths;
using Optepafi.Models.SearchingAlgorithmMan.SearchingStates;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.ReportMan;

public class ReportSubManager<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    public static ReportSubManager<TVertexAttributes, TEdgeAttributes> Instance { get; } = new();
    private ReportSubManager(){}
    
    public ISet<IReportAggregator> PathReportAggregators { get; } =
        ImmutableHashSet.Create<IReportAggregator>();

    public ISet<IReportAggregator> SearchingReportAggregators { get; } =
        ImmutableHashSet.Create<IReportAggregator>();
    
    
    public IPathReport? AggregatePathReport<TPath>(
        TPath path, IUsableUserModel<TVertexAttributes, TEdgeAttributes> userModel, CancellationToken? cancellationToken)
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
    
    public ISearchingReport? AggregateSearchingReport<TSearchingState>(
        TSearchingState searchingState, IUsableUserModel<TVertexAttributes, TEdgeAttributes> userModel)
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