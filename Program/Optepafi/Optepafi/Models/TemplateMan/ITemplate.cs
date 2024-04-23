using System;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan;
using Optepafi.Models.MapRepreMan.MapRepreRepres;
using Optepafi.Models.MapRepreMan.MapRepres;

namespace Optepafi.Models.TemplateMan;

public interface ITemplate
{
    string TemplateName { get; }

    public IMapRepresentation<ITemplate>? VisitByMapRepreRepAndTakeItToVisitMapForCreatingMapRepre(IMapRepreRepresentativ<IMapRepresentation<ITemplate>> mapRepreRep, 
        IMap map, IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken);
    public IMapRepresentation<ITemplate>? VisitByMapRepreRepAndTakeItToVisitMapForCreatingMapRepre(IMapRepreRepresentativ<IMapRepresentation<ITemplate>> mapRepreRep, 
        IMap map, ElevData elevData, IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken);
}