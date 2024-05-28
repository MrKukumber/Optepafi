using Avalonia.Controls.Templates;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.UserModelMan.UserModelTypes;

/// <summary>
/// One of three interfaces whose implementations represent individual user model types that is tied to specific template type. Especially this one represent bond of user model to some template type.
/// The other two interfaces are <see cref="IUserModelRepresentative{TTemplate,TUserModel}"/> and <see cref="IUserModelType{TTemplate,TUserModel}"/>.
/// It should not be implemented right away. All implementations should implement <c>IUserModelRepresentative{TTemplate, TUserModel}</c> instead.
/// Thanks to contravariance of its template type parameter it is useful for correct pattern matching on its template type. 
/// </summary>
/// <typeparam name="TTemplate">Template type which represented user model is tied to.</typeparam>
/// <typeparam name="TUserModel">Type of represented user model.</typeparam>
public interface IUserModelTemplateBond<in TTemplate> where TTemplate : ITemplate;