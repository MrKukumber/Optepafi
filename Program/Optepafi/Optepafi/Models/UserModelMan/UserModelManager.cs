using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.Models.UserModelMan.UserModelTypes;

namespace Optepafi.Models.UserModelMan;

public class UserModelManager : 
    ITemplateGenericVisitor<HashSet<IUserModelType<ITemplate, IUserModel<ITemplate>>>>
{
    public static UserModelManager Instance { get; } = new();
    private UserModelManager() { }

    public IReadOnlySet<IUserModelType<ITemplate, IUserModel<ITemplate>>> UserModelTypes = ImmutableHashSet.Create<IUserModelType<ITemplate, IUserModel<ITemplate>>>(/*TODO: doplnit uzivatelskymi modelmi*/); //TODO: este rozmysliet ako reprezentovat, mozno skor nejakym listom

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

    public IUserModelType<ITemplate, IUserModel<ITemplate>>? GetCorrespondingUserModelTypeTo(string extension)
    {
        foreach (var userModelType in UserModelTypes)
        {
            if (userModelType.UserModelFileExtension == extension) return userModelType;
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

    public enum UserModelLoadResult{Ok, UnableToOpen, UnableToDeserialize, UnknownFileFormat}
    public UserModelLoadResult DeserializeUserModelFromOf(StreamReader serialization, IUserModelType<ITemplate, IUserModel<ITemplate>> userModelType, out IUserModel<ITemplate>? userModel)
    {
        userModel = userModelType.DeserializeUserModel(serialization);
        if (userModel is null) return UserModelLoadResult.UnableToDeserialize;
        return UserModelLoadResult.Ok;
    }

    public UserModelLoadResult LoadUserModelFrom(string pathToSerialization, out IUserModel<ITemplate>? userModel)
    {
        foreach (var userModelType in UserModelTypes)
        {
            if (Path.GetExtension(pathToSerialization) == userModelType.UserModelFileExtension)
            {
                try
                {
                    using (StreamReader serialization = new StreamReader(pathToSerialization))
                    {
                        userModel = userModelType.DeserializeUserModel(serialization);
                        if (userModel is null) return UserModelLoadResult.UnableToDeserialize;
                        return UserModelLoadResult.Ok;
                    }
                }
                catch (System.IO.IOException ex)
                {
                    userModel = null;
                    return UserModelLoadResult.UnableToOpen;
                }
            }
        }
        userModel = null;
        return UserModelLoadResult.UnknownFileFormat;
    }
}