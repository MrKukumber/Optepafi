namespace Optepafi.Models.TemplateMan;

public abstract class Template
{
    public ITemplateRep<Template> TemplateRep { get; }

    protected Template(ITemplateRep<Template> templateRep)
    {
        TemplateRep = templateRep;
    }
}