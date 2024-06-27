using System.Reactive;
using ReactiveUI;

namespace Optepafi.ViewModels.Main;

/// <summary>
/// ViewModel associated with <c>YseNoDialogWindow</c>.
/// It contains reactive commands for users positive and negative responses as well text of message which should be shown to user.
/// </summary>
public class YesNoDialogWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Constructs new instance of this ViewModel.
    /// It retrieves text of message for user, text of positive answer and text of negative answer.
    /// It initialize reactive commands which are used for returning of users will.
    /// </summary>
    /// <param name="message">Message to be shown to user.</param>
    /// <param name="yesText">Text of positive response to message.</param>
    /// <param name="noText">Text of negative response to message</param>
    public YesNoDialogWindowViewModel(string message, string yesText, string noText)
    {
        Message = message;
        YesText = yesText;
        NoText = noText;
        YesCommand = ReactiveCommand.Create(() => true);
        NoCommand = ReactiveCommand.Create(() => false);
    }
    
    /// <summary>
    /// Message to be shown to user in dialog window.
    /// </summary>
    public string Message { get; }
    /// <summary>
    /// Text of positive response to message.
    /// </summary>
    public string YesText { get; }
    /// <summary>
    /// Text of negative response to message.
    /// </summary>
    public string NoText { get; }
    
    /// <summary>
    /// Reactive command used for returning of positive response to message.
    /// </summary>
    public ReactiveCommand<Unit, bool> YesCommand { get; }
    /// <summary>
    /// Reactive command used for returning of negative response to message.
    /// </summary>
    public ReactiveCommand<Unit, bool> NoCommand { get; }
}