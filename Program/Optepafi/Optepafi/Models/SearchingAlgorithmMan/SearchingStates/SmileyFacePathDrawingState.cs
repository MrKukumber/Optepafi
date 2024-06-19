using System.Collections.Generic;
using System.Runtime.InteropServices;
using Optepafi.Models.Graphics.Objects.Path;
using Optepafi.Models.MapMan;
using Optepafi.Models.ReportMan.Aggregators.Path;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.SearchingAlgorithmMan.SearchingStates;

public class SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes> : ISearchingState<TVertexAttributes, TEdgeAttributes> 
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    public SmileyFacePathDrawingState(SmileyFaceObject firstDrawnObject, Leg associatedLeg, int associatedLegsOrder) 
    {
        DrawnObjects = new()
        {
            [associatedLeg] = [firstDrawnObject],
        };
        LastAddedObject = (firstDrawnObject, associatedLegsOrder);
    }

    public SmileyFacePathDrawingState( SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes> smileyFacePathDrawingState)
    {
        DrawnObjects = smileyFacePathDrawingState.DrawnObjects;
        LastAddedObject = smileyFacePathDrawingState.LastAddedObject;
    }
    public enum SmileyFaceObject {LeftEye, RightEye, Nose, Mouth}
    
    public SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes> Add(SmileyFaceObject drawnObject,Leg associatedLeg, int associatedLegsOrder)
    {
        if (DrawnObjects.ContainsKey(associatedLeg))
            DrawnObjects[associatedLeg].Add(drawnObject);
        else DrawnObjects[associatedLeg] = [drawnObject];
        LastAddedObject = (drawnObject, associatedLegsOrder);
        return this;
    }

    public SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes> CreateNewByAdding(SmileyFaceObject drawnObject, Leg associatedLeg, int associatedLegsOrder)
    {
        return new SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes>(this).Add(drawnObject, associatedLeg, associatedLegsOrder);
    }
    
    public (SmileyFaceObject drawnObject, int associatedLegsOrder) LastAddedObject { get; private set; }
    public Dictionary<Leg, HashSet<SmileyFaceObject>> DrawnObjects { get; }
}