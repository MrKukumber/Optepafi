using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.Configurations;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.Graphs.Representatives;
using Optepafi.Models.MapRepreMan.Graphs.Representatives.Specific;
using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.MapRepreMan.Implementations.Representatives;
using Optepafi.Models.MapRepreMan.Implementations.Representatives.Specific.CompleteIterativelySnapping;
using Optepafi.Models.MapRepreMan.MapRepres.Specific;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan.MapRepres.Representatives.Specific;

//TODO: comment
public class CompleteNetIntertwiningMapRepreRep : MapRepreRepresentative<ICompleteNetIntertwiningMapRepre, CompleteNetIntertwiningMapRepreConfiguration>
{
    public static CompleteNetIntertwiningMapRepreRep Instance { get; } = new();
    private CompleteNetIntertwiningMapRepreRep() { }
    
    ///<inheritdoc cref="IMapRepreRepresentative{TMapRepre}.MapRepreName"/>
    public override string MapRepreName => "Complete, net intertwining map representation";
    
    ///<inheritdoc cref="IMapRepreRepresentative{TMapRepre}.GetCorrespondingGraphCreator{TVertexAttributes,TEdgeAttributes}"/>
    public override IGraphCreator<ICompleteNetIntertwiningMapRepre> GetCorrespondingGraphCreator<TVertexAttributes, TEdgeAttributes>() => CompleteNetIntertwiningGraphRep<TVertexAttributes, TEdgeAttributes>.Instance;
    protected override CompleteNetIntertwiningMapRepreConfiguration DefaultConfiguration { get; } = new (2000, 0.25f, 0);
}