using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan.MapRepres.MapRepreInterfaces;

public interface IObjectRepre<out TTemplate> : IMapRepre
    where TTemplate : ITemplate
{
    
}