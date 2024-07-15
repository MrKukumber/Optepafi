using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan.UserModelReps;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.ViewModels.Data.Representatives;

/// <summary>
/// Wrapping ViewModel for <c>IUserModelType{TUserModel,TTemplate}</c> type.
/// 
/// For more information on wrapping data view models see <see cref="WrappingDataViewModel{TData}"/>.  
/// </summary>
/// <param name="userModelType">User model type instance to which will be this ViewModel coupled.</param>
public class UserModelTypeViewModel(IUserModelType<IUserModel<ITemplate>, ITemplate> userModelType) : WrappingDataViewModel<IUserModelType<IUserModel<ITemplate>, ITemplate>>
{
    /// <inheritdoc cref="WrappingDataViewModel{TData}.Data"/>
    protected override IUserModelType<IUserModel<ITemplate>, ITemplate> Data => UserModelType;
     
    /// <summary>
    /// Coupled user model type instance.
    /// </summary>
    public IUserModelType<IUserModel<ITemplate>, ITemplate> UserModelType { get; } = userModelType;
     
    public string UserModelTypeName => UserModelType.UserModelTypeName;
    /// <summary>
    /// Represents suffix of file name to which should be eventual serialization saved.
    /// 
    /// For more information see <c>UserModelFileNameSuffix</c> documentation in <see cref="IUserModelType{TUserModel,TTemplate}"/>.  
    /// </summary>
    public string UserModelFileNameSuffix => UserModelType.UserModelFileNameSuffix;
    /// <summary>
    /// Extension of file format, which is user model serialized to.
    /// </summary>
    public string UserModelFileExtension => UserModelType.UserModelFileExtension;
}