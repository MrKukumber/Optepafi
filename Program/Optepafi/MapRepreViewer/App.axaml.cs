using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using MapRepreViewer.ModelViews;
using MapRepreViewer.ViewModels;
using MapRepreViewer.Views;

namespace MapRepreViewer;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        RecursiveSearchForDataTemplatesIn(Resources);
        DataTemplates.Add(new ViewLocator());
    }
    
    private void RecursiveSearchForDataTemplatesIn(IResourceDictionary resourceDictionary)
    {
        foreach (var mergedProvider in resourceDictionary.MergedDictionaries)
        {
            if(mergedProvider is IResourceDictionary mergedDictionary)
                RecursiveSearchForDataTemplatesIn(mergedDictionary);
        }
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
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(new MainWindowModelView()),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}