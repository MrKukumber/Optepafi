using Optepafi.Models.SearchingAlgorithmMan.Paths;

namespace Optepafi.Models.ReportMan;

public interface IPathReport
{
    IPath ReportedPath { get; }

    TOut AcceptGeneric<TOut, TOtherParams>(IPathReportGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams);
    TOut AcceptGeneric<TOut>(IPathReportGenericVisitor<TOut> genericVisitor);
}