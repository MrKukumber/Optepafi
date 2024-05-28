using System;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Text.RegularExpressions;
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
        this.WhenActivated(_ => AlgorithmSelectingComboBox.SelectedItem = ViewModel!.SelectedSearchingAlgorithm);
    }

    private async void UserModelSelectingButton_OnClick(object? sender, RoutedEventArgs e)
    {
        TopLevel topLevel = TopLevel.GetTopLevel(this)!;
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            AllowMultiple = false,
            FileTypeFilter = ViewModel!.UsableUserModelTypes!
                .Select(userModelType => new FilePickerFileType(userModelType.UserModelTypeName) 
                {
                    Patterns = new[] {"*." + userModelType.UserModelFileNameSuffix + "." + userModelType.UserModelFileExtension}
                }).ToArray()
        });
        
        IStorageFile file;
        if (files.Count >= 1) { file = files[0]; }
        else return;

        if (!ViewModel.UsableUserModelTypes!.Any(userModelType =>
            {
                return Regex.IsMatch(file.Path.LocalPath,
                    @".*\." + userModelType.UserModelFileNameSuffix + @"\." + userModelType.UserModelFileExtension + "$");
            }))
        {
            //TODO: mozno vypisat nejaku hlasku ze vybrany subor nebol platny
            return;
        }
        //There is no need for disposing files[0] instance. It is assigned to no other variable than variable files.
        //Of disposing opened stream on this instance takes care ViewModel.
        try
        {
            _loadUserModelCommandSubscription?.Dispose();
            _loadUserModelCommandSubscription = ViewModel.LoadUserModelCommand
                .Execute((await file.OpenReadAsync(), file.Path.LocalPath))
                .Subscribe();
        } catch (UnauthorizedAccessException) {
            ViewModel.SelectedUserModelFileName = "Unable to open file."; //TODO: localize
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
                    Patterns = new[] {"*." + mapFormat.Extension}
                }).ToArray()
        });
        IStorageFile file;
        if (files.Count >= 1) { file = files[0]; }
        else return;

        if (!ViewModel.UsableMapFormats!.Any(mapFormat =>
            {
                return Regex.IsMatch(file.Path.LocalPath,
                    @".*\." + mapFormat.Extension + "$");
            }))
        {
            //TODO: mozno vypisat nejaku hlasku ze vybrany subor nebol platny
            return;
        }
        //There is no need for disposing files[0] instance. It is assigned to no other variable than variable files.
        //Of disposing opened stream on this instance takes care ViewModel.
        try
        {
            _loadMapCommandSubscription?.Dispose();
            _loadMapCommandSubscription = ViewModel.LoadMapCommand
                .Execute((await file.OpenReadAsync(), file.Path.LocalPath))
                .Subscribe();
        } catch (UnauthorizedAccessException) {
            ViewModel.SelectedMapFileName = "Unable to open file."; //TODO: localize
        } 
    }
}