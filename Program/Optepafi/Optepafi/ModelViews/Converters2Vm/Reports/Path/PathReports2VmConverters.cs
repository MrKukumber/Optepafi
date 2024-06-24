using System;
using System.Collections.Generic;
using Optepafi.Models.Graphics.Sources;
using Optepafi.Models.ReportMan.Reports.Path;
using Optepafi.ViewModels.Data.Graphics;
using Optepafi.ViewModels.Data.Reports;
using Optepafi.ViewModels.Data.Reports.Path;

namespace Optepafi.ModelViews.Converters2Vm.Reports.Path;


/// <summary>
/// Static class which contains dictionary of path report to ViewModel converters. It should contain all such converters.
/// Application logic works directly with this dictionary when it searches for appropriate converter.
/// </summary>
public static class PathReports2VmConverters
{
    /// <summary>
    /// Dictionary of path report to ViewModel converters.
    /// </summary>
    public static Dictionary<Type, IPathReport2VmConverter> Converters { get; } = new()
    {
        [typeof(SmileyFacePathReport)] = SmileyFacePathReport2VmConverter.Instance
    };
}


/// <summary>
/// Converter of smiley face path report to ViewModel.
/// It is included in dictionary of path report to ViewModel converters in <see cref="PathReports2VmConverters"/>.
/// </summary>
public class SmileyFacePathReport2VmConverter : IPathReport2VmConverter<SmileyFacePathReport>
{
    public static SmileyFacePathReport2VmConverter Instance { get; } = new();
    private SmileyFacePathReport2VmConverter() { }
    /// <inheritdoc cref="IPathReport2VmConverter{TPathReport}.ConvertToViewModel"/>
    public PathReportViewModel ConvertToViewModel(SmileyFacePathReport pathReport, IGroundGraphicsSource relatedMapGraphics)
    {
        GraphicsSourceViewModel pathGraphicsSourceViewModel = new GraphicsSourceViewModel(pathReport.PathGraphics, relatedMapGraphics);
        string? horizontallySquishedFacesCountInfo = pathReport.HorizontallySquishedFacesCount == 0
                ? null
                : pathReport.HorizontallySquishedFacesCount == 1 
                    ? $"There is {pathReport.HorizontallySquishedFacesCount} horizontally squished smiley face." //TODO: Localize  
                    : $"There are {pathReport.HorizontallySquishedFacesCount} horizontally squished smiley faces." ; //TODO: Localize 
        string? verticallySquishedFacesCountInfo = pathReport.VerticallySquishedFacesCount == 0
                ? null
                : pathReport.VerticallySquishedFacesCount == 1
                    ? $"There is {pathReport.VerticallySquishedFacesCount} vertically squished smiley face." //TODO: Localize 
                    : $"There are {pathReport.VerticallySquishedFacesCount} vertically squished smiley faces." ; //TODO: Localize 
        string? notSquishedFacesCountInfo = pathReport.NotSquishedFacesCount == 0
                ? null
                : pathReport.NotSquishedFacesCount == 1
                    ? $"There is {pathReport.HorizontallySquishedFacesCount} normally looking smiley face." //TODO: Localize 
                    : $"There are {pathReport.HorizontallySquishedFacesCount} normally looking smiley faces." ; //TODO: Localize 

        return new SmileyFacePathReportViewModel(pathGraphicsSourceViewModel, horizontallySquishedFacesCountInfo, verticallySquishedFacesCountInfo, notSquishedFacesCountInfo);
    }
}