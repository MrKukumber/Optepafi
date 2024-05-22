using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.ViewModels.DataViewModels;

public class MapRepresentativeViewModel : ViewModelBase
{
    public IMapFormat<IMap> MapFormat { get; }

    public MapRepresentativeViewModel(IMapFormat<IMap> mapFormat)
    {
        MapFormat = mapFormat;
    }

    public string MapFormatName => MapFormat.MapFormatName;
    public string Extension => MapFormat.Extension;
}