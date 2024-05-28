using Optepafi.Models.MapRepreMan.Graphs.SpecificGraphs;
using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.MapRepreMan.MapRepres.GraphImplementations;

public abstract class BlankGraphElevDataIndepBlankTemplateTextMapImplementation :
    IBlankGraph<BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes>
{
    public void RestoreConsistency() { }

    public string Name { get; } = "Blank map representation.";
}