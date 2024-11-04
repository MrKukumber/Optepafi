using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.MapRepreMan.MapRepres.Representatives;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Configurations;

namespace Optepafi.ViewModels.Data.Representatives;

    //TODO: comment
public class MapRepreRepresentativeViewModel(IMapRepreRepresentative<IMapRepre> mapRepreRepresentative) : WrappingDataViewModel<IMapRepreRepresentative<IMapRepre>>
{

    /// <inheritdoc cref="WrappingDataViewModel{TData}.Data"/>
    protected override IMapRepreRepresentative<IMapRepre> Data => MapRepreRepresentative;

    /// <summary>
    /// Coupled map representation representative instance.
    /// </summary>
    public IMapRepreRepresentative<IMapRepre> MapRepreRepresentative { get; } = mapRepreRepresentative;

    /// <summary>
    /// Name of represented map representation.
    /// </summary>
    public string MapRepreName => MapRepreRepresentative.MapRepreName;

    /// <summary>
    /// Default configuration of represented map representation.
    /// </summary>
    public IConfiguration DefaultConfigurationCopy => MapRepreRepresentative.DefaultConfigurationDeepCopy;
}