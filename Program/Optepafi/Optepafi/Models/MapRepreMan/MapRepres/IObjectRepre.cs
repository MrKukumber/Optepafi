using Optepafi.Models.MapMan;
using Optepafi.Models.MapRepreMan.MapRepreRepres;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan.MapRepres;

public interface IObjectRepre<out TTemplate> : ISearchLocallyMapRepre<TTemplate> where TTemplate : ITemplate
{
    
}
