using Optepafi.ViewModels.DataViewModels;

namespace Optepafi.ViewModels.Data.Graphics.MapObjects;


/// <summary>
/// ViewModel for <c>WordObject</c> graphic object type.
/// There should exists appropriate convertor of <c>WordObject</c> to this ViewModel type.
/// For more information on graphic object ViewModels see <see cref="GraphicObjectViewModel"/>.
/// </summary>
public class WordObjectViewModel : GraphicObjectViewModel
{
    /// <summary>
    /// Creates an instance of this type from provided parameters.
    /// Text of word should be displayed on provided position.
    /// </summary>
    /// <param name="position">Position where word should be displayed.</param>
    /// <param name="word">Word to be shown on provided position.</param>
    public WordObjectViewModel(CanvasCoordinate position, string word)
    {
        Position = position;
        Priority = 0;
        Word = word;
    }
    
    /// <inheritdoc cref="GraphicObjectViewModel.Position"/>
    public override CanvasCoordinate Position { get; }
    
    /// <inheritdoc cref="GraphicObjectViewModel.Priority"/>
    public override int Priority { get; }
    /// <summary>
    /// Word to be shown on given position on canvas.
    /// </summary>
    public string Word { get; }
}

/// <summary>
/// ViewModel for <c>TrackPointWordObject</c> graphic object type.
/// There should exists appropriate convertor of <c>TrackPointWordObject</c> to this ViewModel type.
/// For more information on graphic object ViewModels see <see cref="GraphicObjectViewModel"/>.
/// </summary>
public class TrackPointWordObjectViewModel : GraphicObjectViewModel
{
    /// <summary>
    /// Creates an instance of this type from provided parameters.
    /// Track point should be displayed on provided position. Track point should be displayed as lowercase letter 'o' with font size equal to 12.
    /// Position of track point is therefore adjusted a little so the letter 'o' was centered on the provided position.
    /// </summary>
    /// <param name="position">Position to which will be graphic object centered.</param>
    public TrackPointWordObjectViewModel(CanvasCoordinate position)
    {
        Position = new CanvasCoordinate(position.LeftPos - 1000, position.BottomPos - 2000);
        Priority = 1;
    }
    
    /// <inheritdoc cref="GraphicObjectViewModel.Position"/>
    public override CanvasCoordinate Position { get; }
    /// <inheritdoc cref="GraphicObjectViewModel.Priority"/>
    public override int Priority { get; }
}