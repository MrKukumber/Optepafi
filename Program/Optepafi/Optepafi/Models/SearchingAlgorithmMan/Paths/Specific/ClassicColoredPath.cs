using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Optepafi.Models.MapMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.SearchingAlgorithmMan.Paths;

public class ClassicColoredPath<TVertexAttributes, TEdgeAttributes> : IPath<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    public TOut AcceptGeneric<TOut, TOtherParams>(IPathGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
    {
        return genericVisitor.GenericVisit<ClassicColoredPath<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>(this, otherParams);
    }

    public Color pathColor;
    public IList<MapCoordinate> pathCoordinates;
}