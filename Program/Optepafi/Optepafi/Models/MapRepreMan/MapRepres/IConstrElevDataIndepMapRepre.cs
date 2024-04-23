using System;
using System.Threading;
using Optepafi.Models.MapMan;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan.MapRepres;

public interface IConstrElevDataIndepMapRepre<TTemplate, TMap> : IConstructableMapRepre<TTemplate,TMap> where TTemplate : ITemplate where TMap : IMap
{
    void FillUp(TTemplate template, TMap map, IProgress<MapRepreConstructionReport>? progress,
        CancellationToken? cancellationToken);
}