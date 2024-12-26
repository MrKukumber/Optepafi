using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using MapRepreViewer.ModelViews;
using Optepafi.Models.MapMan;
using Optepafi.ViewModels.Data.Configuration;
using Optepafi.ViewModels.Data.Graphics;
using Optepafi.ViewModels.Data.Representatives;
using Optepafi.Views.PathFinding.Windows;
using ReactiveUI;

namespace MapRepreViewer.ViewModels;

public class MapRepreViewingViewModel : ViewModelBase
{
    private MapRepreViewingModelView _mapRepreViewingMv;

    public MapRepreViewingViewModel(MapRepreViewingModelView mapRepreViewingMv)
    {
        _mapRepreViewingMv = mapRepreViewingMv;

        UsableMapFormats = _mapRepreViewingMv.GetAllMapFormats();

        Configuration = _mapRepreViewingMv.GetDefaultConfigurationOfCurrentMapRepre();

        LoadMapCommand = ReactiveCommand.CreateFromObservable(((Stream, string) mapFileStreamAndPath) => Observable
            .StartAsync<(MapManager.MapCreationResult, MapFormatViewModel, string)>(async cancellationToken =>
            {
                var (mapFileStream, mapFilePath) = mapFileStreamAndPath;
                string mapFileName = Path.GetFileName(mapFilePath);
                MapFormatViewModel? mapFormat = _mapRepreViewingMv.GetCorrespondingMapFormat(mapFileName);
                if (mapFormat is null) throw new NullReferenceException("Map format should be returned, because chosen file was filtered to be correct.");
                var loadResult = await _mapRepreViewingMv.LoadAndSetMapAsync(mapFileStreamAndPath, mapFormat, cancellationToken);
                mapFileStream.Dispose();
                return (loadResult, mapFormat, mapFilePath);
            })
            .TakeUntil(LoadMapCommand));
        
        LoadMapCommand.Subscribe(commandOutput =>
        {
            var (mapLoadingResult, mapFormat, mapFilePath) = commandOutput;
            switch (mapLoadingResult)
            {
                case MapManager.MapCreationResult.Incomplete:
                    // TODO: vypisat hlasku, ze vytvorena mapa bude nekompletna, teda z velkej pravdepodobnosti nepouzitelna
                    // v modelView-u uz nastavena tato mapa, takze urcite nenechavat aby sa uzivatel mohol navratit ku predchadzajucej
                    Console.WriteLine("Map creation incomplete.");
                    SelectedMapFileName = Path.GetFileName(mapFilePath);
                    CurrentlyUsedMapFormat = mapFormat;
                    break; 
                case MapManager.MapCreationResult.Ok:
                    SelectedMapFileName = Path.GetFileName(mapFilePath);
                    CurrentlyUsedMapFormat = mapFormat;
                    break; 
                case MapManager.MapCreationResult.Cancelled:
                    break;
                case MapManager.MapCreationResult.UnableToParse:
                    //TODO: vypisat nejaku errorovu hlasku, nechat vsetko tak ako bolo
                    break;
            }
        });

        CreateAndViewWholeMapRepreCommand = ReactiveCommand.CreateFromObservable(() => Observable
            .StartAsync<(GraphicsSourceViewModel, GraphicsSourceViewModel)?>(async cancellationToken =>
            {
                IsViewingByPartsActive = false;
                _mapRepreViewingMv.SetWholeMapForUse();
                await _mapRepreViewingMv.CreateMapRepreAsync(Configuration, cancellationToken);
                if (cancellationToken.IsCancellationRequested) return null;
                var mapGraphics = _mapRepreViewingMv.GetMapGraphics();
                if (mapGraphics is null) { Console.WriteLine("MapGraphics could not be obtained because map did not provide its area."); return null;}
                var mapRepreGraphics = _mapRepreViewingMv.GetMapRepreGraphics();
                return (mapGraphics, mapRepreGraphics);
            })
            .TakeUntil(CreateAndViewWholeMapRepreCommand)
            .TakeUntil(CreateAndViewMapRepreByPartsCommand)
            .TakeUntil(LoadMapCommand));

        CreateAndViewWholeMapRepreCommand.Subscribe(SetBothGraphicsSources);
        
        CreateAndViewMapRepreByPartsCommand = ReactiveCommand.CreateFromObservable(() => Observable
            .StartAsync<(GraphicsSourceViewModel, GraphicsSourceViewModel)?>(async cancellationToken =>
            {
                ShownObjectsCount = 1;
                (bool successfulPartitionObtaining, AreAllObjectsShown) = await _mapRepreViewingMv.TrySetPartitionMapOfSizeForUseAsync(ShownObjectsCount, cancellationToken);
                if (cancellationToken.IsCancellationRequested) return null;
                if (!successfulPartitionObtaining) { Console.WriteLine("Could not create partition map."); return null; }
                await _mapRepreViewingMv.CreateMapRepreAsync(Configuration, cancellationToken);
                if (cancellationToken.IsCancellationRequested) return null;
                IsViewingByPartsActive = true;
                var mapGraphics = _mapRepreViewingMv.GetMapGraphics();
                if (mapGraphics is null) { Console.WriteLine("MapGraphics could not be obtained because map did not provide its area."); return null;}
                var mapRepreGraphics = _mapRepreViewingMv.GetMapRepreGraphics();
                return (mapGraphics, mapRepreGraphics);
            })
            .TakeUntil(CreateAndViewMapRepreByPartsCommand)
            .TakeUntil(CreateAndViewWholeMapRepreCommand)
            .TakeUntil(LoadMapCommand));
        
        CreateAndViewMapRepreByPartsCommand.Subscribe(SetBothGraphicsSources);
        
        ZoomCommand = ReactiveCommand.Create((float howMuch) =>
        {
            GraphicsScale += howMuch;
        });

        UnZoomCommand = ReactiveCommand.Create((float howMuch) =>
        {
            GraphicsScale -= howMuch;
        });

        
        RemoveObjectCommand = ReactiveCommand.CreateFromObservable(() => Observable
            .StartAsync<(GraphicsSourceViewModel, GraphicsSourceViewModel)?>(async cancellationToken =>
            {
                --ShownObjectsCount;
                (bool successfulPartitionObtaining, AreAllObjectsShown) = await _mapRepreViewingMv.TrySetPartitionMapOfSizeForUseAsync(ShownObjectsCount, cancellationToken);
                if (cancellationToken.IsCancellationRequested) return null;
                if (!successfulPartitionObtaining) { Console.WriteLine("Could not create partition map."); return null; }
                await _mapRepreViewingMv.CreateMapRepreAsync(Configuration, cancellationToken);
                if (cancellationToken.IsCancellationRequested) return null;
                var mapGraphics = _mapRepreViewingMv.GetMapGraphics();
                if (mapGraphics is null) { Console.WriteLine("MapGraphics could not be obtained because map did not provide its area."); return null;}
                var mapRepreGraphics = _mapRepreViewingMv.GetMapRepreGraphics();
                return (mapGraphics, mapRepreGraphics);
            })
            .TakeUntil(RemoveObjectCommand)
            .TakeUntil(AddObjectCommand)
            .TakeUntil(CreateAndViewWholeMapRepreCommand)
            .TakeUntil(CreateAndViewMapRepreByPartsCommand)
            .TakeUntil(LoadMapCommand));

        RemoveObjectCommand.Subscribe(SetBothGraphicsSources);
        
        AddObjectCommand = ReactiveCommand.CreateFromObservable(() => Observable
            .StartAsync<(GraphicsSourceViewModel, GraphicsSourceViewModel)?>(async cancellationToken =>
            {
                ++ShownObjectsCount;
                (bool successfulPartitionObtaining, AreAllObjectsShown) = await _mapRepreViewingMv.TrySetPartitionMapOfSizeForUseAsync(ShownObjectsCount, cancellationToken);
                if (cancellationToken.IsCancellationRequested) return null;
                if (!successfulPartitionObtaining) { Console.WriteLine("Could not create partition map."); return null; }
                await _mapRepreViewingMv.CreateMapRepreAsync(Configuration, cancellationToken);
                if (cancellationToken.IsCancellationRequested) return null;
                var mapGraphics = _mapRepreViewingMv.GetMapGraphics();
                if (mapGraphics is null) { Console.WriteLine("MapGraphics could not be obtained because map did not provide its area."); return null;}
                var mapRepreGraphics = _mapRepreViewingMv.GetMapRepreGraphics();
                return (mapGraphics, mapRepreGraphics);
            })
            .TakeUntil(AddObjectCommand)
            .TakeUntil(RemoveObjectCommand)
            .TakeUntil(CreateAndViewWholeMapRepreCommand)
            .TakeUntil(CreateAndViewMapRepreByPartsCommand)
            .TakeUntil(LoadMapCommand));
        
        AddObjectCommand.Subscribe(SetBothGraphicsSources);
        
        _scaledGraphicsWidth = this.WhenAnyValue(x => x.GraphicsWidth,
                x => x.GraphicsScale,
                (width, scale) => (int)(width * scale))
            .ToProperty(this, nameof(ScaledGraphicsWidth));
        _scaledGraphicsHeight = this.WhenAnyValue(x => x.GraphicsHeight,
                x => x.GraphicsScale,
                (height, scale) => (int)(height * scale))
            .ToProperty(this, nameof(ScaledGraphicsHeight));
    }

