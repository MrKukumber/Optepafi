using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepres;

/// <summary>
/// Represents graph behaviour of some map representation. It do it so by coupling with graph functionality interfaces, which dictates contracts usable in path finding mechanisms.
/// It is important to note, that this interface does not have meaning of fully created graph instance. Graph on which is the path finding algorithm run can be dynamically created during the run. That depends on the coupled map representation and how it defines inner working and graphs creation. Implementations of this interface defines purely external behaviour of map representation.
/// Each implementation of this interface should implement set of graph functionality interfaces which will define its provided abilities.
/// Each graph type should have it own representative by which it is presented in some map representation representative as map representations corresponding graph type.
/// </summary>
/// <typeparam name="TVertexAttributes">Type of vertex attributes that are provided in vertices of generated graph.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes that are provided in edges of generated graph.</typeparam>
public interface IGraph<TVertexAttributes, TEdgeAttributes> : 
    IMapRepre
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    public void RestoreConsistency();
}