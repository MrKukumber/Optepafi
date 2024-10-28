using Optepafi.Models.MapMan;
using Optepafi.Models.Utils;
using Optepafi.ViewModels.Data;

namespace Optepafi.Models.GraphicsMan.Sources;

/// <summary>
/// Represents ground graphic source of graphic objects. Beside this it contains definition of area, which contains all provided graphic objects.
/// 
/// When any graphics is displayed to user, there is always at least one ground graphics source which defines area of objects.  
/// However, there should not be more than one graphics source of this type. More ground graphics sources could result in inconsistent rendering of graphic objects. The correct way of usage is one ground graphics source with bunch of associated graphics sources.  
/// Graphic area is necessary for correct assigment of width and height of canvas on which graphics will be displayed as well as for converting <see cref="MapCoordinate"/>s to <see cref="CanvasCoordinate"/>s.  
/// </summary>
public interface IGroundGraphicsSource : IGraphicsSource
{
    /// <summary>
    /// Area that contains all graphic objects of this source.
    /// 
    /// It is used for assigning width and height of canvas as well as conversion from <c>MapCoordinate</c>s to <c>CanvasCoordinate</c>s.
    /// </summary>
    GraphicsArea GraphicsArea { get; }
}