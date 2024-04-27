using System.IO;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.UserModelMan;

public interface IUserModelType<out TTemplate, out TUserModel> where TUserModel : IUserModel<TTemplate> where TTemplate : ITemplate
{
    string UserModelTypeName { get; }
    string UserModelFileExtension { get; }
    TTemplate UsedTemplate { get; }

    TUserModel GetNewUserModel();
    TUserModel? LoadUserModelFrom(StreamReader serialization);
}