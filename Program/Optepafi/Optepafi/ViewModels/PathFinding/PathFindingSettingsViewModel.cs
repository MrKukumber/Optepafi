using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Security;
using System.Threading;
using Avalonia.Data.Converters;
using Avalonia.Platform.Storage;
using Optepafi.Models.MapMan;
using Optepafi.Models.ParamsMan.Params;
using Optepafi.Models.UserModelMan;
using Optepafi.ModelViews.Main;
using Optepafi.ModelViews.ModelCreating;
using Optepafi.ModelViews.PathFinding;
using Optepafi.ViewModels.DataViewModels;
using ReactiveUI;

namespace Optepafi.ViewModels.PathFinding;

public class PathFindingSettingsViewModel : ViewModelBase
{
    private PFSettingsModelView _settingsModelView;
    public IDisposable? LoadMapCommandSubscription { get; }
    public IDisposable? LoadUserModelCommandSubscription { get; }
    public PathFindingSettingsViewModel(PFSettingsModelView settingsModelView, MainSettingsModelView mainSettingsModelView)
    {
        _settingsModelView = settingsModelView;

        UsableTemplates = _settingsModelView.GetUsableTemplates(CurrentlyUsedMapFormat);
        UsableMapFormats = _settingsModelView.GetUsableMapFormats(SelectedTemplate);
        UsableSearchingAlgorithms = _settingsModelView.GetUsableAlgorithms(SelectedTemplate, CurrentlyUsedMapFormat);
        UsableUserModelTypes = _settingsModelView.GetUsableUserModelTypes(SelectedTemplate);

        this.WhenAnyValue(x => x.CurrentlySelectedElevDataType)
            .Subscribe(currentlySelectedElevDataType =>
            {
                _settingsModelView.SetElevDataType(currentlySelectedElevDataType);
            });
        
        this.WhenAnyValue(x => x.SelectedTemplate, 
                x => x.CurrentlyUsedMapFormat)
            .Subscribe(tuple =>
            {
                var (template, mapFormat) = tuple;
                //if selectedTemplate null or currenlyUsedMap null, vrat null
                UsableSearchingAlgorithms = _settingsModelView.GetUsableAlgorithms(template, mapFormat);
            });

        this.WhenAnyValue(x => x.SelectedTemplate)
            .Subscribe(selectedTemplate =>
            {
                _settingsModelView.SetTemplate(selectedTemplate);
                //if selectedTemplate null, vrat vsetky mapove formaty
                UsableMapFormats = _settingsModelView.GetUsableMapFormats(selectedTemplate);
                //if selectedTEmplate null, vrat null
                UsableUserModelTypes = _settingsModelView.GetUsableUserModelTypes(selectedTemplate);
            });

        this.WhenAnyValue(x => x.CurrentlyUsedMapFormat)
            .Subscribe(currentlyUsedMapFormat =>
            {
                //if currentlyUsedMapFormat, vrat vsetky templaty
                UsableTemplates = _settingsModelView.GetUsableTemplates(currentlyUsedMapFormat);
            });

        this.WhenAnyValue(x => x.SelectedSearchingAlgorithm)
            .Subscribe(selectedSearchingAlgorithm =>
            {
                _settingsModelView.SetSearchingAlgorithm(selectedSearchingAlgorithm);
            });
        
        
        this.WhenAnyValue(x => x.UsableMapFormats)
            .Subscribe(usableMapFormats =>
            {
                if (usableMapFormats is null || !usableMapFormats.Contains(CurrentlyUsedMapFormat))
                {
                    CurrentlyUsedMapFormat = null;
                    SelectedMapFileName = null;
                    SelectedMapFilePath = null;
                }
            });
        this.WhenAnyValue(x => x.UsableUserModelTypes)
            .Subscribe(usableUserModelTypes =>
            {
                if (usableUserModelTypes is null || !usableUserModelTypes.Contains(CurrentlyUsedUserModelType))
                {
                    CurrentlyUsedUserModelType = null;
                    SelectedUserModelFileName = null;
                    SelectedUserModelFilePath = null;
                }
            });

        LoadMapCommand = ReactiveCommand.CreateFromTask(async ((Stream, string) mapFileStreamAndPath, CancellationToken cancellationToken) =>
        {
            var (mapFileStream, mapFilePath) = mapFileStreamAndPath;
            string mapFileName = Path.GetFileName(mapFilePath);
            MapFormatViewModel? mapFormat = _settingsModelView.GetCorrespondingMapFormat(mapFileName);
            if (mapFormat is null) throw new NullReferenceException("Map format should be returned, because chosen file was filtered to be correct.");
            var loadResult = await _settingsModelView.LoadAndSetMapAsync(mapFileStream, mapFormat, cancellationToken);
            if (!cancellationToken.IsCancellationRequested) ; //TODO: nechat asynchronne (Task.Run()) zavolat vytvorenie grafickeho znazornenia mapy a mozno vykreslit ju na obrazovke....mzono to skor spravit cez dalsi command, nakolko to nema nic moc spolocne s nahravanim mapy....treba dat ale pozor na cancellation token a ukoncenie ziskavania grafickej reprezentaci, cize predsa len to mozno spravit v tomto mieste asynchronne, aby som mohol predat cancellation token
            return (loadResult, mapFormat, mapFilePath);
        });
        
        
        LoadUserModelCommand = ReactiveCommand.CreateFromTask(async ((Stream, string) userModelFileStreamAndPath, CancellationToken cancellationToken) =>
        {
            var (userModelFileStream, userModelFilePath) = userModelFileStreamAndPath;
            string userModelFileName = Path.GetFileName(userModelFilePath);
            UserModelTypeViewModel? userModelType = _settingsModelView.GetCorrespondingUserModelType(userModelFileName);
            if (userModelType is null) throw new NullReferenceException(" User model type should be returned, because chosen file was filtered to be correct.");
            var loadResult = await _settingsModelView.LoadAndSetUserModelAsync(userModelFileStream, userModelType, cancellationToken);
            return (loadResult, userModelType, userModelFilePath); });

        LoadMapCommand.Subscribe(commandOutput =>
        {
            var (mapLoadingResult, mapFormat, mapFilePath) = commandOutput;
            switch (mapLoadingResult)
            {
                case MapManager.MapCreationResult.Incomplete:
                    //TODO vypisat hlasku, ze vytvorena mapa bude nekompletna, teda z velkej pravdepodobnosti nepouzitelna
                    // v modelView-u uz nastavena tato mapa, takze urcite nenechavat aby sa uzivatel mohol navratit ku predchadzajucej
                case MapManager.MapCreationResult.Ok:
                    CurrentlyUsedMapFormat = mapFormat;
                    SelectedMapFileName = Path.GetFileName(mapFilePath);
                    SelectedMapFilePath = mapFilePath;
                    break; 
                case MapManager.MapCreationResult.Cancelled:
                    break;
                case MapManager.MapCreationResult.FileNotFound:
                    //TODO: vypisat nejaku errorovu hlasku, nechat vsetko tak ako bolo
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

        CurrentlySelectedElevDataType = mainSettingsModelView.CurrentElevDataType;

        SelectedTemplate = settingsModelView.DefaultTemplate;

        if (settingsModelView.DefaultMapFilePath is not null)
        {
            using (Stream? mapFileStream = TryFindAndOpenFile(settingsModelView.DefaultMapFilePath))
            {
                if (mapFileStream is not null)
                    LoadMapCommandSubscription = LoadMapCommand
                        .Execute((mapFileStream, settingsModelView.DefaultMapFilePath))
                        .Subscribe();
            }
        }
        
        if (settingsModelView.DefaultUserModelFilePath is not null)
        {
            UserModelTypeViewModel? userModelType =
                _settingsModelView.GetCorrespondingUserModelType(Path.GetFileName(settingsModelView.DefaultUserModelFilePath));
            if (userModelType is not null && UsableUserModelTypes is not null &&
                UsableUserModelTypes.Contains(userModelType))
            {
                using (Stream? userModelFileStream = TryFindAndOpenFile(settingsModelView.DefaultUserModelFilePath))
                {
                    if (userModelFileStream is not null)
                        LoadUserModelCommandSubscription = LoadUserModelCommand
                            .Execute((userModelFileStream, settingsModelView.DefaultUserModelFilePath))
                            .Subscribe();
                }
            }
        }
        
        SearchingAlgorithmViewModel? searchingAlgorithm = settingsModelView.DefaultSearchingAlgorithm;
        if (searchingAlgorithm is not null && UsableSearchingAlgorithms is not null && UsableSearchingAlgorithms.Contains(searchingAlgorithm))
            SelectedSearchingAlgorithm = searchingAlgorithm;

    }

    private Stream? TryFindAndOpenFile(string path)
    {
        if (Path.Exists(path))
        {
            try
            {
                FileStream stream = new FileStream(path, FileMode.Open);
                return stream;
            }
            catch (IOException) { /*TODO: log IOException....mozno*/ }
            catch (SecurityException) { /*TODO: log IOException....mozno*/ }
            catch (ArgumentException) { /*TODO: log IOException....mozno*/ }
        }
        return null;
    }
    


    private ElevDataTypeViewModel? _currentlySelectedElevDataType;
    public ElevDataTypeViewModel? CurrentlySelectedElevDataType 
    { 
        get => _currentlySelectedElevDataType ; 
        set => this.RaiseAndSetIfChanged(ref _currentlySelectedElevDataType, value); 
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
    
    
    
    public ReactiveCommand<(Stream, string), (MapManager.MapCreationResult, MapFormatViewModel, string)> LoadMapCommand { get; }
    public ReactiveCommand<(Stream, string), (UserModelManager.UserModelLoadResult, UserModelTypeViewModel, string)> LoadUserModelCommand { get; }
    
    //TODO: command pre prechod do vytvaranaia mapovej reprezentacie, nech necha ukladanie parametrov na modelView-u a ten tam uklada datove triedy, nie ich ViewModelove wrappre
    
    public static FuncValueConverter<IEnumerable, bool> IsNotEmptyNorNull { get; } =
        new (enumerable => enumerable?.GetEnumerator().MoveNext() ?? false);
}