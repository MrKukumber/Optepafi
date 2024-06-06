using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Metadata;
using DynamicData;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.Graphics;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.MapRepreMan;
using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.ParamsMan;
using Optepafi.Models.ParamsMan.Params;
using Optepafi.Models.SearchingAlgorithmMan;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.ModelViews.Graphics;
using Optepafi.ModelViews.Graphics.Collectors;
using Optepafi.ModelViews.Main;
using Optepafi.ViewModels.DataViewModels;
using Optepafi.ViewModels.Graphics;

namespace Optepafi.ModelViews.PathFinding;

public abstract class PFSettingsModelView : ModelViewBase
{
    protected PFSettingsModelView()
    {
        PathFindingParams? pathFindingParams;
        if ((pathFindingParams = ParamsManager.Instance.GetParams<PathFindingParams>()) is not null)
        {
            _defaultTemplate = GetTemplateByTypeName(pathFindingParams.TemplateTypeName);
            _defaultSearchingAlgorithm = GetSearchingAlgorithmByTypeName(pathFindingParams.SearchingAlgorithmTypeName);
            DefaultMapFilePath = pathFindingParams.MapFilePath;
            DefaultUserModelFilePath = pathFindingParams.UserModelFilePath;
        }
    }

    private ITemplate? GetTemplateByTypeName(string templateTypeName)
    {
        foreach (var template in TemplateManager.Instance.Templates)
        {
            if (template.GetType().Name == templateTypeName)
                return template;
        }
        return null;
    }

    private ISearchingAlgorithm? GetSearchingAlgorithmByTypeName(string searchingAlgorithmTypeName)
    {
        foreach (var serachingAlgorithm in SearchingAlgorithmManager.Instance.SearchingAlgorithms)
        {
            if (serachingAlgorithm.GetType().Name == searchingAlgorithmTypeName)
                return serachingAlgorithm;
        }
        return null;
    }

    private ITemplate? _defaultTemplate;
    public TemplateViewModel? DefaultTemplate => _defaultTemplate is null ? null : new TemplateViewModel(_defaultTemplate);
    private ISearchingAlgorithm? _defaultSearchingAlgorithm;
    public SearchingAlgorithmViewModel? DefaultSearchingAlgorithm => 
        _defaultSearchingAlgorithm is null ? null : new SearchingAlgorithmViewModel(_defaultSearchingAlgorithm);
    public string? DefaultMapFilePath { get; }
    public string? DefaultUserModelFilePath { get; }

    public IReadOnlyCollection<SearchingAlgorithmViewModel>? GetUsableAlgorithms(
        TemplateViewModel? templateViewModel, MapFormatViewModel? mapFormatViewModel)
    {
        if (templateViewModel is not null && mapFormatViewModel is not null)
        {
            ISet<IMapRepreRepresentative<IMapRepre>> usableMapRepreReps = MapRepreManager.Instance.GetUsableMapRepreRepsFor(templateViewModel.Template, mapFormatViewModel.MapFormat);
            HashSet<SearchingAlgorithmViewModel> usableSearchingAlgorithms = new HashSet<SearchingAlgorithmViewModel>();
            foreach (var usableMapRepre in usableMapRepreReps)
            {
                usableSearchingAlgorithms.UnionWith(
                    SearchingAlgorithmManager.Instance.GetUsableAlgorithmsFor(usableMapRepre)
                        .Select(usableSearchingAlgorithm => new SearchingAlgorithmViewModel(usableSearchingAlgorithm)));
            }
            return usableSearchingAlgorithms;
        }
        return null;
    }

    public IReadOnlyCollection<MapFormatViewModel> GetAllUsableMapFormats()
    {
        var usableTemplateMapFormatCombs = MapRepreManager.Instance.GetAllUsableTemplateMapFormatCombinations();
        return usableTemplateMapFormatCombs
            .Select(comb => new MapFormatViewModel(comb.Item2))
            .ToHashSet(); //Gets rid of duplicate MapFormatViewModels.
    }

    public IEnumerable<TemplateViewModel> GetAllUsableTemplates()
    {
        var usableTemplateMapFormatCombs = MapRepreManager.Instance.GetAllUsableTemplateMapFormatCombinations();
        return usableTemplateMapFormatCombs
            .Select(comb => new TemplateViewModel(comb.Item1))
            .ToHashSet(); //Gets rid of duplicate TemplateViewModels.
    }

    public bool AreTheyUsableCombination(TemplateViewModel templateViewModel, MapFormatViewModel mapFormatViewModel)
    {
        var usableMapRepreReps = MapRepreManager.Instance.GetUsableMapRepreRepsFor(templateViewModel.Template, mapFormatViewModel.MapFormat);
        return SearchingAlgorithmManager.Instance.GetUsableAlgorithmsFor(usableMapRepreReps.ToArray()).Count > 0;
    }
    
    public IReadOnlyCollection<UserModelTypeViewModel>? GetUsableUserModelTypes(
        TemplateViewModel? templateViewModel)
    {
        return templateViewModel is null ? null : UserModelManager.Instance.GetCorrespondingUserModelTypesTo(templateViewModel.Template)
            .Where(userModelType => UserModelManager.Instance.DoesRepresentComputingModelTiedTo(userModelType, templateViewModel.Template))
            .Select(usableUserModelType => new UserModelTypeViewModel(usableUserModelType))
            .ToHashSet();
    }


