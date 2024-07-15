using Optepafi.Models.ReportMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.SearchingAlgorithmMan.SearchingStates;

/// <summary>
/// Represents searching state, that can be reported by some searching algorithm.
///
/// Searching states can be used by algorithms independently.  
/// The searching state should be constructed by algorithm and then passed to <see cref="ReportSubManager{TVertexAttributes,TEdgeAttributes}"/> together with user model for aggregating of searching report which is then sent to ModelView and further.  
/// It is not meant to be visible outside of Model. It contains two type parameters which can be used for saving vertex/edge attributes which can be then used in reports aggregation.  
/// </summary>
/// <typeparam name="TVertexAttributes">Type of vertex attributes, which can be included for extraction of information in later aggregations.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes, which can be included for extraction of information in later aggregations.</typeparam>
public interface ISearchingState<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    
}
