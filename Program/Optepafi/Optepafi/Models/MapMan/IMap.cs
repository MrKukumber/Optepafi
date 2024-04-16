using System.IO;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapRepreMan;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapMan;

public interface IMap
{
    public IMapFormat<IMap> MapFormat { get; init; }
    
    public IMapRepre VisitByMapRepreRepWhoBroughtTemplateForCreatingMapRepre<TTemplate>(IMapRepreRep<IMapRepre> mapRepreRep,
        TTemplate template) where TTemplate : ITemplate;
    public IMapRepre VisitByMapRepreRepWhoBroughtTemplateForCreatingMapRepre<TTemplate>(IMapRepreRep<IMapRepre> mapRepreRep, 
        TTemplate template, ElevData elevData) where TTemplate : ITemplate;
}
