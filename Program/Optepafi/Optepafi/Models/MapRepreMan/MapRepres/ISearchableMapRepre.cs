using Avalonia.Controls;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan.MapRepres;

public interface ISearchableMapRepre<out TTemplate> : IMapRepresentation<TTemplate> where TTemplate : ITemplate
{
    
}