using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.Implementations.Representatives;
using Optepafi.Models.MapRepreMan.MapRepres;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan.MapRepreConstrs;

/// <summary>
/// Represents indicator of map representations implementations properties. It can be used to gain information about prerequisites of map repre. implementations creation.
/// This interface should not be implemented right away. Preferred way is to derive either from <see cref="ElevDataDepImplementationRep{TTemplate,TMap,TUsableSubMap,TGraph,TVertexAttributes,TEdgeAttributes}"/> class or from <see cref="ElevDataIndepImplementationRep{TTemplate,TMap,TUsableSubMap,TGraph,TVertexAttributes,TEdgeAttributes}"/> class.
/// For the covariant nature of its type parameters it is suitable type for storing collections of implementation representatives.
/// </summary>
/// <typeparam name="TTemplate">Type of template used by implementation.</typeparam>
/// <typeparam name="TMap">Type of map used by implementation.</typeparam>
/// <typeparam name="TMapRepre">Type of map representation whose implementation is represented by this indicator.</typeparam>
public interface IImplementationIndicator<out TTemplate, out TMap, out TMapRepre>
    where TTemplate : ITemplate
    where TMap : IMap
    where TMapRepre : IMapRepre
{
    TTemplate UsedTemplate { get; }
    IMapFormat<TMap> UsedMapFormat { get; }
    bool RequiresElevData { get; }
}