using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.MapRepreMan.MapRepreRepres;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.MapRepreMan.MapRepres.CompleteGraphRepres;

public class CompleteGraphRepreOrienteeringOmap : ICompleteGraphRepre<>, IConstrElevDataDepMapRepre<OrienteeringISOM, OMAP>
{
    
    public override IMapRepreRepresentativ<IMapRepresentation> MapRepreRep { get; init; }
    public override ITemplate UsedTemplate { get; init; }
    public CompleteGraphRepreOrienteeringOmap() { }
}