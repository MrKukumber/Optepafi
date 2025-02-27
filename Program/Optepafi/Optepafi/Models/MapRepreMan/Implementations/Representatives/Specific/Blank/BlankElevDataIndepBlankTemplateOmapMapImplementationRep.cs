using System;
using System.Threading;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapRepresentatives;
using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.MapRepreMan.Implementations.Specific.Blank;
using Optepafi.Models.TemplateMan.Templates;
using Optepafi.Models.Utils.Configurations;

namespace Optepafi.Models.MapRepreMan.Implementations.Representatives.Specific.Blank;

/// <summary>
/// Represents graph implementation of OMAP map representation which uses blank template and for its creation are not used any
/// elevation data.
/// Represented implementation of map representation is not usable because of blank template. It is just demonstrative type
/// for presenting application functionality.
/// /// </summary>
public class BlankElevDataIndepBlankTemplateOmapMapImplementationRep:
    ElevDataIndepImplementationRep<BlankTemplate, OmapMap, OmapMap, BlankElevDataIndepBlankTemplateOmapMapImplementation, NullConfiguration, IBlankGraph<BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes>.Vertex, IBlankGraph<BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes>.Edge, BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes>
{
    public static BlankElevDataIndepBlankTemplateOmapMapImplementationRep Instance { get; } = new();
    private BlankElevDataIndepBlankTemplateOmapMapImplementationRep(){ }
    public override BlankTemplate UsedTemplate { get; } = BlankTemplate.Instance;
    public override IMapFormat<OmapMap> UsedMapFormat { get; } = OmapMapRepresentative.Instance;

    /// <inheritdoc cref="ElevDataIndepImplementationRep{TTemplate,TMap,TUsableSubMap,TGraph,TConfiguration,TVertex,TEdge,TVertexAttributes,TEdgeAttributes}"/> 
    public override BlankElevDataIndepBlankTemplateOmapMapImplementation CreateImplementation(BlankTemplate template, OmapMap map, NullConfiguration configuration, IProgress<MapRepreConstructionReport>? progress,
        CancellationToken? cancellationToken)
    {
        for (int i = 1; i <= 100; i++)
        {
            if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) 
                return new BlankElevDataIndepBlankTemplateOmapMapImplementation();
            Thread.Sleep(20); //Lot of work.
            progress?.Report(new MapRepreConstructionReport(i));
        }
        return new BlankElevDataIndepBlankTemplateOmapMapImplementation();
    }
}