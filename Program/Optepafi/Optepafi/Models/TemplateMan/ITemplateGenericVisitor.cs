namespace Optepafi.Models.TemplateMan;


public interface ITemplateGenericVisitor<TOut, TConstraint, TOtherParams>
{
    public TOut GenericVisit<TTemplate, TGenericParam>(TTemplate template, TGenericParam genericParam,
        TOtherParams otherParams) 
        where TTemplate : ITemplate 
        where TGenericParam : TConstraint;
}

public interface ITemplateGenericVisitor<TOut, TOtherParams>
{
    public TOut GenericVisit<TTemplate>(TTemplate template,
        TOtherParams otherParams)
        where TTemplate : ITemplate;
}

public interface ITemplateGenericVisitor<TOut>
{
    public TOut GenericVisit<TTemplate>(TTemplate template) where TTemplate : ITemplate;
}

public interface ITemplateGenericVisitor
{
    public void GenericVisit<TTemplate>(TTemplate template) where TTemplate : ITemplate;
}