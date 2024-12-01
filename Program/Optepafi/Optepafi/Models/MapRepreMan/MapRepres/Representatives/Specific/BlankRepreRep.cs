using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.Graphs.Representatives;
using Optepafi.Models.MapRepreMan.Graphs.Representatives.Specific;
using Optepafi.Models.MapRepreMan.Implementations.Representatives;
using Optepafi.Models.MapRepreMan.Implementations.Representatives.Specific.Blank;
using Optepafi.Models.MapRepreMan.MapRepres.Specific;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.TemplateAttributes;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Configurations;

namespace Optepafi.Models.MapRepreMan.MapRepres.Representatives.Specific;

/// <summary>
/// Represents blank representation of map.
/// 
/// It is included in <c>MapRepreManager</c> where indicates representations usability.  
/// For more information about map representations representatives see <see cref="IMapRepreRepresentative{TMapRepre}"/>.  
/// </summary>
public class BlankRepreRep : MapRepreRepresentative<IBlankRepre, NullConfiguration>
{
    public static BlankRepreRep Instance { get; } = new();
    private BlankRepreRep(){}
    
    ///<inheritdoc cref="IMapRepreRepresentative{TMapRepre}.MapRepreName"/>
    public override string MapRepreName { get; } = "Blank representation.";

    ///<inheritdoc cref="IMapRepreRepresentative{TMapRepre}.GetCorrespondingGraphCreator"/>
    public override IGraphCreator<IBlankRepre> GetCorrespondingGraphCreator() => BlankGraphRep.Instance;
    protected override NullConfiguration DefaultConfiguration { get; } = new(); 
}