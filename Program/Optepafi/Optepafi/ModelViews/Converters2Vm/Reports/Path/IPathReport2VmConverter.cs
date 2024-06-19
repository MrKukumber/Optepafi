using Optepafi.Models.Graphics.Sources;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.ReportMan;
using Optepafi.Models.ReportMan.Reports;
using Optepafi.ViewModels.Data.Reports;

namespace Optepafi.ModelViews.Converters2Vm.Reports.Path;

public interface IPathReport2VmConverter<in TPathReport> : IPathReport2VmConverter
    where TPathReport : IPathReport
{
    public PathReportViewModel ConvertToViewModel(TPathReport pathReport, IGroundGraphicsSource relatedMapGraphics);
}

public interface IPathReport2VmConverter
{
    
}