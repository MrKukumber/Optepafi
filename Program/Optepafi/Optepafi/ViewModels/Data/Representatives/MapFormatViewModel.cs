using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.ViewModels.DataViewModels;

namespace Optepafi.ViewModels.Data.Representatives;

/// <summary>
/// Wrapping ViewModel for <c>IMapFormat{IMap}</c> type.
/// For more information on wrapping data view models see <see cref="WrappingDataViewModel{TData}"/>.
/// </summary>
/// <param name="mapFormat">Map format instance to which will be this ViewModel coupled.</param>
public class MapFormatViewModel(IMapFormat<IMap> mapFormat) : WrappingDataViewModel<IMapFormat<IMap>>
{
    
    /// <inheritdoc cref="WrappingDataViewModel{TData}.Data"/>
    protected override IMapFormat<IMap> Data => MapFormat;
    /// <summary>
    /// Coupled map format instance.
    /// </summary>
    public IMapFormat<IMap> MapFormat { get; } = mapFormat;
    
    public string MapFormatName => MapFormat.MapFormatName;
    /// <summary>
    /// File extension of represented maps format.
    /// </summary>
    public string Extension => MapFormat.Extension;
}