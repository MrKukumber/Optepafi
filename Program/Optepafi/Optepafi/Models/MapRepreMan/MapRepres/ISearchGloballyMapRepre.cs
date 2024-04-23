using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan.MapRepres;

public interface ISearchGloballyMapRepre<out TTemplate> : ISearchableMapRepre<TTemplate> where TTemplate : ITemplate
{
    
}