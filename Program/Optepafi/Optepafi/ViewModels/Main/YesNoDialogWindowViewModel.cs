using System.Reactive;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

public class YesNoDialogWindowViewModel : ViewModelBase
{
    public YesNoDialogWindowViewModel(string message, string yesText, string noText)
    {
        Message = message;
        YesText = yesText;
        NoText = noText;
        YesCommand = ReactiveCommand.Create(() => true);
        NoCommand = ReactiveCommand.Create(() => false);
    }
    
    public string Message { get; }
    public string YesText { get; }
    public string NoText { get; }
    
    public ReactiveCommand<Unit, bool> YesCommand { get; }
    public ReactiveCommand<Unit, bool> NoCommand { get; }
}