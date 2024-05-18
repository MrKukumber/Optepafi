using System.Collections.Generic;
using System.Linq;
using Optepafi.Models.ElevationDataMan;

namespace Optepafi.ViewModels.DataViewModels;

public class ElevDataSourceViewModel : ViewModelBase
{
    public IElevDataSource ElevDataSource { get; }
    public ElevDataSourceViewModel(IElevDataSource elevDataSource)
    {
        ElevDataSource = elevDataSource;
    }

    public string Name => ElevDataSource.Name;

    public IEnumerable<ElevDataTypeViewModel> ElevDataTypes => ElevDataSource.ElevDataTypesInSource
        .SelectMany<IElevDataType, ElevDataTypeViewModel>(elevDataType => elevDataType switch
        {
            ICredentialsNotRequiringElevDataType cnredt => [new CredentialsNotRequiringElevDataTypeViewModel(cnredt)],
            ICredentialsRequiringElevDataType credt => [new CredentialsRequiringElevDataTypeViewModel(credt)],
            _ => []
        });
}