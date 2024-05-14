using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Windows.Input;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Optepafi.Models;
using Optepafi.Models.ElevationDataMan;
using Optepafi.ModelViews;
using Optepafi.ModelViews.Main;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

public class ElevConfigViewModel : ViewModelBase
{
    private IElevDataSource? _currentElevDataSource;
    public ElevConfigViewModel(IElevDataSource? currentElevDataSource)
    {
        _currentElevDataSource = currentElevDataSource;
        ReturnCommand = ReactiveCommand.Create(() => _currentElevDataSource);

        this.WhenAnyValue(x => x.CurrentElevDataSource)
            .Subscribe(newElevDataSource =>
            {
                CurrentAvailableRegions = newElevDataSource?.AllRegions;
            });
        
        IObservable<bool> isRegionSelectedAndNotDownloaded = this.WhenAnyValue(
            x => x.SelectedRegion,
            region => !region?.IsDownloaded ?? false);
        IObservable<bool> isRegionSelectedAndDownloaded = this.WhenAnyValue(
            x => x.SelectedRegion,
            region => region?.IsDownloaded ?? false);
        DownloadRegionCommand = ReactiveCommand.Create(() =>
        {
            /*TODO download seleceted region*/
            
        }, isRegionSelectedAndNotDownloaded);
        DeleteRegionCommand = ReactiveCommand.Create(() =>
        {
            /*TODO delete selected region*/
            
        }, isRegionSelectedAndDownloaded);
    }




    public IReadOnlyCollection<IRegion>? CurrentAvailableRegions { get; set; }

    private IRegion? _selectedRegion;
    public IRegion? SelectedRegion
    {
        get => _selectedRegion;
        set => this.RaiseAndSetIfChanged(ref _selectedRegion, value);
    }
    public IElevDataSource? CurrentElevDataSource
    {
        get => _currentElevDataSource;
        set => this.RaiseAndSetIfChanged(ref _currentElevDataSource, value);
    }
    public IReadOnlyCollection<IElevDataSource> ElevDataSources { get; } = ElevDataModelView.Instance.ElevDataSources;
    public ReactiveCommand<Unit, IElevDataSource?> ReturnCommand { get; }
    public ReactiveCommand<Unit, Unit> DownloadRegionCommand { get; }
    public ReactiveCommand<Unit, Unit> DeleteRegionCommand { get; }
    public static FuncValueConverter<bool, IBrush> DownloadedToColorConverter { get; } =
        new (isDownloaded => isDownloaded ? Brushes.GreenYellow : Brushes.White);
}
