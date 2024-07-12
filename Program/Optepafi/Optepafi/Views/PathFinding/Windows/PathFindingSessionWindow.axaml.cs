using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.ReactiveUI;
using Optepafi.ViewModels.PathFinding;
using ReactiveUI;

namespace Optepafi.Views.PathFinding.Windows;


/// <summary>
/// Window of the path finding session. Instances of this window are created and shown to the user when on new path finding sessions creation.
/// This window is bounded to <see cref="PathFindingSessionViewModel"/>. For more information on path finding windows functionality see documentation of this ViewModel.
/// </summary>
public partial class PathFindingSessionWindow : ReactiveWindow<PathFindingSessionViewModel>
{
    /// <summary>
    /// When this component is activated, it registers handler for map representation creation interaction.
    /// I also subscribes on exit command of the path finding part part of the mechanism. It closes itself when this command executes.
    /// On initialization of this component it collects all defined data templates by calling method <see cref="RecursiveSearchForDataTemplatesIn"/>.
    /// </summary>
    public PathFindingSessionWindow()
    {
        InitializeComponent();

        if (Design.IsDesignMode) return;
        
        this.WhenActivated(d =>
        {
            d(ViewModel!.PathFindingSettings.MapRepreCreationInteraction.RegisterHandler( DoShowMapRepreCreatingDialogAsync));
            d(ViewModel!.PathFinding.ExitCommand.Subscribe(_ => Close()));
            d(ViewModel.MainSettingsProvider.WhenAnyValue(x => x.CurrentCulture)
                .Subscribe(_ =>
                {
                    var currentContent = Content;
                    Content = null;
                    Content = currentContent;
                }));
        });
        RecursiveSearchForDataTemplatesIn(Resources);
    }
    
    /// <summary>
    /// Method for recursive search for data templates that are defined in this windows level resource dictionaries.
    /// It adds every found data template in these dictionaries into <c>DataTemplates</c> collection of this window.
    /// </summary>
    /// <param name="resourceDictionary">Resource dictionary which should be recursively searched for data templates.</param>
    private void RecursiveSearchForDataTemplatesIn(IResourceDictionary resourceDictionary)
    {
        foreach (var entry in resourceDictionary)
        {
            Resources.TryGetResource(entry.Key, this.ActualThemeVariant, out object? value);
            if (value is DataTemplates dataTemplates)
            {
                DataTemplates.AddRange(dataTemplates);
            }
            else if (value is IDataTemplate dataTemplate)
            {
                DataTemplates.Add(dataTemplate);
            }
        }
        foreach (var mergedProvider in resourceDictionary.MergedDictionaries)
        {
            if(mergedProvider is IResourceDictionary mergedDictionary)
                RecursiveSearchForDataTemplatesIn(mergedDictionary);
        }
    }
    
    /// <summary>
    /// Method for handling <c>MapRepreCreationInteraction</c>.
    /// It shows <see cref="MapRepreCreatingDialogWindow"/> with appropriate View and awaits the result of map representation creation.
    /// Result is then set as na output of the interaction.
    /// </summary>
    /// <param name="interaction">Interaction to be handled.</param>
    private async Task DoShowMapRepreCreatingDialogAsync(InteractionContext<MapRepreCreatingViewModel, bool> interaction)
    {
        var dialog = new MapRepreCreatingDialogWindow
        {
            DataContext = interaction.Input,
            Content = new MapRepreCreatingView
            {
                DataContext = interaction.Input
            }
        };
        var result = await dialog.ShowDialog<bool>(this);
        interaction.SetOutput(result);
    }

    private bool _alreadyAsked = false;
    /// <summary>
    /// Method for handling <c>OnClosing</c> event of this window.
    /// It designed in such way it could be cancelled by asynchronous dialog from user.
    /// </summary>
    /// <param name="sender">Sender of the <c>OnClosing</c> event.</param>
    /// <param name="e"><c>OnClosing</c> events arguments.</param>
    private async void Window_OnClosing(object? sender, WindowClosingEventArgs e)
    {
        if (_alreadyAsked) return;
        e.Cancel = true;
        bool close = await ViewModel!.OnClosingCommand.Execute();
        if (close)
        {
            _alreadyAsked = true;
            Close();
        }
    }
    
    /// <summary>
    /// Method for handling <c>OnClosed</c> event of this window.
    /// </summary>
    /// <param name="sender">Sender of th <c>OnClosed</c> event.</param>
    /// <param name="e"><c>OnClosed</c> events arguments.</param>
    private void Window_OnClosed(object? sender, EventArgs e)
    {
        ViewModel!.OnClosedCommand.Execute().Subscribe();
    }

}