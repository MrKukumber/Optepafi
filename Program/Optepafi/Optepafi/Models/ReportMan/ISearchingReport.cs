using Optepafi.Models.SearchingAlgorithmMan.SearchingStates;

namespace Optepafi.Models.ReportMan;

/// <summary>
/// Enables to track progress of searching for path. Each algorithm can have its own implementation of this report.
/// Logic of the application can then support these implementations and let user see real time progress of searching.
/// TODO: upravit
/// </summary>
public interface ISearchingReport
{
    TOut AcceptGeneric<TOut, TOtherParams>(ISearchingReportGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams);
    TOut AcceptGeneric<TOut>(ISearchingReportGenericVisitor<TOut> genericVisitor);
}