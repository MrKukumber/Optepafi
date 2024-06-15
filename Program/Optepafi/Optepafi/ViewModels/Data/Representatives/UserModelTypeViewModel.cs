using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.Models.UserModelMan.UserModelTypes;
using Optepafi.ViewModels.DataViewModels;

namespace Optepafi.ViewModels.Data.Representatives;

public class UserModelTypeViewModel : DataViewModel<IUserModelType<IUserModel<ITemplate>, ITemplate>>
{
     protected override IUserModelType<IUserModel<ITemplate>, ITemplate> Data => UserModelType;
     public IUserModelType<IUserModel<ITemplate>, ITemplate> UserModelType { get; }
     public UserModelTypeViewModel(IUserModelType<IUserModel<ITemplate>, ITemplate> userModelType)
     {
          UserModelType = userModelType;
     }

     public string UserModelTypeName => UserModelType.UserModelTypeName;
     public string UserModelFileNameSuffix => UserModelType.UserModelFileNameSuffix;
     public string UserModelFileExtension => UserModelType.UserModelFileExtension;
}