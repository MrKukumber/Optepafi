using System;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan.MapRepres;

public interface IConstrElevDataDepMapRepre<TTemplate, TMap> : IConstructableMapRepre<TTemplate, TMap> where TTemplate : ITemplate where TMap : IMap
{
    
    void FillUp(TTemplate template, TMap map, ElevData elevData, IProgress<MapRepreConstructionReport>? progress,
        CancellationToken? cancellationToken);
}