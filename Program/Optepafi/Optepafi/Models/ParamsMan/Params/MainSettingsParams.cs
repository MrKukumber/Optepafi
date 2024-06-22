namespace Optepafi.Models.ParamsMan.Params;

/// <summary>
/// Represents parameters used by main settings.
/// </summary>
public class MainSettingsParams : IParams
{
    public string? ElevDataTypeViewModelTypeName { get; set; }
    public required string CultureName { get; set; }
    
    /// <inheritdoc cref="IParams.AcceptParamsManager"/>
    public void AcceptParamsManager(ParamsManager paramsManager) => paramsManager.Visit(this); 
}