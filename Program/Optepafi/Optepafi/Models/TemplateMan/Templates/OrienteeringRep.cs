using System.Dynamic;

namespace Optepafi.Models.TemplateMan.Templates;

public class OrienteeringRep : ITemplateRep<Orienteering>
{
    private static OrienteeringRep _instance = new();
    
    public static OrienteeringRep Instance
    {
        get => _instance;
    }
    private OrienteeringRep() { }
    public string TemplateName { get; }

    public Orienteering CreateTemplate() 
    {
        //TODO
        return null;
    }
}