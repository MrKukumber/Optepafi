using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.Models.UserModelMan.UserModelTypes;
using Optepafi.ViewModels.DataViewModels;

namespace Optepafi.ViewModels.Data.Representatives;

public class UserModelTypeViewModel : DataViewModel<IUserModelType<IUserModel>>
{
     protected override IUserModelType<IUserModel> Data => UserModelType;
     public IUserModelType<IUserModel> UserModelType { get; }
     public UserModelTypeViewModel(IUserModelType<IUserModel> userModelType)
     {
          UserModelType = userModelType;
     }

     public string UserModelTypeName => UserModelType.UserModelTypeName;
     public string UserModelFileNameSuffix => UserModelType.UserModelFileNameSuffix;
     public string UserModelFileExtension => UserModelType.UserModelFileExtension;
}