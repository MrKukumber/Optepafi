using System.Dynamic;

namespace Optepafi.Models.MapRepre.MapRepres;

public class CompleteGraphRepreAgent
{
    private static CompleteGraphRepreAgent _instance = new CompleteGraphRepreAgent();
    private CompleteGraphRepreAgent(){}
    public static CompleteGraphRepreAgent Instance { get => _instance; }

}