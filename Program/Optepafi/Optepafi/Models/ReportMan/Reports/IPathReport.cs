using Optepafi.Models.SearchingAlgorithmMan.Paths;

namespace Optepafi.Models.ReportMan.Reports;

public interface IPathReport
{
    TOut AcceptGeneric<TOut, TOtherParams>(IPathReportGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams);
    TOut AcceptGeneric<TOut>(IPathReportGenericVisitor<TOut> genericVisitor);
}