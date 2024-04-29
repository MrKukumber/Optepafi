using System;
using System.IO;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapRepreMan;
using Optepafi.Models.MapRepreMan.MapRepreRepres;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapMan;

public interface IMap
{
    string Name { get; }
    IMapFormat<IMap> MapFormat { get; init; }
    MapManager.MapCreationResult FillUp(StreamReader inputMapFile);
    
    public TOut AcceptGeneric<TOut, TGenericParam, TConstraint, TOtherParams>(
        IMapGenericVisitor<TOut,TConstraint,TOtherParams> genericVisitor,
        TGenericParam genericParam, TOtherParams otherParams) where TGenericParam : TConstraint;
    public TOut AcceptGeneric<TOut, TOtherParams>(IMapGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams);
    public TOut AcceptGeneric<TOut>(IMapGenericVisitor<TOut> genericVisitor);
    public void AcceptGeneric(IMapGenericVisitor genericVisitor);

}
