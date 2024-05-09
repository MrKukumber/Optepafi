using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.UserModelMan.UserModels;

public interface IComputingUserModel<out TTemplate, TVertexAttributes, TEdgeAttributes> : 
    IUserModel<TTemplate> 
    where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    int ComputeWeight(TVertexAttributes from, TEdgeAttributes through, TVertexAttributes to);
}