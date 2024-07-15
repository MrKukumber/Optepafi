using Optepafi.Models.ElevationDataMan.Distributions;

namespace Optepafi.ViewModels.Data.Representatives;

/// <summary>
/// Wrapping ViewModel for <c>IElevDataDistribution</c> type.
/// 
/// For more information on wrapping data view models see <see cref="WrappingDataViewModel{TData}"/>.  
/// </summary>
/// <param name="elevDataDistribution">Elevation data distribution instance to which will be this ViewModel coupled.</param>
public class ElevDataDistributionViewModel(IElevDataDistribution elevDataDistribution) : WrappingDataViewModel<IElevDataDistribution>
{
    /// <inheritdoc cref="WrappingDataViewModel{TData}.Data"/>
    protected override IElevDataDistribution Data => ElevDataDistribution;

    /// <summary>
    /// Coupled elevation data distribution instance.
    /// </summary>
    public IElevDataDistribution ElevDataDistribution { get; } = elevDataDistribution;
    public string Name => ElevDataDistribution.Name;
}

/// <summary>
/// Wrapping ViewModel for <c>ICredentialsNotRequiringElevDataDistribution</c> type.
/// 
/// This type inherits from <c>ElevDataDistributionViewModel</c>. It is mainly used for pattern-matching when requirement of credentials for accessing data is tested.  
/// For more information on wrapping data view models see <see cref="WrappingDataViewModel{TData}"/>.  
/// </summary>
public class CredentialsNotRequiringElevDataDistributionViewModel : ElevDataDistributionViewModel
{
    public new ICredentialsNotRequiringElevDataDistribution ElevDataDistribution { get; }
    public CredentialsNotRequiringElevDataDistributionViewModel(ICredentialsNotRequiringElevDataDistribution elevDataDistribution) : base(elevDataDistribution)
    {
        ElevDataDistribution = elevDataDistribution;
    }
}

/// <summary>
/// Wrapping ViewModel for <c>ICredentialsRequiringElevDataDistribution</c> type.
/// 
/// This type inherits from <c>ElevDataDistributionViewModel</c>. It is mainly used for pattern-matching when requirement of credentials for accessing data is tested.  
/// For more information on wrapping data view models see <see cref="WrappingDataViewModel{TData}"/>.  
/// </summary>
public class CredentialsRequiringElevDataDistributionViewModel : ElevDataDistributionViewModel
{

    public new ICredentialsRequiringElevDataDistribution ElevDataDistribution { get; }
    public CredentialsRequiringElevDataDistributionViewModel( ICredentialsRequiringElevDataDistribution elevDataDistribution) : base(elevDataDistribution)
    {
        ElevDataDistribution = elevDataDistribution;
    }
}