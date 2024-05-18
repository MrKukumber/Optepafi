namespace Optepafi.Models.ParamsMan.Params;

public class MainSettingsParams : IParams
{
    public string? ElevDataTypeViewModelTypeName { get; set; }
    public required string Culture { get; set; }
}