using Optepafi.Models.MapMan;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepre;

public interface IMapRepreAgent
{
    public string MapRepreName { get; }
    public (ITemplateAgent, IMapFormat)[] ImplementedCombinations { get; }
    public IMapRepre? CreateMapRepre(ITemplate template, IMap map);
}