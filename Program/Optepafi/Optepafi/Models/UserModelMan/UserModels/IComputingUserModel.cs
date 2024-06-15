using Avalonia.Controls;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.UserModelMan.UserModels;

/// <summary>
/// Represents user model that can provide services for computing and aggregating properties bounded to some vertex/edge attributes. These services are then required for example in path finding algorithms for computing weights of edges.
/// This interface defines no contract. It just declares, that user model is able to provide some functionality. Cast to some particular successor is required for accessing functionality.
/// This interface should not be implemented right away. Its functionality-defining successors should be implemented instead.
/// </summary>
/// <typeparam name="TTemplate">Associated template type.</typeparam>
/// <typeparam name="TVertexAttributes">Type of vertex attributes, which is user model able to use for delivering services.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes, which is user model able to use for delivering services.</typeparam>
public interface IComputingUserModel<out TTemplate, in TVertexAttributes, in TEdgeAttributes> : IUserModel<TTemplate>
    where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes> where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes;