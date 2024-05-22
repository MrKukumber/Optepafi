using System;
using System.Linq;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using Optepafi.ViewModels;
using Optepafi.ViewModels.PathFinding;
using ReactiveUI;

namespace Optepafi.Views.PathFinding;

public partial class PathFindingSettingsView : ReactiveUserControl<PathFindingSettingsViewModel>
{
    //When View instance will become unreachable, garbage collecting should ensure, that commands will be cancelled.
    private IDisposable? _loadUserModelCommandSubscription;
    private IDisposable? _loadMapCommandSubscription;

    public PathFindingSettingsView()
    {
        InitializeComponent();
        //ViewModel should be assigned only one time. Lambda expression should be therefore executed twice, first time with viewModel = null.
        this.WhenAnyValue(x => x.ViewModel).Subscribe(viewModel =>
        {
            _loadUserModelCommandSubscription ??= viewModel?.LoadUserModelCommandSubscription;
            _loadMapCommandSubscription ??= viewModel?.LoadMapCommandSubscription;
        });
    }

    private async void UserModelSelectingButton_OnClick(object? sender, RoutedEventArgs e)
    {
        TopLevel topLevel = TopLevel.GetTopLevel(this)!;
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            AllowMultiple = false,
            FileTypeFilter = ViewModel!.UsableUserModelTypes?
                .Select(userModelType => new FilePickerFileType(userModelType.UserModelTypeName) 
                {
                    Patterns = new[] {"*" + userModelType.UserModelFileNameSuffix + userModelType.UserModelFileExtension}
                }).ToArray()
        });
        //There is no need for disposing files[0] instance. It is assigned to no other variable than variable files.
        //Of disposing opened stream on this instance takes care ViewModel.
        if (files.Count >= 1)
        {
            try
            {
                _loadUserModelCommandSubscription?.Dispose();
                _loadUserModelCommandSubscription = ViewModel.LoadUserModelCommand
                    .Execute((await files[0].OpenReadAsync(), files[0].Path.AbsolutePath))
                    .Subscribe();
            } catch (UnauthorizedAccessException) {
                ViewModel.SelectedUserModelFileName = "Unable to open file."; //TODO: localize
            }
        }
    }

    private async void MapSelectingButton_OnClick(object? sender, RoutedEventArgs e)
    {
        TopLevel topLevel = TopLevel.GetTopLevel(this)!;
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            AllowMultiple = false,
            FileTypeFilter = ViewModel!.UsableMapFormats?
                .Select(mapFormat => new FilePickerFileType(mapFormat.MapFormatName) 
                {
                    Patterns = new[] {"*" + mapFormat.Extension}
                }).ToArray()
        });
        //There is no need for disposing files[0] instance. It is assigned to no other variable than variable files.
        //Of disposing opened stream on this instance takes care ViewModel.
        if (files.Count >= 1)
        {
            try
            {
                _loadMapCommandSubscription?.Dispose();
                _loadMapCommandSubscription = ViewModel.LoadMapCommand
                    .Execute((await files[0].OpenReadAsync(), files[0].Path.AbsolutePath))
                    .Subscribe();
            } catch (UnauthorizedAccessException) {
                ViewModel.SelectedMapFileName = "Unable to open file."; //TODO: localize
            } 
        }
    }
}