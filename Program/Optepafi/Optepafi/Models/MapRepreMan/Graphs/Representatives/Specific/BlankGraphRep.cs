using System;
using System.Threading;
using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.MapRepreMan.Implementations.Representatives;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.Utils;

namespace Optepafi.Models.MapRepreMan.Graphs.Representatives.Specific;

/// <summary>
/// Singleton class which represents blank graph.
/// 
/// Its instance is contained in <c>BlankRepreRep</c> so it can be used for creation of represented map representation/graph. 
/// </summary>
/// <typeparam name="TVertexAttributes">Type of vertex attributes used in represented graph.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes used in represented graph.</typeparam>
public class BlankGraphRep<TVertexAttributes, TEdgeAttributes> : GraphRepresentative<IBlankGraph<TVertexAttributes, TEdgeAttributes>, TVertexAttributes, TEdgeAttributes>
    where TVertexAttributes : IVertexAttributes
    where TEdgeAttributes : IEdgeAttributes
{
    public static BlankGraphRep<TVertexAttributes, TEdgeAttributes> Instance { get; } = new();
    private BlankGraphRep() { }
}