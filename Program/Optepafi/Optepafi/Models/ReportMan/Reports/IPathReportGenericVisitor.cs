namespace Optepafi.Models.ReportMan.Reports;

/// <summary>
/// One of generic visitor interfaces for <see cref="IPathReport"/> implementations. It provides access to modified visitor pattern on path reports, where only one generic method is required to be implemented.
/// It serves mainly for acquiring generic parameter, that represents real type of visited object.
/// It has one more overload for convenience of use.
/// </summary>
/// <typeparam name="TOut">Specifies type of returned value of <c>GenericVisit</c>.</typeparam>
/// <typeparam name="TOtherParams">Specifies types of rest of the parameters carried through visitor pattern.</typeparam>
public interface IPathReportGenericVisitor<TOut, TOtherParams>
{
    /// <summary>
    /// Visiting method to be implemented.
    /// </summary>
    /// <param name="pathReport">Path report which accepted the visit.</param>
    /// <param name="otherParams">Other parameters carried through visitor pattern.</param>
    /// <typeparam name="TPathReport">Type of accepting path report. Main result of this modified visitor pattern.</typeparam>
    /// <returns>Value of type <c>TOut</c>.</returns>
    TOut GenericVisit<TPathReport>(TPathReport pathReport, TOtherParams otherParams) where TPathReport : IPathReport;
}

/// <summary>
/// One of generic visitor interfaces for <see cref="IPathReport"/> implementations. It provides access to modified visitor pattern on path reports.
/// For more information see <see cref="IPathReportGenericVisitor{TOut,TOtherParams}"/> .
/// </summary>
public interface IPathReportGenericVisitor<TOut>
{
    /// <summary>
    /// Visiting method to be implemented.
    /// For more information of this method see <see cref="IPathReportGenericVisitor{TOut,TOtherParams}"/>.
    /// </summary>
    TOut GenericVisit<TPathReport>(TPathReport pathReport) where TPathReport : IPathReport;
}