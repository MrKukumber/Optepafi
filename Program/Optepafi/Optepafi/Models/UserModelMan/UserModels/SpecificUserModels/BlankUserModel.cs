using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.TemplateMan.Templates;
using Optepafi.Models.UserModelMan.UserModelAdjustables;

namespace Optepafi.Models.UserModelMan.UserModels.SpecificUserModels;

/// <summary>
/// Blank user model tied to blank template. It does not bear any information besides one <c>VoidAdjust</c> adjustable property.
/// This type is just demonstrative template for presenting application functionality.
/// For more information on templates see <see cref="ITemplate{TVertexAttributes,TEdgeAttributes}"/>.
/// </summary>
public class BlankUserModel : 
    IWeightComputingUserModel<BlankTemplate, BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes>, 
    ISettableUserModel
{
    [JsonIgnore]
    public string FilePath { get; set; } = "";
    
    [JsonIgnore]
    public BlankTemplate AssociatedTemplate { get; } = BlankTemplate.Instance;

    /// <inheritdoc cref="IUserModel{TTemplate}.AssociatedTemplate"/>
    ITemplate IUserModel<ITemplate>.AssociatedTemplate => AssociatedTemplate;

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

    /// <inheritdoc cref="IWeightComputingUserModel{TTemplate,TVertexAttributes,TEdgeAttributes}.ComputeWeight"/>
    public int ComputeWeight(BlankTemplate.VertexAttributes from, BlankTemplate.EdgeAttributes through, BlankTemplate.VertexAttributes to)
    {
        return VoidAdjust.Value * 13;
    }

    /// <inheritdoc cref="ISettableUserModel.GetAdjustables"/>
    public IReadOnlySet<IUserModelAdjustable> GetAdjustables()
    {
        return new HashSet<IUserModelAdjustable> {VoidAdjust};
    }

    [JsonInclude] 
    private VoidAdjustable VoidAdjust { get; set; } = new();
    
    /// <summary>
    /// Void adjustable. Just to have some adjustable in blank user model.
    /// This type is just demonstrative adjustable for presenting application functionality.
    /// </summary>
    public class VoidAdjustable : IValueAdjustable<int>
    {
        public VoidAdjustable(){}
        
        [JsonIgnore]
        public string Name { get; } = "Void"; //TODO: localize
        [JsonIgnore]
        public string Caption { get; } = "Void"; //TODO: localize
        [JsonIgnore]
        public string ValueUnit { get; } = "light years"; //TODO: localize
        public int Value { get; set; } = 42;
    }
}