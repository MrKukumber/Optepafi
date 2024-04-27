using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.TemplateMan;

public class TemplateManager
{
    public static TemplateManager Instance { get; } = new();
    private TemplateManager(){}
    
    public ITemplate[] Templates { get; } = {/*TODO: doplnit templateAgents */};
}