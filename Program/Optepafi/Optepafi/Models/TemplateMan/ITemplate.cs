using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan;

namespace Optepafi.Models.TemplateMan;

public interface ITemplate
{
    string TemplateName { get; }

    public IMapRepre VisitByMapRepreRepAndTakeItToVisitMapForCreatingMapRepre(IMapRepreRep<IMapRepre> mapRepreRep, 
        IMap map);
    public IMapRepre VisitByMapRepreRepAndTakeItToVisitMapForCreatingMapRepre(IMapRepreRep<IMapRepre> mapRepreRep, 
        IMap map, ElevData elevData);
}