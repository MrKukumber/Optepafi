using System.Collections.Generic;
using System.Linq;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.SearchingAlgorithmMan.SearchingStates.Specific;

/// <summary>
/// Represents drawing state of <c>SmileyFaceDrawer</c> algorithm.
/// 
/// It indicates which facial objects where most recently drawn by it.  
/// This type (just like associated algorithm) is just demonstrative searching state for presenting application functionality.  
/// For more information on searching states see <see cref="ISearchingState{TVetexAttributes,TEdgeAttributes}"/>.  
/// </summary>
/// <typeparam name="TVertexAttributes">Type of vertex attributes, which can be included for extraction of information in later aggregations.</typeparam>
/// <typeparam name="TEdgeAttributes">Type of edge attributes, which can be included for extraction of information in later aggregations.</typeparam>
public class SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes> : ISearchingState<TVertexAttributes, TEdgeAttributes> 
    where TVertexAttributes : IVertexAttributes where TEdgeAttributes : IEdgeAttributes
{
    /// <summary>
    /// Creates smiley face path drawing state from provided first drawn object and associated leg to this object + its order.
    /// </summary>
    /// <param name="firstDrawnObject">First drawn object.</param>
    /// <param name="associatedLeg">Associated leg to first drawn object.</param>
    /// <param name="legsOrder">Associated legs order.</param>
    public SmileyFacePathDrawingState(SmileyFaceObject firstDrawnObject, Leg associatedLeg, int legsOrder) 
    {
        DrawnObjects = new()
        {
            [associatedLeg] = [firstDrawnObject],
        };
        LastAddedObject = (firstDrawnObject, legsOrder);
    }
    
    /// <summary>
    /// Creates new instance of <c>SmileyFacePathDrawingState</c> as deep copy of provided state.
    /// </summary>
    /// <param name="smileyFacePathDrawingState"></param>
    public SmileyFacePathDrawingState( SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes> smileyFacePathDrawingState)
    {
        DrawnObjects = smileyFacePathDrawingState.DrawnObjects.Select(drawnObject => drawnObject).ToDictionary();
        LastAddedObject = smileyFacePathDrawingState.LastAddedObject;
    }
    
    /// <summary>
    /// Indicators of every smiley face part.
    /// </summary>
    public enum SmileyFaceObject {LeftEye, RightEye, Nose, Mouth}
    
    /// <summary>
    /// Contains facial object added in last drawing states update.
    /// </summary>
    public (SmileyFaceObject drawnObject, int associatedLegsOrder) LastAddedObject { get; private set; }
    
    /// <summary>
    /// Contains collection of all drawn objects added to this state till now.
    /// </summary>
    public Dictionary<Leg, HashSet<SmileyFaceObject>> DrawnObjects { get; }
    
    /// <summary>
    /// Method for adding newly drawn object to current state and updating it by doing so.
    /// 
    /// This method supports "fluent syntax".
    /// </summary>
    /// <param name="drawnObject">Newly drawn facial object that is added to the drawing state.</param>
    /// <param name="associatedLeg">Associated leg to newly added facial object.</param>
    /// <param name="legsOrder">Associated legs order.</param>
    /// <returns>"This" updated by newly added facial object.</returns>
    public SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes> Add(SmileyFaceObject drawnObject,Leg associatedLeg, int legsOrder)
    {
        if (DrawnObjects.ContainsKey(associatedLeg))
            DrawnObjects[associatedLeg].Add(drawnObject);
        else DrawnObjects[associatedLeg] = [drawnObject];
        LastAddedObject = (drawnObject, legsOrder);
        return this;
    }

    /// <summary>
    /// Method for creating new instance by adding newly drawn object to current instance.
    /// </summary>
    /// <param name="drawnObject">Newly drawn facial object that is added to the newly created drawing state.</param>
    /// <param name="associatedLeg">Associated leg to newly added facial object.</param>
    /// <param name="legsOrder">Associated legs order.</param>
    /// <returns>Newly created drawing state instance.</returns>
    public SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes> CreateNewByAdding(SmileyFaceObject drawnObject, Leg associatedLeg, int legsOrder)
    {
        return new SmileyFacePathDrawingState<TVertexAttributes, TEdgeAttributes>(this).Add(drawnObject, associatedLeg, legsOrder);
    }
    
}