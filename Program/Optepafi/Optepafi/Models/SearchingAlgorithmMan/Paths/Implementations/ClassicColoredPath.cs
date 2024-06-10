using System.Collections.Generic;
using System.Drawing;
using Optepafi.Models.MapMan;

namespace Optepafi.Models.SearchingAlgorithmMan.Paths.Implementations;

public class ClassicColoredPath : IPath
{
    public TOut AcceptGeneric<TOut, TOtherParams>(IPathGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
    {
        return genericVisitor.GenericVisit<ClassicColoredPath>(this, otherParams);
    }

    public Color pathColor;
    public IList<MapCoordinate> pathCoordinates;
}