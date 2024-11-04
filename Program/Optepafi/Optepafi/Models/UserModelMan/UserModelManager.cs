using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan.UserModelReps;
using Optepafi.Models.UserModelMan.UserModelReps.SpecificUserModelReps;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Configurations;

namespace Optepafi.Models.UserModelMan;

/// <summary>
/// Singleton class used for managing user models and provides all usable user model types.
///
/// It is main channel between operations on user models and applications logic (ModelViews/ViewModels).  
/// It implements supporting methods for work with user models. User models should be worked with preferably by use of methods implemented by this class.  
/// All operations provided by this class are thread safe as long as same arguments are not used concurrently multiple times.  
/// </summary>
public class UserModelManager : 
    ITemplateGenericVisitor<HashSet<IUserModelType<IUserModel<ITemplate>, ITemplate>>>,
    IUserModelGenericVisitor<bool, IConfiguration>
{
    public static UserModelManager Instance { get; } = new();
    private UserModelManager() { }

    /// <summary>
    ///  Set of usable user model types. Every instance in this set should be of type IUserModelRepresentative.
    /// </summary>
    public IReadOnlySet<IUserModelType<IUserModel<ITemplate>, ITemplate>> UserModelTypes = ImmutableHashSet.Create<IUserModelType<IUserModel<ITemplate>, ITemplate>>(BlankUserModelRepresentative.Instance); //TODO: este rozmysliet ako reprezentovat, mozno skor nejakym listom

    /// <summary>
    /// Returns corresponding user model types to provided template by using generic visitor pattern on it.
    /// 
    /// It runs trough <c>UserModelTypes</c> set and looks for modelTypes that are of type <c>IUserModelTemplateBond{TTemplate}</c>, where TTemplate is type of visited template.  
    /// It uses generic visitor pattern on template in order to gain its real type in form of generic parameter.  
    /// </summary>
    /// <param name="template">Template to which corresponding user model types should be returned.</param>
    /// <returns>Corresponding user model types to inserted template.</returns>
    public HashSet<IUserModelType<IUserModel<ITemplate>, ITemplate>> GetCorrespondingUserModelTypesTo(ITemplate template)
    {
        return template.AcceptGeneric(this);
    }
    HashSet<IUserModelType<IUserModel<ITemplate>,ITemplate>> ITemplateGenericVisitor<HashSet<IUserModelType<IUserModel<ITemplate>,ITemplate>>>.
        GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes>(TTemplate template)
    {

        HashSet<IUserModelType<IUserModel<ITemplate>,ITemplate>> correspondingUserModelTypes = new();
        foreach (var userModelType in UserModelTypes)
        {
            if (userModelType is IUserModelTemplateBond<TTemplate>)
                correspondingUserModelTypes.Add(userModelType);
        }
        return correspondingUserModelTypes;
    }

    /// <summary>
    /// Returns user model type, whose name suffix and file extension matches with the provided file`s name.
    /// 
    /// It uses generic visitor pattern on template in order to gain its real type in form of generic parameter.  
    /// </summary>
    /// <param name="userModelFileName">Name of file for which corresponding user model type should be returned.</param>
    /// <returns>Corresponding user model type to provided file name. If there is no matching user model type, it returns null.</returns>
    public IUserModelType<IUserModel<ITemplate>,ITemplate>? GetCorrespondingUserModelTypeTo(string userModelFileName)
    {
        foreach (var userModelType in UserModelTypes)
        {
            if (Regex.IsMatch(userModelFileName, ".*\\." +userModelType.UserModelFileNameSuffix + "\\." +  userModelType.UserModelFileExtension + "$") ) return userModelType;
        }
        return null;
    }


    /// <summary>
    /// Serializes user model to string.
    /// </summary>
    /// <param name="userModel">User model to be serialized.</param>
    /// <returns>String of serialization.</returns>
    public string SerializeUserModel(IUserModel<ITemplate> userModel)
    {
        return userModel.Serialize();
    }

    /// <summary>
    /// Serialize user model to provided stream.
    /// </summary>
    /// <param name="stream">Stream, to which user model should be serialized.</param>
    /// <param name="userModel">User model to be serialized.</param>
    public void SerializeUserModelTo(Stream stream, IUserModel<ITemplate> userModel)
    {
        userModel.SerializeTo(stream);
    }
    
    /// <summary>
    /// Returns new instance of user model of provided type.
    /// </summary>
    /// <param name="userModelType">User model type of which new user model is requested.</param>
    /// <param name="configuration">Configuration used in newly created user model.</param>
    /// <returns>New instance of user model.</returns>
    public IUserModel<ITemplate> GetNewUserModel(IUserModelType<IUserModel<ITemplate>, ITemplate> userModelType, IConfiguration? configuration)
    {
        return userModelType.GetNewUserModel(configuration);
    }

    public enum UserModelLoadResult{Ok, UnableToReadFromFile, UnableToDeserialize, Canceled}
    
    /// <summary>
    /// Tries to deserialize user model from provided stream.
    /// 
    /// User model type instance dictates, what user model should be result of this deserialization.
    /// </summary>
    /// <param name="userModelStreamWithPath">Provided stream from which user model should be deserialized alongside with path to the serialization file.</param>
    /// <param name="userModelType">User model type defining what user model should be deserialized.</param>
    /// <param name="configuration">Configuration which should be used in deserialized user model.</param>
    /// <param name="cancellationToken">Token for cancellation of deserialization.</param>
    /// <param name="userModel">Out parameter for resulting deserialized user model.</param>
    /// <returns>Result of deserialization.</returns>
    public UserModelLoadResult TryDeserializeUserModelOfTypeFrom((Stream,string) userModelStreamWithPath, IUserModelType<IUserModel<ITemplate>, ITemplate> userModelType, IConfiguration? configuration,CancellationToken? cancellationToken, out IUserModel<ITemplate>? userModel)
    {
        userModel = userModelType.DeserializeUserModel(userModelStreamWithPath, configuration, cancellationToken, out UserModelLoadResult result);
        if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested)
            return UserModelLoadResult.Canceled;
        return result;
    }

    /// <summary>
    /// Method for changing configuration of existing user model instance.
    /// 
    /// This method uses generic visitor pattern on provided user model so that correct user model representative could be chosen to correctly change user model configuration.
    /// </summary>
    /// <param name="userModel">User model which configuration should be changed.</param>
    /// <param name="newConfiguration">New configuration for the user model.</param>
    /// <returns></returns>
    public bool ChangeUserModelConfiguration(IUserModel<ITemplate> userModel, IConfiguration newConfiguration)
    {
        return userModel.AcceptGeneric(this, newConfiguration);
    }

    bool IUserModelGenericVisitor<bool, IConfiguration>.GenericVisit<TUserModel, TTemplate>(TUserModel userModel, IConfiguration newConfiguration)
    {
        foreach (var userModelType in UserModelTypes)
        {
            if (userModelType is IUserModelIdentifier<TUserModel> userModelIdentifier)
            {
                userModelIdentifier.ChangeConfiguration(userModel, newConfiguration);
                return true;
            }
        }
        return false;
    }
}





















    // ITemplateGenericVisitor<bool, IUserModelType<IUserModel<ITemplate>, ITemplate>>

    /// <summary>
    /// Method for testing whether provided user model type represents computing user model tied to specified template.
    /// </summary>
    /// <param name="userModelType">Represents tested user model type.</param>
    /// <returns>True, if provided user model type represents computing user model. False otherwise.</returns>
    // public bool DoesRepresentComputingModel(IUserModelType<IUserModel<ITemplate>, ITemplate> userModelType)
    // {
        // return userModelType.AssociatedTemplate.AcceptGeneric(this, userModelType);
    // }
    // bool ITemplateGenericVisitor<bool, IUserModelType<IUserModel<ITemplate>, ITemplate>>
        // .GenericVisit<TTemplate, TVertexAttributes, TEdgeAttributes>(TTemplate template, IUserModelType<IUserModel<ITemplate>, ITemplate> userModelType)
    // {
        // if (userModelType is IUserModelType<IComputingUserModel<TTemplate, TVertexAttributes, TEdgeAttributes>, TTemplate>) 
            // return true;
        // return false;
    // }
