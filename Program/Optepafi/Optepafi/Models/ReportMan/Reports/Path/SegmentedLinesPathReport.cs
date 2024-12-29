using Optepafi.Models.GraphicsMan.Sources;

namespace Optepafi.Models.ReportMan.Reports.Path;

//TODO: comment
public record class SegmentedLinesPathReport(IGraphicsSource PathGraphics) : IPathReport
{
    public TOut AcceptGeneric<TOut, TOtherParams>(IPathReportGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) 
        => genericVisitor.GenericVisit(this, otherParams);

    public TOut AcceptGeneric<TOut>(IPathReportGenericVisitor<TOut> genericVisitor)
        => genericVisitor.GenericVisit(this);
}