using System.IO;
using System.Threading;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.UserModelMan.UserModelTypes;

public interface IUserModelType<out TTemplate, out TUserModel> where TUserModel : IUserModel<TTemplate> where TTemplate : ITemplate
{
    string UserModelTypeName { get; }
    string UserModelFileNameSuffix { get; }
    string UserModelFileExtension { get; }
    TUserModel GetNewUserModel();
    TUserModel? DeserializeUserModel(Stream serialization, CancellationToken? cancellationToken, out UserModelManager.UserModelLoadResult result);
}