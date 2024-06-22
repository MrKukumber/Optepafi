namespace Optepafi.Models.ParamsMan.Params;

/// <summary>
/// Represents parameters used by path finding sessions to retrieve last used setup.
/// </summary>
public class PathFindingParams : IParams
{
    public required string TemplateTypeName { get; init; }
    public required string SearchingAlgorithmTypeName { get; init; }
    public required string MapFilePath { get; init; }
    public required string UserModelFilePath { get; init; }
    
    /// <inheritdoc cref="IParams.AcceptParamsManager"/>
    public void AcceptParamsManager(ParamsManager paramsManager)
    {
        paramsManager.Visit(this);
    }
}