using System;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan.MapRepreConstrs;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan.MapRepreRepres;

public interface IMapRepreRepresentativ<out TMapRepresentation> where TMapRepresentation : IMapRepresentation<ITemplate>
{
    string MapRepreName { get; }

    //represents all map repre constructors, that returns TMapRepresentation
    IMapRepreConstructor<ITemplate, IMap, TMapRepresentation>[] MapRepreConstrs { get; }

    IMapRepresentation<TTemplate>? CreateMapRepre<TTemplate, TMap>(TTemplate template, TMap map,
        IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken)
        where TTemplate : ITemplate where TMap : IMap
    {
        foreach (var constructor in MapRepreConstrs)
        {
            if(constructor is IElevDataIndependentConstr<TTemplate,TMap, IMapRepresentation<TTemplate>> c)
            {
                return c.ConstructMapRepre(template, map, (IMapRepreRepresentativ<IMapRepresentation<ITemplate>>) this, progress, cancellationToken);
            }
        }
        return default;
    }

    IMapRepresentation<TTemplate>? CreateMapRepre<TTemplate, TMap>(TTemplate template, TMap map, ElevData elevData,
        IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken)
        where TTemplate : ITemplate where TMap : IMap
    {
        foreach (var constructor in MapRepreConstrs)
        {
            if (constructor is IElevDataDependentConstr<TTemplate, TMap, IMapRepresentation<TTemplate>> c)
            {
                return c.ConstructMapRepre(template, map, elevData, (IMapRepreRepresentativ<IMapRepresentation<ITemplate>>) this, progress, cancellationToken);
            }
        }
        return default;
    }
}