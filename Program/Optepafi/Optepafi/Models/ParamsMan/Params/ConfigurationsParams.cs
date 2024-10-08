using System;
using System.Collections.Generic;
using Optepafi.Models.Utils;

namespace Optepafi.Models.ParamsMan.Params;

public class ConfigurationsParams : IParams
{
    public Dictionary<Type, IConfiguration> Configurations { get; set; } = new();
    
    public void AcceptParamsManager(ParamsManager manager)
    {
        manager.Visit(this);
    }
}