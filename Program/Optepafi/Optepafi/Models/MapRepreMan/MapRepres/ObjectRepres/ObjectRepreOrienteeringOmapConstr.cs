using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapFormats;
using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.MapRepreMan.MapRepres.ObjectRepres;

public class ObjectRepreOrienteeringOmapConstr : IElevDataDependentConstr<Orienteering, OMAP, ObjectRepre>
{
    public static ObjectRepreOrienteeringOmapConstr Instance { get; } = new ObjectRepreOrienteeringOmapConstr();
    private ObjectRepreOrienteeringOmapConstr(){}

    public ObjectRepre? ConstructMapRepre(Template template, Map map, ElevData elevData)
    {
        
    }
    public ITemplateRep<Orienteering> UsedTemplateRep { get => OrienteeringRep.Instance; }
    public IMapFormat<OMAP> UsedMapFormat { get => OMAPFormat.Instance; }
}