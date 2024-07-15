using System;
using System.Reactive;
using System.Reactive.Linq;
using Optepafi.ModelViews.Main;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

/// <summary>
/// ViewModel of the main window. It is the top layer of main windows logic.
/// 
/// It contains all ViewModels which are used in main window and have impact on whole applications behaviour. They do not correspond to some individual session.  
/// Additionally it handles closing main window and therefore closing of whole application.  
/// For more information on ViewModels in general see <see cref="ViewModelBase"/>.  
/// </summary>
public class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Corresponding ModelView to this ViewModel
    /// </summary>
    private MainWindowModelView _mainWindowModelView;
    /// <summary>
    /// Main menu ViewModel. It is the first ViewModel which corresponding View is shown to user on start of application.
    /// </summary>
    public MainMenuViewModel MainMenu { get; }
    /// <summary>
    /// Main setting ViewModel. Used for setting of parameters that have impact on whole apllication.
    /// </summary>
    public MainSettingsViewModel MainSettings { get; }
    /// <summary>
    /// Constructs new instance of this ViewModel.
    /// 
    /// It initialize all reactive constructs and initialize with main window associated ViewModels.  
    /// </summary>
    /// <param name="mainWindowModelView">Main window ModelView which contains ModelViews of all parts of application that are associated with main window.</param>
    public MainWindowViewModel(MainWindowModelView mainWindowModelView )
    {
        _mainWindowModelView = mainWindowModelView;
        MainSettings = new MainSettingsViewModel(_mainWindowModelView.MainSettings); 
        MainMenu = new MainMenuViewModel(MainSettings.ProviderOfSettings);
        CurrentViewModel = MainMenu;

        this.WhenAnyObservable(x => x.MainMenu.GoToSettingsCommand)
            .Subscribe(_ => CurrentViewModel = MainSettings);
        this.WhenAnyObservable(x => x.MainSettings.GoToMainMenuCommand)
            .Subscribe(_ => CurrentViewModel = MainMenu);
        
        YesNoInteraction = new Interaction<YesNoDialogWindowViewModel, bool>();
        OnClosingCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (MainMenu.Sessions.Count > 0)
            {
                return await YesNoInteraction
                    .Handle(new YesNoDialogWindowViewModel("There are some opend sessions.\nDo you realy wish to exit?", "Yes", "No")); //TODO: localize
            }
            return true;
        });
        OnClosedCommand = ReactiveCommand.Create(() =>
        {
            _mainWindowModelView.SaveParams();
        });
    }
    /// <summary>
    /// Property which contains currently used ViewModel in main window.
    /// 
    /// It raises notification about change of its value.  
    /// </summary>
    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }
    private ViewModelBase _currentViewModel;

    /// <summary>
    /// Interaction which is handled when user has to be asked some question.
    /// 
    /// Corresponding View should implement handler for this interaction and secure its correct execution, preferably using dialog window.  
    /// Argument of this interaction is <c>YseNoDialogWindowViewModel</c> which contains question to be asked to user.  
    /// Result of interaction states whether user response was positive or negative.  
    /// </summary>
    public Interaction<YesNoDialogWindowViewModel, bool> YesNoInteraction { get; }
    
    /// <summary>
    /// Reactive command which is executed when windows OnClosing event is detected.
    /// 
    /// In this command user can be asked if he is sure that he wants to close the application in some circumstances.  
    /// It returns indicator whether application should be closed or not.  
    /// View should be able to prevent closing of window in case that returning value is false. Also in those cases, when some dialog with user is handled.  
    /// </summary>
    public ReactiveCommand<Unit, bool> OnClosingCommand { get; }
    /// <summary>
    /// Reactive command which is executed when windows OnClosed event is detected.
    /// 
    /// In this command application should take all precautions for its correct exiting.  
    /// it should save all parameters for use in future applications runs.  
    /// </summary>
    public ReactiveCommand<Unit, Unit> OnClosedCommand { get; }
}