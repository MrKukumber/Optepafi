using System.Collections.Generic;
using System.Linq;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.Utils;

namespace Optepafi.Models.SearchingAlgorithmMan.Paths.Specific;

//TODO: comment
public class SegmentedLinesPath<TVertexAttributes, TEdgeAttributes> : IPath<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    public List<List<TVertexAttributes>> Lines { get; }
    
    public SegmentedLinesPath() : this(new List<List<TVertexAttributes>>()){}
    public SegmentedLinesPath(List<TVertexAttributes> firstLine) : this([firstLine]) { }
    public SegmentedLinesPath(List<List<TVertexAttributes>> lines)
    {
        Lines = lines;
    }
    public void MergeWith( SegmentedLinesPath<TVertexAttributes, TEdgeAttributes> segmentedLinesPath)
    {
        Lines.AddRange(segmentedLinesPath.Lines);
    }

    public void Add(TVertexAttributes linePoint)
    {
        Lines.Last().Add(linePoint);
    }
    
    public TOut AcceptGeneric<TOut, TOtherParams>(IPathGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
        => genericVisitor.GenericVisit<SegmentedLinesPath<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>(this, otherParams);
    
}