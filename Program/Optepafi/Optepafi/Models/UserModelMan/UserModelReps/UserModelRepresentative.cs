using System.IO;
using System.Threading;
using Optepafi.Models.TemplateMan;
using Optepafi.Models.UserModelMan.UserModels;
using Optepafi.Models.Utils;

namespace Optepafi.Models.UserModelMan.UserModelReps;

/// <summary>
/// Abstract class which represents individual user model type that is tied to specific template type.
/// 
/// This class should be derived from by all classes which represents individual user model types.
/// Every type of user model should have its representative, so it could be found as viable user model in <see cref="UserModelManager"/> class.  
/// User model representative takes care of deserializing represented user models. Serialization of user models must match with deserialization of their representatives.  
/// Preferred way to interact with representatives is through <see cref="UserModelManager"/>.  
/// </summary>
/// <typeparam name="TUserModel">Type of represented user model.</typeparam>
/// <typeparam name="TTemplate">Template type which represented user model is tied to.</typeparam>
/// <typeparam name="TConfiguration">Type of configuration used by represented user model.</typeparam>
public abstract class UserModelRepresentative<TUserModel, TTemplate, TConfiguration> :
    IUserModelType<TUserModel, TTemplate>,
    IUserModelTemplateBond<TTemplate>,
    IUserModelIdentifier<TUserModel>
    where TTemplate : ITemplate
    where TUserModel : IUserModel<TTemplate>
    where  TConfiguration : IConfiguration
{
    /// <summary>
    /// Provides default configuration for represented model.
    /// </summary>
    protected abstract TConfiguration DefaultConfiguration { get; }
    /// <inheritdoc cref="IUserModelType{TUserModel,TTemplate}.DefaultConfigurationDeepCopy"/>
    IConfiguration IUserModelType<TUserModel, TTemplate>.DefaultConfigurationDeepCopy => DefaultConfiguration.DeepCopy();
    
    /// <inheritdoc cref="IUserModelType{TUserModel,TTemplate}.UserModelTypeName"/>
    public abstract string UserModelTypeName { get; }
    /// <inheritdoc cref="IUserModelType{TUserModel,TTemplate}.UserModelFileNameSuffix"/>
    public abstract string UserModelFileNameSuffix { get; }
    /// <inheritdoc cref="IUserModelType{TUserModel,TTemplate}.UserModelFileExtension"/>
    public abstract string UserModelFileExtension { get; }
    /// <inheritdoc cref="IUserModelType{TUserModel,TTemplate}.AssociatedTemplate"/>
    public abstract TTemplate AssociatedTemplate { get; }
    
    /// <summary>
    /// Returns new instance of user model represented by this user model type.
    /// </summary>
    /// <param name="configuration">Configuration used in newly created user model.</param>
    /// <returns>New instance of user model.</returns>
    protected abstract TUserModel GetNewUserModel(TConfiguration configuration);
    /// <inheritdoc cref="IUserModelType{TUserModel,TTemplate}.GetNewUserModel"/>
    public TUserModel GetNewUserModel(IConfiguration? configuration)
    {
        if (configuration is TConfiguration config)
            return GetNewUserModel(config); 
        //TODO: log wrong configuration type
        return GetNewUserModel(DefaultConfiguration);
    }
    
    /// <summary>
    /// Tries to deserialize user model from provided stream.
    /// </summary>
    /// <param name="serializationWithPath">Provided stream from which user model should be deserialized alongside with path to the serialization file.</param>
    /// <param name="configuration">Configuration which should be used in deserialized user model.</param>
    /// <param name="cancellationToken">Token for cancellation of deserialization. Provided, when the deserialization is not needed anymore.</param>
    /// <param name="result">Out parameter for result of deserialization.</param>
    /// <returns>Resulting deserialized user model.</returns>
    protected abstract TUserModel? DeserializeUserModel((Stream,string) serializationWithPath, TConfiguration configuration, CancellationToken? cancellationToken, out UserModelManager.UserModelLoadResult result);
    
    /// <inheritdoc cref="IUserModelType{TUserModel,TTemplate}.DeserializeUserModel"/>
    public TUserModel? DeserializeUserModel((Stream, string) serializationWithPath,
        IConfiguration? configuration, CancellationToken? cancellationToken, out UserModelManager.UserModelLoadResult result)
    {
        if (configuration is TConfiguration config)
            return DeserializeUserModel(serializationWithPath, config, cancellationToken, out result);
        //TODO: log wrong configuration type
        return DeserializeUserModel(serializationWithPath, DefaultConfiguration, cancellationToken, out result);
    }

    /// <summary>
    /// Changes configuration of provided user model.
    /// Every user model representative must have its way to change represented user models configuration.
    /// </summary>
    /// <param name="userModel">User model which configuration should be changed.</param>
    /// <param name="configuration">New configuration for user model.</param>
    protected abstract void ChangeConfiguration(TUserModel userModel, TConfiguration configuration);

    /// <inheritdoc cref="IUserModelIdentifier{TUserModel}.ChangeConfiguration"/>
    public void ChangeConfiguration(TUserModel userModel, IConfiguration configuration)
    {
        if (configuration is TConfiguration config)
            ChangeConfiguration(userModel, configuration);
        else
        {
            //TODO: log wrong configuration type
        }
    }
}