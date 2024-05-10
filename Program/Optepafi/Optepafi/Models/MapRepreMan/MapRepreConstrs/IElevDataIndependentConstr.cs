using System;
using System.Threading;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan.MapRepreConstrs;

public interface IElevDataIndependentConstr<in TTemplate, in TMap, out TMapRepre> //: IMapRepreConstructor<TTemplate, TMap, TMapRepre> 
    where TTemplate : ITemplate  where TMap : IMap where TMapRepre : IMapRepresentation
{
    public TMapRepre? ConstructMapRepre(TTemplate template, TMap map, 
        IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken);
    
}