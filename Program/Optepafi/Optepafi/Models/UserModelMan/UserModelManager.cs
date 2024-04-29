using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.Models.UserModelMan.UserModelTypes;

namespace Optepafi.Models.UserModelMan;

public class UserModelManager
{
    public static UserModelManager Instance { get; } = new();
    private UserModelManager() { }

    public IReadOnlySet<IUserModelType<ITemplate, IUserModel<ITemplate>>> UserModelTypes = null;

    public ISet<IUserModelType<ITemplate, IUserModel<ITemplate>>> GetCorrespondingUserModelTypesTo(ITemplate template)
    {
        
    }

    public IUserModelType<ITemplate, IUserModel<ITemplate>>? GetCorrespondingUserModelTypeTo(string extension)
    {
        
    }

    public IUserModelType<ITemplate, IUserModel<ITemplate>> GetUserModelTypeOf(IUserModel<ITemplate> userModel)
    {
        
    }
    public string SerializeUserModel(IUserModel<ITemplate> userModel)
    {
        
    }
    public IUserModel<ITemplate> GetNewUserModel(IUserModelType<ITemplate, IUserModel<ITemplate>> userModelType)
    {
        return userModelType.GetNewUserModel();
    }

    public enum UserModelLoadResult{Ok, UnableToOpen, UnableToDeserialize, UnknownFileFormat}
    public UserModelLoadResult LoadUserModelOfFrom(StreamReader serialization, IUserModelType<ITemplate, IUserModel<ITemplate>> userModelType, out IUserModel<ITemplate>? userModel)
    {
        userModel = userModelType.LoadUserModelFrom(serialization);
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
                        userModel = userModelType.LoadUserModelFrom(serialization);
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