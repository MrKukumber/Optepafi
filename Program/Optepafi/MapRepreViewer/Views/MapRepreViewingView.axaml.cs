using System;
using System.Linq;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using MapRepreViewer.ViewModels;
using Optepafi.ViewModels.Data;
using Optepafi.Views.Utils;

namespace MapRepreViewer.Views;

public partial class MapRepreViewingView : ReactiveUserControl<MapRepreViewingViewModel>
{
    public MapRepreViewingView()
    {
        InitializeComponent();
    }

    private async void MapSelectingButton_OnClick(object? sender, RoutedEventArgs e)
    {
        TopLevel topLevel = TopLevel.GetTopLevel(this)!;
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            AllowMultiple = false,
            FileTypeFilter = ViewModel!.UsableMapFormats
                .Select(mapFormat => new FilePickerFileType(mapFormat.MapFormatName)
                {
                    Patterns = new[] { "*." + mapFormat.Extension }
                }).ToArray()
        });
        IStorageFile file;
        if (files.Count >= 1)
        {
            file = files[0];
        }
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
        }
        catch (UnauthorizedAccessException)
        {
            ViewModel.SelectedMapFileName = "Unable to open file."; //TODO: localize
        }
    }

    public static FuncValueConverter<int, bool> IsGreaterThanOne { get; } =
        new(i => i > 1);

    private void InputElement_OnPointerMoved(object? sender, PointerEventArgs e)
    {
        var point = e.GetCurrentPoint(sender as Control);
        CanvasCoordinate coords = (CanvasCoordinate) new CanvasCoordinateToAvaloniaPointConverter().ConvertBack(point.Position);
        XPointerPositionTextBlock.Text = $"Left = {(int)(coords.LeftPos * ViewModel.GraphicsScale)}";
        YPointerPositionTextBlock.Text = $"Top = {(int)(coords.TopPos * ViewModel.GraphicsScale)}";
    }

}