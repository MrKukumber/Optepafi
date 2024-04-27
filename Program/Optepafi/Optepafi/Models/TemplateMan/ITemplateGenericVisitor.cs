namespace Optepafi.Models.TemplateMan;

public interface ITemplateGenericVisitor<TOut, TOtherParams>
{
    public TOut GenericVisit<TTemplate>(TTemplate template,
        TOtherParams otherParams)
        where TTemplate : ITemplate;
}