using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using Optepafi.Models.TemplateMan.Templates;
using Optepafi.Models.UserModelMan.UserModels.SpecificUserModels;

namespace Optepafi.Models.UserModelMan.UserModelTypes.SpecificUserModelReps;

public class BlankUserModelRepresentative : IUserModelRepresentative<BlankTemplate, BlankUserModel>
{
    public static BlankUserModelRepresentative Instance { get; } = new();
    private BlankUserModelRepresentative() { }

    public string UserModelTypeName { get; } = "Blank user model";
    public string UserModelFileNameSuffix { get; } = "blankUM";
    public string UserModelFileExtension { get; } = "json";
    public BlankUserModel GetNewUserModel()
    {
        return new BlankUserModel();
    }

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