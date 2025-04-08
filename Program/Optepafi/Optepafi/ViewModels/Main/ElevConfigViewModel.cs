using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.ElevationDataMan.Distributions;
using Optepafi.Models.ElevationDataMan.Distributions.Specific.USGS;
using Optepafi.ModelViews.Main;
using Optepafi.ViewModels.Data.Credentials;
using Optepafi.ViewModels.Data.Representatives;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

/// <summary>
/// ViewModel which is responsible for control over elevation data configuration mechanism.
/// 
/// This is a special type of ViewModel because all instances of this ViewModel use the same one instance of <c>ElevDataModelView</c> singleton.  
/// It also is special in that way it is designed and intended to be used in dialog windows.  
/// Singleton design of mentioned ModelView is intentional so instance of this ModelView could be used in application on more places at once.  
/// It opens opportunity for having elevation configuration ViewModels opened simultaneously on multiple places. ModelView ensures that this simultaneous opening will run flawlessly.  
/// Among tasks of this ViewModel belongs:
/// 
/// - overseeing of elevation data configuration. It secures validity of actions done by user. It reacts to results of data machinations by processing and providing meaningful information to be shown to user.
/// - understanding principles of elevation data retrieval and removal. Understanding hierarchy of regions.
/// - selection of currently used elevation data distribution.
/// - letting user to assign its credentials for accessing of elevation data from distributions which demand it.
///
/// For more information on ViewModels in general see <see cref="ViewModelBase"/>.  
/// </summary>
public class ElevConfigViewModel : ViewModelBase
{
    /// <summary>
    /// Construction of new elevation configuration ViewModel instance.
    /// 
    /// It initialize all reactive constructs and assigns currently used elevation data distribution to value of provided argument.  
    /// </summary>
    /// <param name="selectedElevDataDist">Argument according to which is currently used elevation data distribution set.</param>
    public ElevConfigViewModel(ElevDataDistributionViewModel? selectedElevDataDist)
    {
        
        _currentAvailableRegions = this.WhenAnyValue(x => x.CurrentElevDataDist)
            .Select(ceds => ceds is null ? null : ElevDataModelView.Instance.TopRegionsOfAllDistributions[ceds] 
                .SelectMany(topRegion => GetAllSubRegions(topRegion))) 
            .ToProperty(this, nameof(CurrentAvailableRegions));
        _requriedCredentialsTypeIsUserNameAndPassword = this.WhenAnyValue(x => x.CurrentElevDataDist)
            .Select(elevDataDist => elevDataDist is CredentialsRequiringElevDataDistributionViewModel credReqElevDataDist && credReqElevDataDist.CredType == CredentialsTypeViewModel.UserNameAndPassword)
            .ToProperty(this, nameof(RequiredCredentialsTypeIsUserNameAndPassword));
        _requiredCredentialsTypeIsAuthenticationToken = this.WhenAnyValue(x => x.CurrentElevDataDist)
            .Select(elevDataDist => elevDataDist is CredentialsRequiringElevDataDistributionViewModel credReqElevDataDist && credReqElevDataDist.CredType == CredentialsTypeViewModel.AuthenticationToken)
            .ToProperty(this, nameof(RequiredCredentialsTypeIsAuthenticationToken));
        _requiredCredentialsTypeIsUserNameAndAuthenticationToken = this.WhenAnyValue(x => x.CurrentElevDataDist)
            .Select(elevDataDist => elevDataDist is CredentialsRequiringElevDataDistributionViewModel credReqElevDataDist && credReqElevDataDist.CredType == CredentialsTypeViewModel.UserNameAndAuthenticationToken)
            .ToProperty(this, nameof(RequiredCredentialsTypeIsUserNameAndAuthenticationToken));
        _isRegionSelectedNotDownloaded = this.WhenAnyValue(x => x.SelectedRegion.Presence)
            .Select(presence => presence is RegionViewModel.PresenceState.NotDownloaded)
            .ToProperty(this, nameof(IsSelectedRegionNotDownloaded));
        
        IObservable<bool> isRegionSelectedNotDownloaded = this.WhenAnyValue(
            x => x.SelectedRegion.Presence,
            region => region == RegionViewModel.PresenceState.NotDownloaded);
        IObservable<bool> isRegionSelectedDownloaded = this.WhenAnyValue(
            x => x.SelectedRegion.Presence,
            presence => presence == RegionViewModel.PresenceState.Downloaded);
        IObservable<bool> isRegionSelectedDownloading = this.WhenAnyValue(
            x => x.SelectedRegion.Presence,
            presence => presence == RegionViewModel.PresenceState.IsDownloading);
        IObservable<bool> areCredentialsSet = this.WhenAnyValue(
            x => x.UserName,
            x => x.Password,
            x => x.AuthenticationToken,
            (userName, password, authToken) =>
            {
                if (RequiredCredentialsTypeIsUserNameAndPassword)
                    return !string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password);
                if (RequiredCredentialsTypeIsUserNameAndAuthenticationToken)
                    return !string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(authToken);
                if (RequiredCredentialsTypeIsAuthenticationToken)
                    return !string.IsNullOrEmpty(authToken);
                return false;
            });
        IObservable<bool> areCredentialsRequired = this.WhenAnyValue(
            x => x.CurrentElevDataDist,
            (ElevDataDistributionViewModel? elevDataDist) => elevDataDist is CredentialsRequiringElevDataDistributionViewModel);

