using Optepafi.Models.Graphics.Sources;
using Optepafi.Models.SearchingAlgorithmMan.SearchingStates;

namespace Optepafi.Models.ReportMan.Reports.SearchingState;

public record class SmileyFacePathDrawingReport(
    IGraphicsSource DrawingStateGraphics,
    SmileyFacePathDrawingReport.SmileyFaceObject LastDrawnSmileyFaceObject,
    int LastDrawnSmileyFaceObjectsAssociatedLegOrder) : ISearchingReport
{
    public enum SmileyFaceObject {LeftEye, RightEye, Nose, Mouth}
    public TOut AcceptGeneric<TOut, TOtherParams>(ISearchingReportGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) { return genericVisitor.GenericVisit(this, otherParams); } 
    public TOut AcceptGeneric<TOut>(ISearchingReportGenericVisitor<TOut> genericVisitor) { return genericVisitor.GenericVisit(this); }
}