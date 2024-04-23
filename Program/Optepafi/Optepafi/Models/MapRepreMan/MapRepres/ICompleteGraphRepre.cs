using Optepafi.Models.MapRepreMan.MapRepreRepres;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan.MapRepres;

public interface ICompleteGraphRepre<out TTemplate> : ISearchLocallyMapRepre<TTemplate>, ISearchGloballyMapRepre<TTemplate> where TTemplate : ITemplate
{
    //TODO: implement IMapRepre interface
}