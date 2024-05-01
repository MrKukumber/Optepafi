using System.Collections.Generic;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModelAdjustables;

namespace Optepafi.Models.UserModelMan.UserModels;

public interface ISettableUserModel<out TTemplate> : IUserModel<TTemplate> where TTemplate : ITemplate
{
    public IReadOnlySet<IUserModelAdjustable> GetAdjustables();
    
}