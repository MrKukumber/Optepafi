using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Optepafi.Models.TemplateMan.Templates;
using Optepafi.Models.UserModelMan.UserModelAdjustables;

namespace Optepafi.Models.UserModelMan.UserModels.SpecificUserModels;

public class BlankUserModel : 
    IUserModel<BlankTemplate>, 
    IComputingUserModel<BlankTemplate.VertexAttributes, BlankTemplate.EdgeAttributes>, 
    ISettableUserModel
{
    [JsonIgnore]
    public string FilePath { get; set; } = "";
    
    public string Serialize()
    {
        return JsonSerializer.Serialize(this);
    }

    public void SerializeTo(Stream stream)
    {
        JsonSerializer.Serialize(stream, this);
    }

    public int ComputeWeight(BlankTemplate.VertexAttributes from, BlankTemplate.EdgeAttributes through, BlankTemplate.VertexAttributes to)
    {
        return VoidAdjust.Value * 13;
    }

    public IReadOnlySet<IUserModelAdjustable> GetAdjustables()
    {
        return new HashSet<IUserModelAdjustable> {VoidAdjust};
    }

    [JsonInclude] 
    private VoidAdjustable VoidAdjust { get; set; } = new();
    
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