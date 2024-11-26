using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapRepreMan.Configurations;
using Optepafi.Models.MapRepreMan.Graphs;
using Optepafi.Models.MapRepreMan.Graphs.Representatives;
using Optepafi.Models.MapRepreMan.Graphs.Representatives.Specific;
using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.MapRepreMan.Implementations.Representatives;
using Optepafi.Models.MapRepreMan.Implementations.Representatives.Specific.CompleteIterativelySnapping;
using Optepafi.Models.MapRepreMan.MapRepres.Specific;
using Optepafi.Models.MapRepreMan.VertecesAndEdges;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.MapRepreMan.MapRepres.Representatives.Specific;

//TODO: comment
public class CompleteSnappingMapRepreRep : MapRepreRepresentative<ICompleteSnappingMapRepre, CompleteSnappingMapRepreConfiguration>
{
    public static CompleteSnappingMapRepreRep Instance { get; } = new();
    private CompleteSnappingMapRepreRep() { }
    
    ///<inheritdoc cref="IMapRepreRepresentative{TMapRepre}.MapRepreName"/>
    public override string MapRepreName { get; } = "Complete, iteratively snapping map representation";
    
    ///<inheritdoc cref="IMapRepreRepresentative{TMapRepre}.GetCorrespondingGraphCreator"/>
    public override IMapRepreCreator<ICompleteSnappingMapRepre> GetCorrespondingGraphCreator() => CompleteSnappingGraphRep.Instance;
    protected override CompleteSnappingMapRepreConfiguration DefaultConfiguration { get; } = new CompleteSnappingMapRepreConfiguration();
}