using Optepafi.Models.GraphicsMan.Sources;

namespace Optepafi.Models.ReportMan.Reports.Path;

/// <summary>
/// Represents report of path found by <c>SmileyFaceDrawer</c> algorithm.
/// 
/// It reports graphics of drawn smiley faces as well as counts of horizontally, vertically and not at all squished drawings.  
/// For more information on path reports see <see cref="IPathReport"/>.  
/// </summary>
/// <param name="PathGraphics">Graphics of drawn smiley faces that will be shown to the user.</param>
/// <param name="HorizontallySquishedFacesCount">Count of horizontally squished drawings.</param>
/// <param name="VerticallySquishedFacesCount">Count of vertically squished drawings.</param>
/// <param name="NotSquishedFacesCount">Count of drawings that are not squished at all.</param>
public record class SmileyFacePathReport(
    IGraphicsSource PathGraphics,
    int HorizontallySquishedFacesCount,
    int VerticallySquishedFacesCount,
    int NotSquishedFacesCount) : IPathReport
{
    /// <inheritdoc cref="IPathReport"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IPathReportGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) { return genericVisitor.GenericVisit(this, otherParams); } 
    /// <inheritdoc cref="IPathReport"/>
    public TOut AcceptGeneric<TOut>(IPathReportGenericVisitor<TOut> genericVisitor) { return genericVisitor.GenericVisit(this); }
}
