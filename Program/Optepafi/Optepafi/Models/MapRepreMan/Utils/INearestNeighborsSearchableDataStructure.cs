using System.Collections.Generic;

namespace Optepafi.Models.MapRepreMan.Utils;

public interface INearestNeighborsSearchableDataStructure<out TValue> : IEnumerable<TValue>
{
    TValue[] GetNearestNeighbors((int, int) coords, int count);
}