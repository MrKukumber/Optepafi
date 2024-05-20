using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Metadata;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan;
using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.ParamsMan;
using Optepafi.Models.ParamsMan.Params;
using Optepafi.Models.SearchingAlgorithmMan;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.ModelViews.Main;
using Optepafi.ViewModels.DataViewModels;

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
            DefaultUserModelFilePath = pathFindingParams.UserModelPath;
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
            ISet<IMapRepreRepresentativ<IMapRepresentation>> usableMapRepreReps = MapRepreManager.Instance.GetUsableMapRepreRepsFor(templateViewModel.Template, mapFormatViewModel.MapFormat);
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

    public IReadOnlyCollection<MapFormatViewModel> GetUsableMapFormats(TemplateViewModel? templateViewModel)
    {
        if (templateViewModel is not null)
        {
            var usableTemplateMapFormatCombs = MapRepreManager.Instance.GetAllUsableTemplateMapFormatCombinations();
            return usableTemplateMapFormatCombs
                .Where(comb => comb.Item1 == templateViewModel.Template)
                .Select(comb => new MapFormatViewModel(comb.Item2))
                .ToHashSet();
        }
        return MapManager.Instance.MapFormats.Select(mapFormat => new MapFormatViewModel(mapFormat)).ToHashSet();
    }

    public IEnumerable<TemplateViewModel> GetUsableTemplates(MapFormatViewModel? mapFormatViewModel)
    {
        if (mapFormatViewModel is not null)
        {
            var usableTemplateMapFormatCombs = MapRepreManager.Instance.GetAllUsableTemplateMapFormatCombinations();
            return usableTemplateMapFormatCombs
                .Where(comb => comb.Item2 == mapFormatViewModel.MapFormat)
                .Select(comb => new TemplateViewModel(comb.Item1))
                .ToHashSet();
        }
        return TemplateManager.Instance.Templates.Select(mapFormat => new TemplateViewModel(mapFormat)).ToHashSet();
    }
    
    public IReadOnlyCollection<UserModelTypeViewModel>? GetUsableUserModelTypes(
        TemplateViewModel? templateViewModel)
    {
        return templateViewModel is null ? null : UserModelManager.Instance.GetCorrespondingUserModelTypesTo(templateViewModel.Template)
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


    public abstract void SetElevDataType(ElevDataTypeViewModel? elevDataTypeViewModel);
    public abstract void SetTemplate(TemplateViewModel? templateViewModel);
    public abstract void SetSearchingAlgorithm(SearchingAlgorithmViewModel? searchingAlgorithmViewModel);
    public abstract Task<MapManager.MapCreationResult> LoadAndSetMapAsync(Stream stream, 
        MapFormatViewModel mapFormatViewModel, CancellationToken cancellationToken);
    public abstract Task<UserModelManager.UserModelLoadResult> LoadAndSetUserModelAsync(Stream stream,
        UserModelTypeViewModel userModelTypeViewModel, CancellationToken cancellationToken);
    
    
}

public partial class PathFindingSessionModelView : SessionModelView
{
    private class PFSettingsIntraModelView : PFSettingsModelView
    {
        public PFSettingsIntraModelView(){}
        
        public IElevDataType? ElevDataType { get; private set; }
        public ITemplate? Template { get; private set; }
        public IMap? Map { get; private set; }
        public IUserModel<ITemplate>? UserModel { get; private set; }
        public ISearchingAlgorithm? SearchingAlgorithm { get; private set; }
        public override void SetElevDataType(ElevDataTypeViewModel? elevDataTypeViewModel)
        {
            ElevDataType = elevDataTypeViewModel?.ElevDataType;
        }
        public override void SetTemplate(TemplateViewModel? templateViewModel)
        {
            Template = templateViewModel?.Template;
        }

        public override void SetSearchingAlgorithm(SearchingAlgorithmViewModel? searchingAlgorithmViewModel)
        {
            SearchingAlgorithm = searchingAlgorithmViewModel?.SearchingAlgorithm;
        }

        public override async Task<MapManager.MapCreationResult> LoadAndSetMapAsync(Stream stream, 
            MapFormatViewModel mapFormatViewModel, CancellationToken cancellationToken)
        {
            var (mapCreationResult, map) = await Task.Run(() =>
            {
                var result = MapManager.Instance.GetMapFromOf(stream, mapFormatViewModel.MapFormat, cancellationToken, out IMap? map);
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

        public override async Task<UserModelManager.UserModelLoadResult> LoadAndSetUserModelAsync(Stream stream, 
            UserModelTypeViewModel userModelTypeViewModel, CancellationToken cancellationToken)
        {
            var (userModelCreationResult, userModel) = await Task.Run(() =>
            {
                var result = UserModelManager.Instance.DeserializeUserModelFromOf(stream, userModelTypeViewModel.UserModelType, cancellationToken, out IUserModel<ITemplate>? userModel);
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
    }
}