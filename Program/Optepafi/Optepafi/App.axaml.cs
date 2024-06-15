using System.Collections;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Templates;
using HarfBuzzSharp;
using Optepafi.ModelViews.Main;
using Optepafi.ViewModels;
using Optepafi.ViewModels.Main;
using Optepafi.Views;
using Optepafi.Views.Main;
using Optepafi.Views.ModelCreating;
using Optepafi.Views.PathFinding;
using MainWindow = Optepafi.Views.Main.MainWindow;

namespace Optepafi;

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