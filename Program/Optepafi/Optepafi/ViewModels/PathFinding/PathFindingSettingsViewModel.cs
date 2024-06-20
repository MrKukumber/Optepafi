using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Data.Converters;
using Avalonia.Platform.Storage;
using Optepafi.Models.MapMan;
using Optepafi.Models.ParamsMan.Params;
using Optepafi.Models.UserModelMan;
using Optepafi.ModelViews.Main;
using Optepafi.ModelViews.ModelCreating;
using Optepafi.ModelViews.PathFinding;
using Optepafi.ViewModels.Data.Graphics;
using Optepafi.ViewModels.Data.Representatives;
using Optepafi.ViewModels.DataViewModels;
using ReactiveUI;

namespace Optepafi.ViewModels.PathFinding;

public class PathFindingSettingsViewModel : PathFindingViewModelBase
{
    private PFSettingsModelView _settingsMv;
    private PFMapRepreCreatingModelView _mapRepreCreatingMv;

    public PathFindingSettingsViewModel(PFSettingsModelView settingsMv, MainSettingsModelView.Provider mainSettingsMvProvider, PFMapRepreCreatingModelView mapRepreCreatingMv)
    {
        _settingsMv = settingsMv;
        _mapRepreCreatingMv = mapRepreCreatingMv;

        UsableTemplates = _settingsMv.GetAllUsableTemplates();
        UsableMapFormats = _settingsMv.GetAllUsableMapFormats();
        UsableUserModelTypes = _settingsMv.GetUsableUserModelTypes(SelectedTemplate);
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

        this.WhenAnyValue(x => x.SelectedTemplate)
            .Subscribe(selectedTemplate =>
            {
                _settingsMv.SetTemplate(selectedTemplate);
                if (selectedTemplate is not null && CurrentlyUsedMapFormat is not null)
                {
                    if (!_settingsMv.AreTheyUsableCombination(selectedTemplate, CurrentlyUsedMapFormat))
                    {
                        CurrentlyUsedMapFormat = null;
                        SelectedMapFileName = null;
                        SelectedMapFilePath = null;
                    }
                }

                UsableUserModelTypes = _settingsMv.GetUsableUserModelTypes(selectedTemplate);
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
        
        
        // this.WhenAnyValue(x => x.UsableMapFormats)
            // .Subscribe(usableMapFormats =>
            // {
                // if (usableMapFormats is null || !usableMapFormats.Contains(CurrentlyUsedMapFormat))
                // {
                    // CurrentlyUsedMapFormat = null;
                    // SelectedMapFileName = null;
                    // SelectedMapFilePath = null;
                // }
            // });
        // this.WhenAnyValue(x => x.UsableUserModelTypes)
            // .Subscribe(usableUserModelTypes =>
            // {
                // if (usableUserModelTypes is null || !usableUserModelTypes.Contains(CurrentlyUsedUserModelType))
                // {
                    // CurrentlyUsedUserModelType = null;
                    // SelectedUserModelFileName = null;
                    // SelectedUserModelFilePath = null;
                // }
            // });

        LoadMapCommand = ReactiveCommand.CreateFromObservable(((Stream, string) mapFileStreamAndPath) => Observable
            .StartAsync(async cancellationToken =>
            {
                var (mapFileStream, mapFilePath) = mapFileStreamAndPath;
                string mapFileName = Path.GetFileName(mapFilePath);
                MapFormatViewModel? mapFormat = _settingsMv.GetCorrespondingMapFormat(mapFileName);
                if (mapFormat is null) throw new NullReferenceException("Map format should be returned, because chosen file was filtered to be correct.");
                var loadResult = await _settingsMv.LoadAndSetMapAsync(mapFileStreamAndPath, mapFormat, cancellationToken);
                var mapGraphics = _settingsMv.GetAndSetLoadedMapGraphics();
                mapFileStream.Dispose();
                return (loadResult, mapFormat, mapFilePath, mapGraphics);
            })
            .TakeUntil(LoadMapCommand));
        
        LoadUserModelCommand = ReactiveCommand.CreateFromObservable(((Stream, string) userModelFileStreamAndPath) => Observable.StartAsync(async (CancellationToken cancellationToken) => 
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

        MapRepreCreationInteraction = new Interaction<MapRepreCreatingWindowViewModel, bool>();
        ProceedTroughMapRepreCreationCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            bool successfulCreation = await MapRepreCreationInteraction.Handle(new MapRepreCreatingWindowViewModel(_mapRepreCreatingMv));
            if (successfulCreation)
            {
                _settingsMv.SaveParameters();
                // _settingsMv.ReleaseMap();
                Task.Run(() => SelectedMapsPreview = null);
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
            CurrentlyUsedUserModelType = userModelType is not null && UsableUserModelTypes is not null && UsableUserModelTypes.Contains(userModelType) ? userModelType : CurrentlyUsedUserModelType; 
            
            UsableSearchingAlgorithms = settingsMv.GetUsableAlgorithms(SelectedTemplate, CurrentlyUsedMapFormat, CurrentlyUsedUserModelType);
            SearchingAlgorithmViewModel? searchingAlgorithm = settingsMv.DefaultSearchingAlgorithm;
            if (searchingAlgorithm is not null && UsableSearchingAlgorithms is not null && UsableSearchingAlgorithms.Contains(searchingAlgorithm))
                SelectedSearchingAlgorithm = searchingAlgorithm;
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
    


    private ElevDataDistributionViewModel? _currentlySelectedElevDataDistribution;
    public ElevDataDistributionViewModel? CurrentlySelectedElevDataDistribution 
    { 
        get => _currentlySelectedElevDataDistribution ; 
        set => this.RaiseAndSetIfChanged(ref _currentlySelectedElevDataDistribution, value); 
    }


    private TemplateViewModel? _selectedTemplate;
    public TemplateViewModel? SelectedTemplate
    {
        get => _selectedTemplate;
        set => this.RaiseAndSetIfChanged(ref _selectedTemplate, value);
    }
    private IEnumerable<TemplateViewModel> _usableTemplates;
    public IEnumerable<TemplateViewModel> UsableTemplates
    {
        get => _usableTemplates;
        set => this.RaiseAndSetIfChanged(ref _usableTemplates, value);
    }
    
    

    private SearchingAlgorithmViewModel? _selectedSearchingAlgorithm;
    public SearchingAlgorithmViewModel? SelectedSearchingAlgorithm
    {
        get => _selectedSearchingAlgorithm;
        set => this.RaiseAndSetIfChanged(ref _selectedSearchingAlgorithm, value);
    }
    private IReadOnlyCollection<SearchingAlgorithmViewModel>? _usableSearchingAlgorithms;
    public IReadOnlyCollection<SearchingAlgorithmViewModel>? UsableSearchingAlgorithms
    {
        get => _usableSearchingAlgorithms;
        set => this.RaiseAndSetIfChanged(ref _usableSearchingAlgorithms, value);
    }
    
    

    private MapFormatViewModel? _currentlyUsedMapFormat;
    public MapFormatViewModel? CurrentlyUsedMapFormat
    {
        get => _currentlyUsedMapFormat;
        set => this.RaiseAndSetIfChanged(ref _currentlyUsedMapFormat, value);
    }
    private string? _selectedMapFileName;
    public string? SelectedMapFileName
    {
        get => _selectedMapFileName;
        set => this.RaiseAndSetIfChanged(ref _selectedMapFileName, value);
    }
    private string? SelectedMapFilePath { get; set; }
    private IReadOnlyCollection<MapFormatViewModel>? _usableMapFormats;
    public IReadOnlyCollection<MapFormatViewModel>? UsableMapFormats
    {
        get => _usableMapFormats;
        set => this.RaiseAndSetIfChanged(ref _usableMapFormats, value);
    }

    private GraphicsSourceViewModel? _selectedMapsPreview;
    public GraphicsSourceViewModel? SelectedMapsPreview
    {
        get => _selectedMapsPreview;
        set => this.RaiseAndSetIfChanged(ref _selectedMapsPreview, value);
    }
    
    

    private UserModelTypeViewModel? _currentlyUsedUserModelType;
    public UserModelTypeViewModel? CurrentlyUsedUserModelType
    {
        get => _currentlyUsedUserModelType;
        set => this.RaiseAndSetIfChanged(ref _currentlyUsedUserModelType, value);
    }
    private string? _selectedUserModelFileName;
    public string? SelectedUserModelFileName
    {
        get => _selectedUserModelFileName;
        set => this.RaiseAndSetIfChanged(ref _selectedUserModelFileName, value);
    }
    private string? SelectedUserModelFilePath { get; set; }
    private IReadOnlyCollection<UserModelTypeViewModel>? _usableUserModelTypes;
    public IReadOnlyCollection<UserModelTypeViewModel>? UsableUserModelTypes
    {
        get => _usableUserModelTypes;
        set => this.RaiseAndSetIfChanged(ref _usableUserModelTypes, value);
    }
    
    
    
    public ReactiveCommand<(Stream, string), (MapManager.MapCreationResult, MapFormatViewModel, string, GraphicsSourceViewModel?)> LoadMapCommand { get; }
    public ReactiveCommand<(Stream, string), (UserModelManager.UserModelLoadResult, UserModelTypeViewModel, string)> LoadUserModelCommand { get; }
    
    //TODO: command pre prechod do vytvaranaia mapovej reprezentacie, nech necha ukladanie parametrov na modelView-u a ten tam uklada datove triedy, nie ich ViewModelove wrappre
    public enum WhereToProceed{Settings, PathFinding}
    public ReactiveCommand<Unit, WhereToProceed> ProceedTroughMapRepreCreationCommand { get; }
    public Interaction<MapRepreCreatingWindowViewModel, bool> MapRepreCreationInteraction { get; }
    public static FuncValueConverter<IEnumerable, bool> IsNotEmptyNorNull { get; } =
        new (enumerable => enumerable?.GetEnumerator().MoveNext() ?? false);
}