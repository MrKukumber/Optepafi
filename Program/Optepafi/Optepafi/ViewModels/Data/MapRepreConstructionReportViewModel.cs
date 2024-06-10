using Optepafi.Models.MapRepreMan;

namespace Optepafi.ViewModels.DataViewModels;

public class MapRepreConstructionReportViewModel : DataViewModel<MapRepreConstructionReport>
{
    protected override MapRepreConstructionReport Data { get; }

    public MapRepreConstructionReportViewModel(MapRepreConstructionReport mapRepreConstructionReport)
    {
        Data = mapRepreConstructionReport;
    }

    public float PercentProgress => Data.PercentProgress;
}