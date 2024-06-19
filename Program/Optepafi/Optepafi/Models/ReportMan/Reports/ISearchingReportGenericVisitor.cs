namespace Optepafi.Models.ReportMan.Reports;


public interface ISearchingReportGenericVisitor<TOut, TOtherParams>
{
    TOut GenericVisit<TSearchingReport>(TSearchingReport searchingReport, TOtherParams otherParams) where TSearchingReport : ISearchingReport;
}
public interface ISearchingReportGenericVisitor<TOut>
{
    TOut GenericVisit<TSearchingReport>(TSearchingReport searchingReport) where TSearchingReport : ISearchingReport;
}