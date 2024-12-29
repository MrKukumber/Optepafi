using Optepafi.ViewModels.Data.Graphics;

namespace Optepafi.ViewModels.Data.Reports.Path;

//TODO: comment
public class SegmentedLinesPathReportViewModel(GraphicsSourceViewModel graphicsSource) : PathReportViewModel
{
    public override GraphicsSourceViewModel? GraphicsSource { get; } = graphicsSource;
}