        this.WhenAnyValue(x => x.CurrentElevDataDist)
            .Subscribe(_ => { UserName = ""; Password = ""; AuthenticationToken = ""; }); 
            
        DownloadRegionCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var currentSelectedRegion = SelectedRegion;
            var currentElevDataDist = CurrentElevDataDist;
            currentSelectedRegion!.Presence = RegionViewModel.PresenceState.IsDownloading;
            foreach(var subRegion in currentSelectedRegion.SubRegions ) 
                SetRecursivelySubRegionsPresenceToIsDownloadingAsSubregion(subRegion);
            currentSelectedRegion.DownloadingCancellationTokenSource = new CancellationTokenSource();
            switch (currentElevDataDist)
            {
                case CredentialsNotRequiringElevDataDistributionViewModel cnredtvm :
                   return (currentSelectedRegion, ElevDataModelView.Instance.DownloadAsync(cnredtvm, currentSelectedRegion));
                case CredentialsRequiringElevDataDistributionViewModel credtvm :
                    switch (credtvm.CredType)
                    {
                        case CredentialsTypeViewModel.UserNameAndPassword:
                            return (currentSelectedRegion, ElevDataModelView.Instance.DownloadAsync(credtvm, currentSelectedRegion, new CredentialsViewModel() { UserName = UserName, Password = Password }));
                        case CredentialsTypeViewModel.AuthenticationToken:
                             return (currentSelectedRegion, ElevDataModelView.Instance.DownloadAsync(credtvm, currentSelectedRegion, new CredentialsViewModel(){AuthenticationToken = AuthenticationToken}));
                        case CredentialsTypeViewModel.UserNameAndAuthenticationToken:
                            return (currentSelectedRegion, ElevDataModelView.Instance.DownloadAsync(credtvm, currentSelectedRegion, new CredentialsViewModel(){UserName = UserName, AuthenticationToken = AuthenticationToken}));
                        default:
                            throw new InvalidEnumArgumentException(nameof(credtvm.CredType));
                    }
                default:
                    throw new ArgumentException("Not supported " + nameof(ElevDataDistributionViewModel)+" ancestor type given.");
            }

        }, isRegionSelectedNotDownloaded.CombineLatest(
            areCredentialsRequired.CombineLatest(areCredentialsSet, 
                (x,y) => !x || y), 
            (x,y) => x && y));

        DownloadRegionCommand.Subscribe(async input =>
        {
            var result = await input.Item2;
            var selectedRegion = input.Item1;

            UpdateRecursivelySubRegionsPresence(selectedRegion);
            switch (result)
            {
                case ElevDataManager.DownloadingResult.Downloaded:
                    break;
                case ElevDataManager.DownloadingResult.Canceled:
                    break;
                case ElevDataManager.DownloadingResult.WrongCredentials:
                    //TODO: nejake upozornenie, ze zadane kredencialy neboli platn
                    break;
                case ElevDataManager.DownloadingResult.UnableToDownload:
                    //TODO: nejake upozornenie, ze sa stahovanie nepodarilo
                    break;
                default:
                    throw new InvalidEnumArgumentException("result", (int)result, typeof(ElevDataManager.DownloadingResult));
            }
        });
        
        DeleteRegionCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var currentSelectedRegion = SelectedRegion;
            var currentElevDataDist = CurrentElevDataDist;
            SetRecursivelySubRegionsPresenceToIsDeletingIfNotDeletedAlready(currentSelectedRegion!);
            SetRecursivelyUpperRegionsPresenceToIsDeletingIfNotDeletedAlready(currentSelectedRegion!);
            return (currentSelectedRegion!, ElevDataModelView.Instance.RemoveAsync(currentElevDataDist!, currentSelectedRegion!));
        }, isRegionSelectedDownloaded);

        DeleteRegionCommand.Subscribe(async input =>
        {
            await input.Item2;
            var selectedRegion = input.Item1;
            UpdateRecursivelySubRegionsPresence(selectedRegion);
            UpdateRecursivelyUpperRegionsPresence(selectedRegion);
        });
        
        CancelDownloadingCommand = ReactiveCommand.Create(() =>
        {
            SelectedRegion!.DownloadingCancellationTokenSource.Cancel();   
        }, isRegionSelectedDownloading);
        
        ReturnCommand = ReactiveCommand.Create(() =>
        {
            UserName = ""; Password = ""; AuthenticationToken = "";
            return _currentElevDataDist;
        });
        
        
        ElevDataDistributions = ElevDataModelView.Instance.GetElevDataSources()
            .SelectMany(elevDataSource => elevDataSource.ElevDataDistributions);
        CurrentElevDataDist = ElevDataDistributions.Contains(selectedElevDataDist) ? selectedElevDataDist : null;
    }

    private List<RegionViewModel> GetAllSubRegions(RegionViewModel region)
    {
        List<RegionViewModel> subRegions = [region];
        foreach (var subRegion in region.SubRegions)
        {
            subRegions.AddRange(GetAllSubRegions(subRegion));
        }
        return subRegions;
    }

    private void SetRecursivelySubRegionsPresenceToIsDownloadingAsSubregion(RegionViewModel region)
    {
        region.Presence = RegionViewModel.PresenceState.IsDownloadingAsSubRegion;
        foreach (var subRegion in region.SubRegions)
        {
            SetRecursivelySubRegionsPresenceToIsDownloadingAsSubregion(subRegion);
        }
    }
    private void SetRecursivelySubRegionsPresenceToIsDeletingIfNotDeletedAlready(RegionViewModel region)
    {
        
        if (region.Presence is not RegionViewModel.PresenceState.NotDownloaded)
            region.Presence = RegionViewModel.PresenceState.IsDeleting;
        foreach (var subRegion in region.SubRegions)
        {
            SetRecursivelySubRegionsPresenceToIsDeletingIfNotDeletedAlready(subRegion);
        }
    }

    private void SetRecursivelyUpperRegionsPresenceToIsDeletingIfNotDeletedAlready(RegionViewModel region)
    {
        if (region.Presence is not RegionViewModel.PresenceState.NotDownloaded)
            region.Presence = RegionViewModel.PresenceState.IsDeleting;
        if(region is SubRegionViewModel subRegion) SetRecursivelyUpperRegionsPresenceToIsDeletingIfNotDeletedAlready(subRegion.UpperRegion);
    }

    private void UpdateRecursivelySubRegionsPresence(RegionViewModel region)
    {
        region.UpdatePresence();
        foreach (var subRegion in region.SubRegions)
        {
            UpdateRecursivelySubRegionsPresence(subRegion);
        }
    }

    private void UpdateRecursivelyUpperRegionsPresence(RegionViewModel region)
    {
        region.UpdatePresence();
        if(region is SubRegionViewModel subRegionViewModel) UpdateRecursivelyUpperRegionsPresence(subRegionViewModel.UpperRegion);
    }

    /// <summary>
    /// Currently selected elevation data distribution which regions and their corresponding elev. data can be managed by user.
    /// 
    /// When application is closing this properties value is returned as currently used elevation data distribution.  
    /// </summary>
    public ElevDataDistributionViewModel? CurrentElevDataDist
    {
        get => _currentElevDataDist;
        set => this.RaiseAndSetIfChanged(ref _currentElevDataDist, value);
    }
    private ElevDataDistributionViewModel? _currentElevDataDist;

    /// <summary>
    /// Collection of all currently available regions for which corresponding elevation data can be downloaded.
    /// </summary>
    public IEnumerable<RegionViewModel>? CurrentAvailableRegions => _currentAvailableRegions.Value;
    private ObservableAsPropertyHelper<IEnumerable<RegionViewModel>?> _currentAvailableRegions;

    /// <summary>
    /// Currently selected region by user. Selected region can be managed by user according to its <c>RegionViewModel.PresenceState</c>.
    /// </summary>
    public RegionViewModel? SelectedRegion
    {
        get => _selectedRegion;
        set => this.RaiseAndSetIfChanged(ref _selectedRegion, value);
    }
    private RegionViewModel? _selectedRegion;
    
    /// <summary>
    /// Collection of all elevation data distributions provided for accessing the data.
    /// </summary>
    public IEnumerable<ElevDataDistributionViewModel> ElevDataDistributions { get; } 

    /// <summary>
    /// Username part of credentials used for accessing of elevation data from distributions which demand it.
    /// </summary>
    public string? UserName
    {
        get => _userName;
        set => this.RaiseAndSetIfChanged(ref _userName, value);
    }
    private string? _userName;

    /// <summary>
    /// Password part of credentials used for accessing of elevation data from distributions which demand it.
    /// </summary>
    public string? Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }
    private string? _password;
    
    /// <summary>
    /// Authentication token part of credentials used for accessing of elevation data from distributions which demand it.
    /// </summary>
    public string? AuthenticationToken
    {
        get => _authenticationToken;
        set => this.RaiseAndSetIfChanged(ref _authenticationToken, value);
    }
    private string? _authenticationToken;

    /// <summary>
    /// Indicates whether providing of credentials for accessing elevation data form distribution which demand it from user are required
    /// and whether the credentials are username and password
    /// </summary>
    public bool RequiredCredentialsTypeIsUserNameAndPassword => _requriedCredentialsTypeIsUserNameAndPassword.Value;
    private ObservableAsPropertyHelper<bool> _requriedCredentialsTypeIsUserNameAndPassword;
    
    /// <summary>
    /// Indicates whether providing of credentials for accessing elevation data form distribution which demand it from user are required
    /// and whether the credentials are authentication token.
    /// </summary>
    public bool RequiredCredentialsTypeIsAuthenticationToken => _requiredCredentialsTypeIsAuthenticationToken.Value;
    private ObservableAsPropertyHelper<bool> _requiredCredentialsTypeIsAuthenticationToken;

    /// <summary>
    /// Indicates whether providing of credentials for accessing elevation data form distribution which demand it from user are required
    /// and whether the credentials are user name and authentication token.
    /// </summary>
    public bool RequiredCredentialsTypeIsUserNameAndAuthenticationToken => _requiredCredentialsTypeIsUserNameAndAuthenticationToken.Value;
    private ObservableAsPropertyHelper<bool> _requiredCredentialsTypeIsUserNameAndAuthenticationToken;
    
    /// <summary>
    /// Indicates whether selected region is not downloaded.
    /// </summary>
    public bool IsSelectedRegionNotDownloaded  => _isRegionSelectedNotDownloaded.Value; 
    private ObservableAsPropertyHelper<bool> _isRegionSelectedNotDownloaded;
    
    /// <summary>
    /// Reactive command used for downloading of elevation data for currently selected region.
    /// 
    /// Firstly it sets currently selected region to state <c>IsDownloading</c> and its sub-regions presence state to <c>IsDownloadingAsSubregion</c>.  
    /// Then according to distributions requirement for credentials it calls asynchronously correct method on ModelView for downloading of elevation data which correspond to selected region.  
    /// It receives result of this download.  
    /// It the lets selected region and its subregions update their presence status.  
    /// In the end it processes result of download so that user could be informed about it.  
    /// Downloading of elevation data is enabled only in case that selected region is not downloaded yet and current distribution does not requires credentials or if it does, they are set.  
    /// </summary>
    public ReactiveCommand<Unit, (RegionViewModel,Task<ElevDataManager.DownloadingResult>)> DownloadRegionCommand { get; }
    /// <summary>
    /// Reactive command used for deleting of downloaded elevation data for currently selected region.
    /// 
    /// It is simpler than <c>DownloadRegionCommand</c>.  
    /// Firstly currently selected regions and its subregions presence state is set to <c>IsDeleting</c>. The state is set in such way only if given region is not deleted already.  
    /// Also all upper regions of currently selected region states are set in that way.  
    /// Then asynchronous removal of elevation data itself takes place.  
    /// After it finishes, selected regions sub-regions and upper-regions are let to update their presence status.  
    /// Removing of selected regions elevation data is enabled only if they are downloaded already.  
    /// </summary>
    public ReactiveCommand<Unit, (RegionViewModel, Task)> DeleteRegionCommand { get; }
    /// <summary>
    /// Reactive command for cancelling of elevation data download
    /// </summary>
    public ReactiveCommand<Unit, Unit> CancelDownloadingCommand { get; }
    /// <summary>
    /// Reactive command used for returning form elevation data configuration ViewModel.
    /// 
    /// It returns currently selected elevation data distribution which should be used now used in whole application as default one.  
    /// </summary>
    public ReactiveCommand<Unit, ElevDataDistributionViewModel?> ReturnCommand { get; }
}
