using Optepafi.Models.Graphics.Sources;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.ReportMan;
using Optepafi.Models.ReportMan.Reports;
using Optepafi.ViewModels.Data.Reports;

namespace Optepafi.ModelViews.Converters2Vm.Reports.Searching;

public interface ISearchingReport2VmConverter<in TSearchingReport> : ISearchingReport2VmConverter
    where TSearchingReport : ISearchingReport
{
    public SearchingReportViewModel ConvertToViewModel(TSearchingReport searchingReport, IGroundGraphicsSource relatedMapGraphics);
}

public interface ISearchingReport2VmConverter;
