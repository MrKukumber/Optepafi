namespace Optepafi.Models.MapRepreMan.MapRepres;

/// <summary>
/// Represents map representation used in path finding algorithms.
/// 
/// Each implementation of this class represents different way to represent map and way of retrieving usable graph for path finding.  
/// It can be said that it represents inner structure of maps representation, how it looks and behaves internally. All implementations of specific map representation should behave in a way it describes.  
/// It is not so much a contract as some representative of an constructive idea.  
/// Each map representation type should have one dedicated derived graph type which represents outer behaviour and usability of map representation.  
/// Each map representation should have its own representative by which it is presented in <see cref="MapRepreManager"/> as viable map representation for use.  
/// </summary>
public interface IMapRepre
{
    public string Name { get; }
    
    // public TOut AcceptGeneric<TOut, TGenericParam, TConstraint, TOtherParams>(
        // IMapRepresentationGenericVisitor<TOut, TConstraint, TOtherParams> genericVisitor,
        // TGenericParam genericParam, TOtherParams otherParams) where TGenericParam : TConstraint;
    // public TOut AcceptGeneric<TOut, TOtherParams>(IMapRepresentationGenericVisitor<TOut, TOtherParams> genericVisitor,
        // TOtherParams otherParams);
    // public TOut AcceptGeneric<TOut>(IMapRepresentationGenericVisitor<TOut> genericVisitor);
    // public void AcceptGeneric(ITemplateGenericVisitor genericVisitor);
}