    private void SetBothGraphicsSources((GraphicsSourceViewModel, GraphicsSourceViewModel)? graphics)
    {
        if (graphics is not null)
        {
            MapGraphicsSource = graphics.Value.Item1;
            MapRepreGraphicsSource = graphics.Value.Item2;
            GraphicsWidth = graphics.Value.Item2.GraphicsWidth;
            GraphicsHeight = graphics.Value.Item2.GraphicsHeight;
        }
    }

    public bool IsViewingByPartsActive
    {
        get => _isViewingByPartsActive;
        set => this.RaiseAndSetIfChanged(ref _isViewingByPartsActive, value);
    }
    private bool _isViewingByPartsActive = false;

    public int ShownObjectsCount
    {
        get => _shownObjectsCount;
        set => this.RaiseAndSetIfChanged(ref _shownObjectsCount, value);
    }
    private int _shownObjectsCount = 0;

    public bool AreAllObjectsShown
    {
        get => _areAllObjectsShown;
        set => this.RaiseAndSetIfChanged(ref _areAllObjectsShown, value);
    }
    private bool _areAllObjectsShown = false;
    
    
    public IReadOnlyCollection<MapFormatViewModel> UsableMapFormats
    {
        get => _usableMapFormats;
        set => this.RaiseAndSetIfChanged(ref _usableMapFormats, value);
    }
    private IReadOnlyCollection<MapFormatViewModel> _usableMapFormats;

