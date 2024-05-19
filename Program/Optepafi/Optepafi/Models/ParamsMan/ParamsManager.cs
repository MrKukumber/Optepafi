using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Optepafi.ModelViews;

namespace Optepafi.Models.ParamsMan;

public sealed class ParamsManager : ModelViewBase
{
    private ParamsManager(){}
    public static ParamsManager Instance { get; } = new();

    private Dictionary<Type, IParams?> paramsStorage = new();
    
    public void SetParams(IParams parameters)
    {
        Type paramsType = parameters.GetType();
        paramsStorage[paramsType] = parameters;
    }

    public TParams? GetParams<TParams>()
        where TParams : IParams
    {
        Type requestedParamsType = typeof(TParams);
        if (paramsStorage.ContainsKey(requestedParamsType)) 
            return (TParams?) paramsStorage[requestedParamsType];

        TParams? parameters = ParamsSerializer.TryDeserialize<TParams>();
        paramsStorage.Add(requestedParamsType, parameters);
        return parameters;
    }

    public void SaveAllParams()
    {
        foreach (var (_, param) in paramsStorage)
        {
            if (param is not null)
            {
                Task.Run(() => ParamsSerializer.Serialize(param));
            }
        }
    }
}