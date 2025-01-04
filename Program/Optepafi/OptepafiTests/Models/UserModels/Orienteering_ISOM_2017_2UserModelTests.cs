using Optepafi.Models.UserModelMan;
using Optepafi.Models.UserModelMan.Configurations;
using Optepafi.Models.UserModelMan.UserModelReps.SpecificUserModelReps;

namespace OptepafiTests.Models.UserModels;

[TestClass]
public class Orienteering_ISOM_2017_2UserModelTests
{
    
    private string _testFilesPath = Path.Combine("test_files", "models", "userModels", "Orienteering_ISOM_2017_2");
    
    [TestMethod]
    public void NewUserModelCreatingAndSerializingTest()
    {
        var newUserModel = UserModelManager.Instance.GetNewUserModel(Orienteering_ISOM_2017_2UserModelRepresentative.Instance, new Orienteering_ISOM_2017_2UserModelConfiguration());
        string serialization = UserModelManager.Instance.SerializeUserModel(newUserModel);
    }

    [TestMethod]
    public void VikiUserModelDeserializingTest()
    {
        using (FileStream fs = new FileStream(Path.Combine(_testFilesPath, "Viki_User_Model.ori17UM.json"), FileMode.Open, FileAccess.Read))
        {
            var result = UserModelManager.Instance.TryDeserializeUserModelOfTypeFrom((fs, Path.Combine(_testFilesPath, "Viki_User_Model.ori17UM.json")), Orienteering_ISOM_2017_2UserModelRepresentative.Instance, new Orienteering_ISOM_2017_2UserModelConfiguration(), null, out var vikisUserModel);
        }
    }
    
}