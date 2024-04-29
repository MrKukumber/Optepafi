using System.IO;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.UserModelMan.UserModelTypes;

public interface IUserModelType<out TTemplate, out TUserModel> where TUserModel : IUserModel<TTemplate> where TTemplate : ITemplate
{
    string UserModelTypeName { get; }
    string UserModelFileExtension { get; }

    TUserModel GetNewUserModel();
    TUserModel? LoadUserModelFrom(StreamReader serialization);
}