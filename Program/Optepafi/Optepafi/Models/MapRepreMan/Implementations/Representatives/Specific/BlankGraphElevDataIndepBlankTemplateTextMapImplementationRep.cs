using System;
using System.Threading;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapRepresentatives;
using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.MapRepreMan.Implementations.Specific;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.MapRepreMan.Implementations.Representatives.Specific;

/// <summary>
/// Represents implementation of blank map representation/graph for <c>BlankTemplate</c> template type and <c>TextMap</c> map type. Represented implementation does not need elevation data for its creation.
/// For more information on representatives of elevation data independent implementations see <see cref="ElevDataIndepImplementationRep{TTemplate,TMap,TUsableSubMap,TGraph,TVertexAttributes,TEdgeAttributes}"/>.
/// </summary>
public class BlankGraphElevDataIndepBlankTemplateTextMapImplementationRep :
    ElevDataIndepImplementationRep<BlankTemplate, TextMap, TextMap, IBlankGraph<BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes>, BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes>
{
    public static BlankGraphElevDataIndepBlankTemplateTextMapImplementationRep Instance { get; } = new();
    private BlankGraphElevDataIndepBlankTemplateTextMapImplementationRep() { }


    /// <inheritdoc cref="ElevDataIndepImplementationRep{TTemplate,TMap,TUsableSubMap,TGraph,TVertexAttributes,TEdgeAttributes}.UsedTemplate"/>
    public override BlankTemplate UsedTemplate { get; } = BlankTemplate.Instance;
    
    /// <inheritdoc cref="ElevDataIndepImplementationRep{TTemplate,TMap,TUsableSubMap,TGraph,TVertexAttributes,TEdgeAttributes}.UsedMapFormat"/>
    public override IMapFormat<TextMap> UsedMapFormat { get; } = TextMapRepresentative.Instance;

    /// <inheritdoc cref="ElevDataIndepImplementationRep{TTemplate,TMap,TUsableSubMap,TGraph,TVertexAttributes,TEdgeAttributes}.ConstructMapRepre"/>
    /// <remarks>
    /// It simulates implementations creation with reporting state of simulated process.
    /// </remarks>
    public override IBlankGraph<BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes> ConstructMapRepre
    (BlankTemplate template, TextMap map, IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken)
    {
        for (int i = 1; i <= 100; i++)
        {
            if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) 
                return null;
            Thread.Sleep(30); //Lot of work.
            progress?.Report(new MapRepreConstructionReport(i));
        }
        return new BlankGraphElevDataIndepBlankTemplateTextMapIntraImplementation();
    }
    /// <summary>
    /// Hidden intra class which inherits form <c>BlankGraphElevDataIndepBlankTemplateTextMapImplementation </c>. Its instance is returned in <c>ConstructMapRepre</c> method.
    /// </summary>
    private class BlankGraphElevDataIndepBlankTemplateTextMapIntraImplementation :
        BlankGraphElevDataIndepBlankTemplateTextMapImplementation { }
}