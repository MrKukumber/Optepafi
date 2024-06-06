using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
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

        Resources.TryGetResource("TextMapObjectTemplates", this.ActualThemeVariant, out object? value);
        if (value is DataTemplates mapObjectDataTemplates)
        {
            DataTemplates.AddRange(mapObjectDataTemplates);
        }
        Resources.TryGetResource("PathObjectTemplates", this.ActualThemeVariant, out value);
        if(value is DataTemplates pathObjectDataTemplates)
        {
            DataTemplates.AddRange(pathObjectDataTemplates);
        }
        //Add every dataTemplate collection needed in application
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