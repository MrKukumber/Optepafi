using Optepafi.Models.Utils;

namespace Optepafi.Models.UserModelMan.UserModelReps;

/// <summary>
/// One of three interfaces whose implementations represent properties of individual user model types that is tied to specific template type.
///
/// This interface identifies individual user model type or its derivatives.
/// The other two interfaces are <see cref="IUserModelTemplateBond{TTemplate}"/> and <see cref="IUserModelType{TTemplate,TUserModel}"/>.
/// It should not be implemented right away. All implementations should derive from <see cref="UserModelRepresentative{TUserModel,TTemplate,TConfiguration}"/> instead.  
/// Thanks to contravariant nature of its type parameter it is useful for correct pattern matching and identifying user model representatives.  
/// </summary>
/// <typeparam name="TUserModel"></typeparam>
public interface IUserModelIdentifier<in TUserModel>
{
    void ChangeConfiguration(TUserModel userModel, IConfiguration configuration);
}