using System.Collections.Generic;
using System.Collections.Immutable;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.TemplateMan;

public class TemplateManager
{
    public static TemplateManager Instance { get; } = new();
    private TemplateManager(){}
    
    public IReadOnlySet<ITemplate> Templates { get; } = ImmutableHashSet.Create<ITemplate>(/*TODO: doplnit templates */);
}