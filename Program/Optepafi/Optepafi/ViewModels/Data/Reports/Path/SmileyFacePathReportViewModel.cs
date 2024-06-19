using Optepafi.Models.Graphics.Sources;
using Optepafi.Models.ReportMan.Reports.Path;
using Optepafi.ViewModels.Data.Graphics;

namespace Optepafi.ViewModels.Data.Reports.Path;

public class SmileyFacePathReportViewModel(GraphicsSourceViewModel pathGraphicsSouce, string? horizontallySquishedFacesCountInfo, string? verticallySquishedFacesCountInfo, string? notSquishedFacesCountInfo) : PathReportViewModel
{
    public override GraphicsSourceViewModel GraphicsSource { get; } = pathGraphicsSouce;
    public string? HorizontallySquishedFacesCountInfo { get; } = horizontallySquishedFacesCountInfo;
    public string? VerticallySquishedFacesCountInfo { get; } = verticallySquishedFacesCountInfo;
    public string? NotSquishedFacesCountInfo { get; } = notSquishedFacesCountInfo;
}