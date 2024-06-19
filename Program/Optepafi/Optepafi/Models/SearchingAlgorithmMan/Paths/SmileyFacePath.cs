using System.Collections.Generic;
using Optepafi.Models.MapMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.SearchingAlgorithmMan.Paths;

public class SmileyFacePath<TVertexAttributes, TEdgeAttributes> : IPath<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    public SmileyFacePath(MapCoordinate legStart, MapCoordinate legFinish) : this([(legStart, legFinish)]){}

    public SmileyFacePath()
    {
        Path = new();
    }
    public SmileyFacePath(List<(MapCoordinate legStart, MapCoordinate legFinish)> path)
    {
        Path = path;
    }
    public TOut AcceptGeneric<TOut, TOtherParams>(IPathGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
    {
        return genericVisitor.GenericVisit<SmileyFacePath<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>(this, otherParams);
    }
    public SmileyFacePath<TVertexAttributes, TEdgeAttributes> MergeWith( SmileyFacePath<TVertexAttributes, TEdgeAttributes> smileyFacePath)
    {
        Path.AddRange(smileyFacePath.Path);
        return this;
    }
    public List<(MapCoordinate legStart, MapCoordinate legFinish)> Path { get; }
}