using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Security;
using System.Threading.Tasks;
using Optepafi.Models.MapMan;
using Optepafi.Models.UserModelMan;
using Optepafi.ModelViews.Main;
using Optepafi.ModelViews.PathFinding;
using Optepafi.ViewModels.Data.Graphics;
using Optepafi.ViewModels.Data.Representatives;
using ReactiveUI;

namespace Optepafi.ViewModels.PathFinding;

/// <summary>
/// ViewModel which is responsible for control over parameter setting for path finding session.
/// Its tasks include:
/// - overseeing of parameter selection by user. It secures validity of selected parameters by restricting availability of users actions.
/// - use of corresponding ModelViews methods for retrieving of needed data and for setting of their inner parameters which will be used further in path finding mechanism
/// - providing interaction for map representation creation and handling its result
/// - providing data ViewModels for their displaying to user. 
/// - initiate save of lastly used parameters after successful creation of map representation, before proceeding further through path finding mechanism
///
/// For mor information on path finding ViewModels see <see cref="PathFindingViewModelBase"/>.
/// </summary>
public class PathFindingSettingsViewModel : PathFindingViewModelBase
{
    /// <summary>
    /// Corresponding ModelView to this ViewModel used for providing data and services over Model layer of application.
    /// </summary>
    private PFSettingsModelView _settingsMv;
    /// <summary>
    /// Map representation creation ModelView used in handling of map representation creation interaction as corresponding ModelView.
    /// </summary>
    private PFMapRepreCreatingModelView _mapRepreCreatingMv;

