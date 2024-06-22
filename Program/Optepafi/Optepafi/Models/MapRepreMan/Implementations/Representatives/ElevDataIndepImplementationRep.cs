using System;
using System.Threading;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.MapRepreConstrs;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.Implementations.Representatives;


/// <summary>
/// Abstract class which represents map representations/graphs implementation for some template and map format.
/// This class has two functions: it contains properties that indicates prerequisites of implementations creation and also is able of creating such implementation.
/// This class or its elevation dependent counterpart should be derived from by representatives of specific implementations. These subclasses can then be presented in map representation representatives as indicators of viable map representation implementations.
/// Thanks to constructive nature of these subclasses they can then be used in graph representatives for constructing of particular implementations of graphs too.
/// Each implementation representative is:
/// - indicates defined type of map representations graph successor
/// - constructing defined type of graph
/// - indicating which template and map types are needed for construction of graph. Map type is indicated by its corresponding format instance.
/// - constructing graph with this template and some particular maps descendant without need of using elevation data in this process
///<para>
/// Last two points may seem little bit tricky ... the indicated map type does not have to be same one as is required by constructor.
/// The reason for this design is that indicator returns map format which represents defined map type.
/// However there can be case, when map format represents the predecessor map type of some map subtypes. (for example if some map format is known to be geo-referenced but in som cases the geo-reference is not available in map file. So the represented map type can have geo-referenced and non-geo-referenced successors.)
/// This inconsistency of types is resolved either by <see cref="MapRepreManager"/> or <see cref="IMapRepreRepresentative{TMapRepre}"/> which will for construction of map representation require some specific properties of provided map (for example that it was geo-located when the elevation data are required for construction).
/// This will then ensure that only right subclass of indicated map type will be admitted to map representation construction process.
/// </para>
/// 
/// </summary>
/// <typeparam name="TTemplate">Indicated type of template and used for creating of map representation/graph.</typeparam>
/// <typeparam name="TMap">Indicated type of map whose representative format is provided.</typeparam>
/// <typeparam name="TUsableSubMap">Subtype of map type that has correct properties to be used for map representation/graph creation.</typeparam>
/// <typeparam name="TGraph">Type of graph (map representation) which will be constructed.</typeparam>
/// <typeparam name="TVertexAttributes">Type of vertex attributes which will be used in vertices of created graph, defined by used template.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes which will be used in edges of created graph, defined by used template.</typeparam>
public abstract class ElevDataIndepImplementationRep<TTemplate, TMap, TUsableSubMap,TGraph, TVertexAttributes, TEdgeAttributes> : 
    IImplementationIndicator<TTemplate, TMap, TGraph>, IImplementationElevDataIndependentConstr<TTemplate,TUsableSubMap,TGraph,TVertexAttributes,TEdgeAttributes>
    where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes> 
    where TMap : IMap 
    where TUsableSubMap : TMap, IMap
    where TGraph : IGraph<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    public abstract TTemplate UsedTemplate { get; }
    public abstract IMapFormat<TMap> UsedMapFormat { get; }
    public bool RequiresElevData { get; } = false;

    public abstract TGraph ConstructMapRepre(TTemplate template, TUsableSubMap map,
        IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken);
}