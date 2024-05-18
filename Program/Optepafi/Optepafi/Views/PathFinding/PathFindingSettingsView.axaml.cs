using System;
using System.Linq;
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
    private IDisposable? _loadUserModelCommandSubscription;
    private IDisposable? _loadMapCommandSubscription;

    public PathFindingSettingsView()
    {
        InitializeComponent();
        this.WhenAnyValue(x => x.ViewModel).Subscribe(viewModel =>
        {
            _loadUserModelCommandSubscription ??= viewModel!.LoadUserModelCommandSubscription;
            _loadMapCommandSubscription ??= viewModel!.LoadMapCommandSubscription;
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
        if (files.Count >= 1)
        {
            _loadUserModelCommandSubscription?.Dispose();
            _loadUserModelCommandSubscription = ViewModel.LoadUserModelCommand
                .Execute((await files[0].OpenReadAsync(), files[0].Path.AbsolutePath))
                .Subscribe();
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
        if (files.Count >= 1)
        {
            
            _loadMapCommandSubscription?.Dispose();
            _loadMapCommandSubscription = ViewModel.LoadMapCommand
                .Execute((await files[0].OpenReadAsync(), files[0].Path.AbsolutePath))
                .Subscribe();
        }
    }
}