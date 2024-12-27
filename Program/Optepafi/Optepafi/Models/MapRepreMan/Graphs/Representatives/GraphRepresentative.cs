using System;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.Implementations;
using Optepafi.Models.MapRepreMan.Implementations.Representatives;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.SearchingAlgorithmMan;
using Optepafi.Models.SearchingAlgorithmMan.SearchingAlgorithms;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.UserModelMan.UserModelReps;
using Optepafi.Models.UserModelMan.UserModels.Functionalities;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Configurations;

namespace Optepafi.Models.MapRepreMan.Graphs.Representatives;

/// <summary>
/// Represents representative of graph that is tied to some map representation.
/// 
/// It contains method for creating graph with and without use of elevation data.  
/// Each graph representative should correspond to some map representation representative which will then hold reference on it. They together then represent couple of map representation and its graph.  
/// Each graph should have its representative so it could be presented as corresponding graph of some map representation.  
/// Preferred way to interact with representatives is through <see cref="MapRepreManager"/>.  
/// </summary>
/// <typeparam name="TGraph">Type of represented graph.</typeparam>
/// <typeparam name="TVertex">Type of vertices used in represented graph.</typeparam>
/// <typeparam name="TEdge">Type of edges used in represented graph.</typeparam>
public abstract class GraphRepresentative<TGraph, TVertex, TEdge> : 
    IGraphRepresentative<TGraph, TVertex, TEdge>,
    IGraphCreator<TGraph> 
    where TGraph : IGraph<TVertex, TEdge>
    where TVertex: IVertex
    where TEdge: IEdge
{
    
    /// <inheritdoc cref="IGraphCreator{TMapRepre}.CreateableImplementationsIndicators"/>
    public abstract IImplementationIndicator<ITemplate, IMap, IMapRepre>[] CreateableImplementationsIndicators { get; }
    
    /// <inheritdoc cref="IGraphCreator{TMapRepre}.CreateGraph{TTemplate,TMap,TConfiguration,TVertexAttributes,TEdgeAttributes}(TTemplate,TMap,TConfiguration,System.IProgress{Optepafi.Models.MapRepreMan.MapRepreConstructionReport}?,System.Nullable{System.Threading.CancellationToken})"/> 
    public TGraph CreateGraph<TTemplate, TMap, TConfiguration, TVertexAttributes, TEdgeAttributes>(
        TTemplate template, TMap map, TConfiguration configuration,
        IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken)
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
        where TMap : IMap
        where TConfiguration : IConfiguration
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes
    {
        foreach (var indicator in CreateableImplementationsIndicators)
        {
            if (indicator is IImplementationElevDataIndepCreator<TTemplate, TMap, TGraph, TConfiguration, TVertex, TEdge, TVertexAttributes, TEdgeAttributes> constructor)
            {
                return constructor.CreateImplementation(template, map, configuration, progress, cancellationToken);
            }
        }
        throw new ArgumentException(
            "There is no constructor for given template and map which does not require elevation data. Existence of constructor should be checked before creation.");
    }


    ///<inheritdoc cref="IGraphCreator{TMapRepre}.CreateGraph{TTemplate,TMap,TConfiguration,TVertexAttributes,TEdgeAttributes}(TTemplate,TMap,Optepafi.Models.ElevationDataMan.IElevData,TConfiguration,System.IProgress{Optepafi.Models.MapRepreMan.MapRepreConstructionReport}?,System.Nullable{System.Threading.CancellationToken})"/> 
    public TGraph CreateGraph<TTemplate, TMap, TConfiguration, TVertexAttributes, TEdgeAttributes>(
        TTemplate template, TMap map, IElevData elevData,
        TConfiguration configuration, IProgress<MapRepreConstructionReport>? progress,
        CancellationToken? cancellationToken)
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
        where TMap : IGeoLocatedMap
        where TConfiguration : IConfiguration
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes
    {
        foreach (var indicator in CreateableImplementationsIndicators)
        {
            if (indicator is IImplementationElevDataDepCreator<TTemplate, TMap, TGraph, TConfiguration, TVertex, TEdge, TVertexAttributes, TEdgeAttributes> constructor) 
            {
                return constructor.CreateImplementation(template, map, elevData, configuration, progress, cancellationToken);
            }
        }

        throw new ArgumentException(
            "There is no constructor for given template and map which requires elevation data.");
    }

    public abstract bool RevelationForSearchingAlgorithmMan<TTemplate, TVertexAttributes, TEdgeAttributes>(
        SearchingAlgorithmManager searchingAlgorithmMan,
        IUserModelType<IComputing<TTemplate, TVertexAttributes, TEdgeAttributes>, TTemplate> userModelType,
        ISearchingAlgorithm algorithm)
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
        where TVertexAttributes : IVertexAttributes
        where TEdgeAttributes : IEdgeAttributes;
        
}