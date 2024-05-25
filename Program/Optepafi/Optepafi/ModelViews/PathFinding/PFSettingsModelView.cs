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
        TemplateViewModel? templateViewModel, MapRepresentativeViewModel? mapFormatViewModel)
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

    public IReadOnlyCollection<MapRepresentativeViewModel> GetUsableMapFormats(TemplateViewModel? templateViewModel)
    {
        if (templateViewModel is not null)
        {
            var usableTemplateMapFormatCombs = MapRepreManager.Instance.GetAllUsableTemplateMapFormatCombinations();
            return usableTemplateMapFormatCombs
                .Where(comb => comb.Item1 == templateViewModel.Template)
                .Select(comb => new MapRepresentativeViewModel(comb.Item2))
                .ToHashSet();
        }
        return MapManager.Instance.MapFormats.Select(mapFormat => new MapRepresentativeViewModel(mapFormat)).ToHashSet();
    }

    public IEnumerable<TemplateViewModel> GetUsableTemplates(MapRepresentativeViewModel? mapFormatViewModel)
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


    public MapRepresentativeViewModel? GetCorrespondingMapFormat(string mapFileName)
    {
        var correspondingMapFormat = MapManager.Instance.GetCorrespondingMapFormatTo(mapFileName);
        return correspondingMapFormat is null ? null : new MapRepresentativeViewModel(correspondingMapFormat);
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
        MapRepresentativeViewModel mapRepresentativeViewModel, CancellationToken cancellationToken);
    public abstract Task<UserModelManager.UserModelLoadResult> LoadAndSetUserModelAsync((Stream,string) streamWithPath,
        UserModelTypeViewModel userModelTypeViewModel, CancellationToken cancellationToken);
    
    
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
            MapRepresentativeViewModel mapRepresentativeViewModel, CancellationToken cancellationToken)
        {
            var (mapCreationResult, map) = await Task.Run(() =>
            {
                var result = MapManager.Instance.TryGetMapFromOf(streamWithPath, mapRepresentativeViewModel.MapFormat, cancellationToken, out IMap? map);
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
                UserModelFilePath = UserModel.FilePath 
            });        
        }
    }
}