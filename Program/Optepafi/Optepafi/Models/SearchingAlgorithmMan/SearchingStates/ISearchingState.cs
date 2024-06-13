using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.SearchingAlgorithmMan.SearchingStates;

public interface ISearchingState<TVetexAttributes, TEdgeAttributes> : ISearchingState
    where TVetexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    
}

public interface ISearchingState
{
    
}