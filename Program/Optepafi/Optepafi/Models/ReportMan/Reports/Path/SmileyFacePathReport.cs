using Optepafi.Models.Graphics.Sources;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.SearchingAlgorithmMan.Paths;

namespace Optepafi.Models.ReportMan.Reports.Path;

public record class SmileyFacePathReport(
    IGraphicsSource PathGraphics,
    int HorizontallySquishedFacesCount,
    int VerticallySquishedFacesCount,
    int NotSquishedFacesCount) : IPathReport
{
    public TOut AcceptGeneric<TOut, TOtherParams>(IPathReportGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) { return genericVisitor.GenericVisit(this, otherParams); } 
    public TOut AcceptGeneric<TOut>(IPathReportGenericVisitor<TOut> genericVisitor) { return genericVisitor.GenericVisit(this); }
}
