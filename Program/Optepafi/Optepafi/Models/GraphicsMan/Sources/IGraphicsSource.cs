using DynamicData;
using Optepafi.Models.Graphics.Objects;

namespace Optepafi.Models.Graphics.Sources;

/// <summary>
/// Represents source of graphic objects that can be shown to user.
/// It does not contain area which encapsulates all given objects, so it is typically used along side some <see cref="IGroundGraphicsSource"/> which defines area for it.
/// Graphic objects are stored in <see cref="SourceList{T}"/>. This ReactiveUI list class can be used as source for some <c>ObservableCollection</c> which can then provide elements to View. More info on <c>SourceList</c> see ReactiveUI sites.
/// </summary>
public interface IGraphicsSource
{
    /// <summary>
    /// Source list of graphic objects that are presented to user.
    /// </summary>
    public SourceList<IGraphicObject> GraphicObjects { get; }
}