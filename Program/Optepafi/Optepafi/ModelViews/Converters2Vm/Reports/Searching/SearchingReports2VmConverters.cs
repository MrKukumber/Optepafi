using System;
using System.Collections.Generic;
using Optepafi.Models.Graphics.Sources;
using Optepafi.Models.ReportMan.Reports.SearchingState;
using Optepafi.ViewModels.Data.Graphics;
using Optepafi.ViewModels.Data.Reports;
using Optepafi.ViewModels.Data.Reports.Searching;

namespace Optepafi.ModelViews.Converters2Vm.Reports.Searching;

public static class SearchingReports2VmConverters
{
    public static Dictionary<Type, ISearchingReport2VmConverter> Converters { get; } = new()
    {
        [typeof(SmileyFacePathDrawingReport)] = SmileyFacePathDrawingReport2VmConverter.Instance
    };
}

public class SmileyFacePathDrawingReport2VmConverter : ISearchingReport2VmConverter<SmileyFacePathDrawingReport>
{
    public static SmileyFacePathDrawingReport2VmConverter Instance { get; } = new();
    private SmileyFacePathDrawingReport2VmConverter() { }

    public SearchingReportViewModel ConvertToViewModel(SmileyFacePathDrawingReport searchingReport, IGroundGraphicsSource relatedMapGraphics)
    {
        GraphicsSourceViewModel drawingStateGraphicsSourceViewModel = new GraphicsSourceViewModel(searchingReport.DrawingStateGraphics, relatedMapGraphics);
        string lastDrawnObjectInfo = searchingReport.LastDrawnSmileyFaceObject switch
        {
            SmileyFacePathDrawingReport.SmileyFaceObject.LeftEye =>
                $"Left eye of {searchingReport.LastDrawnSmileyFaceObjectsAssociatedLegOrder}. smiley face drawn.", //TODO: Localize
            SmileyFacePathDrawingReport.SmileyFaceObject.RightEye =>
                $"Right eye of {searchingReport.LastDrawnSmileyFaceObjectsAssociatedLegOrder}. smiley face drawn.", //TODO: Localize
            SmileyFacePathDrawingReport.SmileyFaceObject.Nose =>
                $"Nose of {searchingReport.LastDrawnSmileyFaceObjectsAssociatedLegOrder}. smiley face drawn.", //TODO: Localize
            SmileyFacePathDrawingReport.SmileyFaceObject.Mouth =>
                $"Mouth of {searchingReport.LastDrawnSmileyFaceObjectsAssociatedLegOrder}. smiley face drawn.", //TODO: Localize
            _ => ""
        };
        return new SmileyFacePathDrawingReportViewModel(drawingStateGraphicsSourceViewModel, lastDrawnObjectInfo);
    }
}