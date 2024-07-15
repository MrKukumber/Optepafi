namespace Optepafi.Models.MapRepreMan;

/// <summary>
/// Record class used for reporting progress of construction of map representation.
/// </summary>
/// <param name="PercentProgress">Percent progress of map repre. construction.</param>
public record MapRepreConstructionReport(float PercentProgress);