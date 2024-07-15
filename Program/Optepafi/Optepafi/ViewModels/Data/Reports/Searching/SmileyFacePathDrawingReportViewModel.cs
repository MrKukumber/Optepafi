using Optepafi.ViewModels.Data.Graphics;

namespace Optepafi.ViewModels.Data.Reports.Searching;

/// <summary>
/// ViewModel for <c>SmileyFacePathDrawingReport</c> type.
/// 
/// There should exists appropriate convertor of <c>SmileyFacePathDrawingReport</c> to this ViewModel type.  
/// For more information on searching report ViewModels see <see cref="SearchingReportViewModel"/>.  
/// </summary>
/// <param name="drawingStateGraphicsSource">Graphics of drawing state of smiley face drawer.</param>
/// <param name="lastDrawnSmileyFaceObjectInfo">Textual information about progress of drawing.</param>
public class SmileyFacePathDrawingReportViewModel(GraphicsSourceViewModel drawingStateGraphicsSource, string lastDrawnSmileyFaceObjectInfo) : SearchingReportViewModel
{
    /// <summary>
    /// Graphics of drawing state of smiley face drawer.
    /// </summary>
    public override GraphicsSourceViewModel? GraphicsSource { get; } = drawingStateGraphicsSource;
    /// <summary>
    /// Textual information about progress of drawing.
    /// </summary>
    public string LastDrawnSmileyFaceObjectInfo { get; } = lastDrawnSmileyFaceObjectInfo;
}