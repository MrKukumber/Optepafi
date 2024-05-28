using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.UserModelMan.UserModelTypes;

/// <summary>
/// One of three interfaces whose implementations represent individual user model types that is tied to specific template type.
/// The other two interfaces are <see cref="IUserModelType{TTemplate,TUserModel}"/> and <see cref="IUserModelTemplateBond{TTemplate}"/>.
/// Every type of user model should have its representative, so it could be found as viable user model in <see cref="UserModelManager"/> class.
/// User model representative takes care of deserializing represented user models. Serialization of user models must match with deserialization of their representatives.
/// Preferred way to interact with representatives is through <see cref="UserModelManager"/>.
/// </summary>
/// <typeparam name="TTemplate">Template type which represented user model is tied to.</typeparam>
/// <typeparam name="TUserModel">Type of represented user model.</typeparam>
public interface IUserModelRepresentative<in TTemplate, TUserModel> :
    IUserModelType<TUserModel>,
    IUserModelTemplateBond<TTemplate> 
    where TTemplate : ITemplate
    where TUserModel : IUserModel<TTemplate>
{
    
}