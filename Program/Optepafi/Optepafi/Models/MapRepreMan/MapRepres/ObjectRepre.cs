using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan.MapRepres.ObjectRepreConstrs;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan.MapRepres;

public abstract class ObjectRepre : IMapRepre
{
    public IMapRepreRep<IMapRepre> MapRepreRep { get; } = ObjectRepreRep.Instance;
    public IMapRepreConstr<ITemplate, IMap, IMapRepre>[] MapRepreElevDataDependentConstr { get; } = { ObjectRepreOrienteeringOmapConstr.Instance };
}
