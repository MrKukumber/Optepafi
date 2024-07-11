namespace Optepafi.ViewModels.Data.Graphics;

/// <summary>
/// Predecessor for all graphic object ViewModels.
/// It defines position and rendering priority of given graphic object on canvas.
/// For every graphic object ViewModel there should be implemented appropriate View/DataTemplate that is able draw it on canvas. 
/// For more information on data ViewModels see <see cref="DataViewModel"/>.
/// </summary>
public abstract class GraphicObjectViewModel : DataViewModel
{
    /// <summary>
    /// Position of graphic object on canvas.
    /// </summary>
    public abstract CanvasCoordinate Position { get; }
    /// <summary>
    /// Priority of rendering of graphic object on canvas. It should be non negative integer.
    /// </summary>
    public abstract int Priority { get; }
}