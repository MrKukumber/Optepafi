using Optepafi.Models.TemplateMan;
using Optepafi.ViewModels.DataViewModels;

namespace Optepafi.ViewModels.Data.Representatives;

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