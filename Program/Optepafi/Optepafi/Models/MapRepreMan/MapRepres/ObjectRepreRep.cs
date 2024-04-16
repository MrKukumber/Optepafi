using Avalonia.Controls;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan.MapRepres.ObjectRepreConstrs;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan.MapRepres;

public class ObjectRepreRep : IMapRepreRep<ObjectRepre>
{
    public static ObjectRepreRep Instance { get; } = new ();
    private ObjectRepreRep(){}
    public IMapRepreConstr<ITemplate, IMap, ObjectRepre>[] MapRepreConstrs { get; } = { ObjectRepreOrienteeringOmapConstr.Instance };
}