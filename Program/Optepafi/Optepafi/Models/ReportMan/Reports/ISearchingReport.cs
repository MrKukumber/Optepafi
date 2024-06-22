namespace Optepafi.Models.ReportMan.Reports;

/// <summary>
/// Represents report of searching state of algorithm. Searching reports are meant to bear and display information about searching state of algorithms for users.
/// Each implementation of this interface represents some particular type of report for which there should be defined some view model and template for displaying.
/// Reports are aggregated in <see cref="ReportManager"/> / <see cref="ReportSubManager{TVertexAttributes,TEdgeAttributes}"/> by use of appropriate aggregator. They are then returned, converted into view models and presented to user.
/// Report can include some graphic source for graphic reporting on some canvas.
///
/// This interface provides modification of visitor pattern, so-called "generic visitor pattern".
/// The main goal is not ensuring that caller implements specific method for every <c>ISearchingReport</c> implementation, but for ability to retrieve objects real type in form of type parameter.
/// </summary>
public interface ISearchingReport
{
    /// <summary>
    /// One of methods used in generic visitor pattern of <c>ISearchingReport</c> interface.
    /// For more information on this pattern see <see cref="ISearchingReportGenericVisitor{TOut,TOtherParams}"/>
    /// </summary>
    /// <param name="genericVisitor">Visiting visitor ;).</param>
    /// <param name="otherParams">Other parameters carried through visitor pattern.</param>
    /// <typeparam name="TOut">Specifies type of return value carried through visitor pattern.</typeparam>
    /// <typeparam name="TOtherParams">Specifies types of the rest of parameters carried through visitor pattern.</typeparam>
    /// <returns>Value of type <c>TOut</c>.</returns>
    TOut AcceptGeneric<TOut, TOtherParams>(ISearchingReportGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams);
    
    /// <summary>
    /// One of methods used in generic visitor pattern of <c>ISearchingReport</c> interface.
    /// For more information about this method see <see cref="AcceptGeneric{TOut,TOtherParams}"/>.
    /// </summary>
    TOut AcceptGeneric<TOut>(ISearchingReportGenericVisitor<TOut> genericVisitor);
}