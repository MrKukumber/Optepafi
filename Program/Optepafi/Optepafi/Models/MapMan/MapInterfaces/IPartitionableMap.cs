using System.Threading;

namespace Optepafi.Models.MapMan.MapInterfaces;

//TODO: comment
public interface IPartitionableMap
{
    IMap GetPartitionOfSize(int size, CancellationToken? cancellationToken, out bool wholeMapReturned);
}