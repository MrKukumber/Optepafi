using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Optepafi.Models.ElevationDataMan.Distributions;
using Optepafi.Models.GraphicsMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.MapRepreMan.MapRepres.Representatives;
using Optepafi.Models.ParamsMan;
using Optepafi.Models.ParamsMan.Params;
using Optepafi.Models.SearchingAlgorithmMan;
using Optepafi.Models.SearchingAlgorithmMan.SearchingAlgorithms;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan;
using Optepafi.Models.UserModelMan.UserModelReps;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.ModelViews.PathFinding.Utils;
using Optepafi.ViewModels.Data.Graphics;
using Optepafi.ViewModels.Data.Representatives;

namespace Optepafi.ModelViews.PathFinding;

/// <summary>
/// ModelView which is responsible for logic of parameters setting for path finding session.
/// 
/// It uses all sort of managers for:
/// 
/// - retrieving usable templates, map formats, user model types, algorithms and map representation representatives
/// - checking dependencies of individual data types and applying applications logic to provide only consistent options for selection
/// - aggregating map graphics
/// - saving parameter selection for future use
/// 
/// This is an abstract class. The path finding session ModelView will create its successor which will then be able to implement methods of this class by using data hidden from the outside world.  
/// For more information on ModelViews see <see cref="ModelViewBase"/>.  
/// </summary>
public abstract class PFSettingsModelView : ModelViewBase
{
    
    /// <summary>
    /// Upon creation it loads (by previous path finding session) saved parameters and sets them to "default" properties.
    /// 
    /// Corresponding ViewModel can then take these unchecked "default" parameters and execute mechanism for their correctness testing and assign them as default parameters of current session.  
    /// </summary>
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

    /// <summary>
    /// Method for getting <c>ITemplate</c> instance which corresponds to provided type name.
    /// 
    /// It runs through all template instances collected in <c>TemplateManager</c> and returns the corresponding one.  
    /// </summary>
    /// <param name="templateTypeName">Name of templates type.</param>
    /// <returns>Corresponding template if exists. Otherwise null.</returns>
    private ITemplate? GetTemplateByTypeName(string templateTypeName)
    {
        foreach (var template in TemplateManager.Instance.Templates)
        {
            if (template.GetType().Name == templateTypeName)
                return template;
        }
        return null;
    }

    /// <summary>
    /// Method for getting <c>ISearchingAlgorithm</c> instance which corresponds to provided type name.
    /// 
    /// It runs through all searching algorithm instances collected in <c>SearchingAlgorithmManager</c> and returns the corresponding one.  
    /// </summary>
    /// <param name="searchingAlgorithmTypeName">Name of searching algorithms type.</param>
    /// <returns>Corresponding searching algorithm if exists. Otherwise null.</returns>
    private ISearchingAlgorithm? GetSearchingAlgorithmByTypeName(string searchingAlgorithmTypeName)
    {
        foreach (var serachingAlgorithm in SearchingAlgorithmManager.Instance.SearchingAlgorithms)
        {
            if (serachingAlgorithm.GetType().Name == searchingAlgorithmTypeName)
                return serachingAlgorithm;
        }
        return null;
    }

    /// <summary>
    /// Default not checked template retrieved from saved parameters.
    /// </summary>
    private ITemplate? _defaultTemplate;
    /// <summary>
    /// ViewModel for default not checked template retrieved from saved parameters.
    /// </summary>
    public TemplateViewModel? DefaultTemplate => _defaultTemplate is null ? null : new TemplateViewModel(_defaultTemplate);
    /// <summary>
    /// Default not checked searching algorithm retrieved from saved parameters.
    /// </summary>
    private ISearchingAlgorithm? _defaultSearchingAlgorithm;
    /// <summary>
    /// ViewModel for default not checked searching algorithm retrieved from saved parameters.
    /// </summary>
    public SearchingAlgorithmViewModel? DefaultSearchingAlgorithm => _defaultSearchingAlgorithm is null ? null : new SearchingAlgorithmViewModel(_defaultSearchingAlgorithm);
    /// <summary>
    /// Default not checked path of previously used map file retrieved from saved parameters.
    /// </summary>
    public string? DefaultMapFilePath { get; }
    /// <summary>
    /// Default not checked path of previously used user model file retrieved from saved parameters.
    /// </summary>
    public string? DefaultUserModelFilePath { get; }

