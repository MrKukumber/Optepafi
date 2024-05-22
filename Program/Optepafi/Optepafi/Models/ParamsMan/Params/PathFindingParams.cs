namespace Optepafi.Models.ParamsMan.Params;

public class PathFindingParams : IParams
{
    
    public required string TemplateTypeName { get; init; }
    public required string SearchingAlgorithmTypeName { get; init; }
    public required string MapFilePath { get; init; }
    public required string UserModelFilePath { get; init; }
}