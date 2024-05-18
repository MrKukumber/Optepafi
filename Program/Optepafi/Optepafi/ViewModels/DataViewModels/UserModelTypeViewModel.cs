using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.Models.UserModelMan.UserModelTypes;

namespace Optepafi.ViewModels.DataViewModels;

public class UserModelTypeViewModel : ViewModelBase
{
     public IUserModelType<ITemplate, IUserModel<ITemplate>> UserModelType { get; }
     public UserModelTypeViewModel(IUserModelType<ITemplate, IUserModel<ITemplate>> userModelType)
     {
          UserModelType = userModelType;
     }

     public string UserModelTypeName => UserModelType.UserModelTypeName;
     public string UserModelFileNameSuffix => UserModelType.UserModelFileNameSuffix;
     public string UserModelFileExtension => UserModelType.UserModelFileExtension;
}