    /// <summary>
    /// Returns all usable searching algorithms that are able to ensure their service accordingly to provided template, map format and user model type. 
    /// </summary>
    /// <param name="templateViewModel">ViewModel of template according to which are searching algorithms looked for.</param>
    /// <param name="mapFormatViewModel">ViewModel of map format according to which are searching algorithms looked for.</param>
    /// <param name="userModelTypeViewModel">ViewModel of user model type according to which are searching algorithms looked for.</param>
    /// <returns>Usable searching algorithm collection. If some of provided ViewModels is null, returns blank collection.</returns>
    public HashSet<SearchingAlgorithmViewModel> GetUsableAlgorithms(
        TemplateViewModel? templateViewModel, MapFormatViewModel? mapFormatViewModel, UserModelTypeViewModel? userModelTypeViewModel)
    {
        if (templateViewModel is not null && mapFormatViewModel is not null && userModelTypeViewModel is not null)
        {
            HashSet<IMapRepreRepresentative<IMapRepre>> usableMapRepreReps = MapRepreManager.Instance.GetUsableMapRepreRepsFor(templateViewModel.Template, mapFormatViewModel.MapFormat);
            return SearchingAlgorithmManager.Instance.GetUsableAlgorithmsFor(usableMapRepreReps, [userModelTypeViewModel.UserModelType])
                .Select(searchingAlgorithm => new SearchingAlgorithmViewModel(searchingAlgorithm))
                .ToHashSet();
        }
        return [];
    }

    /// <summary>
    /// Returns all map formats.
    /// </summary>
    /// <returns>Collection of map formats.</returns>
    public IReadOnlyCollection<MapFormatViewModel> GetAllMapFormats()
    {
        return MapManager.Instance.MapFormats.Select(mapFormat => new MapFormatViewModel(mapFormat)).ToHashSet();
    }

    /// <summary>
    /// Returns all templates.
    /// </summary>
    /// <returns>Collection of templates.</returns>
    public HashSet<TemplateViewModel> GetAllTemplates()
    {
        return TemplateManager.Instance.Templates.Select(template => new TemplateViewModel(template)).ToHashSet();
    }

    /// <summary>
    /// Method for checking whether provided template and map format are usable combination.
    /// 
    /// That means checking whether there exists some map representation type which corresponds to this combination together with user model type which corresponds to template.  
    /// If there are such map representation and user model types, there must exist at least one searching algorithm that is able to use at least one combination of these types.  
    /// </summary>
    /// <param name="templateViewModel">ViewModel of template whose usability is checked.</param>
    /// <param name="mapFormatViewModel">ViewModel of map format whose usability is checked.</param>
    /// <returns>True if they are usable combination. False otherwise.</returns>
    public bool AreTheyUsableCombination(TemplateViewModel templateViewModel, MapFormatViewModel mapFormatViewModel)
    {
        var usableMapRepreReps = MapRepreManager.Instance.GetUsableMapRepreRepsFor(templateViewModel.Template, mapFormatViewModel.MapFormat);
        var usableUserModelTypes = UserModelManager.Instance.GetCorrespondingUserModelTypesTo(templateViewModel.Template);
        return (SearchingAlgorithmManager.Instance.GetUsableAlgorithmsFor(usableMapRepreReps, usableUserModelTypes) .Count > 0);
    }
    
