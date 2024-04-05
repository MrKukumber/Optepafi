namespace Optepafi.Models.TemplateMan;

public interface ITemplateAgent
{
    public string TemplateName { get; }
    public ITemplate CreateTemplate();
}