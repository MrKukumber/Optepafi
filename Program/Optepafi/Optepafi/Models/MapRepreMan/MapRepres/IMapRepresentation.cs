using Optepafi.Models.MapRepreMan.MapRepreRepres;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan.MapRepres;

public interface IMapRepresentation<out TTemplate> where TTemplate : ITemplate
{
    public IMapRepreRepresentativ<IMapRepresentation<ITemplate>> MapRepreRep { get; init; }
    public TTemplate UsedTemplate { get; }
}