using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan.UserModels;

namespace Optepafi.Models.UserModelMan.UserModelReps;

/// <summary>
/// One of three interfaces whose implementations represent properties of individual user model types that is tied to specific template type.
/// 
/// Especially this one represents bond of user model to some template which represents some vertex/edge attributes types used in user models computations.  
/// The other two interfaces are <see cref="IUserModelIdentifier{TUserModel}"/> and <see cref="IUserModelType{TTemplate,TUserModel}"/>.  
/// It should not be implemented right away. All implementations should derive from <see cref="UserModelRepresentative{TUserModel,TTemplate,TConfiguration}"/> instead.  
/// Thanks to contravariance of its template type parameter it is useful for correct pattern matching on its template type.  
/// </summary>
/// <typeparam name="TTemplate">Template type which represented user model is tied to.</typeparam>
public interface IUserModelTemplateBond<in TTemplate>
     where TTemplate : ITemplate
{
    
}