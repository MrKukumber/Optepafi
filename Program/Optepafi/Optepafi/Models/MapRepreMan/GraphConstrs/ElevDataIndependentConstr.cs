// using System;
// using System.Data.SqlTypes;
// using System.Threading;
// using Optepafi.Models.MapMan;
// using Optepafi.Models.MapRepreMan.MapRepreReps;
// using Optepafi.Models.MapRepreMan.MapRepres;
// using Optepafi.Models.TemplateMan;
// using Optepafi.Models.TemplateMan.TemplateAttributes;

// namespace Optepafi.Models.MapRepreMan.MapRepreConstrs;

// public sealed class ElevDataIndependentConstr<TTemplate, TMap, TMapRepre, TVertexAttributes, TEdgeAttributes> : 
    // IElevDataIndependentConstr<TTemplate, TMap, TMapRepre> 
    // where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
    // where TMap : IMap 
    // where TMapRepre : IDefinedFunctionalityMapRepre<TVertexAttributes, TEdgeAttributes>, IConstrElevDataIndepMapRepre<TTemplate, TMap>, new()
    // where TVertexAttributes : IVertexAttributes
    // where TEdgeAttributes : IEdgeAttributes
// {
    // public TTemplate UsedTemplate { get; }
    // public IMapFormat<TMap> UsedMapFormat { get; }
    // public bool RequiresElevData { get; } = false;
    
    // public ElevDataIndependentConstr(TTemplate usedTemplate, IMapFormat<TMap> usedMapFormat)
    // {
        // UsedTemplate = usedTemplate;
        // UsedMapFormat = usedMapFormat;
    // }
    // public TMapRepre? ConstructMapRepre(TTemplate template, TMap map, 
        // IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken)
    // {

        // TMapRepre mapRepre = new TMapRepre();
        // mapRepre.FillUp(template, map, progress, cancellationToken);
        // return mapRepre;
    // }
// }