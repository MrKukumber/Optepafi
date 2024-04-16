using System.Dynamic;

namespace Optepafi.Models.TemplateMan.Templates;

public class Orienteering : ITemplate
{
    public static Orienteering Instance { get; } = new();
    private Orienteering() {}
    public string TemplateName { get; }
}