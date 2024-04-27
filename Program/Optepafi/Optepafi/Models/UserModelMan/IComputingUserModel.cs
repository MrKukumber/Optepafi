using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.UserModelMan;

public interface IComputingUserModel<TTemplate> : IUserModel<TTemplate> where TTemplate : ITemplate
{
    int ComputeWeight(OrientedEdge<TTemplate> edge);
}