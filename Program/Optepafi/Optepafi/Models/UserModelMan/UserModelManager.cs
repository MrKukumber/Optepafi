using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Optepafi.Models.TemplateMan;

namespace Optepafi.Models.UserModelMan;

public class UserModelManager
{
    public static UserModelManager Instance { get; } = new();
    private UserModelManager() { }

    public IReadOnlySet<IUserModelType<ITemplate, IUserModel<ITemplate>>> UserModelTypes = null;

    public string[] GetUsableUserModelTypeExtensionsFor(ITemplate template)
    {
        
    }

    public ISet<IUserModelType<ITemplate, IUserModel<ITemplate>>> GetCorrespondingUserModelTypesTo(ITemplate template)
    {
        
    }
    
    public IUserModel<ITemplate> GetNewUserModel(IUserModelType<ITemplate, IUserModel<ITemplate>> userModelType)
    {
        return userModelType.GetNewUserModel();
    }

    public enum UserModelLoadResult{Ok, UnableToOpen, UnableToDeserialize, UnknownFileFormat}
    public UserModelLoadResult LoadUserModelOfFrom(IUserModelType<ITemplate, IUserModel<ITemplate>> userModelType, StreamReader serialization, out IUserModel<ITemplate>? userModel)
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
                string serializationFileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathToSerialization);
                if (Path.GetExtension(serializationFileNameWithoutExtension) ==
                    userModelType.UsedTemplate.TemplateFileExtension)
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
        }

        userModel = null;
        return UserModelLoadResult.UnknownFileFormat;
    }
    
}