using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Optepafi.ModelViews.Main;
using Optepafi.ViewModels.Main;
using MainWindow = Optepafi.Views.Main.Windows.MainWindow;

namespace Optepafi;

/// <summary>
/// Class which represents application itself.
/// </summary>
public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        RecursiveSearchForDataTemplatesIn(Resources);
        DataTemplates.Add(new ViewLocator());
    }

    /// <summary>
    /// Method for recursive search for data templates that are defined in application level resource dictionaries.
    /// It adds every found data template in these dictionaries into <c>DataTemplates</c> collection of application.
    /// </summary>
    /// <param name="resourceDictionary">Resource dictionary which should be recursively searched for data templates.</param>
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
    
    /// <inheritdoc cref="Application.OnFrameworkInitializationCompleted"/>
    /// <remarks>
    /// New <c>MainWindow</c> instance is assign into MainWindow property of application lifetime.
    /// </remarks>
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