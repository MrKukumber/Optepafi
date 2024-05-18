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
using Optepafi.ModelViews;
using Optepafi.ModelViews.Main;
using Optepafi.ViewModels.DataViewModels;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

public class ElevConfigViewModel : ViewModelBase
{
    private ElevDataTypeViewModel? _currentElevDataType;
    public ElevConfigViewModel(ElevDataTypeViewModel? currentElevDataType)
    {
        _currentElevDataType = currentElevDataType;
        ReturnCommand = ReactiveCommand.Create(() => _currentElevDataType);

        _currentAvailableRegions = this.WhenAnyValue(x => x.CurrentElevDataType)
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
            x => x.CurrentElevDataType,
            (ElevDataTypeViewModel? elevDataType) => elevDataType is CredentialsRequiringElevDataTypeViewModel);
        
        _credentialsAreRequired = areCredentialsRequired
            .ToProperty(this, nameof(CredentialsAreRequired));
            
        DownloadRegionCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            SelectedRegion!.Presence = RegionViewModel.PresenceState.IsDownloading;
            SelectedRegion.DownloadingCancellationTokenSource = new CancellationTokenSource();
            var result = CurrentElevDataType switch
            {
                CredentialsNotRequiringElevDataTypeViewModel cnredtvm => await ElevDataModelView.Instance.DownloadAsync(
                    cnredtvm, SelectedRegion),
                CredentialsRequiringElevDataTypeViewModel credtvm => await ElevDataModelView.Instance.DownloadAsync(credtvm,
                    SelectedRegion, new NetworkCredential(UserName, Password)),
                _ => throw new ArgumentException("Not supported ElevDataTypeViewModel ancestor type given.")
            };
            switch (result)
            {
                case ElevDataManager.DownloadingResult.Downloaded:
                    SelectedRegion.Presence = RegionViewModel.PresenceState.Downloaded;
                    break;
                case ElevDataManager.DownloadingResult.Canceled:    
                    SelectedRegion.Presence = RegionViewModel.PresenceState.NotDownloaded;
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
            await ElevDataModelView.Instance.RemoveAsync(CurrentElevDataType!, SelectedRegion);
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
    public ElevDataTypeViewModel? CurrentElevDataType
    {
        get => _currentElevDataType;
        set => this.RaiseAndSetIfChanged(ref _currentElevDataType, value);
    }
    public IEnumerable<ElevDataTypeViewModel> ElevDataTypes { get; } = ElevDataModelView.Instance.ElevDataSoruceViewModels
        .SelectMany(elevDataSource => elevDataSource.ElevDataTypes);

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
    public ReactiveCommand<Unit, ElevDataTypeViewModel?> ReturnCommand { get; }
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
