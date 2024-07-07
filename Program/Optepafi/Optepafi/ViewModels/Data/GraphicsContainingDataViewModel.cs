using Optepafi.ViewModels.Data.Graphics;

namespace Optepafi.ViewModels.Data;

/// <summary>
/// <c>DataViewModel</c> that bears some kind of graphics source.
/// For more information on data view models see <see cref="DataViewModel"/>.
/// </summary>
public abstract class GraphicsContainingDataViewModel : DataViewModel
{
    public abstract GraphicsSourceViewModel? GraphicsSource { get; }
}