using System.Dynamic;

namespace Optepafi.Models.TemplateMan.Templates;

public class OrienteeringAgent
{
    private static OrienteeringAgent _instance = new();
    
    public static OrienteeringAgent Instance
    {
        get => _instance;
    }
    private OrienteeringAgent() { }
    
    public ITemplate GetTemplate() 
    {
        //TODO
        return null;
    }
}