using System;
using System.Threading;
using Avalonia.Controls.Templates;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan.MapRepreRepres;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan.MapRepreConstrs;

public sealed class ElevDataDependentConstr<TTemplate, TMap, TMapRepre> : IElevDataDependentConstr<TTemplate, TMap, TMapRepre> 
    where TTemplate : ITemplate where TMap : IMap where TMapRepre : IConstrElevDataDepMapRepre<TTemplate, TMap>, new()
{
    public TTemplate UsedTemplate { get; }
    public IMapFormat<TMap> UsedMapFormat { get; }
    public bool RequiresElevData { get; } = true;
    public ElevDataDependentConstr(TTemplate usedTemplate, IMapFormat<TMap> usedMapFormat)
    {
        UsedTemplate = usedTemplate;
        UsedMapFormat = usedMapFormat;
    }
    public TMapRepre? ConstructMapRepre(TTemplate template, TMap map, ElevData elevData, IMapRepreRepresentativ<IMapRepresentation> mapRepreRep,
        IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken)
    {
        TMapRepre mapRepre = new TMapRepre()
        {
            MapRepreRep = mapRepreRep,
        };
        mapRepre.FillUp(template, map, elevData, progress, cancellationToken);
        return mapRepre;
    }
}