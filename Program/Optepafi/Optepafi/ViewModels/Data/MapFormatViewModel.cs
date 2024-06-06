using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.ViewModels.DataViewModels;

public class MapFormatViewModel : DataViewModel<IMapFormat<IMap>>
{
    protected override IMapFormat<IMap> Data => MapFormat;
    public IMapFormat<IMap> MapFormat { get; }

    public MapFormatViewModel(IMapFormat<IMap> mapFormat)
    {
        MapFormat = mapFormat;
    }

    public string MapFormatName => MapFormat.MapFormatName;
    public string Extension => MapFormat.Extension;
}