    public MapFormatViewModel? CurrentlyUsedMapFormat
    {
        get => _currentlyUsedMapFormat;
        set => this.RaiseAndSetIfChanged(ref _currentlyUsedMapFormat, value);
    }
    private MapFormatViewModel? _currentlyUsedMapFormat = null;
    
    public string? SelectedMapFileName
    {
        get => _selectedMapFileName;
        set => this.RaiseAndSetIfChanged(ref _selectedMapFileName, value);
    }
    private string? _selectedMapFileName = null;

    public ConfigurationViewModel? Configuration
    {
        get => _configuration; 
        set => this.RaiseAndSetIfChanged(ref _configuration, value);
    }
    private ConfigurationViewModel? _configuration; 
    
    public GraphicsSourceViewModel? MapGraphicsSource
    {
        get => _mapGraphicsSource;
        set => this.RaiseAndSetIfChanged(ref _mapGraphicsSource, value);
    }
    private GraphicsSourceViewModel? _mapGraphicsSource = null;
    
    public GraphicsSourceViewModel? MapRepreGraphicsSource
    {
        get => _mapRepreGraphicsSource;
        set => this.RaiseAndSetIfChanged(ref _mapRepreGraphicsSource, value);
    }
    private GraphicsSourceViewModel? _mapRepreGraphicsSource = null;
    
    public int GraphicsWidth
    {
        get => _graphicsWidth;
        set => this.RaiseAndSetIfChanged(ref _graphicsWidth, value);
    }
    private int _graphicsWidth = 0;
    
    public int GraphicsHeight
    {
        get => _graphicsHeight;
        set => this.RaiseAndSetIfChanged(ref _graphicsHeight, value);
    }
    private int _graphicsHeight = 0;
    
    public float GraphicsScale
    {
        get => _graphicsScale;
        set => this.RaiseAndSetIfChanged(ref _graphicsScale, value);
    }
    private float _graphicsScale = 1;
    
    public int ScaledGraphicsWidth => _scaledGraphicsWidth.Value;
    private readonly ObservableAsPropertyHelper<int> _scaledGraphicsWidth;
    
    public int ScaledGraphicsHeight => _scaledGraphicsHeight.Value;
    private readonly ObservableAsPropertyHelper<int> _scaledGraphicsHeight;
    
    
    public string? GraphicsProblemText
    {
        get => _graphicsProblemText;
        set => this.RaiseAndSetIfChanged(ref _graphicsProblemText, value);
    }
    private string? _graphicsProblemText;

    
    public ReactiveCommand<(Stream, string), (MapManager.MapCreationResult, MapFormatViewModel, string)> LoadMapCommand { get; }
    
    public ReactiveCommand<Unit, (GraphicsSourceViewModel, GraphicsSourceViewModel)?> CreateAndViewWholeMapRepreCommand { get; }
    public ReactiveCommand<Unit, (GraphicsSourceViewModel, GraphicsSourceViewModel)?> CreateAndViewMapRepreByPartsCommand { get; }
    
    public ReactiveCommand<float, Unit> ZoomCommand { get; }
    public ReactiveCommand<float, Unit> UnZoomCommand { get; }
    
    public ReactiveCommand<Unit, (GraphicsSourceViewModel, GraphicsSourceViewModel)?> RemoveObjectCommand { get; }
    public ReactiveCommand<Unit, (GraphicsSourceViewModel, GraphicsSourceViewModel)?> AddObjectCommand { get; }
    
}