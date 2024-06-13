using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.UserModelMan.UserModels;

/// <summary>
/// Represents user model that can provide services for aggregating properties bounded to some vertex/edge attributes.
/// This interface defines no contract. It just declares, that user model is able to provide some functionality. Cast to some particular successor is required for accessing functionality.
/// This interface should not be implemented right away. Its functionality-defining successors should be implemented instead.
/// </summary>
/// <typeparam name="TVertexAttributes">Type of vertex attributes, which is user model able to use for delivering services.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes, which is user model able to use for delivering services.</typeparam>
public interface IUsableUserModel< in TVertexAttributes, in TEdgeAttributes> : IUserModel
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes;