using Optepafi.Models.TemplateMan;

namespace Optepafi.ViewModels.DataViewModels;

public class TemplateViewModel : DataViewModel<ITemplate>
{
    protected override ITemplate Data => Template;
    public ITemplate Template { get; }
    public TemplateViewModel(ITemplate template)
    {
        Template = template;
    }

    public string Name => Template.TemplateName;
}