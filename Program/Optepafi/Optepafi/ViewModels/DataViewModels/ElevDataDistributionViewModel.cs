using System.Collections.Generic;
using System.Linq;
using Optepafi.Models.ElevationDataMan;

namespace Optepafi.ViewModels.DataViewModels;

public class ElevDataDistributionViewModel : DataViewModel<IElevDataDistribution>
{
    protected override IElevDataDistribution Data => ElevDataDistribution;
    public IElevDataDistribution ElevDataDistribution { get; }
    public ElevDataDistributionViewModel(IElevDataDistribution elevDataDistribution)
    {
        ElevDataDistribution = elevDataDistribution;
        Name = elevDataDistribution.Name;
        AllTopRegions = elevDataDistribution.AllTopRegions.Select(region => new TopRegionViewModel(region));
    }
    public string Name { get; }
    public IEnumerable<TopRegionViewModel> AllTopRegions { get; }
}

public class CredentialsNotRequiringElevDataDistributionViewModel : ElevDataDistributionViewModel
{
    
    protected override IElevDataDistribution Data => ElevDataDistribution;
    public new ICredentialsNotRequiringElevDataDistribution ElevDataDistribution { get; }
    public CredentialsNotRequiringElevDataDistributionViewModel(ICredentialsNotRequiringElevDataDistribution elevDataDistribution) : base(elevDataDistribution)
    {
        ElevDataDistribution = elevDataDistribution;
    }
}

public class CredentialsRequiringElevDataDistributionViewModel : ElevDataDistributionViewModel
{

    protected override IElevDataDistribution Data => ElevDataDistribution;
    public new ICredentialsRequiringElevDataDistribution ElevDataDistribution { get; }

    public CredentialsRequiringElevDataDistributionViewModel(
        ICredentialsRequiringElevDataDistribution elevDataDistribution) : base(elevDataDistribution)
    {
        ElevDataDistribution = elevDataDistribution;
    }
}