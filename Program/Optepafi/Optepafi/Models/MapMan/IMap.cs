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
    
    public TOut AcceptGenericWithSomeone<TOut, TSomeone, TSomeonesConstraint, TOtherParams>(
        IMapGenericVisitorWithSomeone<TOut,TSomeonesConstraint,TOtherParams> genericVisitorWithSomeone,
        TSomeone someone, TOtherParams otherParams) where TSomeone : TSomeonesConstraint;
}
