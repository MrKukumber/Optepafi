using System;
using System.IO;
using System.Text.Json;

namespace Optepafi.Models.ParamsMan;

/// <summary>
/// It is used for serializing of all kinds of data. Mainly is is used by <see cref="ParamsManager"/> for serializing apps parameters.
/// It uses <see cref="JsonSerializer"/> as main serialization technology.
/// <para>
/// The directory serialization is saved to can be specified by caller, or it is set by default to defaultSerializationDir directory.
/// </para>
/// </summary>
public static class DataSerializer
{
    
    private static readonly string DefaultSerializationsDirRelativePath = "defaultSerializationsDir";
    /// <summary>
    /// Serialize provided object <paramref name="obj"/> to json file named after type put as type parameter <typeparamref name="T"/>.
    /// File is saved to default relative directory "defaultSerializationsDir".
    /// </summary>
    /// <param name="obj">Object to be serialized</param>
    /// <typeparam name="T">Type of provided object to be serialized. The name of generated file will bear name of given type.</typeparam>
    /// <returns></returns>
    public static bool Serialize<T>(T obj)
    {
        return Serialize(obj, DefaultSerializationsDirRelativePath);
    }
    
    /// <summary>
    /// Serialize provided object <paramref name="obj"/> to json file named after type put as type parameter <typeparamref name="T"/>.
    /// File is saved to directory specified by <paramref name="dirPath"/> parameter.
    /// </summary>
    /// <param name="obj">Object to be serialized</param>
    /// <param name="dirPath">Specified path of directory, to which obj will be serialized.</param>
    /// <typeparam name="T">Type of provided object to be serialized. The name of generated file will bear name of given type.</typeparam>
    /// <returns></returns>
    public static bool Serialize<T>(T obj, string dirPath)
    {
        string typeName = typeof(T).Name;
        if (!Path.Exists(dirPath)) Directory.CreateDirectory(dirPath);
        string serializationFileRelativePath = dirPath + Path.DirectorySeparatorChar + typeName + ".json";
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
    
    /// <summary>
    /// Tries to deserialize object of type <typeparamref name="T"/> from file named by this type.
    /// Path of directory, where the file is looked for, is set to default relative path "defaultSerializationDir".
    /// </summary>
    /// <typeparam name="T">Defines what type should be returned and what name of file is looked for.</typeparam>
    /// <returns>Object of type <typeparamref name="T"/> if deserialization succeeds, otherwise null.</returns>
    public static T? TryDeserialize<T>()
        where T : IParams
    {
        return TryDeserialize<T>(DefaultSerializationsDirRelativePath);
    }
    
    /// <summary>
    /// Tries to deserialize object of type <typeparamref name="T"/> from file named by this type.
    /// Parameter <paramref name="dirPath"/> defines directory path, where the file should be looked for.
    /// </summary>
    /// <param name="dirPath">Path to directory, where the file with serialization should be looked for.</param>
    /// <typeparam name="T">Defines what type should be returned and what name of file is looked for.</typeparam>
    /// <returns>Object of type <typeparamref name="T"/> if deserialization succeeds, otherwise null.</returns>
    public static T? TryDeserialize<T>(string dirPath)
        where T : IParams
    {
        string typeName = typeof(T).Name;
        string serializationFileRelativePath = dirPath + Path.DirectorySeparatorChar + typeName + ".json";
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