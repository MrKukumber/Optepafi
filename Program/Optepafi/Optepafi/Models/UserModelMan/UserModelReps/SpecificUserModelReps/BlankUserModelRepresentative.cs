using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using Optepafi.Models.TemplateMan.Templates;
using Optepafi.Models.UserModelMan.UserModels.Specific;
using Optepafi.Models.Utils;

namespace Optepafi.Models.UserModelMan.UserModelReps.SpecificUserModelReps;

/// <summary>
/// Representative of blank user model.
/// 
/// For more information on user model representatives see <see cref="UserModelRepresentative{TUserModel,TTemplate,TConfiguration}"/>, <see cref="IUserModelType{TUserModel,TTemplate}"/> or <see cref="IUserModelTemplateBond{TUserModel,TTemplate}"/>.  
/// </summary>
public class BlankUserModelRepresentative : UserModelRepresentative<BlankUserModel,BlankTemplate,NullConfiguration>
{
    public static BlankUserModelRepresentative Instance { get; } = new();
    private BlankUserModelRepresentative() { }
    
    /// <inheritdoc cref="IUserModelType{TUserModel,TTemplate}.AssociatedTemplate"/>
    public override BlankTemplate AssociatedTemplate  => BlankTemplate.Instance;

    /// <inheritdoc cref="IUserModelType{TUserModel,TTemplate}.DefaultConfiguration"/>
    protected override NullConfiguration DefaultConfiguration => new();
    
    /// <inheritdoc cref="IUserModelType{TUserModel,TTemplate}.UserModelTypeName"/>
    public override string UserModelTypeName  => "Blank user model";
    /// <inheritdoc cref="IUserModelType{TUserModel,TTemplate}.UserModelFileNameSuffix"/>
    public override string UserModelFileNameSuffix  => "blankUM";
    /// <inheritdoc cref="IUserModelType{TUserModel,TTemplate}.UserModelFileExtension"/>
    public override string UserModelFileExtension  => "json";
    
    /// <inheritdoc cref="IUserModelType{TUserModel,TTemplate}.GetNewUserModel"/>
    protected override BlankUserModel GetNewUserModel(NullConfiguration configuration)
    {
        return new BlankUserModel();
    }

    /// <inheritdoc cref="UserModelRepresentative{TUserModel,TTemplate,TConfiguration}.ChangeConfiguration(TUserModel,TConfiguration)"/>
    protected override void ChangeConfiguration(BlankUserModel userModel, NullConfiguration configuration) { }

    /// <inheritdoc cref="IUserModelType{TUserModel,TTemplate}.DeserializeUserModel"/>
    protected override BlankUserModel? DeserializeUserModel((Stream, string) serializationWithPath, NullConfiguration configuration, CancellationToken? cancellationToken,
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