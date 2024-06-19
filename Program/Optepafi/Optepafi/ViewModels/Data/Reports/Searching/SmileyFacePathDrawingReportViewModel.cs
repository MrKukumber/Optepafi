using Optepafi.ViewModels.Data.Graphics;

namespace Optepafi.ViewModels.Data.Reports.Searching;

public class SmileyFacePathDrawingReportViewModel(GraphicsSourceViewModel drawingStateGraphicsSource, string lastDrawnSmileyFaceObjectInfo) : SearchingReportViewModel
{
    public override GraphicsSourceViewModel? GraphicsSource { get; } = drawingStateGraphicsSource;
    public string LastDrawnSmileyFaceObjectInfo { get; } = lastDrawnSmileyFaceObjectInfo;
}