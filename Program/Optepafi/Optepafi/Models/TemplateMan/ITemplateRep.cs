namespace Optepafi.Models.TemplateMan;

public interface ITemplateRep<out TTemplate> where TTemplate : Template
{
    string TemplateName { get; }

    TTemplate? CastTemplate(Template template)
    {
        if (template is TTemplate cTemplate) return cTemplate;
        return null;
    }
    TTemplate CreateTemplate();
}