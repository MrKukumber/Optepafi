using System.IO;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan.UserModelTypes;

namespace Optepafi.Models.UserModelMan.UserModels;

public interface IUserModel<out TTemplate> where TTemplate : ITemplate
{
    public string FilePath { get; init; }
    public string Serialize();
    public void SerializeTo(Stream stream);
    public TOut AcceptGeneric<TOut, TGenericParam, TConstraint, TOtherParams>(
        IUserModelGenericVisitor<TOut, TConstraint, TOtherParams> genericVisitor,
        TGenericParam genericParam, TOtherParams otherParams) where TGenericParam : TConstraint;
    public TOut AcceptGeneric<TOut, TOtherParams>(IUserModelGenericVisitor<TOut, TOtherParams> genericVisitor,
        TOtherParams otherParams);
    public TOut AcceptGeneric<TOut>(IUserModelGenericVisitor<TOut> genericVisitor);
    public void AcceptGeneric(IUserModelGenericVisitor genericVisitor);
}