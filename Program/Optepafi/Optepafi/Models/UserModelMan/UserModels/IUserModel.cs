using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.UserModelMan.UserModels;

public interface IUserModel<out TTemplate> where TTemplate : ITemplate
{
    public TOut AcceptGeneric<TOut, TGenericParam, TConstraint, TOtherParams>(
        IUserModelGenericVisitor<TOut, TConstraint, TOtherParams> genericVisitor,
        TGenericParam genericParam, TOtherParams otherParams) where TGenericParam : TConstraint;
    public TOut AcceptGeneric<TOut, TOtherParams>(IUserModelGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams);
    public TOut AcceptGeneric<TOut>(IUserModelGenericVisitor<TOut> genericVisitor);
    public void AcceptGeneric(IUserModelGenericVisitor genericVisitor);
}