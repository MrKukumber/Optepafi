using System;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapRepresentatives;
using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.MapRepreMan.Implementations.Specific;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.MapRepreMan.Implementations.Representatives.Specific;

/// <summary>
/// Represents implementation of blank map representation/graph for <c>BlankTemplate</c> template type and <c>TextMap</c> map type.
/// 
/// Represented implementation needs elevation data for its creation even though it does not use them.  
/// For more information on representatives of elevation data independent implementations see <see cref="ElevDataIndepImplementationRep{TTemplate,TMap,TUsableSubMap,TGraph,TVertexAttributes,TEdgeAttributes}"/>.  
/// </summary>
public class BlankGraphElevDataDepBlankTemplateTextMapImplementationRep :
    ElevDataDepImplementationRep<BlankTemplate, TextMap, TextMap, IBlankGraph<BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes>, BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes>
{
    public static BlankGraphElevDataDepBlankTemplateTextMapImplementationRep Instance { get; } = new();
    private BlankGraphElevDataDepBlankTemplateTextMapImplementationRep() { }


    /// <inheritdoc cref="ElevDataIndepImplementationRep{TTemplate,TMap,TUsableSubMap,TGraph,TVertexAttributes,TEdgeAttributes}.UsedTemplate"/>
    public override BlankTemplate UsedTemplate { get; } = BlankTemplate.Instance;
    
    /// <inheritdoc cref="ElevDataIndepImplementationRep{TTemplate,TMap,TUsableSubMap,TGraph,TVertexAttributes,TEdgeAttributes}.UsedMapFormat"/>
    public override IMapFormat<TextMap> UsedMapFormat { get; } = TextMapRepresentative.Instance;

    /// <inheritdoc cref="ElevDataIndepImplementationRep{TTemplate,TMap,TUsableSubMap,TGraph,TVertexAttributes,TEdgeAttributes}.ConstructMapRepre"/>
    /// <remarks>
    /// It simulates implementations creation with reporting state of simulated process.
    /// </remarks>
    public override IBlankGraph<BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes> ConstructMapRepre
    (BlankTemplate template, TextMap map, IElevData elevData, IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken)
    {
        for (int i = 1; i <= 100; i++)
        {
            if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) 
                return new BlankGraphElevDataDepBlankTemplateTextMapIntraImplementation();
            Thread.Sleep(30); //Lot of work.
            progress?.Report(new MapRepreConstructionReport(i));
        }
        return new BlankGraphElevDataDepBlankTemplateTextMapIntraImplementation();
    }

    /// <summary>
    /// Hidden intra class which inherits form <c>BlankGraphElevDataDepBlankTemplateTextMapImplementation </c>. Its instance is returned in <c>ConstructMapRepre</c> method.
    /// </summary>
    private class BlankGraphElevDataDepBlankTemplateTextMapIntraImplementation :
        BlankGraphElevDataDepBlankTemplateTextMapImplementation;
}