    public MapFormatViewModel? GetCorrespondingMapFormat(string mapFileName)
    {
        var correspondingMapFormat = MapManager.Instance.GetCorrespondingMapFormatTo(mapFileName);
        return correspondingMapFormat is null ? null : new MapFormatViewModel(correspondingMapFormat);
    }

    public UserModelTypeViewModel? GetCorrespondingUserModelType(string userModelFileName)
    {
        var correspondingUserModelType = UserModelManager.Instance.GetCorrespondingUserModelTypeTo(userModelFileName);
        return correspondingUserModelType is null ? null : new UserModelTypeViewModel(correspondingUserModelType);
    }

    public abstract void SaveParameters();

    public abstract void SetElevDataDistribution(ElevDataDistributionViewModel? elevDataDistViewModel);
    public abstract void SetTemplate(TemplateViewModel? templateViewModel);
    public abstract void SetSearchingAlgorithm(SearchingAlgorithmViewModel? searchingAlgorithmViewModel);
    public abstract Task<MapManager.MapCreationResult> LoadAndSetMapAsync((Stream,string) streamWithPath, 
        MapFormatViewModel mapFormatViewModel, CancellationToken cancellationToken);
    public abstract Task<UserModelManager.UserModelLoadResult> LoadAndSetUserModelAsync((Stream,string) streamWithPath,
        UserModelTypeViewModel userModelTypeViewModel, CancellationToken cancellationToken);
    public abstract GraphicsViewModel? GetLoadedMapGraphics();

}

public partial class PathFindingSessionModelView : SessionModelView
{
    private class PFSettingsIntraModelView : PFSettingsModelView
    {
        public PFSettingsIntraModelView(){}
        
        public IElevDataDistribution? ElevDataDistribution { get; private set; }
        public ITemplate? Template { get; private set; }
        public IMap? Map { get; private set; }
        public IUserModel? UserModel { get; private set; }
        public IMapRepreRepresentative<IMapRepre>? MapRepreRepresentative { get; private set; }
        public ISearchingAlgorithm? SearchingAlgorithm { get; private set; }
        public override void SetElevDataDistribution(ElevDataDistributionViewModel? elevDataDistViewModel)
        {
            ElevDataDistribution = elevDataDistViewModel?.ElevDataDistribution;
        }
        public override void SetTemplate(TemplateViewModel? templateViewModel)
        {
            Template = templateViewModel?.Template;
        }

        public override void SetSearchingAlgorithm(SearchingAlgorithmViewModel? searchingAlgorithmViewModel)
        {
            SearchingAlgorithm = searchingAlgorithmViewModel?.SearchingAlgorithm;
            MapRepreRepresentative = searchingAlgorithmViewModel is not null
                ? MapRepreManager.Instance.GetUsableMapRepreRepsFor(searchingAlgorithmViewModel.SearchingAlgorithm)
                    .First()
                : null;

        }

        public override async Task<MapManager.MapCreationResult> LoadAndSetMapAsync((Stream,string) streamWithPath, 
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
                    break;
            }
            return mapCreationResult;
        }

        public override async Task<UserModelManager.UserModelLoadResult> LoadAndSetUserModelAsync((Stream,string) streamWithPath, 
            UserModelTypeViewModel userModelTypeViewModel, CancellationToken cancellationToken)
        {
            var (userModelCreationResult, userModel) = await Task.Run(() =>
            {
                var result = UserModelManager.Instance.TryDeserializeUserModelOfTypeFrom(streamWithPath, userModelTypeViewModel.UserModelType, cancellationToken, out IUserModel? userModel);
                return (result, userModel);
            });
            switch (userModelCreationResult)
            {
                case UserModelManager.UserModelLoadResult.Ok:
                    UserModel = userModel;
                    break;
            }
            return userModelCreationResult;
        }

        public override void SaveParameters()
        {
            ParamsManager.Instance.SetParams(new PathFindingParams
            {
                TemplateTypeName = Template!.GetType().Name,
                SearchingAlgorithmTypeName = SearchingAlgorithm!.GetType().Name,
                MapFilePath = Map!.FilePath,
                UserModelFilePath = UserModel!.FilePath 
            });        
        }

        public override GraphicsViewModel? GetLoadedMapGraphics()
        {
            if (Map is null) throw new NullReferenceException("Map property should be instantiated before calling this method.");
            IMap map = Map;
            
            var extremes = GraphicsManager.Instance.GetAxisExtremesOf(map);
            if (extremes is null) { return null; }
            var (minXPos, minYPos, maxXPos, maxYPos) = extremes.Value;
            
            GraphicsViewModel graphicsViewModel = new (maxXPos - minXPos, maxYPos - minYPos);
            ICollection<GraphicObjectViewModel> graphicObjectViewModels = graphicsViewModel.GraphicObjectsCollection;
            GraphicsObjects2VmConvertingCollector collector = new (graphicObjectViewModels,ConvertersCollections.MapObjects2VmConverters, minXPos, minYPos);
            
            Task.Run(() => GraphicsManager.Instance.AggregateMapGraphics(map, collector));
            return graphicsViewModel;
        }
    }
}