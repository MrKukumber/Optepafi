using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.Templates;
using Optepafi.Models.UserModelMan.UserModels.Functionalities;

namespace Optepafi.Models.UserModelMan.UserModels.Specific;

/// <summary>
/// Blank user model tied to blank template.
/// 
/// It does not bear any information besides one <c>VoidAdjust</c> adjustable property.  
/// This type is just demonstrative template for presenting application functionality.  
/// For more information on templates see <see cref="ITemplate{TVertexAttributes,TEdgeAttributes}"/>.  
/// </summary>
public class BlankUserModel : 
    IWeightComputing<BlankTemplate, BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes>, 
    ISettable
{
    
    /// <inheritdoc cref="IUserModel{TTemplate}.FilePath"/>
    [JsonIgnore]
    public string FilePath { get; set; } = "";
    
    /// <inheritdoc cref="IUserModel{TTemplate}.AssociatedTemplate"/>
    [JsonIgnore]
    public BlankTemplate AssociatedTemplate { get; } = BlankTemplate.Instance;

    /// <inheritdoc cref="IUserModel{TTemplate}.AssociatedTemplate"/>
    BlankTemplate IUserModel<BlankTemplate>.AssociatedTemplate => AssociatedTemplate;

    /// <inheritdoc cref="IUserModel{TTemplate}.Serialize"/>
    public string Serialize()
    {
        return JsonSerializer.Serialize(this);
    }

    /// <inheritdoc cref="IUserModel{TTemplate}.SerializeTo"/>
    public void SerializeTo(Stream stream)
    {
        JsonSerializer.Serialize(stream, this);
    }

    /// <inheritdoc cref="IWeightComputing{TTemplate,TVertexAttributes,TEdgeAttributes}.ComputeWeight"/>
    public float ComputeWeight(BlankTemplate.VertexAttributes from, BlankTemplate.EdgeAttributes through, BlankTemplate.VertexAttributes to)
    {
        return VoidAdjust.Value * 13;
    }

    /// <inheritdoc cref="ISettable.GetAdjustables"/>
    public IReadOnlySet<IUserModelAdjustable> GetAdjustables()
    {
        return new HashSet<IUserModelAdjustable> {VoidAdjust};
    }

    /// <inheritdoc cref="IUserModel{TTemplate}.AcceptGeneric{TOut,TOtherParams}"/>
    public TOut AcceptGeneric<TOut, TOtherParams>(IUserModelGenericVisitor<TOut, TOtherParams> genericVisitor, TOtherParams otherParams)
    {
        return genericVisitor.GenericVisit<BlankUserModel, BlankTemplate>(this, otherParams);
    }

    /// <inheritdoc cref="IUserModel{TTemplate}.AcceptGeneric{TOut}"/>
    public TOut AcceptGeneric<TOut>(IUserModelGenericVisitor<TOut> genericVisitor)
    {
        return genericVisitor.GenericVisit<BlankUserModel, BlankTemplate>(this);
    }

    /// <inheritdoc cref="IUserModel{TTemplate}.AcceptGeneric"/>
    public void AcceptGeneric(IUserModelGenericVisitor genericVisitor)
    {
        genericVisitor.GenericVisit<BlankUserModel, BlankTemplate>(this);
    }

    /// <summary>
    /// Void adjustable that represents void inside this template.
    /// </summary>
    [JsonInclude] 
    public VoidAdjustable VoidAdjust { get; private set; } = new();
    
    /// <summary>
    /// Void adjustable. It represents void inside blank user model.
    /// 
    /// This type is just demonstrative adjustable for presenting application functionality.
    /// </summary>
    public class VoidAdjustable : IValueAdjustable<int>
    {
        /// <inheritdoc cref="IUserModelAdjustable.Name"/>
        [JsonIgnore]
        public string Name { get; } = "Void"; //TODO: localize
        
        /// <inheritdoc cref="IUserModelAdjustable.Caption"/>
        [JsonIgnore]
        public string Caption { get; } = "Void"; //TODO: localize
        
        /// <inheritdoc cref="IUserModelAdjustable.ValueUnit"/>
        [JsonIgnore]
        public string? Unit { get; } = "light years"; //TODO: localize
        
        /// <summary>
        /// Value of void adjustable.
        /// </summary>
        public int Value { get; set; } = 42;
    }
}