    /// <summary>
    /// Returns all user model types which are tied to provided template and are able to be used in some searching algorithm together with specific map representation created according to provided template and map format.
    /// 
    /// If provided template or map is null, returns blank collection.  
    /// </summary>
    /// <param name="templateViewModel">ViewModel of template to which user model must be tied.</param>
    /// <param name="mapFormatViewModel">ViewModel of map format which is used together with provided template in map representation that can be used with tested user model in some searching algorithm.</param>
    /// <returns>Collection of user model types which are valid according to provided template and map format.</returns>
    public IReadOnlyCollection<UserModelTypeViewModel> GetUsableUserModelTypes(
        TemplateViewModel? templateViewModel, MapFormatViewModel? mapFormatViewModel)
    {
        if (templateViewModel is null || mapFormatViewModel is null) return [];
        var usableMapRepreReps = MapRepreManager.Instance.GetUsableMapRepreRepsFor(templateViewModel.Template, mapFormatViewModel.MapFormat);
        return UserModelManager.Instance.GetCorrespondingUserModelTypesTo(templateViewModel.Template)
            .Where(userModelType => SearchingAlgorithmManager.Instance.GetUsableAlgorithmsFor(usableMapRepreReps, [userModelType]).Count > 0)
            .Select(usableUserModelType => new UserModelTypeViewModel(usableUserModelType))
            .ToHashSet();
    }


    /// <summary>
    /// Returns map format which corresponds to provided file name (specifically to its extension).
    /// </summary>
    /// <param name="mapFileName">Name of map file.</param>
    /// <returns>Corresponding map format if exists. Null otherwise.</returns>
    public MapFormatViewModel? GetCorrespondingMapFormat(string mapFileName)
    {
        var correspondingMapFormat = MapManager.Instance.GetCorrespondingMapFormatTo(mapFileName);
        return correspondingMapFormat is null ? null : new MapFormatViewModel(correspondingMapFormat);
    }

    /// <summary>
    /// Returns user model type which corresponds to provided serialization file name (specifically to its suffix).
    /// </summary>
    /// <param name="userModelFileName">Name of user model serialization file.</param>
    /// <returns>Corresponding user model type if exists. Null otherwise.</returns>
    public UserModelTypeViewModel? GetCorrespondingUserModelType(string userModelFileName)
    {
        var correspondingUserModelType = UserModelManager.Instance.GetCorrespondingUserModelTypeTo(userModelFileName);
        return correspondingUserModelType is null ? null : new UserModelTypeViewModel(correspondingUserModelType);
    }

    /// <summary>
    /// Method for saving currently used parameters by <c>ParamsManager</c>.
    /// </summary>
    public abstract void SaveParameters();
    
    /// <summary>
    /// Method for setting selected elevation data distribution.
    /// </summary>
    /// <param name="elevDataDistViewModel">ViewModel of selected elevation data distribution.</param>
    public abstract void SetElevDataDistribution(ElevDataDistributionViewModel? elevDataDistViewModel);
    
    /// <summary>
    /// Method for setting chosen template.
    /// </summary>
    /// <param name="templateViewModel">ViewModel of chosen template.</param>
    public abstract void SetTemplate(TemplateViewModel? templateViewModel);
    
    /// <summary>
    /// Method for setting chosen searching algorithm. It also chooses one of suitable map representation representative.
    /// </summary>
    /// <param name="searchingAlgorithmViewModel">ViewModel of chosen searching algorithm.</param>
    public abstract void SetSearchingAlgorithm(SearchingAlgorithmViewModel? searchingAlgorithmViewModel);
    
    /// <summary>
    /// Method for asynchronous loading and setting of map from provided stream. Map will be created by using of provided format.
    /// 
    /// <c>MapManager</c> is used for this time. It lets parse the stream into map object that will then wait for its further use.  
    /// </summary>
    /// <param name="streamWithPath">Stream from which map should be loaded. It comes together with path to map file from which stream was gathered.</param>
    /// <param name="mapFormatViewModel">Map format by which map should be parsed and created.</param>
    /// <param name="cancellationToken">Cancellation token for cancelling maps loading.</param>
    /// <returns>Task with result of maps creation.</returns>
    public abstract Task<MapManager.MapCreationResult> LoadAndSetMapAsync((Stream,string) streamWithPath, 
        MapFormatViewModel mapFormatViewModel, CancellationToken cancellationToken);
    
