using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.Models.UserModelMan.UserModelTypes;

namespace Optepafi.Models.UserModelMan;

/// <summary>
/// Singleton class used for managing user models and provides all usable user model types. It is main channel between operations on user models and applications logic (ModelViews/ViewModels).
/// It implements supporting methods for work with user models. User models should be worked with preferably by use of methods implemented by this class.
/// All operations provided by this class are thread safe as long as same arguments are not used concurrently multiple times.
/// </summary>
public class UserModelManager : 
    ITemplateGenericVisitor<HashSet<IUserModelType<ITemplate, IUserModel>>>
{
    public static UserModelManager Instance { get; } = new();
    private UserModelManager() { }

    /// <summary>
    ///  Set of usable user model types. Every instance in this set should be of type IUserModelRepresentative.
    /// </summary>
    public IReadOnlySet<IUserModelType<ITemplate, IUserModel>> UserModelTypes = ImmutableHashSet.Create<IUserModelType<ITemplate, IUserModel>>(/*TODO: doplnit uzivatelskymi modelmi*/); //TODO: este rozmysliet ako reprezentovat, mozno skor nejakym listom

    /// <summary>
    /// Returns corresponding user model types to provided template by using generic visitor pattern on it.
    /// It runs trough <c>UserModelTypes</c> set and looks for modelTypes that are of type <c>IUserModelIdentifier{TTemplate, IUserModel{TTemplate}}</c>, where TTemplate is type of visited template.
    /// </summary>
    /// <param name="template">Template to which corresponding user model types should be returned.</param>
    /// <returns>Corresponding user model types to inserted template.</returns>
    public HashSet<IUserModelType<ITemplate, IUserModel>> GetCorrespondingUserModelTypesTo(ITemplate template)
    {
        return template.AcceptGeneric(this);
    }
    HashSet<IUserModelType<ITemplate, IUserModel>> ITemplateGenericVisitor<HashSet<IUserModelType<ITemplate, IUserModel>>>.
        GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes>(TTemplate template)
    {

        HashSet<IUserModelType<ITemplate, IUserModel>> correspondingUserModelTypes = new();
        foreach (var userModelType in UserModelTypes)
        {
            if (userModelType is IUserModelIdentifier<TTemplate, IUserModel<TTemplate>>)
                correspondingUserModelTypes.Add(userModelType);
        }
        return correspondingUserModelTypes;
    }

    /// <summary>
    /// Returns user model type, whose name suffix and file extension matches with the provided file`s name. 
    /// </summary>
    /// <param name="userModelFileName">Name of file for which corresponding user model type should be returned.</param>
    /// <returns>Corresponding user model type to provided file name. If there is no matching user model type, it returns null.</returns>
    public IUserModelType<ITemplate, IUserModel>? GetCorrespondingUserModelTypeTo(string userModelFileName)
    {
        foreach (var userModelType in UserModelTypes)
        {
            if (Regex.IsMatch(userModelFileName, ".*" +userModelType.UserModelFileNameSuffix + userModelType.UserModelFileExtension + "$") ) return userModelType;
        }
        return null;
    }


    /// <summary>
    /// Serializes user model to string.
    /// </summary>
    /// <param name="userModel">User model to be serialized.</param>
    /// <returns>String of serialization.</returns>
    public string SerializeUserModel(IUserModel userModel)
    {
        return userModel.Serialize();
    }

    /// <summary>
    /// Serialize user model to provided stream.
    /// </summary>
    /// <param name="stream">Stream, to which user model should be serialized.</param>
    /// <param name="userModel">User model to be serialized.</param>
    public void SerializeUserModelTo(Stream stream, IUserModel userModel)
    {
        userModel.SerializeTo(stream);
    }
    
    /// <summary>
    /// Returns new instance of user model of provided type.
    /// </summary>
    /// <param name="userModelType">User model type of which new user model is requested.</param>
    /// <returns>New instance of user model.</returns>
    public IUserModel GetNewUserModel(IUserModelType<ITemplate, IUserModel> userModelType)
    {
        return userModelType.GetNewUserModel();
    }

    public enum UserModelLoadResult{Ok, UnableToReadFromFile, UnableToDeserialize, Canceled}
    
    /// <summary>
    /// Tries to deserialize user model from provided stream.
    /// User model type instance dictates, what user model should be result of this deserialization.
    /// </summary>
    /// <param name="userModelStreamWithPath">Provided stream from which user model should be deserialized alongside with path to the serialization file.</param>
    /// <param name="userModelType">User model type defining what user model should be deserialized.</param>
    /// <param name="cancellationToken">Token for cancellation of deserialization.</param>
    /// <param name="userModel">Out parameter for resulting deserialized user model.</param>
    /// <returns>Result of deserialization.</returns>
    public UserModelLoadResult TryDeserializeUserModelOfTypeFrom((Stream,string) userModelStreamWithPath, IUserModelType<ITemplate, IUserModel> userModelType, CancellationToken? cancellationToken, out IUserModel? userModel)
    {
        userModel = userModelType.DeserializeUserModel(userModelStreamWithPath, cancellationToken, out UserModelLoadResult result);
        if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested)
            return UserModelLoadResult.Canceled;
        return result;
    }
}



















    // public UserModelLoadResult LoadUserModelFrom(string pathToSerialization, out IUserModel<ITemplate>? userModel)
    // {
        // foreach (var userModelType in UserModelTypes)
        // {
            // if (Path.GetExtension(pathToSerialization) == userModelType.UserModelFileExtension) // tuna mozna chyba, lebo to berie iba extensions za jendou botkou, pricom extensions modelov su .UserModel.FileType
            // {
                // try
                // {
                    // using (StreamReader serialization = new StreamReader(pathToSerialization))
                    // {
                        // userModel = userModelType.DeserializeUserModel(serialization);
                        // if (userModel is null) return UserModelLoadResult.UnableToDeserialize;
                        // return UserModelLoadResult.Ok;
                    // }
                // }
                // catch (System.IO.IOException ex)
                // {
                    // userModel = null;
                    // return UserModelLoadResult.UnableToOpen;
                // }
            // }
        // }
        // userModel = null;
        // return UserModelLoadResult.UnknownFileFormat;
    // }
