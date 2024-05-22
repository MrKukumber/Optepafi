using System;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.MapRepreConstrs;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.MapRepreReps;

public interface IGraphRepresentative<out TGraph, TVertexAttributes, TEdgeAttributes>
    where TGraph : IGraph<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    
    sealed TGraph CreateGraph<TTemplate, TMap, TMapRepre>(TTemplate template, TMap map,
        IProgress<GraphCreationReport>? progress, CancellationToken? cancellationToken, IImplementationIdentifier<ITemplate, IMap, TMapRepre>[] constructors)
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
        where TMap : IMap
        where TMapRepre : IMapRepre
    {
        foreach (var constructor in constructors)
        {
            if (constructor is IImplementationElevDataIndependentConstr<TTemplate, TMap, TGraph,TVertexAttributes, TEdgeAttributes> c)
            {
                return c.ConstructMapRepre(template, map, progress, cancellationToken);
            }
        }

        throw new ArgumentException(
            "There is no constructor for given template and map which does not require elevation data. Existence of constructor should be checked before creation.");
    }
    
    sealed TGraph CreateGraph<TTemplate, TMap, TMapRepre>(TTemplate template, TMap map, IElevData elevData,
        IProgress<GraphCreationReport>? progress, CancellationToken? cancellationToken, IImplementationIdentifier<ITemplate, IMap, TMapRepre>[] identifiers)
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
        where TMap : IMap
        where TMapRepre : IMapRepre
    {
        foreach (var identifier in identifiers)
        {
            if (identifier is IImplementationElevDataDependentConstr<TTemplate,TMap,TGraph,TVertexAttributes,TEdgeAttributes> constructor)
            {
                return constructor.ConstructMapRepre(template, map, elevData, progress, cancellationToken);
            }
        }
        throw new ArgumentException(
            "There is no constructor for given template and map which requires elevation data.");
    }
}
