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
/// Singleton class used for managing user models and provides all usable user model types.
/// It implements supporting methods for work with user models. User models should be worked with preferably by use of methods implemented by this class.
/// All operations provided by this class should be thread safe as long as same arguments are not provided concurrently multiple times.
/// 
/// </summary>
public class UserModelManager : 
    ITemplateGenericVisitor<HashSet<IUserModelType<ITemplate, IUserModel<ITemplate>>>>
{
    public static UserModelManager Instance { get; } = new();
    private UserModelManager() { }

    public IReadOnlySet<IUserModelType<ITemplate, IUserModel<ITemplate>>> UserModelTypes = ImmutableHashSet.Create<IUserModelType<ITemplate, IUserModel<ITemplate>>>(/*TODO: doplnit uzivatelskymi modelmi*/); //TODO: este rozmysliet ako reprezentovat, mozno skor nejakym listom

    /// <summary>
    /// Returns corresponding user model types to provided template by using generic visitor pattern on it.
    /// </summary>
    /// <param name="template"></param>
    /// <returns></returns>
    public HashSet<IUserModelType<ITemplate, IUserModel<ITemplate>>> GetCorrespondingUserModelTypesTo(ITemplate template)
    {
        return template.AcceptGeneric(this);
    }

    HashSet<IUserModelType<ITemplate, IUserModel<ITemplate>>> ITemplateGenericVisitor<HashSet<IUserModelType<ITemplate, IUserModel<ITemplate>>>>.
        GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes>(TTemplate template)
    {

        HashSet<IUserModelType<ITemplate, IUserModel<ITemplate>>> correspondingUserModelTypes = new();
        foreach (var userModelType in UserModelTypes)
        {
            if (userModelType is IUserModelType<TTemplate, IUserModel<TTemplate>>)
                correspondingUserModelTypes.Add(userModelType);
        }
        return correspondingUserModelTypes;
    }

    public IUserModelType<ITemplate, IUserModel<ITemplate>>? GetCorrespondingUserModelTypeTo(string userModelFileName)
    {
        foreach (var userModelType in UserModelTypes)
        {
            if (Regex.IsMatch(userModelFileName, ".*" +userModelType.UserModelFileNameSuffix + userModelType.UserModelFileExtension + "$") ) return userModelType;
        }
        return null;
    }

    public string SerializeUserModel(IUserModel<ITemplate> userModel)
    {
        return userModel.Serialize();
    }

    public void SerializeUserModelTo(Stream stream, IUserModel<ITemplate> userModel)
    {
        userModel.SerializeTo(stream);
    }
    public IUserModel<ITemplate> GetNewUserModel(IUserModelType<ITemplate, IUserModel<ITemplate>> userModelType)
    {
        return userModelType.GetNewUserModel();
    }

    public enum UserModelLoadResult{Ok, UnableToReadFromFile, UnableToDeserialize, Canceled}
    public UserModelLoadResult DeserializeUserModelOfTypeFrom((Stream,string) userModelStreamWithPath, IUserModelType<ITemplate, IUserModel<ITemplate>> userModelType, CancellationToken? cancellationToken, out IUserModel<ITemplate>? userModel)
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
