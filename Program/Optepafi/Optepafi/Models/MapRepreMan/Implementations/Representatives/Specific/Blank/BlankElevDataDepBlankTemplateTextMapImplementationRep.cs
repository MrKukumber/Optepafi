using System;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapRepresentatives;
using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.MapRepreMan.Graphs.Specific.Blank;
using Optepafi.Models.MapRepreMan.Implementations.Specific.Blank;
using Optepafi.Models.TemplateMan.Templates;
using Optepafi.Models.Utils.Configurations;

namespace Optepafi.Models.MapRepreMan.Implementations.Representatives.Specific.Blank;

/// <summary>
/// Represents implementation of blank map representation/graph for <c>BlankTemplate</c> template type and <c>TextMap</c> map type.
/// 
/// Represented implementation needs elevation data for its creation even though it does not use them.  
/// For more information on representatives of elevation data independent implementations see <see cref="ElevDataDepImplementationRep{TTemplate,TMap,TUsableSubMap,TImplementation,TConfiguration,TVertex,TEdge,TVertexAttributes,TEdgeAttributes}"/>.  
/// </summary>
public class BlankElevDataDepBlankTemplateTextMapImplementationRep :
    ElevDataDepImplementationRep<BlankTemplate, TextMap, TextMap, BlankElevDataDepBlankTemplateTextMapImplementation, NullConfiguration, IBlankGraph<BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes>.Vertex, IBlankGraph<BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes>.Edge, BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes>
{
    public static BlankElevDataDepBlankTemplateTextMapImplementationRep Instance { get; } = new();
    private BlankElevDataDepBlankTemplateTextMapImplementationRep() { }


    /// <inheritdoc cref="ElevDataDepImplementationRep{TTemplate,TMap,TUsableSubMap,TImplementation,TConfiguration,TVertex,TEdge,TVertexAttributes,TEdgeAttributes}.UsedTemplate"/>
    public override BlankTemplate UsedTemplate { get; } = BlankTemplate.Instance;
    
    /// <inheritdoc cref="ElevDataDepImplementationRep{TTemplate,TMap,TUsableSubMap,TImplementation,TConfiguration,TVertex,TEdge,TVertexAttributes,TEdgeAttributes}.UsedMapFormat"/>
    public override IMapFormat<TextMap> UsedMapFormat { get; } = TextMapRepresentative.Instance;

    /// <inheritdoc cref="ElevDataIndepImplementationRep{TTemplate,TMap,TUsableSubMap,TImplementation,TConfiguration,TVertex,TEdge,TVertexAttributes,TEdgeAttributes}.CreateImplementation"/>
    /// <remarks>
    /// It simulates implementations creation with reporting state of simulated process.
    /// </remarks>
    public override BlankElevDataDepBlankTemplateTextMapImplementation CreateImplementation
    (BlankTemplate template, TextMap map, IElevData elevData, NullConfiguration configuration, IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken)
    {
        for (int i = 1; i <= 100; i++)
        {
            if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) 
                return new BlankElevDataDepBlankTemplateTextMapIntraImplementation();
            Thread.Sleep(30); //Lot of work.
            progress?.Report(new MapRepreConstructionReport(i));
        }
        return new BlankElevDataDepBlankTemplateTextMapIntraImplementation();
    }

    /// <summary>
    /// Hidden intra class which inherits form <c>BlankGraphElevDataDepBlankTemplateTextMapImplementation </c>. Its instance is returned in <c>ConstructMapRepre</c> method.
    /// </summary>
    private class BlankElevDataDepBlankTemplateTextMapIntraImplementation :
        BlankElevDataDepBlankTemplateTextMapImplementation;
}