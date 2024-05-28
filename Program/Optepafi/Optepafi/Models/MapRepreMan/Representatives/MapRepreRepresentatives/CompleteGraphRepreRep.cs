using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.Graphs.SpecificGraphs;
using Optepafi.Models.MapRepreMan.MapRepres.MapRepreInterfaces;

namespace Optepafi.Models.MapRepreMan.MapRepreReps.MapRepreRepsInterfaces;

public class CompleteGraphRepreRep 
{
    public static CompleteGraphRepreRep Instance { get; } = new();
    private CompleteGraphRepreRep(){}

}