    /// <summary>
    /// Method for asynchronous loading and setting of user model from provided serialization stream.
    ///
    /// User model will be deserialized by provided user model type. <c>UserModelManager</c> is used for this time. It lets deserialize the stream into user model object that will then wait for its further use.
    /// </summary>
    /// <param name="streamWithPath">Stream from which user model should be deserialized. It comes together with path to user models serialization file from which stream was gathered.</param>
    /// <param name="userModelTypeViewModel">User model type by which user model should be deserialized.</param>
    /// <param name="cancellationToken">Cancellation token for cancelling user models loading.</param>
    /// <returns>Task with result of user models load.</returns>
    public abstract Task<UserModelManager.UserModelLoadResult> LoadAndSetUserModelAsync((Stream,string) streamWithPath,
        UserModelTypeViewModel userModelTypeViewModel, CancellationToken cancellationToken);
    
    
    /// <summary>
    /// Method which sets first found usable map representation representative so that map representation was usable with provided arguments.
    /// </summary>
    /// <param name="templateViewModel">Represents template, which map representation must use.</param>
    /// <param name="mapFormatViewModel">Represents map format, which map representation must use.</param>
    /// <param name="userModelTypeViewModel"></param>
    /// <param name="searchingAlgorithmViewModel"></param>
    public abstract void SetSomeSuitableMapRepreRepresentative(TemplateViewModel templateViewModel, MapFormatViewModel mapFormatViewModel, UserModelTypeViewModel userModelTypeViewModel, SearchingAlgorithmViewModel searchingAlgorithmViewModel);
    
    /// <summary>
    /// Method for retrieving maps graphics so its preview could be displayed to user.
    /// 
    /// It should create <c>GraphicSource</c> whose ViewModel is sent to be displayed for user while it is filled by map graphic objects in parallel.  
    /// </summary>
    /// <returns>Graphics source view model which will be continuously filled by maps graphics.</returns>
    public abstract GraphicsSourceViewModel? GetAndSetLoadedMapGraphics(MapManager.MapCreationResult mapCreationResult);
    
    /// <summary>
    /// It was meant to release map after map representation creation completes and maps graphics is gathered.  
    /// For this time being it does not serve for any purpose and maybe it will be removed in future.  
    /// </summary>
    public abstract void ReleaseMap();

}

