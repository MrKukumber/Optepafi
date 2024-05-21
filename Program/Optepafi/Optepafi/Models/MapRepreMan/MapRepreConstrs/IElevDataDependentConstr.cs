using System;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan.MapRepreConstrs;

public interface IElevDataDependentConstr<in TTemplate, in TMap, out  TMapRepre> //: IMapRepreConstructor<TTemplate, TMap, TMapRepre> 
    where TTemplate : ITemplate where TMap : IMap where TMapRepre : IMapRepresentation
{
    public TMapRepre ConstructMapRepre(TTemplate template, TMap map, IElevData elevData, 
        IProgress<MapRepreCreationReport>? progress, CancellationToken? cancellationToken);

}