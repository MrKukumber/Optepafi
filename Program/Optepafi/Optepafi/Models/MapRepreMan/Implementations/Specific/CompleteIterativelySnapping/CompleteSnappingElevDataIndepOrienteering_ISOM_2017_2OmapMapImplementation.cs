using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.TemplateMan.Templates;

namespace Optepafi.Models.MapRepreMan.Implementations.Specific.CompleteIterativelySnapping;

//TODO: comment
public class CompleteSnappingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation : 
    ICompleteSnappingGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>
{
    public string Name { get; } = "Complete snapping map repre. elev. data independent implementation for Orienteering (ISOM 2017-2) and Omap file format.";
    public TOut AcceptGeneric<TOut, TOtherParams>(IMapRepreGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams) => genericVisitor.GenericVisit<CompleteSnappingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation, Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>(this, otherParams);
    // public void RestoreConsistency()
    // {
        // throw new System.NotImplementedException();
    // }
}