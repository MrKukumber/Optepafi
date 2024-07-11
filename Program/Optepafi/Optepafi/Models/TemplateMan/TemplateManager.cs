using System.Collections.Generic;
using System.Collections.Immutable;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.TemplateMan;

/// <summary>
/// Singleton class that manages and provides usable template instances. It represents main channel between operations on templates and applications logic (ModelView/ViewModel).
/// </summary>
public class TemplateManager
{
    public static TemplateManager Instance { get; } = new();
    private TemplateManager(){}
    public ISet<ITemplate> Templates { get; } = ImmutableHashSet.Create<ITemplate>(BlankTemplate.Instance, NotUsableTemplate.Instance);
}