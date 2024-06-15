using Optepafi.Models.ReportMan;
using Optepafi.Models.ReportMan.Aggregators;
using Optepafi.Models.SearchingAlgorithmMan.SearchingStates;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.ReportSubMan.Aggregators;

public interface ISearchingReportAggregator<in TSearchingState, TVertexAttributes, TEdgeAttributes> : IReportAggregator
    where TSearchingState : ISearchingState<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    ISearchingReport AggregateReport(TSearchingState searchingState, IComputingUserModel< ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel);
}

// public interface ISearchingReportAggregator<TSearchingState> : IReportAggregator
    // where TSearchingState : ISearchingState
// {
    // ISearchingReport AggregateReport(TSearchingState searchingState);
// }
