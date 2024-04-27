using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.UserModelMan;

public interface ISettableUserModel<TTemplate> : IUserModel<TTemplate> where TTemplate : ITemplate
{
    
}