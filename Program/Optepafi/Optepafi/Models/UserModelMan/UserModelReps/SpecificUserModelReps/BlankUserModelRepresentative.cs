using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using Optepafi.Models.TemplateMan.Templates;
using Optepafi.Models.UserModelMan.UserModels.Specific;

namespace Optepafi.Models.UserModelMan.UserModelReps.SpecificUserModelReps;

/// <summary>
/// Representative of blank user model.
/// 
/// For more information on user model representatives see <see cref="IUserModelRepresentative{TUserModel,TTemplate}"/>, <see cref="IUserModelType{TUserModel,TTemplate}"/> or <see cref="IUserModelTemplateBond{TUserModel,TTemplate}"/>.  
/// </summary>
public class BlankUserModelRepresentative : IUserModelRepresentative<BlankUserModel, BlankTemplate>
{
    public static BlankUserModelRepresentative Instance { get; } = new();
    private BlankUserModelRepresentative() { }
    
    /// <inheritdoc cref="IUserModelType{TUserModel,TTemplate}.AssociatedTemplate"/>
    public BlankTemplate AssociatedTemplate  => BlankTemplate.Instance;
    
    
    /// <inheritdoc cref="IUserModelType{TUserModel,TTemplate}.UserModelTypeName"/>
    public string UserModelTypeName  => "Blank user model";
    /// <inheritdoc cref="IUserModelType{TUserModel,TTemplate}.UserModelFileNameSuffix"/>
    public string UserModelFileNameSuffix  => "blankUM";
    /// <inheritdoc cref="IUserModelType{TUserModel,TTemplate}.UserModelFileExtension"/>
    public string UserModelFileExtension  => "json";
    
    /// <inheritdoc cref="IUserModelType{TUserModel,TTemplate}.GetNewUserModel"/>
    public BlankUserModel GetNewUserModel()
    {
        return new BlankUserModel();
    }

    /// <inheritdoc cref="IUserModelType{TUserModel,TTemplate}.DeserializeUserModel"/>
    public BlankUserModel? DeserializeUserModel((Stream, string) serializationWithPath, CancellationToken? cancellationToken,
        out UserModelManager.UserModelLoadResult result)
    {
        if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) { result = UserModelManager.UserModelLoadResult.Canceled; return null; }

        BlankUserModel? deserUserModel = null;
        try { deserUserModel = JsonSerializer.Deserialize<BlankUserModel>(serializationWithPath.Item1); } 
        catch (JsonException) { } catch (NotSupportedException){}

        if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) { result = UserModelManager.UserModelLoadResult.Canceled; return null; }
        
        if (deserUserModel is not null)
        {
            deserUserModel.FilePath = serializationWithPath.Item2;
            result = UserModelManager.UserModelLoadResult.Ok;
        }
        else 
            result = UserModelManager.UserModelLoadResult.UnableToDeserialize;
        return deserUserModel;
    }
}