using Optepafi.Models.TemplateMan;

namespace Optepafi.ViewModels.Data.Representatives;

/// <summary>
/// Wrapping ViewModel for <c>ITemplate</c> type.
/// For more information on wrapping data view models see <see cref="WrappingDataViewModel{TData}"/>.
/// </summary>
/// <param name="template">Template instance to which will be this ViewModel coupled.</param>
public class TemplateViewModel(ITemplate template) : WrappingDataViewModel<ITemplate>
{
    /// <inheritdoc cref="WrappingDataViewModel{TData}.Data"/>
    protected override ITemplate Data => Template;
    /// <summary>
    /// Coupled user template instance. 
    /// </summary>
    public ITemplate Template { get; } = template;
    
    public string Name => Template.TemplateName;
}
