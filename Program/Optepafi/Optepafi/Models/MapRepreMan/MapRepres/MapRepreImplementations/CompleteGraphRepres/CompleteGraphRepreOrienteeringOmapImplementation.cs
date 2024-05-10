using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.MapRepreMan.MapRepres.ConstructableInterfaces;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan.MapRepres.MapRepreImplementations.CompleteGraphRepres;

public class CompleteGraphRepreOrienteeringOmapImplementation : ICompleteGraphRepre<>, IConstrElevDataDepMapRepre<OrienteeringISOM, OMAP>
{
    
    public override IMapRepreRepresentativ<IMapRepresentation> MapRepreRep { get; init; }
    public override ITemplate UsedTemplate { get; init; }
    public CompleteGraphRepreOrienteeringOmapImplementation() { }
}