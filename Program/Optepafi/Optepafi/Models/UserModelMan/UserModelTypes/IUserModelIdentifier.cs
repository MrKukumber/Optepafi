using Avalonia.Controls.Templates;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.UserModelMan.UserModelTypes;

/// <summary>
/// One of three interfaces whose implementations represent individual user model types that is tied to specific template type.
/// The other two interfaces are <see cref="IUserModelRepresentative{TTemplate,TUserModel}"/> and <see cref="IUserModelType{TTemplate,TUserModel}"/>.
/// It should not be implemented right away. All implementations should implement <c>IUserModelRepresentative{TTemplate, TUserModel}</c> instead.
/// Thanks to contravariance of its type parameters it is useful for correct pattern matching and identifying user model representatives. 
/// </summary>
/// <typeparam name="TTemplate">Template type which represented user model is tied to.</typeparam>
/// <typeparam name="TUserModel">Type of represented user model.</typeparam>
public interface IUserModelIdentifier<in TTemplate, in TUserModel> where TTemplate : ITemplate where TUserModel : IUserModel<TTemplate> { }