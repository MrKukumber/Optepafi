using System.Dynamic;

namespace Optepafi.Models.MapRepreMan.MapRepres;

public class CompleteGraphRepreRep
{
    private static CompleteGraphRepreRep _instance = new CompleteGraphRepreRep();
    private CompleteGraphRepreRep(){}
    public static CompleteGraphRepreRep Instance { get => _instance; }

}