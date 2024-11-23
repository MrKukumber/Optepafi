using System.Threading;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapInterfaces;

namespace MapRepreViewer.Models.MapPartitioningMan;

public class MapPartitioningManager
{
    public static MapPartitioningManager Instance { get; } = new();
    private MapPartitioningManager(){}

    public bool TryGetMapPartition(IMap map, int size, CancellationToken cancellationToken, out IMap? partitionedMap, out bool wholeMapReturned)
    {
        if (map is IPartitionableMap partitionableMap)
        {
            partitionedMap = partitionableMap.GetPartitionOfSize(size, cancellationToken, out wholeMapReturned);
            return true;
        }
        wholeMapReturned = false;
        partitionedMap = null;
        return false;
    }
}