using Optepafi.Models.MapRepreMan;
using Optepafi.ViewModels.DataViewModels;
using ReactiveUI;

namespace Optepafi.ViewModels.Data.Reports;

/// <summary>
/// ViewModel for <c>MapRepreConstructionReport</c> type.
/// It contains percent progress of map construction.
/// For more information on data view models see <see cref="DataViewModel"/>.
/// </summary>
public class MapRepreConstructionReportViewModel(MapRepreConstructionReport mapRepreConstructionReport) : DataViewModel
{
    /// <summary>
    /// Percent progress of map representation creation.
    /// </summary>
    public float PercentProgress { get; } = mapRepreConstructionReport.PercentProgress;
}