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
    IMapFormat<IMap> MapFormat { get; init; }
    void FillUp(StreamReader inputMapFile);
    
    IMapRepresentation<ITemplate>? VisitByMapRepreRepWhoBroughtTemplateForCreatingMapRepre<TTemplate>(IMapRepreRepresentativ<IMapRepresentation<ITemplate>> mapRepreRep,
        TTemplate template, IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken) where TTemplate : ITemplate;
    IMapRepresentation<ITemplate>? VisitByMapRepreRepWhoBroughtTemplateForCreatingMapRepre<TTemplate>(IMapRepreRepresentativ<IMapRepresentation<ITemplate>> mapRepreRep, 
        TTemplate template, ElevData elevData, IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken) where TTemplate : ITemplate;
    
    
}
