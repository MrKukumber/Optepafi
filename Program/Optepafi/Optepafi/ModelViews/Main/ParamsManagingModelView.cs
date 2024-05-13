using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using HarfBuzzSharp;
using Optepafi.Models.ParamsMan;

namespace Optepafi.ModelViews.Main;

public sealed class ParamsManagingModelView : ModelViewBase
{
    private ParamsManagingModelView(){}
    public static ParamsManagingModelView Instance { get; } = new();

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