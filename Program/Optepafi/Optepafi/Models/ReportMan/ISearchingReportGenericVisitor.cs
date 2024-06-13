namespace Optepafi.Models.ReportMan;


public interface ISearchingReportGenericVisitor<TOut, TOtherParams>
{
    TOut GenericVisit<TSearchingReport>(TSearchingReport searchingReport, TOtherParams otherParams) where TSearchingReport : ISearchingReport;
}
public interface ISearchingReportGenericVisitor<TOut>
{
    TOut GenericVisit<TSearchingReport>(TSearchingReport searchingReport) where TSearchingReport : ISearchingReport;
}