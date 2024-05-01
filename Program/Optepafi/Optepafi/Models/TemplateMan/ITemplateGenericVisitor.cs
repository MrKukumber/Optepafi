using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.TemplateMan;


public interface ITemplateGenericVisitor<TOut, TConstraint, TOtherParams>
{
    public TOut GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes, TGenericParam>(TTemplate template, TGenericParam genericParam,
        TOtherParams otherParams) 
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes
        where TGenericParam : TConstraint;
}

public interface ITemplateGenericVisitor<TOut, TOtherParams>
{
    public TOut GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes>(TTemplate template,
        TOtherParams otherParams)
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;
}

public interface ITemplateGenericVisitor<TOut>
{
    public TOut GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes>(TTemplate template) 
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;
}

public interface ITemplateGenericVisitor
{
    public void GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes>(TTemplate template)
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;
}