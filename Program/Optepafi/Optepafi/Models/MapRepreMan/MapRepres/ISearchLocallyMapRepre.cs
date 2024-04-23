using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan.MapRepres;

public interface ISearchLocallyMapRepre<out TTemplate> : ISearchableMapRepre<TTemplate> where TTemplate : ITemplate
{
    
}