using System.IO;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.UserModelMan.UserModels;

/// <summary>
/// Represents user models tied to specific template. User models are used mainly as weight computing instances for path finding algorithms.
/// With map representations they are base data sources in this process.
/// They are meant to allow user to create his own specific model that reflects his preferences for computing of ideal path.
/// Each user model that wants to be used in path finding should implement <see cref="IComputingUserModel{TVertexAttributes,TEdgeAttributes}"/> too.
/// If user model is meant to be modifiable by user, it should implement <see cref="ISettableUserModel"/> interface.
/// 
/// Its predecessor <see cref="IUserModel"/> should not be directly implemented. Its only implementation should be this interface.
/// Each user model type should have its own user model representative, sou it could pe presented as viable option for use.
/// 
/// Instances of user models are meant to be serializable so they could be used between different runs of programs.
/// The name suffix and file extension of files, to which are these models serialized are kept by user model representatives.
/// Each user model should correctly implement its own serialization. Deserialization is then provided by user model representative. Deserialization and serialization must be corresponding.
/// </summary>
/// <typeparam name="TTemplate"></typeparam>
public interface IUserModel<in TTemplate> : IUserModel where TTemplate : ITemplate
{
}

/// <summary>
/// Predecessor of <see cref="IUserModel{TTemplate}"/> type used for more convenient transport of user models.
/// This interface should not be directly implemented.
/// For more information see <see cref="ITemplate{TVertexAttributes,TEdgeAttributes}"/>.
/// </summary>
public interface IUserModel
{
    /// <summary>
    /// Path to the serialization source file of this user model.
    /// </summary>
    public string FilePath { get; }
    public string Serialize();
    public void SerializeTo(Stream stream);
}
