using System;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Configurations;

namespace Optepafi.Models.MapRepreMan.Implementations.Representatives;

/// <summary>
/// Abstract class which represents map representations/graphs implementation for some template and map format.
/// 
/// Its constructive ability is dependent on providing elevation data for map representations/graphs construction.  
/// This class has elevation data independent counterpart. For more information about both of these classes see <see cref="ElevDataIndepImplementationRep{TTemplate,TMap,TUsableSubMap,TGraph,TConfiguration,TVertexAttributes,TEdgeAttributes}"/>.  
/// Everything what is stated for <c>ElevDataIndepImplementationRep</c> holds for this class too except the fact that this class needs providing of elevation data for map representations/graphs creation.  
/// </summary>
public abstract class ElevDataDepImplementationRep<TTemplate, TMap, TUsableSubMap, TGraph, TConfiguration, TVertex, TEdge, TVertexAttributes, TEdgeAttributes> :
    IImplementationIndicator<TTemplate, TMap, TGraph>, 
    IImplementationElevDataDepCreator<TTemplate, TUsableSubMap, TGraph, TConfiguration, TVertex, TEdge, TVertexAttributes,TEdgeAttributes>
    where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes> 
    where TMap : IMap 
    where TUsableSubMap : TMap, IGeoLocatedMap
    where TGraph : IGraph<TVertex, TEdge>
    where TConfiguration : IConfiguration
    where TVertex : IAttributeBearingVertex<TVertexAttributes>
    where TEdge : IAttributesBearingEdge<TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    public abstract TTemplate UsedTemplate { get; }
    public abstract IMapFormat<TMap> UsedMapFormat { get; }
    public bool RequiresElevData { get; } = true;
    
    public abstract TGraph CreateImplementation(TTemplate template, TUsableSubMap map, IElevData elevData, TConfiguration configuration, 
        IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken);
}