using System.Collections.Generic;
using Optepafi.Models.Utils.Shapes.Segments;

namespace Optepafi.Models.Utils.Shapes;

//TODO: comment
public record class Polygon(List<Segment> Segments)
{
    public void AddSegment(Segment segment) => Segments.Add(segment);
}