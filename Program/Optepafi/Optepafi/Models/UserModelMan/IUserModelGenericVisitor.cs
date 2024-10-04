using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.UserModelMan;

// public interface IUserModelGenericVisitor<TOut, TConstraint, TOtherParams>
// {
    // public TOut GenerciVisit<TUserModel, TTemplate, TGenericParam>(TUserModel userModel, TGenericParam genericParam,
        // TOtherParams otherParams)
        // where TUserModel : IUserModel<TTemplate>
        // where TTemplate : ITemplate
        // where TGenericParam : TConstraint;
// }

public interface IUserModelGenericVisitor<TOut, TOtherParams>
{
    public TOut GenericVisit<TUserModel, TTemplate>(TUserModel userModel, TOtherParams otherParams)
        where TUserModel : IUserModel<TTemplate>
        where TTemplate : ITemplate;
}
public interface IUserModelGenericVisitor<TOut>
{
    public TOut GenericVisit<TUserModel, TTemplate>(TUserModel userModel)
        where TUserModel : IUserModel<TTemplate>
        where TTemplate : ITemplate;
}
public interface IUserModelGenericVisitor
{
    public void GenericVisit<TUserModel, TTemplate>(TUserModel userModel)
        where TUserModel : IUserModel<TTemplate>
        where TTemplate : ITemplate;
}
