using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapFormats;
using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.MapRepreMan.MapRepres.ObjectRepreConstrs;

public class ObjectRepreOrienteeringOmapConstr : IElevDataDependentConstr<Orienteering, OMAP, ObjectRepre>
{
    public static ObjectRepreOrienteeringOmapConstr Instance { get; } = new ObjectRepreOrienteeringOmapConstr();
    private ObjectRepreOrienteeringOmapConstr(){}

    public ObjectRepre? ConstructMapRepre(Orienteering templateType, OMAP map, ElevData elevData)
    {
        return null;
    }
    public Orienteering UsedTemplateType { get => Orienteering.Instance; }
    public IMapFormat<OMAP> UsedMapFormat { get => OMAPFormat.Instance; }
}