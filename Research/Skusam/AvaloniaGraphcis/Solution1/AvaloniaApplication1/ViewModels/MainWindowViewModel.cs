using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;

namespace AvaloniaApplication1.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{

    private string _status;
    public string Status
    {
        get => _status;
        set => this.RaiseAndSetIfChanged(ref _status, value);
    }
    
    public MainWindowViewModel()
    {
        // Create the command that executes a task asynchronously
        ExecuteCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            Status = "Execution started...";

            // Simulate a long-running task
            
            await Task.Run(() => Thread.Sleep(5000)); // 5 seconds delay to simulate a long-running task

            Status = "Execution finished!";
        }, Observable.Return(true));
    }

    // Define the ReactiveCommand for async execution
    public ReactiveCommand<Unit, Unit> ExecuteCommand { get; } 
}