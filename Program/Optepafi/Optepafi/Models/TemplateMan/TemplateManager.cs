using System.Collections.Generic;
using System.Collections.Immutable;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.TemplateMan;

/// <summary>
/// Manages and provides usable template instances.
/// </summary>
public class TemplateManager
{
    public static TemplateManager Instance { get; } = new();
    private TemplateManager(){}
    public ISet<ITemplate> Templates { get; } = ImmutableHashSet.Create<ITemplate>(/*TODO: doplnit templates */);
}