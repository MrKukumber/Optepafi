using Optepafi.Models.MapRepreMan;
using Optepafi.ViewModels.DataViewModels;
using ReactiveUI;

namespace Optepafi.ViewModels.Data.Reports;

public class MapRepreConstructionReportViewModel : ReactiveObject
{

    public MapRepreConstructionReportViewModel(MapRepreConstructionReport mapRepreConstructionReport)
    {
        PercentProgress = mapRepreConstructionReport.PercentProgress;
    }

    public float PercentProgress { get; }
}