using System.Collections.Generic;
using System.Collections.Immutable;

namespace Optepafi.Models.ElevationDataMan;

public class ElevationDataManager
{
    public static ElevationDataManager Instance { get; } = new();
    private ElevationDataManager(){}

    public IReadOnlySet<IElevDataSource> ElevDataSources { get; } =
        ImmutableHashSet.Create<IElevDataSource>( /*TODO: add elevation data sources*/); //TODO: este premysliet ako reprezentovat, mozno skor nejakym listom koli poradiu
    
    
    
    
}