    /// <summary>
    /// Constructs path findings settings ViewModel.
    /// It initialize all reactive constructs and creates various reactions to them.
    /// It also includes mechanism for initializing of default parameters based on saved parameters by previously run session.
    /// </summary>
    /// <param name="settingsMv">Corresponding ModelView to this ViewModel.</param>
    /// <param name="mainSettingsMvProvider">Provider of main settings ModelView. It is used for main parameters retrieval.</param>
    /// <param name="mapRepreCreatingMv">Map representation creation ModelView used in handling of map representation creation interaction.</param>
    public PathFindingSettingsViewModel(PFSettingsModelView settingsMv, MainSettingsModelView.Provider mainSettingsMvProvider, PFMapRepreCreatingModelView mapRepreCreatingMv)
    {
        _settingsMv = settingsMv;
        _mapRepreCreatingMv = mapRepreCreatingMv;

        UsableTemplates = _settingsMv.GetAllTemplates();
        UsableMapFormats = _settingsMv.GetAllMapFormats(); 
        UsableUserModelTypes = _settingsMv.GetUsableUserModelTypes(SelectedTemplate, CurrentlyUsedMapFormat);
        UsableSearchingAlgorithms = _settingsMv.GetUsableAlgorithms(SelectedTemplate, CurrentlyUsedMapFormat, CurrentlyUsedUserModelType);

        this.WhenAnyValue(x => x.CurrentlySelectedElevDataDistribution)
            .Subscribe(currentlySelectedElevDataDistribution =>
            {
                _settingsMv.SetElevDataDistribution(currentlySelectedElevDataDistribution);
            });
        
        this.WhenAnyValue(x => x.SelectedTemplate, 
                x => x.CurrentlyUsedMapFormat,
                x => x.CurrentlyUsedUserModelType)
            .Subscribe(tuple =>
            {
                var (template, mapFormat, userModelType) = tuple;
                UsableSearchingAlgorithms = _settingsMv.GetUsableAlgorithms(template, mapFormat, userModelType);
            });
        this.WhenAnyValue(x => x.SelectedTemplate,
                x => x.CurrentlyUsedMapFormat)
            .Subscribe(tuple =>
            {
                var (template, mapFormat) = tuple;
                UsableUserModelTypes = _settingsMv.GetUsableUserModelTypes(template, mapFormat);
                if (!UsableUserModelTypes.Contains(CurrentlyUsedUserModelType))
                {
                    CurrentlyUsedUserModelType = null;
                    SelectedUserModelFileName = null;
                    SelectedUserModelFilePath = null;
                }
            });

        this.WhenAnyValue(x => x.SelectedTemplate)
            .Subscribe(selectedTemplate =>
            {
                _settingsMv.SetTemplate(selectedTemplate);
                if (!_settingsMv.AreTheyUsableCombination(selectedTemplate, CurrentlyUsedMapFormat))
                {
                    CurrentlyUsedMapFormat = null;
                    SelectedMapFileName = null;
                    SelectedMapFilePath = null;
                }
            });

        this.WhenAnyValue(x => x.CurrentlyUsedMapFormat)
            .Subscribe(currentlyUsedMapFormat =>
            {
                if (SelectedTemplate is not null && currentlyUsedMapFormat is not null)
                {
                    if (!_settingsMv.AreTheyUsableCombination(SelectedTemplate, currentlyUsedMapFormat))
                        SelectedTemplate = null;
                }
            });

        this.WhenAnyValue(x => x.SelectedSearchingAlgorithm)
            .Subscribe(selectedSearchingAlgorithm =>
            {
                _settingsMv.SetSearchingAlgorithm(selectedSearchingAlgorithm);
            });

        this.WhenAnyValue(x => x.UsableSearchingAlgorithms)
            .Subscribe(usableSearchingAlgorithms =>
            {
                if (SelectedSearchingAlgorithm is not null && !usableSearchingAlgorithms.Contains( SelectedSearchingAlgorithm))
                    SelectedSearchingAlgorithm = null;
            });
        this.WhenAnyValue(x => x.UsableTemplates)
            .Subscribe(usableTemplates =>
            {
                if (SelectedTemplate is not null && !usableTemplates.Contains(SelectedTemplate))
                    SelectedTemplate = null;
            });
        
        LoadMapCommand = ReactiveCommand.CreateFromObservable(((Stream, string) mapFileStreamAndPath) => Observable
            .StartAsync(async cancellationToken =>
            {
                var (mapFileStream, mapFilePath) = mapFileStreamAndPath;
                string mapFileName = Path.GetFileName(mapFilePath);
                MapFormatViewModel? mapFormat = _settingsMv.GetCorrespondingMapFormat(mapFileName);
                if (mapFormat is null) throw new NullReferenceException("Map format should be returned, because chosen file was filtered to be correct.");
                var loadResult = await _settingsMv.LoadAndSetMapAsync(mapFileStreamAndPath, mapFormat, cancellationToken);
                var mapGraphics = _settingsMv.GetAndSetLoadedMapGraphics(loadResult);
                mapFileStream.Dispose();
                return (loadResult, mapFormat, mapFilePath, mapGraphics);
            })
            .TakeUntil(LoadMapCommand));
        
        LoadUserModelCommand = ReactiveCommand.CreateFromObservable(((Stream, string) userModelFileStreamAndPath) => Observable.StartAsync(async cancellationToken => 
        { 
            var (userModelFileStream, userModelFilePath) = userModelFileStreamAndPath; 
            string userModelFileName = Path.GetFileName(userModelFilePath);
            UserModelTypeViewModel? userModelType = _settingsMv.GetCorrespondingUserModelType(userModelFileName);
            if (userModelType is null) throw new NullReferenceException(" User model type should be returned, because chosen file was filtered to be correct.");
            var loadResult = await _settingsMv.LoadAndSetUserModelAsync(userModelFileStreamAndPath, userModelType, cancellationToken);
            userModelFileStream.Dispose();
            return (loadResult, userModelType, userModelFilePath);
        }).TakeUntil(LoadUserModelCommand) );
        
        LoadMapCommand.Subscribe(commandOutput =>
        {
            var (mapLoadingResult, mapFormat, mapFilePath, mapGraphics) = commandOutput;
            switch (mapLoadingResult)
            {
                case MapManager.MapCreationResult.Incomplete:
                    // TODO: vypisat hlasku, ze vytvorena mapa bude nekompletna, teda z velkej pravdepodobnosti nepouzitelna
                    // v modelView-u uz nastavena tato mapa, takze urcite nenechavat aby sa uzivatel mohol navratit ku predchadzajucej
                case MapManager.MapCreationResult.Ok:
                    CurrentlyUsedMapFormat = mapFormat;
                    SelectedMapFileName = Path.GetFileName(mapFilePath);
                    SelectedMapFilePath = mapFilePath;
                    SelectedMapsPreview = mapGraphics;
                    break; 
                case MapManager.MapCreationResult.Cancelled:
                    break;
                case MapManager.MapCreationResult.UnableToParse:
                    //TODO: vypisat nejaku errorovu hlasku, nechat vsetko tak ako bolo
                    break;
            }
        });
        LoadUserModelCommand.Subscribe(commandOutput =>
        {
            var (userModelLoadingResult, userModelType, userModelFilePath) = commandOutput;
            switch (userModelLoadingResult)
            {
                case UserModelManager.UserModelLoadResult.Ok:
                    CurrentlyUsedUserModelType = userModelType;
                    SelectedUserModelFileName = Path.GetFileName(userModelFilePath);
                    SelectedUserModelFilePath = userModelFilePath;
                    break;
                case UserModelManager.UserModelLoadResult.Canceled:
                    break;
                case UserModelManager.UserModelLoadResult.UnableToDeserialize:
                    //TODO vypisat nejaku errorovu hlasku, nechat vsetko tak ako bolo 
                    break;
                case UserModelManager.UserModelLoadResult.UnableToReadFromFile:
                    //TODO vypisat nejaku errorovu hlasku, nechat vsetko tak ako bolo 
                    break;
            }
        });

        IObservable<bool> isEverythingSet = this.WhenAnyValue(x => x.SelectedTemplate,
            x => x.CurrentlyUsedMapFormat,
            x => x.SelectedSearchingAlgorithm,
            x => x.CurrentlyUsedUserModelType,
            (template, mapFormat, searchingAlgorithm, userModel) => template is not null && mapFormat is not null && searchingAlgorithm is not null && userModel is not null);

        MapRepreCreationInteraction = new Interaction<MapRepreCreatingViewModel, bool>();
        ProceedTroughMapRepreCreationCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            _settingsMv.SetSomeSuitableMapRepreRepresentative(SelectedTemplate!, CurrentlyUsedMapFormat!, CurrentlyUsedUserModelType!, SelectedSearchingAlgorithm!);
            bool successfulCreation = await MapRepreCreationInteraction.Handle(new MapRepreCreatingViewModel(_mapRepreCreatingMv));
            if (successfulCreation)
            {
                _settingsMv.SaveParameters();
                _ = Task.Run(() => SelectedMapsPreview = null);
                return WhereToProceed.PathFinding;
            }
            return WhereToProceed.Settings;
        }, isEverythingSet);

        
        CurrentlySelectedElevDataDistribution = mainSettingsMvProvider.CurrentElevDataDistribution;

        SelectedTemplate = settingsMv.DefaultTemplate;

        if (settingsMv.DefaultMapFilePath is not null && settingsMv.DefaultUserModelFilePath is not null)
        {
            CurrentlyUsedMapFormat = settingsMv.GetCorrespondingMapFormat(Path.GetFileName(settingsMv.DefaultMapFilePath));
            var userModelType = settingsMv.GetCorrespondingUserModelType(Path.GetFileName(settingsMv.DefaultUserModelFilePath));
            CurrentlyUsedUserModelType = userModelType is not null && UsableUserModelTypes.Contains(userModelType) ? userModelType : CurrentlyUsedUserModelType; 
            
            UsableSearchingAlgorithms = settingsMv.GetUsableAlgorithms(SelectedTemplate, CurrentlyUsedMapFormat, CurrentlyUsedUserModelType);
            SearchingAlgorithmViewModel? searchingAlgorithm = settingsMv.DefaultSearchingAlgorithm;
            if (searchingAlgorithm is not null && UsableSearchingAlgorithms.Contains(searchingAlgorithm))
            {
                SelectedSearchingAlgorithm = searchingAlgorithm;
            }
        }
        
        if (CurrentlyUsedMapFormat is not null && settingsMv.DefaultMapFilePath is not null)
        {
            Stream? mapFileStream = TryFindAndOpenFile(settingsMv.DefaultMapFilePath);
            if (mapFileStream is not null)
                LoadMapCommand
                    .Execute((mapFileStream, settingsMv.DefaultMapFilePath))
                    .Subscribe(commandOutput =>
                    {
                        var (_, mapFormat,_,_) = commandOutput;
                        CurrentlyUsedMapFormat = mapFormat;
                    });
            else
            { 
                CurrentlyUsedMapFormat = null;
            }
        }


        if (CurrentlyUsedUserModelType is not null && settingsMv.DefaultUserModelFilePath is not null)
        {
            Stream? userModelFileStream = TryFindAndOpenFile(settingsMv.DefaultUserModelFilePath);
            if (userModelFileStream is not null)
                LoadUserModelCommand
                    .Execute((userModelFileStream, settingsMv.DefaultUserModelFilePath))
                    .Subscribe(commandOutput =>
                    {
                        var (_, userModelType, _) = commandOutput;
                        CurrentlyUsedUserModelType = userModelType;
                    });
            else
            {
                CurrentlyUsedUserModelType = null;
            }
        }
    }

    /// <summary>
    /// Helper method for finding and opening of file with provided path.
    /// Stream gathered from this file is returned if opening of file was successful.
    /// </summary>
    /// <param name="path">Path of file which stream should be opened.</param>
    /// <returns>Stream if opening of file was successful. Null otherwise.</returns>
    private Stream? TryFindAndOpenFile(string path)
    {
        if (Path.Exists(path))
        {
            try
            {
                FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                return stream;
            }
            catch (IOException) { /*TODO: log IOException....mozno*/ }
            catch (SecurityException) { /*TODO: log IOException....mozno*/ }
            catch (ArgumentException) { /*TODO: log IOException....mozno*/ }
        }
        return null;
    }
    


    /// <summary>
    /// Property which indicates currently selected and used elevation data distribution.
    /// It raises notification about change of its value.
    /// </summary>
    public ElevDataDistributionViewModel? CurrentlySelectedElevDataDistribution 
    { 
        get => _currentlySelectedElevDataDistribution ; 
        set => this.RaiseAndSetIfChanged(ref _currentlySelectedElevDataDistribution, value); 
    }
    private ElevDataDistributionViewModel? _currentlySelectedElevDataDistribution;

    /// <summary>
    /// Property which indicates currently selected template. 
    /// It raises notification about change of its value.
    /// </summary>
    public TemplateViewModel? SelectedTemplate
    {
        get => _selectedTemplate;
        set => this.RaiseAndSetIfChanged(ref _selectedTemplate, value);
    }
    private TemplateViewModel? _selectedTemplate;
    /// <summary>
    /// Collection of usable templates in current state of parameter setting.
    /// It raises notification about change of its value.
    /// </summary>
    public IEnumerable<TemplateViewModel> UsableTemplates
    {
        get => _usableTemplates;
        set => this.RaiseAndSetIfChanged(ref _usableTemplates, value);
    }
    private IEnumerable<TemplateViewModel> _usableTemplates;
    
    
    /// <summary>
    /// Currently selected searching algorithm.
    /// It raises notification about change of its value.
    /// </summary>
    public SearchingAlgorithmViewModel? SelectedSearchingAlgorithm
    {
        get => _selectedSearchingAlgorithm;
        set => this.RaiseAndSetIfChanged(ref _selectedSearchingAlgorithm, value);
    }
    private SearchingAlgorithmViewModel? _selectedSearchingAlgorithm;
    /// <summary>
    /// Collection of usable searching algorithms in current state of parameter setting.
    /// It raises notification about change of its value.
    /// </summary>
    public IReadOnlySet<SearchingAlgorithmViewModel> UsableSearchingAlgorithms
    {
        get => _usableSearchingAlgorithms;
        set => this.RaiseAndSetIfChanged(ref _usableSearchingAlgorithms, value);
    }
    private IReadOnlySet<SearchingAlgorithmViewModel> _usableSearchingAlgorithms;
    
    
    /// <summary>
    /// Map format of currently chosen map.
    /// It raises notification about change of its value.
    /// </summary>
    public MapFormatViewModel? CurrentlyUsedMapFormat
    {
        get => _currentlyUsedMapFormat;
        set => this.RaiseAndSetIfChanged(ref _currentlyUsedMapFormat, value);
    }
    private MapFormatViewModel? _currentlyUsedMapFormat;
    /// <summary>
    /// Name of currently selected maps file. 
    /// It raises notification about change of its value.
    /// </summary>
    public string? SelectedMapFileName
    {
        get => _selectedMapFileName;
        set => this.RaiseAndSetIfChanged(ref _selectedMapFileName, value);
    }
    private string? _selectedMapFileName;
    /// <summary>
    /// Path of currently selected maps file.
    /// </summary>
    private string? SelectedMapFilePath { get; set; }
    /// <summary>
    /// Collection of usable map formats in current state of parameter setting.
    /// It raises notification about change of its value.
    /// </summary>
    public IReadOnlyCollection<MapFormatViewModel> UsableMapFormats
    {
        get => _usableMapFormats;
        set => this.RaiseAndSetIfChanged(ref _usableMapFormats, value);
    }
    private IReadOnlyCollection<MapFormatViewModel> _usableMapFormats;
    /// <summary>
    /// Graphics source of selected map used for showing its preview.
    /// It raises notification about change of its value.
    /// </summary>
    public GraphicsSourceViewModel? SelectedMapsPreview
    {
        get => _selectedMapsPreview;
        set => this.RaiseAndSetIfChanged(ref _selectedMapsPreview, value);
    }
    private GraphicsSourceViewModel? _selectedMapsPreview;
    
    
    /// <summary>
    /// User model type of currently selected user model.
    /// It raises notification about change of its value.
    /// </summary>
    public UserModelTypeViewModel? CurrentlyUsedUserModelType
    {
        get => _currentlyUsedUserModelType;
        set => this.RaiseAndSetIfChanged(ref _currentlyUsedUserModelType, value);
    }
    private UserModelTypeViewModel? _currentlyUsedUserModelType;
    /// <summary>
    /// Name of currently selected user model file. 
    /// It raises notification about change of its value.
    /// </summary>
    public string? SelectedUserModelFileName
    {
        get => _selectedUserModelFileName;
        set => this.RaiseAndSetIfChanged(ref _selectedUserModelFileName, value);
    }
    private string? _selectedUserModelFileName;
    /// <summary>
    /// Path of currently selected user model file.
    /// </summary>
    private string? SelectedUserModelFilePath { get; set; }
    /// <summary>
    /// Collection of usable user model types in current state of parameter setting.
    /// It raises notification about change of its value.
    /// </summary>
    public IReadOnlyCollection<UserModelTypeViewModel> UsableUserModelTypes
    {
        get => _usableUserModelTypes;
        set => this.RaiseAndSetIfChanged(ref _usableUserModelTypes, value);
    }
    private IReadOnlyCollection<UserModelTypeViewModel> _usableUserModelTypes;
    
    
    /// <summary>
    /// Enumeration of options which indicates where to proceed from path finding settings ViewModel when all parameters are set and map representation is created.
    /// </summary>
    public enum WhereToProceed{Settings, PathFinding}
    
    /// <summary>
    /// Reactive command for loading of selected map from the stream.
    /// On input it gets stream to be parsed and path of file from which stream was generated.
    /// At first it finds out which map format corresponds to extension of files name. It should be secured by View that only files of usable map formats can be chosen by user.
    /// Then comes map loading itself. Loaded map will be of detected format and will be parsed from the stream. This process will run asynchronously so the UI stayed responsive.
    /// After finishing loading map its graphic representation is loaded. Source of map graphics will be returned immediately but in the background can run asynchronous process that will concurrently generate graphic objects and fill the source with them.
    /// At the end is the stream disposed and result of load is returned for anyone who would care to subscribe on this command.
    /// Commands execution will take until its next execution takes place. In that case current execution is cancelled. 
    /// </summary>
    public ReactiveCommand<(Stream, string), (MapManager.MapCreationResult, MapFormatViewModel, string, GraphicsSourceViewModel?)> LoadMapCommand { get; }
    /// <summary>
    /// Reactive command for loading of selected user model from the stream.
    /// On input it gets stream to be parsed and path of file from which stream was generated.
    /// At first it finds out which user model type corresponds to files name suffix. It should be secured by View that only files of usable user model types can be chosen by user.
    /// Then comes user model loading itself. Loaded user model will be of detected type and will be deserialized from the stream. This process will run asynchronously so the UI stayed responsive.
    /// At the end is the stream disposed and result of load is returned for anyone who would care to subscribe on this command.
    /// Commands execution will take until its next execution takes place. In that case current execution is cancelled. 
    /// </summary>
    public ReactiveCommand<(Stream, string), (UserModelManager.UserModelLoadResult, UserModelTypeViewModel, string)> LoadUserModelCommand { get; }
    /// <summary>
    /// Reactive command for proceeding from settings to map representations creation.
    /// It can be executed only when every necessary parameter is set.
    /// At the start it lets application set some representative of map representation, that corresponds to selected template, map format, user model type and searching algorithm.
    /// Then it calls for handling map representation creation interaction. This interaction is handled by View preferably by dialog Window.
    /// After interactions end the indicator of successful map representations creation is returned.
    /// If creation of map repre. was successful application proceeds.
    /// If it was not it stays in settings of path finding.
    /// The path finding session ViewModel subscribes on result of this command and changes current ViewModel according to its result.
    /// </summary>
    public ReactiveCommand<Unit, WhereToProceed> ProceedTroughMapRepreCreationCommand { get; }
    /// <summary>
    /// Interaction to be handled when map representations creation takes place.
    /// Corresponding View should implement handler for this interaction nad secure its correct execution, preferably using dialog window.
    /// Argument of this interaction is map representation creation ModelView which should be used for processing of maps repre. creation.
    /// Result of interaction states whether creation of map representation was successful.
    /// </summary>
    public Interaction<MapRepreCreatingViewModel, bool> MapRepreCreationInteraction { get; }
}