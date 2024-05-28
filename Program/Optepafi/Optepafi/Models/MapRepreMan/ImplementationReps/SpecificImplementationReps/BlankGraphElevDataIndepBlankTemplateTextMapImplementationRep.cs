using System;
using System.Threading;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapFormats;
using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.MapRepreMan.Graphs.SpecificGraphs;
using Optepafi.Models.MapRepreMan.MapRepres.GraphImplementations;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.MapRepreMan.MapRepreImplementationReps.SpecificImplementationReps;

public class BlankGraphElevDataIndepBlankTemplateTextMapImplementationRep :
    ElevDataIndepImplementationRep<BlankTemplate, TextMap, TextMap, IBlankGraph<BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes>, BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes>
{
    public static BlankGraphElevDataIndepBlankTemplateTextMapImplementationRep Instance { get; } = new();
    private BlankGraphElevDataIndepBlankTemplateTextMapImplementationRep() { }


    public override BlankTemplate UsedTemplate { get; } = BlankTemplate.Instance;
    public override IMapFormat<TextMap> UsedMapFormat { get; } = TextMapRepresentative.Instance;

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
    private class BlankGraphElevDataIndepBlankTemplateTextMapIntraImplementation :
        BlankGraphElevDataIndepBlankTemplateTextMapImplementation { }
}