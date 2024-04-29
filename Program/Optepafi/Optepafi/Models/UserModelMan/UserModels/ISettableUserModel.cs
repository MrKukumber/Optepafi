using System.Collections.Generic;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan.UserModelAdjustables;

namespace Optepafi.Models.UserModelMan.UserModels;

public interface ISettableUserModel<TTemplate> : IUserModel<TTemplate> where TTemplate : ITemplate
{
    public IReadOnlySet<IUserModelAdjustable> GetAdjustables();
    
}