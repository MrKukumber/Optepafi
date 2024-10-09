using System;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.Implementations.Representatives;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.Utils;

namespace Optepafi.Models.MapRepreMan.Graphs.Representatives;


/// <summary>
/// Represents representative of graph that is tied to some map representation.
/// 
/// It contains method for creating graph with and without use of elevation data.  
/// Each graph representative should correspond to some map representation representative which will then hold reference on it. They together then represent couple of map representation and its graph.  
/// Each graph should have its representative so it could presented as corresponding graph of some map representation.  
/// Preferred way to interact with representatives is through <see cref="MapRepreManager"/>.  
/// </summary>
/// <typeparam name="TGraph">Type of represented graph.</typeparam>
/// <typeparam name="TConfiguration">Type of configuration used in graph construction process.</typeparam>
/// <typeparam name="TVertexAttributes">Type of vertex attributes used in represented graph.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes used in represented graph.</typeparam>
public interface IGraphRepresentative<out TGraph, TVertexAttributes, TEdgeAttributes>
    where TGraph : IGraph<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    /// <summary>
    /// Method which creates graph from provided template and map represented by the representative by using one of provided implementation indicator (constructor).
    /// 
    /// It constructs the graph without requiring elevation data.  
    /// Before calling this method it should be checked that provided collection of indicators contains constructor for provided template and map which does not require elevation data. Graph construction will throw exception if no such constructor is provided.  
    /// </summary>
    /// <param name="template">Used template in graphs creation.</param>
    /// <param name="map">Used map in graphs creation.</param>
    /// <param name="configuration">Configuration of created graph.</param>
    /// <param name="progress">Object by which can be progress of construction subscribed .</param>
    /// <param name="cancellationToken">Token for cancelling construction.</param>
    /// <param name="indicators">Indicators of implementations which are used for graphs construction. They should be convertible to implementation constructors.</param>
    /// <typeparam name="TTemplate">Type of provided template. It is used for testing and pattern matching of indicators to corresponding constructors.</typeparam>
    /// <typeparam name="TMap">Type of provided map. It is used for testing and pattern matching of indicators to corresponding constructors.</typeparam>
    /// <typeparam name="TMapRepre">Type of map representation whose implementations indicators are provided for constructing the graph.</typeparam>
    /// <returns>Created graph tide to some map representation.</returns>
    /// <exception cref="ArgumentException">When none of provided indicators is usable implementation constructor.</exception>
    TGraph CreateGraph<TTemplate, TMap, TMapRepre, TConfiguration>(TTemplate template, TMap map, TConfiguration configuration,
        IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken, IImplementationIndicator<ITemplate, IMap, TMapRepre>[] indicators)
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
        where TMap : IMap
        where TMapRepre : IMapRepre
        where TConfiguration : IConfiguration;

    /// <summary>
    /// Method which creates graph from provided template and map represented by th representative by using one of provided implementation indicator (constructor).
    /// For construction of this graph are required elevation data for provided maps area.
    /// Before calling this method it should be checked that provided collection of indicators contains constructor for provided template and map which requires elevation data. Graph construction will throw exception if no such constructor is provided.
    /// </summary>
    /// <param name="template">Used template in graphs creation.</param>
    /// <param name="map">Used map in graphs creation.</param>
    /// <param name="configuration">Configuration of created graph.</param>
    /// <param name="elevData">Elevation data corresponding to map area used in graphs creation.</param>
    /// <param name="progress">Object by which can be progress of construction subscribed .</param>
    /// <param name="cancellationToken">Token for cancelling construction.</param>
    /// <param name="indicators">Indicators of implementations which are used for graphs construction. They should be convertible to implementation constructors.</param>
    /// <typeparam name="TTemplate">Type of provided template. It is used for testing and pattern matching of indicators to corresponding constructors.</typeparam>
    /// <typeparam name="TMap">Type of provided map. It is used for testing and pattern matching of indicators to corresponding constructors.</typeparam>
    /// <typeparam name="TMapRepre">Type of map representation whose implementations indicators are provided for constructing the graph.</typeparam>
    /// <returns>Created graph tide to some map representation.</returns>
    /// <exception cref="ArgumentException">When none of provided indicators is usable implementation constructor.</exception>
    TGraph CreateGraph<TTemplate, TMap, TMapRepre, TConfiguration>(TTemplate template, TMap map, IElevData elevData,
        TConfiguration configuration, IProgress<MapRepreConstructionReport>? progress,
        CancellationToken? cancellationToken,
        IImplementationIndicator<ITemplate, IMap, TMapRepre>[] indicators)
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
        where TMap : IGeoLocatedMap
        where TMapRepre : IMapRepre
        where TConfiguration : IConfiguration;

}
