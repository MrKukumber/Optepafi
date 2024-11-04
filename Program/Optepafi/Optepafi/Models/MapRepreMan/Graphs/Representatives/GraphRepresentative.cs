using System;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.Implementations.Representatives;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Configurations;

namespace Optepafi.Models.MapRepreMan.Graphs.Representatives;

public abstract class GraphRepresentative<TGraph, TVertexAttributes, TEdgeAttributes> : IGraphRepresentative<TGraph, TVertexAttributes, TEdgeAttributes>
    where TGraph : IGraph<TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{

    public TGraph CreateGraph<TTemplate, TMap, TMapRepre, TConfiguration>(TTemplate template, TMap map, TConfiguration configuration,
        IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken, IImplementationIndicator<ITemplate, IMap, TMapRepre>[] indicators) 
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes> 
        where TMap : IMap 
        where TMapRepre : IMapRepre 
        where TConfiguration : IConfiguration
        {
             foreach (var indicator in indicators)
             {
                 if (indicator is IImplementationElevDataIndepConstr<TTemplate, TMap, TGraph, TConfiguration, TVertexAttributes, TEdgeAttributes> constructor)
                 {
                     return constructor.ConstructMapRepre(template, map, configuration, progress, cancellationToken);
                 }
             }
     
             throw new ArgumentException(
                 "There is no constructor for given template and map which does not require elevation data. Existence of constructor should be checked before creation.");
         }


    public TGraph CreateGraph<TTemplate, TMap, TMapRepre, TConfiguration>(TTemplate template, TMap map, IElevData elevData,
        TConfiguration configuration, IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken, IImplementationIndicator<ITemplate, IMap, TMapRepre>[] indicators)
        where TTemplate : ITemplate<TVertexAttributes, TEdgeAttributes>
        where TMap : IGeoLocatedMap
        where TMapRepre : IMapRepre
        where TConfiguration : IConfiguration
    {
        foreach (var indicator in indicators)
        {
            if (indicator is IImplementationElevDataDepConstr<TTemplate, TMap, TGraph, TConfiguration, TVertexAttributes , TEdgeAttributes> constructor)
            {
                return constructor.ConstructMapRepre(template, map, elevData, configuration, progress, cancellationToken);
            }
        }
        throw new ArgumentException(
            "There is no constructor for given template and map which requires elevation data.");
    }
}