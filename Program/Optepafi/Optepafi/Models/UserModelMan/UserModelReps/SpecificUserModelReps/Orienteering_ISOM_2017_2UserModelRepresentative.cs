using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using Optepafi.Models.TemplateMan.Templates;
using Optepafi.Models.UserModelMan.Configurations;
using Optepafi.Models.UserModelMan.UserModels.Specific;

namespace Optepafi.Models.UserModelMan.UserModelReps.SpecificUserModelReps;

//TODO: comment
public class Orienteering_ISOM_2017_2UserModelRepresentative : UserModelRepresentative<Orienteering_ISOM_2017_2UserModel, Orienteering_ISOM_2017_2, Orienteering_ISOM_2017_2UserModelConfiguration>
{
    public static Orienteering_ISOM_2017_2UserModelRepresentative Instance { get; } = new();
    private Orienteering_ISOM_2017_2UserModelRepresentative(){}

    protected override Orienteering_ISOM_2017_2UserModelConfiguration DefaultConfiguration { get; } = new ();
    public override string UserModelTypeName { get; } = "Orienteering (ISOM-2017-2) user model";
    public override string UserModelFileNameSuffix { get; } = "ori17UM";
    public override string UserModelFileExtension { get; } = "json";
    public override Orienteering_ISOM_2017_2 AssociatedTemplate { get; } = Orienteering_ISOM_2017_2.Instance;
    protected override Orienteering_ISOM_2017_2UserModel GetNewUserModel(Orienteering_ISOM_2017_2UserModelConfiguration configuration)
    {
        return new Orienteering_ISOM_2017_2UserModel();
    }

    protected override Orienteering_ISOM_2017_2UserModel? DeserializeUserModel((Stream, string) serializationWithPath,
        Orienteering_ISOM_2017_2UserModelConfiguration configuration, CancellationToken? cancellationToken,
        out UserModelManager.UserModelLoadResult result)
    {
        if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) { result = UserModelManager.UserModelLoadResult.Canceled; return null; }
        
        Orienteering_ISOM_2017_2UserModel? deserUserModel = null;
        try { deserUserModel = JsonSerializer.Deserialize<Orienteering_ISOM_2017_2UserModel>(serializationWithPath.Item1, new JsonSerializerOptions { NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals}); } 
        catch (JsonException) { } catch (NotSupportedException){}

        if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) { result = UserModelManager.UserModelLoadResult.Canceled; return null; }
        
        if (deserUserModel is not null)
        {
            deserUserModel.FilePath = serializationWithPath.Item2;
            ChangeConfiguration(deserUserModel, configuration);
            result = UserModelManager.UserModelLoadResult.Ok;
        }
        else 
            result = UserModelManager.UserModelLoadResult.UnableToDeserialize;
        return deserUserModel;
    }

    protected override void ChangeConfiguration(Orienteering_ISOM_2017_2UserModel userModel,
        Orienteering_ISOM_2017_2UserModelConfiguration configuration)
    { }
}