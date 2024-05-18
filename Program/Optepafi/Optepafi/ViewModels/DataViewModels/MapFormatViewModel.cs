using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.ViewModels.DataViewModels;

public class MapFormatViewModel : ViewModelBase
{
    public IMapFormat<IMap> MapFormat { get; }

    public MapFormatViewModel(IMapFormat<IMap> mapFormat)
    {
        MapFormat = mapFormat;
    }

    public string MapFormatName => MapFormat.MapFormatName;
    public string Extension => MapFormat.Extension;
}