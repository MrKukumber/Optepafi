using System.Collections.Generic;
using Optepafi.Models.Utils.Shapes.Segments;

namespace Optepafi.Models.Utils.Shapes;


/// <summary>
/// Class which represents polygon shape.
/// </summary>
/// <param name="Segments">Polygon consists of these segments. There should be no gap between first and last segment
/// (the <c>point0</c> of the first segment should be same as <c>LastPoint</c> of the last segment) .</param>
public record class Polygon(List<Segment> Segments)
{
    public void AddSegment(Segment segment) => Segments.Add(segment);
}
