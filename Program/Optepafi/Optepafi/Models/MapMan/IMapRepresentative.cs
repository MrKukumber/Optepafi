using Optepafi.Models.MapMan.MapInterfaces;

namespace Optepafi.Models.MapMan;

public interface IMapRepresentative<TMap> : IMapFormat<TMap>, IMapIdentifier<TMap>
    where TMap : IMap
{
    
}