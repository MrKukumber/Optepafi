using System;
using System.Collections;
using System.Linq;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using Optepafi.ViewModels.PathFinding;

namespace Optepafi.Views.PathFinding;

/// <summary>
/// View for parameter setting part of the path finding mechanism.
/// 
/// For more information on this part of mechanism see <see cref="PathFindingSettingsViewModel"/>.  
/// </summary>
public partial class PathFindingSettingsView : ReactiveUserControl<PathFindingSettingsViewModel>
{
    public PathFindingSettingsView()
    {
        InitializeComponent();
        
        if (Design.IsDesignMode) return;
    }

    /// <summary>
    /// Method for handling of user model selecting buttons <c>OnClick</c> event.
    /// 
    /// It opens file picker, so that user could select file with serialized user model.  
    /// It lets user choose format, which is permitted by applications logic.  
    /// After user chooses file, its format is tested again.  
    /// It is tested again because user is able to choose file with not valid format despite previously mentioned restriction on shown files.  
    /// If format of selected file is valid, its stream and name is passed to execution of <c>LoadUserModelCommand</c>.  
    /// </summary>
    /// <param name="sender">Sender of <c>OnClick</c> event.</param>
    /// <param name="e"><c>OnClick</c> events arguments.</param>
    private async void UserModelSelectingButton_OnClick(object? sender, RoutedEventArgs e)
    {
        TopLevel topLevel = TopLevel.GetTopLevel(this)!;
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            AllowMultiple = false,
            FileTypeFilter = ViewModel!.UsableUserModelTypes
                .Select(userModelType => new FilePickerFileType(userModelType.UserModelTypeName) 
                {
                    Patterns = new[] {"*." + userModelType.UserModelFileNameSuffix + "." + userModelType.UserModelFileExtension}
                }).ToArray()
        });
        
        IStorageFile file;
        if (files.Count >= 1) { file = files[0]; }
        else return;

        if (!ViewModel.UsableUserModelTypes.Any(userModelType =>
            {
                return Regex.IsMatch(file.Path.LocalPath,
                    @".*\." + userModelType.UserModelFileNameSuffix + @"\." + userModelType.UserModelFileExtension + "$");
            }))
        {
            //TODO: mozno vypisat nejaku hlasku ze vybrany subor nebol platny
            return;
        }
        //There is no need for disposing files[0] instance. It is assigned to no other variable than variable files.
        //For disposing opened stream on this instance takes care ViewModel.
        try
        {
                await ViewModel.LoadUserModelCommand
                .Execute((await file.OpenReadAsync(), file.Path.LocalPath));
        } catch (UnauthorizedAccessException) {
            ViewModel.SelectedUserModelFileName = "Unable to open file."; //TODO: localize
        }
    }

    /// <summary>
    /// Method for handling of map selecting buttons <c>OnClick</c> event.
    /// 
    /// It opens file picker, so that user could select file with map.  
    /// It lets user choose only format, which is permitted by applications logic.  
    /// After user chooses file, its format is tested again.  
    /// It is tested again because user is able to choose file with not valid format despite previously mentioned restriction on shown files.  
    /// If format of selected file is valid, its stream and name is passed to execution of <c>LoadMapCommand</c>.  
    /// </summary>
    /// <param name="sender">Sender of <c>OnClick</c> event.</param>
    /// <param name="e"><c>OnClick</c> events arguments.</param>
    private async void MapSelectingButton_OnClick(object? sender, RoutedEventArgs e)
    {
        TopLevel topLevel = TopLevel.GetTopLevel(this)!;
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            AllowMultiple = false,
            FileTypeFilter = ViewModel!.UsableMapFormats
                .Select(mapFormat => new FilePickerFileType(mapFormat.MapFormatName) 
                {
                    Patterns = new[] {"*." + mapFormat.Extension}
                }).ToArray()
        });
        IStorageFile file;
        if (files.Count >= 1) { file = files[0]; }
        else return;

        if (!ViewModel.UsableMapFormats.Any(mapFormat =>
            {
                return Regex.IsMatch(file.Path.LocalPath,
                    @".*\." + mapFormat.Extension + "$");
            }))
        {
            //TODO: mozno vypisat nejaku hlasku ze vybrany subor nebol platny
            return;
        }
        //There is no need for disposing files[0] instance. It is assigned to no other variable than variable files.
        //For disposing opened stream on this instance takes care ViewModel.
        try
        {
                await ViewModel.LoadMapCommand
                .Execute((await file.OpenReadAsync(), file.Path.LocalPath));
        } catch (UnauthorizedAccessException) {
            ViewModel.SelectedMapFileName = "Unable to open file."; //TODO: localize
        } 
    }
    
    /// <summary>
    /// Value converter that indicates whether inserted enumerable is not null or empty.
    /// </summary>
    public static FuncValueConverter<IEnumerable, bool> IsNotEmptyNorNull { get; } =
        new (enumerable => enumerable?.GetEnumerator().MoveNext() ?? false);
}