using System;
using System.Threading;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan.MapRepreRepres;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan.MapRepreConstrs;

public interface IElevDataIndependentConstr<TTemplate, TMap, TMapRepre> : IMapRepreConstructor<TTemplate, TMap, TMapRepre> 
    where TTemplate : ITemplate  where TMap : IMap where TMapRepre : IMapRepresentation
{
    public TMapRepre? ConstructMapRepre(TTemplate template, TMap map, IMapRepreRepresentativ<IMapRepresentation> mapRepreRep,
        IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken);
    
}