using System.Collections.Generic;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.Graphs.Representatives;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModelReps;
using Optepafi.Models.UserModelMan.UserModels.Functionalities;
using Optepafi.Models.Utils;

namespace Optepafi.Models.SearchingAlgorithmMan.Implementations;

/// <summary>
/// Represents indicator of searching algorithms requirements for specific graph and user model functionalities.
/// 
/// Every implementation defines its requirements for used map representations functionality in process of searching. This requirements can be tested by <see cref="IsUsableGraph{TVertexAttributes,TEdgeAttributes}"/> and <see cref="DoesRepresentUsableGraph"/> methods.  
/// This interface should not be implemented right away. Preferred way is to derive from <see cref="ISearchingAlgorithmImplementation{TConfiguration}"/> interface.  
/// For the covariant nature of its type parameters it is suitable type for storing collections of algorithm implementations.  
/// </summary>
public interface ISearchingAlgorithmImplementationRequirementsIndicator
{
    /// <summary>
        /// For provided graph representative resolves whether represented graph type satisfies implementations functionality requirements.
        /// 
        /// This test has to correspond to test provided by <see cref="IsUsableGraph{TVertexAttributes,TEdgeAttributes}"/> method.  
        /// Test is done on no particular vertex-edge attribute types.  
        /// </summary>
        /// <param name="mapRepreCreator">Representative of graph type whose functionalities are tested.</param>
        /// <returns>True if all requirements are satisfied. Otherwise, false.</returns>
        bool DoesRepresentUsableGraph<TVertex, TEdge>(IGraphRepresentative<IGraph<TVertex, TEdge>, TVertex, TEdge> mapRepreCreator) 
            where TVertex : IVertex
            where TEdge : IEdge;
    
        /// <summary>
        /// For provided user model type resolves whether represented user model type satisfies implementations functionality requirements.
        /// 
        /// This test has to correspond to test provided by <see cref="IsUsableUserModel{TVertexAttributes,TEdgeAttributes}"/> method.  
        /// Test is done on particular vertex-edge attribute types.  
        /// </summary>
        /// <param name="userModelType">Representative of user model type whose functionalities are tested.</param>
        /// <typeparam name="TVertexAttributes">Type of vertex attributes used in represented user model type.</typeparam>
        /// <typeparam name="TEdgeAttributes">Type of edge attributes used in represented user model type.</typeparam>
        /// <returns>True, if all requirements are satisfied. False otherwise.</returns>
        bool DoesRepresentUsableUserModel<TVertexAttributes, TEdgeAttributes>(IUserModelType<IComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>, ITemplate<TVertexAttributes, TEdgeAttributes>> userModelType) where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes;
    
        /// <summary>
        /// Checks whether provided graphs functionality satisfies implementations requirements.
        /// 
        /// This test has to correspond to test provided by <see cref="DoesRepresentUsableGraph"/> method.
        /// </summary>
        /// <param name="graph">Graph to be tested for its functionalities.</param>
        /// <typeparam name="TVertexAttributes">Type of vertex attributes used in vertices of graph.</typeparam>
        /// <typeparam name="TEdgeAttributes">Type of edge attributes used in edges of graph.</typeparam>
        /// <returns>True if all requirements are satisfied. Otherwise false.</returns>
        bool IsUsableGraph<TVertexAttributes, TEdgeAttributes>(
            IGraph<IAttributeBearingVertex<TVertexAttributes>, IAttributesBearingEdge<TEdgeAttributes>> graph)
            where TVertexAttributes : IVertexAttributes
            where TEdgeAttributes : IEdgeAttributes;
    
        /// <summary>
        /// Checks whether provided user models functionality satisfies implementations requirements.
        /// 
        /// This test has to correspond to test provided by <see cref="DoesRepresentUsableUserModel{TVertexAttributes,TEdgeAttributes}"/> method.  
        /// </summary>
        /// <param name="userModel">User model to be tested for its functionalities.</param>
        /// <typeparam name="TVertexAttributes">Type of vertex attributes used by user model.</typeparam>
        /// <typeparam name="TEdgeAttributes">Type of edge attributes used by user model.</typeparam>
        /// <returns>True, if all requirements are satisfied. False otherwise.</returns>
        bool IsUsableUserModel<TVertexAttributes, TEdgeAttributes>(
            IComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes> userModel)
            where TVertexAttributes : IVertexAttributes
            where TEdgeAttributes : IEdgeAttributes;
    
        /// <summary>
        /// Checks whether functionalities of all provided user models satisfy implementations requirements.
        /// 
        /// Default implementation iterates through all user models and calls <see cref="IsUsableUserModel{TVertexAttributes,TEdgeAttributes}"/> method on them.  
        /// </summary>
        /// <param name="userModels">User models to be tested for their functionalities.</param>
        /// <typeparam name="TVertexAttributes">Type of vertex attributes used by user model.</typeparam>
        /// <typeparam name="TEdgeAttributes">Type of edge attributes used by user model.</typeparam>
        /// <returns>True, if all requirements for all user models are satisfied. False otherwise.</returns>
        bool AreUsableUserModels<TVertexAttributes, TEdgeAttributes>(
            IEnumerable<IComputing<ITemplate<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>> userModels)
            where TVertexAttributes : IVertexAttributes
            where TEdgeAttributes : IEdgeAttributes
        {
            foreach (var userModel in userModels)
            {
                if (!IsUsableUserModel(userModel)) return false;
            }
            return true;
        }
}