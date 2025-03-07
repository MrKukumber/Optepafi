using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Markup.Xaml.Templates;
using MapRepreViewer.Models.MapPartitioningMan;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.ElevationDataMan.Distributions;
using Optepafi.Models.ElevationDataMan.Distributions.Specific.Simulating;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.GraphicsMan.Sources;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapMan.MapRepresentatives;
using Optepafi.Models.MapRepreMan;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.MapRepreMan.MapRepres.Representatives;
using Optepafi.Models.MapRepreMan.MapRepres.Representatives.Specific;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.Templates;
using Optepafi.Models.Utils.Configurations;
using Optepafi.ModelViews.PathFinding.Utils;
using Optepafi.ViewModels.Data.Configuration;
using Optepafi.ViewModels.Data.Graphics;
using Optepafi.ViewModels.Data.Reports;
using Optepafi.ViewModels.Data.Representatives;

namespace MapRepreViewer.ModelViews;

public class MapRepreViewingModelView
{
    public IReadOnlyCollection<MapFormatViewModel> GetAllMapFormats()
    {
        return new HashSet<MapFormatViewModel>() { new MapFormatViewModel(OmapMapRepresentative.Instance) };
    }

    public MapFormatViewModel? GetCorrespondingMapFormat(string mapFileName)
    {
        var correspondingMapFormat = MapManager.Instance.GetCorrespondingMapFormatTo(mapFileName);
        return correspondingMapFormat is null ? null : new MapFormatViewModel(correspondingMapFormat);
    }

    public ConfigurationViewModel? GetDefaultConfigurationOfCurrentMapRepre()
    {
        if (MapRepreRepresentative is null) return null;
        var defaultConfigurationCopy = MapRepreRepresentative.DefaultConfigurationDeepCopy;
        return new ConfigurationViewModel(defaultConfigurationCopy);
    }
    
    public async Task<MapManager.MapCreationResult> LoadAndSetMapAsync((Stream,string) streamWithPath, 
        MapFormatViewModel mapFormatViewModel, CancellationToken cancellationToken)
    {
        var (mapCreationResult, map) = await Task.Run(() =>
        {
            var result = MapManager.Instance.TryGetMapFromOf(streamWithPath, mapFormatViewModel.MapFormat, cancellationToken, out IMap? map);
            return (result, map);
        });
        switch (mapCreationResult)
        {
            case MapManager.MapCreationResult.Ok:
            case MapManager.MapCreationResult.Incomplete:
                Map = map;
                MapFormat = mapFormatViewModel.MapFormat;
                break;
        }
        return mapCreationResult;
    }

    public void SetWholeMapForUse() => MapForUse = Map;

    public async Task<(bool, bool)> TrySetPartitionMapOfSizeForUseAsync(int partitionSize,
        CancellationToken cancellationToken)
    {
        if (Map is null)throw new NullReferenceException(nameof(Map) + " property should be instantiated before calling this method.");
        if (MapPartitioningManager.Instance.TryGetMapPartition(Map, partitionSize, cancellationToken, out IMap? paritionedMap, out bool returnedWholeMap))
        {
            if (cancellationToken.IsCancellationRequested) return (false, false);
            MapForUse = paritionedMap;
            return (true, returnedWholeMap);
        }
        return (false, false);
    }

    private bool _useElevData = false;
    public async Task CreateMapRepreAsync(ConfigurationViewModel mapRepresentationConfigurationVm, CancellationToken cancellationToken)
    {
        if (MapForUse is null || MapRepreRepresentative is null) throw new NullReferenceException(nameof(MapForUse) + " and  " + nameof(MapRepreRepresentative) + " property should be instantiated before calling this method.");
        IConfiguration mapRepresentationConfiguration = mapRepresentationConfigurationVm.Configuration;
        if (_useElevData)
        {
            if (ElevDataDistribution is null) throw new NullReferenceException(nameof(ElevDataDistribution) + " property should be instantiated before calling this method.");
            if (MapForUse is IAreaQueryableMap areaQueryableMap)
            {
                IElevData elevData = await Task.Run(() =>
                    ElevDataManager.Instance.GetElevDataFromDistFor(areaQueryableMap, ElevDataDistribution, cancellationToken));
                if (cancellationToken.IsCancellationRequested) return;
                
                MapRepresentation = await Task.Run(() => 
                    MapRepreManager.Instance.CreateMapRepre(Template, areaQueryableMap, 0, MapRepreRepresentative, elevData, mapRepresentationConfiguration, null, cancellationToken));
                if (cancellationToken.IsCancellationRequested) MapRepresentation = null;
            }
            else throw new InvalidOperationException("There is some error in prerequisites check method, that allows _useElevData to be set to true, when map is not even IGeoLocatedMap.");
        }
        else
        {
            MapRepresentation = await Task.Run(() =>
                MapRepreManager.Instance.CreateMapRepre(Template, MapForUse, 0, MapRepreRepresentative, mapRepresentationConfiguration, null, cancellationToken));
            if (cancellationToken.IsCancellationRequested) MapRepresentation = null;
        }
    }

    public GraphicsSourceViewModel? GetMapGraphics()
    {
        if (MapForUse is null) throw new NullReferenceException(nameof(MapForUse) + " property should be instantiated before calling this method.");
        IMap map = MapForUse;
        GraphicsArea? graphicsArea = GraphicsManager.Instance.GetAreaOf(map);
        if (graphicsArea is null) { return null; }

        CollectingGroundGraphicsSource graphicsSource = new CollectingGroundGraphicsSource(graphicsArea.Value);
        GraphicsSourceViewModel graphicsSourceViewModel = new (graphicsSource);
        MapGraphics = graphicsSource;
        
        Task.Run(() => GraphicsManager.Instance.AggregateMapGraphics(map, graphicsSource.Collector));
        return graphicsSourceViewModel;
    }

    public GraphicsSourceViewModel GetMapRepreGraphics()
    {
        if (MapRepresentation is null || MapGraphics is null) throw new NullReferenceException(nameof(MapRepresentation) + " and " + nameof(MapGraphics) + " property should be instantiated before calling this method.");
        IMapRepre mapRepre = MapRepresentation;

        CollectingGraphicsSource graphicsSource = new CollectingGraphicsSource();
        GraphicsSourceViewModel graphicsSourceViewModel = new (graphicsSource, MapGraphics);
        
        Task.Run(() => GraphicsManager.Instance.AggregateMapRepreGraphics(mapRepre, graphicsSource.Collector));
        return graphicsSourceViewModel;
    }
    
    
    private IMap? Map { get; set; }
    private IMap? MapForUse { get; set; }
    private IMapFormat<IMap>? MapFormat { get; set; }
    private IMapRepre? MapRepresentation { get; set; }
    private IGroundGraphicsSource? MapGraphics { get; set; }
    private ITemplate Template { get; set; } = Orienteering_ISOM_2017_2.Instance;
    private IMapRepreRepresentative<IMapRepre>? MapRepreRepresentative { get; set; } = CompleteNetIntertwiningMapRepreRep.Instance;
    private IElevDataDistribution? ElevDataDistribution { get; set; }

}