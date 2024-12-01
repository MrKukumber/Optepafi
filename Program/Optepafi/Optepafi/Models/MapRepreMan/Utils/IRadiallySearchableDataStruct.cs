using System.Collections.Generic;

namespace Optepafi.Models.MapRepreMan.Utils;

public interface IRadiallySearchableDataStruct<out TValue> : IEnumerable<TValue>
{
    IReadOnlyList<TValue> FindInEuclideanDistanceFrom((int, int) coords, int distance);
}