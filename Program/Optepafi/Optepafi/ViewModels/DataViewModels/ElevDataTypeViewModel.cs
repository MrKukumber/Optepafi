using System.Collections.Generic;
using System.Linq;
using Optepafi.Models.ElevationDataMan;

namespace Optepafi.ViewModels.DataViewModels;

public class ElevDataTypeViewModel : ViewModelBase
{
    public IElevDataType ElevDataType { get; }
    public ElevDataTypeViewModel(IElevDataType elevDataType)
    {
        ElevDataType = elevDataType;
        Name = elevDataType.Name;
        AllTopRegions = elevDataType.AllTopRegions.Select(region => new TopRegionViewModel(region));
    }
    public string Name { get; }
    public IEnumerable<TopRegionViewModel> AllTopRegions { get; }
}

public class CredentialsNotRequiringElevDataTypeViewModel : ElevDataTypeViewModel
{
    public new ICredentialsNotRequiringElevDataType ElevDataType { get; }
    public CredentialsNotRequiringElevDataTypeViewModel(ICredentialsNotRequiringElevDataType elevDataType) : base(elevDataType)
    {
        ElevDataType = elevDataType;
    }
    
}

public class CredentialsRequiringElevDataTypeViewModel : ElevDataTypeViewModel
{
    
    public new ICredentialsRequiringElevDataType ElevDataType { get; }

    public CredentialsRequiringElevDataTypeViewModel(ICredentialsRequiringElevDataType elevDataType) : base(elevDataType)
    {
        ElevDataType = elevDataType;
    }
}