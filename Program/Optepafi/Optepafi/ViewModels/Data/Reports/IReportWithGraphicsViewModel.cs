using Optepafi.ViewModels.Data.Graphics;

namespace Optepafi.ViewModels.Data.Reports;

public interface IReportWithGraphicsViewModel
{
    public GraphicsSourceViewModel? GraphicsSource { get; }
}