public partial class PathFindingSessionModelView
{
    /// <summary>
    /// Successor of <see cref="PFSettingsModelView"/> created by this session ModelView so some of its methods could be implemented by using data hidden from the outside world.
    /// </summary>
    private class PFSettingsIntraModelView : PFSettingsModelView
    {
        /// <inheritdoc cref="PFSettingsModelView.SetElevDataDistribution"/> 
        public override void SetElevDataDistribution(ElevDataDistributionViewModel? elevDataDistViewModel)
        {
            ElevDataDistribution = elevDataDistViewModel?.ElevDataDistribution;
        }
        /// <inheritdoc cref="PFSettingsModelView.SetTemplate"/> 
        public override void SetTemplate(TemplateViewModel? templateViewModel)
        {
            Template = templateViewModel?.Template;
        }
        /// <inheritdoc cref="PFSettingsModelView.SetSearchingAlgorithm"/> 
        public override void SetSearchingAlgorithm(SearchingAlgorithmViewModel? searchingAlgorithmViewModel)
        {
            SearchingAlgorithm = searchingAlgorithmViewModel?.SearchingAlgorithm;

        }
        /// <inheritdoc cref="PFSettingsModelView.LoadAndSetMapAsync"/> 
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
                    MapFormat = mapFormatViewModel.MapFormat;
                    break;
            }
            return mapCreationResult;
        }

        
        /// <inheritdoc cref="PFSettingsModelView.LoadAndSetUserModelAsync"/> 
        public override async Task<UserModelManager.UserModelLoadResult> LoadAndSetUserModelAsync((Stream,string) streamWithPath, 
            UserModelTypeViewModel userModelTypeViewModel, CancellationToken cancellationToken)
        {
            var (userModelCreationResult, userModel) = await Task.Run(() =>
            {
                var result = UserModelManager.Instance.TryDeserializeUserModelOfTypeFrom(streamWithPath, userModelTypeViewModel.UserModelType, cancellationToken, out IUserModel<ITemplate>? userModel);
                return (result, userModel);
            });
            switch (userModelCreationResult)
            {
                case UserModelManager.UserModelLoadResult.Ok:
                    UserModel = userModel;
                    UserModelType = userModelTypeViewModel.UserModelType;
                    break;
            }
            return userModelCreationResult;
        }


        /// <inheritdoc cref="PFSettingsModelView.SetSomeSuitableMapRepreRepresentative"/> 
        public override void SetSomeSuitableMapRepreRepresentative(TemplateViewModel templateViewModel, MapFormatViewModel mapFormatViewModel, UserModelTypeViewModel userModelTypeViewModel, SearchingAlgorithmViewModel searchingAlgorithmViewModel)
        {
            var mapRepresImplementedForTemplateMapFormatCombination = MapRepreManager.Instance.GetUsableMapRepreRepsFor(templateViewModel.Template, mapFormatViewModel.MapFormat);
            var mapRepresUsableWithUserModelTypeInSearchingAlgorithm = MapRepreManager.Instance.GetUsableMapRepreRepsFor(searchingAlgorithmViewModel.SearchingAlgorithm, userModelTypeViewModel.UserModelType);
            mapRepresImplementedForTemplateMapFormatCombination.IntersectWith(mapRepresUsableWithUserModelTypeInSearchingAlgorithm);
            MapRepreRepresentative = mapRepresImplementedForTemplateMapFormatCombination.First();
        }

        /// <inheritdoc cref="PFSettingsModelView.SaveParameters"/> 
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

        /// <inheritdoc cref="PFSettingsModelView.ReleaseMap"/>
        public override void ReleaseMap()
        {
            //TODO: mozno vymazat obsah mapy....zatial nechat tak
        }

        /// <inheritdoc cref="PFSettingsModelView.GetAndSetLoadedMapGraphics"/>
        public override GraphicsSourceViewModel? GetAndSetLoadedMapGraphics(MapManager.MapCreationResult mapCreationResult)
        {
            if (Map is null) throw new NullReferenceException("Map property should be instantiated before calling this method.");
            if (mapCreationResult is not (MapManager.MapCreationResult.Ok or MapManager.MapCreationResult.Incomplete))
                return null;
            IMap map = Map;
            
            GraphicsArea? graphicsArea = GraphicsManager.Instance.GetAreaOf(map);
            if (graphicsArea is null) { return null; }

            CollectingGroundGraphicsSource graphicsSource = new CollectingGroundGraphicsSource(graphicsArea.Value);
            GraphicsSourceViewModel graphicsSourceViewModel = new (graphicsSource);
            MapGraphics = graphicsSource;
            
            Task.Run(() => GraphicsManager.Instance.AggregateMapGraphics(map, graphicsSource.Collector));
            return graphicsSourceViewModel;
        }
        
        
        /// <summary>
        /// Selected elevation data distribution. Other ModelViews may use it for further work.
        /// </summary>
        public IElevDataDistribution? ElevDataDistribution { get; private set; }
        /// <summary>
        /// Chosen template. Other ModelViews may use it for further work.
        /// </summary>
        public ITemplate? Template { get; private set; }
        /// <summary>
        /// Loaded map. Other ModelViews may use it for further work.
        /// </summary>
        public IMap? Map { get; private set; }
        /// <summary>
        /// Selected map format. Other ModelViews may use it for further work.
        /// </summary>
        public IMapFormat<IMap>? MapFormat { get; private set; }
        /// <summary>
        /// Generated map graphics source. Other ModelViews may use it for further work.
        /// </summary>
        public CollectingGroundGraphicsSource? MapGraphics { get; private set; }
        /// <summary>
        /// Loaded user model. Other ModelViews may use it for further work.
        /// </summary>
        public IUserModel<ITemplate>? UserModel { get; private set; }
        /// <summary>
        /// Indicates type of currently set user model. Other ModelViews may use it for further work. 
        /// </summary>
        public IUserModelType<IUserModel<ITemplate>, ITemplate>? UserModelType { get; private set; }
        /// <summary>
        /// Selected map representation representative. Other ModelViews may use it for further work.
        /// </summary>
        public IMapRepreRepresentative<IMapRepre>? MapRepreRepresentative { get; private set; }
        /// <summary>
        /// Chosen searching algorithm. Other ModelViews may use it for further work.
        /// </summary>
        public ISearchingAlgorithm? SearchingAlgorithm { get; private set; }
    }
}