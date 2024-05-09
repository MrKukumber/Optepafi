using System;
using System.IO;
using System.Text.Json;

namespace Optepafi.Models.ParamsMan;

public static class ParamsSerializer
{
    private static string paramsDirRelativePath = "appStateParams";

    public static bool Serialize(IParams obj)
    {
        string typeName = obj.GetType().Name;
        string serializationFileRelativePath = paramsDirRelativePath + Path.PathSeparator + typeName + ".json";
        try
        {
            using (FileStream fs = new FileStream(serializationFileRelativePath, FileMode.Create, FileAccess.Write))
            {
                JsonSerializer.Serialize(fs, obj);
            }

            return true;
        }
        catch (IOException) { return false; } catch (NotSupportedException) { return false; } 
    }
    public static T? TryDeserialize<T>()
        where T : IParams
    {
        string typeName = typeof(T).Name;
        string serializationFileRelativePath = paramsDirRelativePath + Path.PathSeparator + typeName + ".json";
        if (!File.Exists(serializationFileRelativePath)) return default;
        try
        {
            using (FileStream fs = new FileStream(serializationFileRelativePath, FileMode.Open, FileAccess.Read))
            {
                return JsonSerializer.Deserialize<T>(fs);
            }
        }
        catch (IOException) { return default; } catch (NotSupportedException) { return default; }
        catch (JsonException) { return default; } 
    }
}