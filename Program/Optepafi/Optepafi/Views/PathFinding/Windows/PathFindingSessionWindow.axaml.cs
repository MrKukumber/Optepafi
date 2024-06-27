using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.ReactiveUI;
using Optepafi.ViewModels.PathFinding;
using ReactiveUI;

namespace Optepafi.Views.PathFinding.Windows;


public partial class PathFindingSessionWindow : ReactiveWindow<PathFindingSessionViewModel>
{
    public PathFindingSessionWindow()
    {
        InitializeComponent();

        if (Design.IsDesignMode) return;
        
        this.WhenActivated(d =>
        {
            d(ViewModel!.PathFindingSettings.MapRepreCreationInteraction.RegisterHandler( DoShowMapRepreCreatingDialogAsync));
            d(ViewModel!.PathFinding.ExitCommand.Subscribe(_ => Close()));
        });
        RecursiveSearchForDataTemplatesIn(Resources);
    }

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

    private bool _alreadyAsked = false;
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

    private void Window_OnClosed(object? sender, EventArgs e)
    {
        ViewModel!.OnClosedCommand.Execute().Subscribe();
    }

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
}