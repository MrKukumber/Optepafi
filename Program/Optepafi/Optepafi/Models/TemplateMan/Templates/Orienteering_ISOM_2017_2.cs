using System.Dynamic;

namespace Optepafi.Models.TemplateMan.Templates;

public class Orienteering_ISOM_2017_2 : ITemplate
{
    public static Orienteering_ISOM_2017_2 Instance { get; } = new();
    private Orienteering_ISOM_2017_2() {}
    public string TemplateName { get; } = "Orienteering (ISOM 2017-2)";
}