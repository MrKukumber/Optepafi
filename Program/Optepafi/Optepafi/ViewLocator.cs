using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Optepafi.ViewModels;

namespace Optepafi;

/// <summary>
/// The View Locator is a mechanism in Avalonia that is used to resolve the view (user interface) that corresponds to a specific ViewModel.
/// 
/// The View Locator uses naming conventions to map ViewModel types to view types.  
/// By default, it replaces every occurrence of the string "ViewModel" within the fully-qualified ViewModel type name with "View".  
/// </summary>
public class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data is null)
            return null;

        var name = data.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var type = Type.GetType(name);

        if (type != null)
        {
            var control = (Control)Activator.CreateInstance(type)!;
            control.DataContext = data;
            return control;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}