namespace Optepafi.Models.MapRepre.MapRepres;

public class ObjectRepreAgent
{
    
    private static ObjectRepreAgent _instance = new ObjectRepreAgent();
    private ObjectRepreAgent(){}
    public static ObjectRepreAgent Instance { get => _instance; }
    
}