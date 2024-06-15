using System.IO;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.UserModelMan.UserModels;

/// <summary>
/// Represents user models tied to specific template. User models are used mainly as representations of users preferences and for computing and aggregating of weights, values and information from vertex/edge attributes.
/// These computational services are mainly used by searching algorithms for computing weights of edges. With map representations they are base data sources in path finding processes.
/// They are meant to allow user to create his own specific model that reflects his preferences for computing of ideal path.
/// Each user model that wants to be used in path finding should implement <see cref="IComputingUserModel{TTemplate,TVertexAttributes,TEdgeAttributes}"/> too.
/// If user model is meant to be modifiable by user, it should implement <see cref="ISettableUserModel"/> interface.
/// 
/// Each user model type should have its own user model representative, so it could pe presented as viable option for use.
/// 
/// Instances of user models are meant to be serializable so they could be used between different runs of programs.
/// The name suffix and file extension of files, to which are these models serialized are kept by user model representatives.
/// Each user model should correctly implement its own serialization. Deserialization is then provided by user model representative. Deserialization and serialization must be corresponding.
/// </summary>
/// <typeparam name="TTemplate">Associated template type.</typeparam>
public interface IUserModel<out TTemplate> where TTemplate : ITemplate
{
    /// <summary>
    /// Template to which is user model tied to.
    /// </summary>
    public TTemplate AssociatedTemplate { get; }
    /// <summary>
    /// Path to the serialization source file of this user model.
    /// </summary>
    public string FilePath { get; }
    public string Serialize();
    public void SerializeTo(Stream stream);
}
