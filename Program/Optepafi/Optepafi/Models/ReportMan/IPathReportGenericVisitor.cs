namespace Optepafi.Models.ReportMan;

public interface IPathReportGenericVisitor<TOut, TOtherParams>
{
    TOut GenericVisit<TPathReport>(TPathReport pathReport, TOtherParams otherParams) where TPathReport : IPathReport;
}
public interface IPathReportGenericVisitor<TOut>
{
    TOut GenericVisit<TPathReport>(TPathReport pathReport) where TPathReport : IPathReport;
}