using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.MapRepreMan.VerticesAndEdges;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.Graphs;

/// <summary>
/// Represents graph behaviour of some map representation.
/// 
/// It do it so by coupling with graph functionality interfaces, which dictates contracts usable in path finding mechanisms.  
/// It is important to note, that this interface does not have meaning of fully created graph instance. Graph on which is the path finding algorithm run can be dynamically created during the run. That depends on the coupled map representation and how it defines inner working and graphs creation. Implementations of this interface defines purely external behaviour of map representation.  
/// Each implementation of this interface should implement set of graph functionality interfaces which will define its provided abilities.  
/// Each graph type should have it own representative by which it is presented in some map representation representative as map representations corresponding graph type.  
/// </summary>
/// <typeparam name="TVertex">Type of vertices that are provided in generated graph.</typeparam>
/// <typeparam name="TEdge">Type of edges that are provided in generated graph.</typeparam>
public interface IGraph<out TVertex, out TEdge> : 
    IMapRepre
    where TVertex : IVertex
    where TEdge : IEdge
{
    public void RestoreConsistency();
    
    public TOut AcceptGeneric<TOut, TGenericParam1, TGenericParam2, TGenericConstraint1, TGenericConstraint2, TOtherParams>(IGraphGenericVisitor<TOut, TGenericConstraint1, TGenericConstraint2, TOtherParams> genericVisitor, 
        TOtherParams otherParams)
        where TGenericParam1 : TGenericConstraint1
        where TGenericParam2 : TGenericConstraint2;
}