using System;
using System.Data.SqlTypes;
using System.Threading;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan.MapRepreRepres;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan.MapRepreConstrs;

public class ElevDataIndependentConstr<TTemplate, TMap, TMapRepre> : IElevDataIndependentConstr<TTemplate, TMap, TMapRepre> 
    where TTemplate : ITemplate where TMap : IMap where TMapRepre : IConstrElevDataIndepMapRepre<TTemplate, TMap>, new()
{
    public TTemplate UsedTemplate { get; }
    public IMapFormat<TMap> UsedMapFormat { get; }
    public bool RequiresElevData { get; } = false;
    
    public ElevDataIndependentConstr(TTemplate usedTemplate, IMapFormat<TMap> usedMapFormat)
    {
        UsedTemplate = usedTemplate;
        UsedMapFormat = usedMapFormat;
    }
    public TMapRepre? ConstructMapRepre(TTemplate template, TMap map, IMapRepreRepresentativ<IMapRepresentation<ITemplate>> mapRepreRep, 
        IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken)
    {

        TMapRepre mapRepre = new TMapRepre()
        {
            MapRepreRep = mapRepreRep,
        };
        mapRepre.FillUp(template, map, progress, cancellationToken);
        return mapRepre;
    }
}