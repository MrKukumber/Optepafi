using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.Graphs.Representatives;
using Optepafi.Models.MapRepreMan.Graphs.Representatives.Specific;
using Optepafi.Models.MapRepreMan.Implementations.Representatives;
using Optepafi.Models.MapRepreMan.Implementations.Representatives.Specific;
using Optepafi.Models.MapRepreMan.MapRepres.Specific;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.Utils;

namespace Optepafi.Models.MapRepreMan.MapRepres.Representatives.Specific;

/// <summary>
/// Represents blank representation of map.
/// 
/// It is included in <c>MapRepreManager</c> where indicates representations usability.  
/// For more information about map representations representatives see <see cref="IMapRepreRepresentative{TMapRepre}"/>.  
/// </summary>
public class BlankRepreRep : IMapRepreRepresentative<IBlankRepre>
{
    public static BlankRepreRep Instance { get; } = new();
    private BlankRepreRep(){}
    
    ///<inheritdoc cref="IMapRepreRepresentative{TMapRepre}.MapRepreName"/>
    public string MapRepreName { get; } = "Blank representation.";
    
    ///<inheritdoc cref="IMapRepreRepresentative{TMapRepre}.ImplementationIndicators"/>
    public IImplementationIndicator<ITemplate, IMap, IBlankRepre>[] ImplementationIndicators { get; } = [BlankGraphElevDataDepBlankTemplateTextMapImplementationRep.Instance];
    
    ///<inheritdoc cref="IMapRepreRepresentative{TMapRepre}.GetCorrespondingGraphRepresentative{TVertexAttributes, TEdgeAttributes}"/>
    public IGraphRepresentative<IGraph<TVertexAttributes, TEdgeAttributes>, IConfiguration, TVertexAttributes, TEdgeAttributes> GetCorrespondingGraphRepresentative<TVertexAttributes, TEdgeAttributes>() where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
    {
        return BlankGraphRep<TVertexAttributes, TEdgeAttributes>.Instance;
    }
}