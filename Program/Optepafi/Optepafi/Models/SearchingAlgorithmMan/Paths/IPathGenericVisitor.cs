using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.SearchingAlgorithmMan.Paths;


public interface IPathGenericVisitor<TOut, TOtherParams>
{
    public TOut GenericVisit<TPath, TVertexAttributes, TEdgeAttributes>(TPath path, TOtherParams otherParams)
        where TPath : IPath<TVertexAttributes, TEdgeAttributes> where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes;
}