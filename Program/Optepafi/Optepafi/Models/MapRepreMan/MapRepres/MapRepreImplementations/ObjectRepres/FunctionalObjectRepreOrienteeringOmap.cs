using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.MapRepreMan.MapRepres.ConstructableInterfaces;
using Optepafi.Models.MapRepreMan.MapRepres.MapRepreInterfaces;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.MapRepreMan.MapRepres.MapRepreImplementations.ObjectRepres;

public class FunctionalObjectRepreOrienteeringOmap : 
    IFunctionalObjectRepre<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>, 
    IConstrElevDataDepMapRepre<Orienteering_ISOM_2017_2, OMAP>
{
    public IMapRepreRepresentativ<IMapRepresentation> MapRepreRep { get; init; }

    public FunctionalObjectRepreOrienteeringOmap()
    {
    }
}