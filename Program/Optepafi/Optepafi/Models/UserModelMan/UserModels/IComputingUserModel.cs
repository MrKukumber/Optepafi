using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.UserModelMan.UserModels;

public interface IComputingUserModel<TTemplate> : IUserModel<TTemplate> where TTemplate : ITemplate
{
    int ComputeWeight(IOrientedEdge<TTemplate> edge);
}