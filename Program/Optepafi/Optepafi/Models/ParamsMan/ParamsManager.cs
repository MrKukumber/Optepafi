using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Optepafi.ModelViews;

namespace Optepafi.Models.ParamsMan;

/// <summary>
/// Manages saving all kinds of application parameters. It provides saved parameters from previous applications runs,
/// as well as parameters set in current session.
/// From each type of parameters only one instance can be saved. Saved instances are obtainable by specifying type of requested params.
/// Setting is done by specified type of inserted params.
/// Saving of parameters is done by using <see cref="DataSerializer"/> class.
/// The load of parameters form serializations is lazy. Only when caller asks for specific parameters, they are loaded
/// from file and cashed.
/// </summary>
public sealed class ParamsManager : ModelViewBase
{
    private ParamsManager(){}
    public static ParamsManager Instance { get; } = new();

    private static string paramsDirRelativePath = "appStateParams";
    
    private Dictionary<Type, IParams?> paramsStorage = new();
    
    /// <summary>
    /// Sets and caches provided pameters in <paramref name="parameters"/>.
    /// The key, by which they are cached is their type specified by type parameter <typeparamref name="TParams"/>.
    /// </summary>
    /// <param name="parameters">Parameters to be cached</param>
    /// <typeparam name="TParams">Specifies, what type of parameter is set and it is used as kay, by which it is set.</typeparam>
    public void SetParams<TParams>(TParams parameters)
    where TParams : IParams
    {
        paramsStorage[typeof(TParams)] = parameters;
    }

    /// <summary>
    /// Provides cached or loaded parameters instance of type <typeparamref name="TParams"/>.
    /// At first is parameter looked for in dictonary of cached parameters. If it is not found there, it is tried to be loaded from
    /// serialization. If this try fails, null is returned.
    /// The result of load is cached in dictonary.
    /// </summary>
    /// <typeparam name="TParams">Type of parameters, which should be looked for.</typeparam>
    /// <returns>Cached or loaded parameters instance, if search was successful. Null if it was not.</returns>
    public TParams? GetParams<TParams>()
        where TParams : IParams
    {
        Type requestedParamsType = typeof(TParams);
        if (paramsStorage.ContainsKey(requestedParamsType)) 
            return (TParams?) paramsStorage[requestedParamsType];

        TParams? parameters = DataSerializer.TryDeserialize<TParams>(paramsDirRelativePath);
        paramsStorage.Add(requestedParamsType, parameters);
        return parameters;
    }
    
    /// <summary>
    /// Saves all cached params by serializing them. It is done in parallel way.
    /// The type set as serialization type parameter is received by use of visitor pattern on parameters instances.
    /// </summary>
    public void SaveAllParams()
    {
        foreach (var (_, param) in paramsStorage)
        {
            if (param is not null)
            {
                Task.Run(() => param.AcceptParamsManager(this));
            }
        }
    }
    public void Visit<TParam>(TParam param)
    {
        DataSerializer.Serialize(param, paramsDirRelativePath);
    }
    
}