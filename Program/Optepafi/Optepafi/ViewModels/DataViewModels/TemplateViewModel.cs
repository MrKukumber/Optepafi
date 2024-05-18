using Optepafi.Models.TemplateMan;

namespace Optepafi.ViewModels.DataViewModels;

public class TemplateViewModel : ViewModelBase
{
    public ITemplate Template { get; }
    public TemplateViewModel(ITemplate template)
    {
        Template = template;
    }

    public string Name => Template.TemplateName;
}