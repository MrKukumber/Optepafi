using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapFormats;
using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.MapRepreMan.MapRepreConstrs;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.MapRepreMan.MapRepres.ObjectRepres;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.MapRepreMan.MapRepreRepres;

public class ObjectRepreRep : IMapRepreRepresentativ<IObjectRepre<ITemplate<IVertexAttributes, IEdgeAttributes>, IVertexAttributes, IEdgeAttributes>>
{
    public static ObjectRepreRep Instance { get; } = new ();
    private ObjectRepreRep(){}
    
    public string MapRepreName { get; } = ""; //TODO: vymysliet pekne meno
    public IMapRepreConstructor<ITemplate, IMap, IObjectRepre<ITemplate<IVertexAttributes, IEdgeAttributes>,IVertexAttributes, IEdgeAttributes>>[] MapRepreConstrs { get; } = { new ElevDataDependentConstr<Orienteering_ISOM_2017_2, OMAP, ObjectRepreOrienteeringOmap>(Orienteering_ISOM_2017_2.Instance, OMAPFormat.Instance) };
}
