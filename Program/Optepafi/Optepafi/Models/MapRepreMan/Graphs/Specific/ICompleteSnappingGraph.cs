using Optepafi.Models.MapRepreMan.MapRepres.Specific;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.Graphs.Specific;

//TODO: comment + add functionalities
public interface ICompleteSnappingGraph<TVertexAttributes, TEdgeAttributes> : 
    ICompleteSnappingMapRepre,
    IGraph<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    
}