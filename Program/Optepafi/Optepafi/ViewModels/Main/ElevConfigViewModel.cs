using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.ModelViews.Main;
using Optepafi.ViewModels.Data.Representatives;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

/// <summary>
/// ViewModel which is responsible for control over elevation data configuration mechanism.
/// This is a special type of ViewModel because all instances of this ViewModel use the same one instance of <c>ElevDataModelView</c> singleton.
/// It also is special in that way it is designed and intended to be used in dialog windows. 
/// Singleton design of mentioned ModelView is intentional so instance of this ModelView could be used in application on more places at once.
/// It opens opportunity for having elevation configuration ViewModels opened simultaneously on multiple places. ModelView ensures that this simultaneous opening will run flawlessly.
/// Among tasks of this ViewModel belongs:
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
    /// It initialize all reactive constructs and assigns currently used elevation data distribution to value of provided argument.
    /// </summary>
    /// <param name="selectedElevDataDist">Argument according to which is currently used elevation data distribution set.</param>
    public ElevConfigViewModel(ElevDataDistributionViewModel? selectedElevDataDist)
    {
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
            (userName, password) => !string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password));
        IObservable<bool> areCredentialsRequired = this.WhenAnyValue(
            x => x.CurrentElevDataDist,
            (ElevDataDistributionViewModel? elevDataDist) => elevDataDist is CredentialsRequiringElevDataDistributionViewModel);
        
            
        
        DownloadRegionCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var currentSelectedRegion = SelectedRegion;
            var currentElevDataDist = CurrentElevDataDist;
            currentSelectedRegion!.Presence = RegionViewModel.PresenceState.IsDownloading;
            foreach(var subRegion in currentSelectedRegion.SubRegions ) 
                SetRecursivelySubRegionsPresenceToIsDownloadingAsSubregion(subRegion);
            currentSelectedRegion.DownloadingCancellationTokenSource = new CancellationTokenSource();
            ElevDataManager.DownloadingResult result;
            switch (currentElevDataDist)
            {
                case CredentialsNotRequiringElevDataDistributionViewModel cnredtvm :
                   result =  await ElevDataModelView.Instance.DownloadAsync(cnredtvm, currentSelectedRegion);
                   break;
                case CredentialsRequiringElevDataDistributionViewModel credtvm :
                    var userName = UserName; var password = Password;
                    UserName = ""; Password = "";
                    result = await ElevDataModelView.Instance.DownloadAsync(credtvm, currentSelectedRegion, new NetworkCredential(userName, password));
                    break;
                default:
                    throw new ArgumentException("Not supported " + nameof(ElevDataDistributionViewModel)+" ancestor type given.");
            }
            UpdateRecursivelySubRegionsPresence(currentSelectedRegion);
            switch (result)
            {
                case ElevDataManager.DownloadingResult.Downloaded:
                    break;
                case ElevDataManager.DownloadingResult.Canceled:    
                    break;
                case ElevDataManager.DownloadingResult.WrongCredentials:
                    //TODO: nejake upozornenie, ze zadane kredencialy neboli platne
                    break;
                case ElevDataManager.DownloadingResult.UnableToDownload:
                    //TODO: nejake upozornenie, ze sa stahovanie nepodarilo
                    break;
                default:
                    throw new InvalidEnumArgumentException("result", (int) result, typeof(ElevDataManager.DownloadingResult));
            }

        }, isRegionSelectedNotDownloaded.CombineLatest(
            areCredentialsRequired.CombineLatest(areCredentialsSet, 
                (x,y) => !x || y), 
            (x,y) => x && y));
        DeleteRegionCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var currentSelectedRegion = SelectedRegion;
            var currentElevDataDist = CurrentElevDataDist;
            SetRecursivelySubRegionsPresenceToIsDeletingIfNotDeletedAlready(currentSelectedRegion!);
            SetRecursivelyUpperRegionsPresenceToIsDeletingIfNotDeletedAlready(currentSelectedRegion!);
            await ElevDataModelView.Instance.RemoveAsync(currentElevDataDist!, currentSelectedRegion!);
            UpdateRecursivelySubRegionsPresence(currentSelectedRegion!);
            UpdateRecursivelyUpperRegionsPresence(currentSelectedRegion!);
        }, isRegionSelectedDownloaded);
        
        CancelDownloadingCommand = ReactiveCommand.Create(() =>
        {
            SelectedRegion!.DownloadingCancellationTokenSource.Cancel();   
        }, isRegionSelectedDownloading);
        
        ReturnCommand = ReactiveCommand.Create(() => _currentElevDataDist);
        
        _currentAvailableRegions = this.WhenAnyValue(x => x.CurrentElevDataDist)
            .Select(ceds => ceds is null 
                ? null : ElevDataModelView.Instance.TopRegionsOfAllDistributions[ceds]
                    .SelectMany(topRegion => GetAllSubRegions(topRegion))) 
            .ToProperty(this, nameof(CurrentAvailableRegions));
        _credentialsAreRequired = this.WhenAnyValue( x => x.CurrentElevDataDist)
            .Select(elevDataDist => elevDataDist is CredentialsRequiringElevDataDistributionViewModel)
            .ToProperty(this, nameof(CredentialsAreRequired));
        
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
    /// User name part of credentials used for accessing of elevation data from distributions which demand it.
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
    /// Indicates whether providing of credentials for accessing elevation data form distribution which demand it from user is required.
    /// </summary>
    public bool CredentialsAreRequired => _credentialsAreRequired.Value;
    private ObservableAsPropertyHelper<bool> _credentialsAreRequired;
    
    /// <summary>
    /// Reactive command used for downloading of elevation data for currently selected region.
    /// Firstly it sets currently selected region to state <c>IsDownloading</c> and its sub-regions presence state to <c>IsDownloadingAsSubregion</c> .
    /// Then according to distributions requirement for credentials it calls asynchronously correct method on ModelView for downloading of elevation data which correspond to selected region.
    /// It receives result of this download.
    /// It the lets selected region and its subregions update their presence status.
    /// In the end it processes result of download so that user could be informed about it.
    /// Downloading of elevation data is enabled only in case that selected region is not downloaded yet and current distribution does not requires credentials or if it does, they are set.
    /// </summary>
    public ReactiveCommand<Unit, Unit> DownloadRegionCommand { get; }
    /// <summary>
    /// Reactive command used for deleting of downloaded elevation data for currently selected region.
    /// It is simpler than <c>DownloadRegionCommand</c>.
    /// Firstly currently selected regions and its subregions presence state is set to <c>IsDeleting</c>. The state is set in such way only if given region is not deleted already.
    /// Also all upper regions of currently selected region states are set in that way.
    /// Then asynchronous removal of elevation data itself takes place.
    /// After it finishes, selected regions sub-regions and upper-regions are let to update their presence status.
    /// Removing of selected regions elevation data is enabled only if they are downloaded already. 
    /// </summary>
    public ReactiveCommand<Unit, Unit> DeleteRegionCommand { get; }
    /// <summary>
    /// Reactive command for cancelling of elevation data download
    /// </summary>
    public ReactiveCommand<Unit, Unit> CancelDownloadingCommand { get; }
    /// <summary>
    /// Reactive command used for returning form elevation data configuration ViewModel.
    /// It returns currently selected elevation data distribution which should be used now used in whole application as default one. 
    /// </summary>
    public ReactiveCommand<Unit, ElevDataDistributionViewModel?> ReturnCommand { get; }
}
