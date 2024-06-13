using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Input;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Optepafi.Models;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.UserModelMan;
using Optepafi.ModelViews;
using Optepafi.ModelViews.Main;
using Optepafi.ViewModels.Data.Representatives;
using Optepafi.ViewModels.DataViewModels;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

public class ElevConfigViewModel : ViewModelBase
{
    private ElevDataDistributionViewModel? _currentElevDataDist;
    public ElevConfigViewModel(ElevDataDistributionViewModel? currentElevDataDist)
    {
        _currentElevDataDist = currentElevDataDist;
        ReturnCommand = ReactiveCommand.Create(() => _currentElevDataDist);

        _currentAvailableRegions = this.WhenAnyValue(x => x.CurrentElevDataDist)
            .Select(ceds => ceds?.AllTopRegions.SelectMany(topRegion => GetAllSubRegions(topRegion)))
            .ToProperty(this, nameof(CurrentAvailableRegions));
        
        IObservable<bool> isRegionSelectedNotDownloaded = this.WhenAnyValue(
            x => x.SelectedRegion,
            region => region?.Presence == RegionViewModel.PresenceState.NotDownloaded);
        IObservable<bool> isRegionSelectedDownloaded = this.WhenAnyValue(
            x => x.SelectedRegion,
            region => region?.Presence == RegionViewModel.PresenceState.Downloaded);
        IObservable<bool> isRegionSelectedDownloading = this.WhenAnyValue(
            x => x.SelectedRegion,
            region => region?.Presence == RegionViewModel.PresenceState.IsDownloading);
        IObservable<bool> areCredentialsSet = this.WhenAnyValue(
            x => x.UserName,
            x => x.Password,
            (userName, password) => !string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password));
        IObservable<bool> areCredentialsRequired = this.WhenAnyValue(
            x => x.CurrentElevDataDist,
            (ElevDataDistributionViewModel? elevDataDist) => elevDataDist is CredentialsRequiringElevDataDistributionViewModel);
        
        _credentialsAreRequired = areCredentialsRequired
            .ToProperty(this, nameof(CredentialsAreRequired));
            
        DownloadRegionCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            SelectedRegion!.Presence = RegionViewModel.PresenceState.IsDownloading;
            SelectedRegion.DownloadingCancellationTokenSource = new CancellationTokenSource();
            ElevDataManager.DownloadingResult result;
            switch (CurrentElevDataDist)
            {
                case CredentialsNotRequiringElevDataDistributionViewModel cnredtvm :
                   result =  await ElevDataModelView.Instance.DownloadAsync(cnredtvm, SelectedRegion);
                   break;
                case CredentialsRequiringElevDataDistributionViewModel credtvm :
                    var userName = UserName; var password = Password;
                    UserName = ""; Password = "";
                    result = await ElevDataModelView.Instance.DownloadAsync(credtvm, SelectedRegion, new NetworkCredential(userName, password));
                    break;
                default:
                    throw new ArgumentException("Not supported " + nameof(ElevDataDistributionViewModel)+" ancestor type given.");
            }
            switch (result)
            {
                case ElevDataManager.DownloadingResult.Downloaded:
                    SelectedRegion.Presence = RegionViewModel.PresenceState.Downloaded;
                    break;
                case ElevDataManager.DownloadingResult.Canceled:    
                    SelectedRegion.Presence = RegionViewModel.PresenceState.NotDownloaded;
                    break;
                case ElevDataManager.DownloadingResult.WrongCredentials:
                    //TODO: nejake upozornenie, ze zadane kredencialy neboli platne
                    break;
                default:
                    SelectedRegion.Presence = RegionViewModel.PresenceState.NotDownloaded;
                    //TODO: nejake upozornenie, ze sa stahovanie nepodarilo
                    break;
            }

        }, isRegionSelectedNotDownloaded.CombineLatest(
            areCredentialsRequired.CombineLatest(areCredentialsSet, 
                (x,y) => !x || y), 
            (x,y) => x && y));
        DeleteRegionCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            SelectedRegion!.Presence = RegionViewModel.PresenceState.IsDeleting;
            await ElevDataModelView.Instance.RemoveAsync(CurrentElevDataDist!, SelectedRegion);
            SelectedRegion.Presence = RegionViewModel.PresenceState.NotDownloaded;
        }, isRegionSelectedDownloaded);
        
        CancelDownloadingCommand = ReactiveCommand.Create(() =>
        {
            SelectedRegion!.DownloadingCancellationTokenSource.Cancel();   
        }, isRegionSelectedDownloading);
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




    private ObservableAsPropertyHelper<IEnumerable<RegionViewModel>?> _currentAvailableRegions;
    public IEnumerable<RegionViewModel>? CurrentAvailableRegions => _currentAvailableRegions.Value;

    private RegionViewModel? _selectedRegion;
    public RegionViewModel? SelectedRegion
    {
        get => _selectedRegion;
        set => this.RaiseAndSetIfChanged(ref _selectedRegion, value);
    }
    public ElevDataDistributionViewModel? CurrentElevDataDist
    {
        get => _currentElevDataDist;
        set => this.RaiseAndSetIfChanged(ref _currentElevDataDist, value);
    }
    public IEnumerable<ElevDataDistributionViewModel> ElevDataDistributions { get; } = ElevDataModelView.Instance.ElevDataSoruceViewModels
        .SelectMany(elevDataSource => elevDataSource.ElevDataDistributions);

    private string? _userName;
    public string? UserName
    {
        get => _userName;
        set => this.RaiseAndSetIfChanged(ref _userName, value);
    }

    private string? _password;
    public string? Password
    {
        private get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }

    private ObservableAsPropertyHelper<bool> _credentialsAreRequired;
    public bool CredentialsAreRequired
    {
        get => _credentialsAreRequired.Value;
    }
    public ReactiveCommand<Unit, ElevDataDistributionViewModel?> ReturnCommand { get; }
    public ReactiveCommand<Unit, Unit> DownloadRegionCommand { get; }
    public ReactiveCommand<Unit, Unit> DeleteRegionCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelDownloadingCommand { get; }
    public static FuncValueConverter<RegionViewModel.PresenceState, IBrush> PresenceToColorConverter { get; } =
        new(presence => presence switch
        {
            RegionViewModel.PresenceState.Downloaded => Brushes.GreenYellow,
            RegionViewModel.PresenceState.NotDownloaded => Brushes.White,
            _ => Brushes.DarkOrange
        });
}
