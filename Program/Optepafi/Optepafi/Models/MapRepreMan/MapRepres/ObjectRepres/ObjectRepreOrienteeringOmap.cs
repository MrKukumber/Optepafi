using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.MapRepreMan.MapRepreRepres;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.MapRepreMan.MapRepres.ObjectRepres;

public class ObjectRepreOrienteeringOmap : IObjectRepre<Orienteering_ISOM_2017_2, Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>, IConstrElevDataDepMapRepre<Orienteering_ISOM_2017_2, OMAP>
{
    public IMapRepreRepresentativ<IMapRepresentation> MapRepreRep { get; init; }

    public ObjectRepreOrienteeringOmap()
    {
    }
}