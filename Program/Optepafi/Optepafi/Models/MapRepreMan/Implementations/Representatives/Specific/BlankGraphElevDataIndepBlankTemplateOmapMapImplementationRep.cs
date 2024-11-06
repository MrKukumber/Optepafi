using System;
using System.Threading;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapFormats;
using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.MapRepreMan.Implementations.Specific;
using Optepafi.Models.TemplateMan.Templates;
using Optepafi.Models.Utils.Configurations;

namespace Optepafi.Models.MapRepreMan.Implementations.Representatives.Specific;

//TODO: comment
public class BlankGraphElevDataIndepBlankTemplateOmapMapImplementationRep:
    ElevDataIndepImplementationRep<BlankTemplate, OmapMap, OmapMap, IBlankGraph<BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes>, NullConfiguration, BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes>
{
    public static BlankGraphElevDataIndepBlankTemplateOmapMapImplementationRep Instance { get; } = new();
    private BlankGraphElevDataIndepBlankTemplateOmapMapImplementationRep(){ }
    public override BlankTemplate UsedTemplate { get; } = BlankTemplate.Instance;
    public override IMapFormat<OmapMap> UsedMapFormat { get; } = OmapMapRepresentative.Instance;

    public override IBlankGraph<BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes> ConstructMapRepre(BlankTemplate template, OmapMap map, NullConfiguration configuration, IProgress<MapRepreConstructionReport>? progress,
        CancellationToken? cancellationToken)
    {
        for (int i = 1; i <= 100; i++)
        {
            if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) 
                return new BlankGraphElevDataIndepBlankTemplateOmapMapImplementation();
            Thread.Sleep(20); //Lot of work.
            progress?.Report(new MapRepreConstructionReport(i));
        }
        return new BlankGraphElevDataIndepBlankTemplateOmapMapImplementation();
    }
}