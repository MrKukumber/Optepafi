using System.Collections.Generic;
using System.Collections.Immutable;
using Optepafi.Models.MapRepreMan.MapRepreReps;
using Optepafi.Models.MapRepreMan.MapRepres;

namespace Optepafi.Models.SearchingAlgorithmMan;

public class SearchAlgorithmManager
{
    public static SearchAlgorithmManager Instance { get; } = new();
    private SearchAlgorithmManager(){}

    public IReadOnlySet<ISearchingAlgorithm> SearchingAlgorithms { get; } =
        ImmutableHashSet.Create<ISearchingAlgorithm>(/*TODO: doplnit algoritmy*/);

    public HashSet<ISearchingAlgorithm> GetUsableAlgorithmsFor(
        IMapRepreRepresentativ<IMapRepresentation> mapRepreRep)
    {
        
    }

    public HashSet<ISearchingAlgorithm> GetUsableAlgorithmsFor(
        IMapRepreRepresentativ<IMapRepresentation>[] mapRepreRepresentatives)
    {
        
    }

    public bool DoesRepresentUsableMapRepreFor(IMapRepreRepresentativ<IMapRepresentation> mapRepreRep,
        ISearchingAlgorithm algorithm)
    {
        return algorithm.DoesRepresentUsableMapRepre(mapRepreRep);
    }
    
}