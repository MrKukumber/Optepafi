using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.Utils;

namespace Optepafi.Models.UserModelMan.UserModels.Functionalities;

//TODO : comment
public interface IPositionRetrieving<out TTemplate, in TVertexAttributes, in TEdgeAttributes> : IComputing<TTemplate, TVertexAttributes, TEdgeAttributes>
    where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    MapCoordinates RetrievePosition(TVertexAttributes vertexAttributes);
}