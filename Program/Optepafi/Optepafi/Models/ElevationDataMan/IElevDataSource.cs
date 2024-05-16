using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.ElevationDataMan;

public interface IElevDataSource
{
    public string Name { get; }
    
    public IReadOnlySet<IElevDataType> ElevDataTypesInSource { get; }
    
}