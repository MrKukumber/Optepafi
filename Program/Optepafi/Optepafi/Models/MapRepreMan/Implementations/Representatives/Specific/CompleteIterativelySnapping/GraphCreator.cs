using Optepafi.Models.ElevationDataMan;
using Optepafi.Models.MapRepreMan.VerticesAndEdges;
using Optepafi.Models.TemplateMan.TemplateAttributes;

namespace Optepafi.Models.MapRepreMan.Implementations.Representatives.Specific.CompleteIterativelySnapping;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Optepafi.Models.GraphicsMan.Objects.Map;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapRepresentatives;
using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.MapRepreMan.Configurations;
using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.MapRepreMan.Graphs.Specific.CompleteNetIntertwining;
using Optepafi.Models.MapRepreMan.Implementations.Specific.CompleteIterativelySnapping;
using Optepafi.Models.MapRepreMan.Utils;
using Optepafi.Models.TemplateMan.Templates;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Shapes.Segments;
public class GraphCreator<TVertex, TEdge>
    where TVertex : IBuildableVertex<TEdge>, IInitializableVertex<TEdge, Orienteering_ISOM_2017_2.VertexAttributes>, new()
    where TEdge : IEdge, IInitializableEdge<TVertex, Orienteering_ISOM_2017_2.EdgeAttributes>, new()
{
    public GraphCreator() { }
    
    public int processedObjectsCount;
    // public int debugLimit = 5000; // for debugging
    public IEnumerable<VertexBuilder<TVertex, TEdge>> Create(OmapMap map,
        CompleteNetIntertwiningMapRepreConfiguration configuration, IProgress<MapRepreConstructionReport>? progress,
        CancellationToken? cancellationToken) 
    {
        List<NetVertexBuilder<TVertex, TEdge>> netVertices = CreateNet(map, configuration);
        // List<NetVertexBuilder> netVertices = new(); // for debugging
        if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return [];
        
        IEditableRadiallySearchableDataStruct<VertexBuilder<TVertex, TEdge>> vertexBuilders = new RadiallySearchableKdTree<VertexBuilder<TVertex, TEdge>>(netVertices, vb => (vb.Position.XPos, vb.Position.YPos));
        int allObjectsCount = 0;
        foreach (var crossablePolygonalSymbolCode in OrderedPolygonalSymbolsCodes)
            if (map.Symbols.Contains(new OmapMap.Symbol(crossablePolygonalSymbolCode)))
                allObjectsCount += map.Objects[crossablePolygonalSymbolCode].Count;
        foreach (var pathSymbolCode in OrderedPathsSymbolsCodes)
            if (map.Symbols.Contains(new OmapMap.Symbol(pathSymbolCode)))
                allObjectsCount += map.Objects[pathSymbolCode].Count;
        foreach (var linearobstacleSymbolCode in OrderedLinearObstacleSymbolsCodes)
            if (map.Symbols.Contains(new OmapMap.Symbol(linearobstacleSymbolCode)))
                allObjectsCount += map.Objects[linearobstacleSymbolCode].Count;
        Console.WriteLine(allObjectsCount);
        processedObjectsCount = 0;
        foreach (var polygonalSymbolCode in OrderedPolygonalSymbolsCodes)
        {
            Console.WriteLine(polygonalSymbolCode);
            if (map.Symbols.Contains(new OmapMap.Symbol(polygonalSymbolCode)))
                foreach (var obj in map.Objects[polygonalSymbolCode])
                {
                    // if (processedObjectsCount > debugLimit) break; // for debugging
                    if (processedObjectsCount % 100 == 0) { Console.WriteLine($"Processed objects count is {processedObjectsCount}"); }
                    // if (processedObjectsCount == debugLimit) // for debugging
                    ProcessCrossablePolygonalObject(obj, polygonalSymbolCode, vertexBuilders, configuration, cancellationToken);
                    ++processedObjectsCount;
                    if (allObjectsCount >= 100 && processedObjectsCount % (allObjectsCount / 100) == 0 && progress is not null) progress.Report( new MapRepreConstructionReport(processedObjectsCount / (float)allObjectsCount * 100));
                    if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return [];
                }
        }
        foreach (var pathSymbolCode in OrderedPathsSymbolsCodes)
        {
            Console.WriteLine(pathSymbolCode);
            if (map.Symbols.Contains(new OmapMap.Symbol(pathSymbolCode)))
                foreach (var obj in map.Objects[pathSymbolCode])
                {
                    // if (processedObjectsCount > debugLimit) break; // for debugging
                    if (processedObjectsCount % 100 == 0) { Console.WriteLine($"Processed objects count is {processedObjectsCount}"); }
                    // if (processedObjectsCount == debugLimit) // for debugging
                    ProcessPathObject(obj, pathSymbolCode, vertexBuilders, configuration, cancellationToken);
                    ++processedObjectsCount;
                    if (allObjectsCount >= 100 && processedObjectsCount % (allObjectsCount/100) == 0  && progress is not null) progress.Report(new MapRepreConstructionReport(processedObjectsCount/(float)allObjectsCount * 100));
                    if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return [];
                }
        }
        foreach (var linearObstacleSymbolCode in OrderedLinearObstacleSymbolsCodes)
            if (map.Symbols.Contains(new OmapMap.Symbol(linearObstacleSymbolCode)))
                foreach (var obj in map.Objects[linearObstacleSymbolCode])
                {
                    // if (processedObjectsCount > debugLimit) break; // for debugging
                    if (processedObjectsCount % 100 == 0) { Console.WriteLine($"Processed objects count is {processedObjectsCount}"); }
                    // if (processedObjectsCount == debugLimit) // for debugging
                    ProcessLinearObstacleObject(obj, linearObstacleSymbolCode, vertexBuilders, configuration, cancellationToken);
                    ++processedObjectsCount;
                    if (allObjectsCount >= 100 && processedObjectsCount % (allObjectsCount/100) == 0 && progress is not null) progress.Report(new MapRepreConstructionReport(processedObjectsCount/(float)allObjectsCount * 100));
                    if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return [];
                }
        return vertexBuilders;
    }
    
    private List<decimal> OrderedPolygonalSymbolsCodes =
    [
        403, 404, 404.1m, 413.1m, 213, 414.1m, 401, 402, 402.1m, 413, 412, 414, 407, 409, 406, 406.1m, 408, 408.1m, 408.2m,
        410, 410.1m, 410.2m, 410.3m, 410.4m, 214, 405, 310, 308, 302, 302.1m, 302.5m, 113, 114, 210, 211, 212, 208, 209, 501, 501.1m,
        520, 307, 307.1m, 301, 301.1m, 301.2m, 301.3m, 206, 521, 521.2m, 521.3m, 520.2m
    ];

    private List<decimal> OrderedPathsSymbolsCodes =
    [
        508, 508.2m, 508.3m, 508.4m, 508.1m, 507, 506, 505, 504, 503, 502, 502.2m, 532 
    ];
    
    private List<decimal> OrderedLinearObstacleSymbolsCodes =
    [
        104, 105, 107, 305, 304, 201, 201.3m, 202, 202.2m, 215, 513, 515, 516, 518, 528, 529
    ];

    private List<NetVertexBuilder<TVertex, TEdge>> CreateNet(OmapMap map, CompleteNetIntertwiningMapRepreConfiguration configuration) 
        => configuration.typeOfNet.AllValues[configuration.typeOfNet.IndexOfSelectedValue] switch
    {
        CompleteNetIntertwiningMapRepreConfiguration.NetTypesEnumeration.Triangular => CreateTriangularNet(map, configuration),
        _ => throw new InvalidEnumArgumentException()
    };

    private List<NetVertexBuilder<TVertex, TEdge>> CreateTriangularNet(OmapMap map, CompleteNetIntertwiningMapRepreConfiguration configuration)
    {
        (int left, int top, int right, int bottom) boundaries = (map.BottomLeftBoundingCorner.XPos, map.TopRightBoundingCorner.YPos, map.TopRightBoundingCorner.XPos, map.BottomLeftBoundingCorner.YPos);
        int edgeLength = configuration.standardEdgeLength.Value;
        int colls = (boundaries.right - boundaries.left)/edgeLength + 1;
        int rows = (int)((boundaries.top - boundaries.bottom)/(Math.Sqrt(3)*edgeLength/2)) + 1;
        List<NetVertexBuilder<TVertex, TEdge>> vertices = new ();
        List<NetVertexBuilder<TVertex, TEdge>> lastRow = new ();
        List<NetVertexBuilder<TVertex, TEdge>> currentRow = new ();
        (Orienteering_ISOM_2017_2.Grounds?, Orienteering_ISOM_2017_2.Boulders?, Orienteering_ISOM_2017_2.Stones?, Orienteering_ISOM_2017_2.Water?, Orienteering_ISOM_2017_2.VegetationAndManMade?, Orienteering_ISOM_2017_2.VegetationGoodVis?) justForest =  (null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.Forest_405, null);
        (Orienteering_ISOM_2017_2.NaturalLinearObstacles?, Orienteering_ISOM_2017_2.Paths?, Orienteering_ISOM_2017_2.ManMadeLinearObstacles?) blankLinearFeatures = (null, null, null);
        for (int coll = 0; coll < colls; ++coll)
            lastRow.Add(new NetVertexBuilder<TVertex, TEdge>( new Orienteering_ISOM_2017_2.VertexAttributes( new MapCoordinates(boundaries.left + coll * edgeLength, boundaries.top)))
                {Surroundings = justForest});

        for (int row = 1; row < rows; ++row)
        {
            for (int coll = 0; coll < colls - 1; ++coll)
            {
                NetVertexBuilder<TVertex, TEdge> vertex = new NetVertexBuilder<TVertex, TEdge>(new Orienteering_ISOM_2017_2.VertexAttributes(new MapCoordinates( boundaries.left + edgeLength/2 + coll * edgeLength , (int) (boundaries.top - row * Math.Sqrt(3) * edgeLength / 2))))
                    {Surroundings = justForest};
                vertex.NonBoundaryEdges[lastRow[coll]] = blankLinearFeatures ;
                lastRow[coll].NonBoundaryEdges[vertex] = blankLinearFeatures;
                vertex.NonBoundaryEdges[lastRow[coll + 1]] = blankLinearFeatures ;
                lastRow[coll + 1].NonBoundaryEdges[vertex] = blankLinearFeatures ;
                lastRow[coll + 1].NonBoundaryEdges[lastRow[coll]] = blankLinearFeatures;
                lastRow[coll].NonBoundaryEdges[lastRow[coll + 1]] = blankLinearFeatures;
                currentRow.Add(vertex);
            }
            
            vertices.AddRange(lastRow);
            lastRow.Clear();
            lastRow.AddRange(currentRow);
            currentRow.Clear();
            ++row;
            
            NetVertexBuilder<TVertex, TEdge> leftVertex = new NetVertexBuilder<TVertex, TEdge>(new Orienteering_ISOM_2017_2.VertexAttributes(new MapCoordinates(boundaries.left, (int) (boundaries.top - row * Math.Sqrt(3) * edgeLength / 2))))
                {Surroundings = justForest};
            leftVertex.NonBoundaryEdges[lastRow[0]] = blankLinearFeatures;
            lastRow[0].NonBoundaryEdges[leftVertex] = blankLinearFeatures;
            currentRow.Add(leftVertex);
            for (int coll = 1; coll < colls - 1; ++coll)
            {
                NetVertexBuilder<TVertex, TEdge> vertex =  new NetVertexBuilder<TVertex, TEdge>(new Orienteering_ISOM_2017_2.VertexAttributes(new MapCoordinates( boundaries.left + coll * edgeLength , (int) (boundaries.top - row * Math.Sqrt(3) * edgeLength / 2)))) 
                    {Surroundings = justForest};
                vertex.NonBoundaryEdges[lastRow[coll - 1]] = blankLinearFeatures;
                lastRow[coll - 1].NonBoundaryEdges[vertex] = blankLinearFeatures;
                vertex.NonBoundaryEdges[lastRow[coll]] = blankLinearFeatures;
                lastRow[coll].NonBoundaryEdges[vertex] = blankLinearFeatures;
                lastRow[coll].NonBoundaryEdges[lastRow[coll-1]] = blankLinearFeatures;
                lastRow[coll - 1].NonBoundaryEdges[lastRow[coll]] = blankLinearFeatures;
                currentRow.Add(vertex);
            }
            NetVertexBuilder<TVertex, TEdge> rightVertex = new NetVertexBuilder<TVertex, TEdge>(new Orienteering_ISOM_2017_2.VertexAttributes(new MapCoordinates(boundaries.left + (colls - 1) * edgeLength, (int) (boundaries.top - row * Math.Sqrt(3) * edgeLength / 2))))
                {Surroundings = justForest};
            rightVertex.NonBoundaryEdges[lastRow[colls - 2]] = blankLinearFeatures;
            lastRow[colls - 2].NonBoundaryEdges[rightVertex] = blankLinearFeatures;
            currentRow.Add(rightVertex);
            
            vertices.AddRange(lastRow);
            lastRow.Clear();
            lastRow.AddRange(currentRow);
            currentRow.Clear();
        }
        
        for (int coll = 1; coll < colls; ++coll)
        {
            lastRow[coll].IsStationary = true;
            lastRow[coll].NonBoundaryEdges[lastRow[coll - 1]] = blankLinearFeatures;
            lastRow[coll - 1].NonBoundaryEdges[lastRow[coll]] = blankLinearFeatures;
        }
        vertices.AddRange(lastRow);
        return vertices;
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void ProcessCrossablePolygonalObject(OmapMap.Object obj, decimal symbolCode,
        IEditableRadiallySearchableDataStruct<VertexBuilder<TVertex, TEdge>> allVertices,
        CompleteNetIntertwiningMapRepreConfiguration configuration, CancellationToken? cancellationToken)
    {
        // if(processedObjectsCount == debugLimit) {} // for debugging
        // 0
        // symbol of code 410.4m is line symbol that represents very thin polygonal object so it has to be processed separately
        BoundaryVertexBuilder<TVertex, TEdge>[][] potentiallyEntangledChains;
        if (symbolCode != 410.4m)
            potentiallyEntangledChains = PolygonalObjectsProcessing.GetBlankBoundaryChains(obj, configuration.standardEdgeLength.Value, configuration.minBoundaryEdgeRatio.Value);
        else
        {
            var potentiallyEntangledChain = PolygonalObjectsProcessing.GetBlankBoundaryChainOfSymbol410_4(obj, configuration.standardEdgeLength.Value, configuration.minBoundaryEdgeRatio.Value);
            potentiallyEntangledChains = potentiallyEntangledChain is not null ? [potentiallyEntangledChain]: [];
        }

        if (potentiallyEntangledChains.Length == 0) return;
        // 1
        var chains = PolygonalObjectsProcessing.SplitChainToMoreIfItIsEntangledAndMakeThemTurnRight(potentiallyEntangledChains, configuration.standardEdgeLength.Value);
        // 2
        var (verticesOfCutNonBoundaryEdges, verticesOfCutBoundaryEdges) = PolygonalObjectsProcessing.CutAllCrossedEdges(chains, allVertices, configuration.standardEdgeLength.Value);
        // 3
        var (outerVerticesOfCutEdges, innerVerticesOfCutEdges, allInnerVertices) = PolygonalObjectsProcessing.FindNodesInsideThePolygonByDfs(verticesOfCutNonBoundaryEdges, verticesOfCutBoundaryEdges, chains); 
        // 4
        // if(processedObjectsCount != debugLimit) // for debugging
        PolygonalObjectsProcessing.UpdateAttributesOfInnerEdges(allInnerVertices, symbolCode);
        // 5
        var chainsEnrichedByNewCrossSectionVertices = 
            // processedObjectsCount == debugLimit ? chain : // for debugging
                PolygonalObjectsProcessing.ProcessCutBoundaryEdgesByChain(chains, verticesOfCutBoundaryEdges, outerVerticesOfCutEdges, symbolCode);
        // 6
        // if(processedObjectsCount != debugLimit) // for debugging
        PolygonalObjectsProcessing.ConnectChainToVerticesOfCutEdgesAndOtherVerticesOfChain(chainsEnrichedByNewCrossSectionVertices, outerVerticesOfCutEdges, innerVerticesOfCutEdges, allVertices, configuration.standardEdgeLength.Value);
        // 7
        // if(processedObjectsCount != debugLimit) // for debugging
        PolygonalObjectsProcessing.SetAttributesOfChainsEdges(chainsEnrichedByNewCrossSectionVertices, outerVerticesOfCutEdges, symbolCode);
        // 8
        PolygonalObjectsProcessing.AddChainVerticesToTheGraph(chainsEnrichedByNewCrossSectionVertices, allVertices);
        // 9
        // if(processedObjectsCount != debugLimit) // for debugging
        PolygonalObjectsProcessing.SetAttributesOfNonBoundaryEdgesBetweenChainAndBoundaryVertices(chainsEnrichedByNewCrossSectionVertices);
    }
    
    private void ProcessPathObject(OmapMap.Object obj, decimal symbolCode, IEditableRadiallySearchableDataStruct<VertexBuilder<TVertex, TEdge>> allVertices,
        CompleteNetIntertwiningMapRepreConfiguration configuration, CancellationToken? cancellationToken)
    {
        // 0
        var potentiallyEntangledMultiOccuringVerticesChain = PathObjectsProcessing.GetBlankBoundaryChain(obj, configuration.standardEdgeLength.Value, configuration.minBoundaryEdgeRatio.Value);
        if (potentiallyEntangledMultiOccuringVerticesChain is null) return;
        // 1
        var potentiallyMultiOccuringVerticesChain = PathObjectsProcessing.AddCrossSectionVerticesIfItIsEntangled(potentiallyEntangledMultiOccuringVerticesChain);
        // 2
        var (verticesOfCutNonBoundaryEdges, verticesOfCutBoundaryEdges) = PathObjectsProcessing.CutAllCrossedEdges(potentiallyMultiOccuringVerticesChain, allVertices, configuration.standardEdgeLength.Value);
        // 3
        var potentiallyMultiOccuringVerticesChainEnrichedByNewCrossSectionVertices = 
            // processedObjectsCount == debugLimit ? potentiallyMultiOccuringVerticesChain : // for debugging
            PathObjectsProcessing.ProcessCutBoundaryEdgesByChain(potentiallyMultiOccuringVerticesChain, verticesOfCutBoundaryEdges);
        // 4
        // if(processedObjectsCount != debugLimit) // for debugging
        PathObjectsProcessing.ConnectChainToVerticesOfCutEdgesAndOtherVerticesOfChain(potentiallyMultiOccuringVerticesChainEnrichedByNewCrossSectionVertices , verticesOfCutNonBoundaryEdges, verticesOfCutBoundaryEdges, allVertices, configuration.standardEdgeLength.Value);
        // 5
        // if(processedObjectsCount != debugLimit) // for debugging
        PathObjectsProcessing.SetAttributesOfChainsEdges(potentiallyMultiOccuringVerticesChainEnrichedByNewCrossSectionVertices, symbolCode);
        // 6
        PathObjectsProcessing.AddChainVerticesToTheGraph(potentiallyMultiOccuringVerticesChainEnrichedByNewCrossSectionVertices, allVertices);
        // 7
        // if(processedObjectsCount != debugLimit) // for debugging
        PathObjectsProcessing.SetAttributesOfNonBoundaryEdgesBetweenChainAndBoundaryVertices(potentiallyMultiOccuringVerticesChainEnrichedByNewCrossSectionVertices);
    }
    
    private void ProcessLinearObstacleObject(OmapMap.Object obj, decimal symbolCode, IEditableRadiallySearchableDataStruct<VertexBuilder<TVertex, TEdge>> allVertices,
        CompleteNetIntertwiningMapRepreConfiguration configuration, CancellationToken? cancellationToken)
    {
        // 0
        var chain = LinearObstaclesProcessing.GetBlankBoundaryChain(obj, configuration.standardEdgeLength.Value, configuration.minBoundaryEdgeRatio.Value);
        if (chain is null) return;
        // 1
        var (verticesOfCrossedNonBoundaryEdges, verticesOfCrossedBoundaryEdges) = LinearObstaclesProcessing.FindAllCrossedEdges(chain, allVertices, configuration.standardEdgeLength.Value);
        // 2
        LinearObstaclesProcessing.SetLinearObstacleAttributesToAllCrossedEdges(verticesOfCrossedNonBoundaryEdges, verticesOfCrossedBoundaryEdges, symbolCode);
    }
    
    private static class PolygonalObjectsProcessing
    {
        #region 0 \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        
        public static BoundaryVertexBuilder<TVertex, TEdge>[][] GetBlankBoundaryChains(OmapMap.Object obj, int standardEdgeLength, float minBoundaryEdgeRatio)
        {
            var multiPolygonalSegments = obj.CollectSegmentsForMultiPolygon();
            List<BoundaryVertexBuilder<TVertex, TEdge>[]> boundaryChains = new List<BoundaryVertexBuilder<TVertex, TEdge>[]>();
            // last and first vertices will be on the same position
            foreach (var polygonalSegments in multiPolygonalSegments)
            {
                var boundaryChain = Utils.GetBlankChainOfSegmentedLine(polygonalSegments.Last().LastPoint, polygonalSegments, standardEdgeLength, minBoundaryEdgeRatio);
                if (boundaryChain.Count <= 2)  continue; 
                //connects last and second vertex of chain for polygonal shape, removes first one
                boundaryChain.Last().BoundaryEdges[boundaryChain[1]] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                boundaryChain[1].BoundaryEdges[boundaryChain.Last()] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                boundaryChain[1].BoundaryEdges.Remove(boundaryChain[0]);
                boundaryChain.RemoveAt(0);
                if (boundaryChain.Count <= 2)  continue; 
                boundaryChains.Add(boundaryChain.ToArray());
            }
            return boundaryChains.ToArray();
        }
        
        public static BoundaryVertexBuilder<TVertex, TEdge>[]? GetBlankBoundaryChainOfSymbol410_4(OmapMap.Object obj,
            int standardEdgeLength, float minBoundaryEdgeRatio)
        {
            var segments = obj.CollectSegmentsForPath();
            var leftBoundaryChain = Utils.GetBlankShiftedChainOfSegmentedLine(obj.TypedCoords[0].coords, segments, standardEdgeLength, minBoundaryEdgeRatio, true, 125);
            var rightBoundaryChain = Utils.GetBlankShiftedChainOfSegmentedLine(obj.TypedCoords[0].coords, segments, standardEdgeLength, minBoundaryEdgeRatio, false, 125);   
            if (leftBoundaryChain.Count + rightBoundaryChain.Count <= 2) return null;
            // connect left and right boundary chain
            leftBoundaryChain[0].BoundaryEdges[rightBoundaryChain[0]] = new Orienteering_ISOM_2017_2.EdgeAttributes();
            rightBoundaryChain[0].BoundaryEdges[leftBoundaryChain[0]] = new Orienteering_ISOM_2017_2.EdgeAttributes();
            leftBoundaryChain.Last().BoundaryEdges[rightBoundaryChain.Last()] = new Orienteering_ISOM_2017_2.EdgeAttributes();
            rightBoundaryChain.Last().BoundaryEdges[leftBoundaryChain.Last()] = new Orienteering_ISOM_2017_2.EdgeAttributes();
            //return left boundary chain together with reversed right boundary chain
            rightBoundaryChain.Reverse();
            leftBoundaryChain.AddRange(rightBoundaryChain);
            return leftBoundaryChain.ToArray();
        }
        
        #endregion
        
        #region 1 \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public static BoundaryVertexBuilder<TVertex, TEdge>[][] SplitChainToMoreIfItIsEntangledAndMakeThemTurnRight(
            BoundaryVertexBuilder<TVertex, TEdge>[][] potentiallyEntangledChains, int standardEdgeLength)
        {
            var (indicesOfCutEdgesWithCorrespondingTemporaryVertices, allTemporaryVertices) = CreateTemporaryVertices(potentiallyEntangledChains);
            List<BoundaryVertexBuilder<TVertex, TEdge>[]> collectedChains;
            // if no cross-sections of edges are found, chains are not entangled so we can return them whole
            // we just have to check their orientation
            if (indicesOfCutEdgesWithCorrespondingTemporaryVertices.Count == 0)
                collectedChains = TurnThemRight(potentiallyEntangledChains);
            else
            {
                DisconnectCrossingEdgesInChain(potentiallyEntangledChains, indicesOfCutEdgesWithCorrespondingTemporaryVertices);
                InterconnectTemporaryVertices(indicesOfCutEdgesWithCorrespondingTemporaryVertices, potentiallyEntangledChains);
                ProcessTemporaryVertices(allTemporaryVertices); 
                collectedChains = CollectRightTurningChains(potentiallyEntangledChains.SelectMany(chain => chain).ToHashSet(), standardEdgeLength);
            }
            // we have to reverse chains which creates holes in multipolygon, so that the right side of these chains was inner.
            return ReverseChainsOfHoles(collectedChains).ToArray();
        }

        private static List<BoundaryVertexBuilder<TVertex, TEdge>[]> TurnThemRight(BoundaryVertexBuilder<TVertex, TEdge>[][] chains)
        {
            var chainsTurnedRight = new List<BoundaryVertexBuilder<TVertex, TEdge>[]>();
            for (int i = 0; i < chains.Length; ++i)
                if (!Utils.IsRightSideInner(chains[i][0], chains[i].Last().Position, chains[i][1].Position, chains[i])) chainsTurnedRight.Add(chains[i].Reverse().ToArray());
                else chainsTurnedRight.Add(chains[i]);
            return chainsTurnedRight;
        }
        

        private static (SortedList<(int, int), List<ChainEntanglementTemporaryVertexBuilder>>, List<ChainEntanglementTemporaryVertexBuilder>) CreateTemporaryVertices(BoundaryVertexBuilder<TVertex, TEdge>[][] potentiallyEntangledChains)
        {
            SortedList<(int, int), List<ChainEntanglementTemporaryVertexBuilder>> indicesOfCutEdgesWithCorrespondingTemporaryVertices = new();
            List<ChainEntanglementTemporaryVertexBuilder> allCreatedTemporaryVertices = new();
            
            // For each cross-section of chain edges is created new temporary vertex
            // this vertex is then added to lists of created temporary vertices which are hold in dictionary under the indices of crossed edges
            for (int ich = 0; ich < potentiallyEntangledChains.Length; ++ich)
                for (int ie = 0; ie < potentiallyEntangledChains[ich].Length - 1; ++ie)
                {
                    for (int je = ie + 1; je < potentiallyEntangledChains[ich].Length; ++je)
                        AddTemporaryVertexIfEdgesAreCrossing(potentiallyEntangledChains, indicesOfCutEdgesWithCorrespondingTemporaryVertices, allCreatedTemporaryVertices, ich, ie, ich, je);
                    for (int jch = ich + 1; jch < potentiallyEntangledChains.Length; ++jch)
                        for (int je = 0; je < potentiallyEntangledChains[jch].Length; ++je)
                            AddTemporaryVertexIfEdgesAreCrossing(potentiallyEntangledChains, indicesOfCutEdgesWithCorrespondingTemporaryVertices, allCreatedTemporaryVertices, ich, ie, jch, je);
                            
                }
            return (indicesOfCutEdgesWithCorrespondingTemporaryVertices, allCreatedTemporaryVertices);
        }

        private static void AddTemporaryVertexIfEdgesAreCrossing(BoundaryVertexBuilder<TVertex, TEdge>[][] potentiallyEntangledChains, SortedList<(int, int), List<ChainEntanglementTemporaryVertexBuilder>> indicesOfCutEdgesWithCorrespondingTemporaryVertices, List<ChainEntanglementTemporaryVertexBuilder> allCreatedTemporaryVertices, int ich, int ie, int jch, int je)
        {
            // if (i == 0 && j == potentiallyEntangledChains.Length - 1) return;
            int jes = je == potentiallyEntangledChains[jch].Length - 1 ? 0 : je + 1;
            MapCoordinates? crossSectionCoords = Utils.GetLineSegmentsCrossSectionCoordsParallelCrossSectionIncluded( potentiallyEntangledChains[ich][ie].Position, potentiallyEntangledChains[ich][ie + 1].Position, potentiallyEntangledChains[jch][je].Position, potentiallyEntangledChains[jch][jes].Position);
            if (crossSectionCoords is not null)
            {
                // if cross-section of successive edges is checked, cross-section is valid only if it is not positioned in common vertex of these edges (vertex on position je). This happens when these edges are parallel.
                if (ich != jch || je != ie + 1 || crossSectionCoords != potentiallyEntangledChains[jch][je].Position)
                {
                    var newTemporaryVertex = new ChainEntanglementTemporaryVertexBuilder( new Orienteering_ISOM_2017_2.VertexAttributes(crossSectionCoords.Value));
                    if (indicesOfCutEdgesWithCorrespondingTemporaryVertices.ContainsKey((ich, ie))) indicesOfCutEdgesWithCorrespondingTemporaryVertices[(ich, ie)].Add(newTemporaryVertex);
                    else indicesOfCutEdgesWithCorrespondingTemporaryVertices[(ich, ie)] = [newTemporaryVertex];
                    if (indicesOfCutEdgesWithCorrespondingTemporaryVertices.ContainsKey((jch, je))) indicesOfCutEdgesWithCorrespondingTemporaryVertices[(jch, je)].Add(newTemporaryVertex);
                    else indicesOfCutEdgesWithCorrespondingTemporaryVertices[(jch, je)] = [newTemporaryVertex];
                    allCreatedTemporaryVertices.Add(newTemporaryVertex);
                }
            }
        }
        
        private static void DisconnectCrossingEdgesInChain(BoundaryVertexBuilder<TVertex, TEdge>[][] potentiallyEntangledChains, SortedList<(int, int), List<ChainEntanglementTemporaryVertexBuilder>> indicesOfVerticesWhoseEdgesWereCutWithCorrespondingTemporaryVertices)
        {
            // every edge, that was crossing some other edge will be removed from chain
            foreach (var (chainIndex, edgeIndex) in indicesOfVerticesWhoseEdgesWereCutWithCorrespondingTemporaryVertices.Keys)
            {
                int edgeIndexSuccessor = edgeIndex == potentiallyEntangledChains[chainIndex].Length - 1 ? 0 : edgeIndex + 1; 
                potentiallyEntangledChains[chainIndex][edgeIndex].BoundaryEdges.Remove(potentiallyEntangledChains[chainIndex][edgeIndexSuccessor]);
                potentiallyEntangledChains[chainIndex][edgeIndexSuccessor].BoundaryEdges.Remove(potentiallyEntangledChains[chainIndex][edgeIndex]);
            }
        }

        private static void InterconnectTemporaryVertices(SortedList<(int, int), List<ChainEntanglementTemporaryVertexBuilder>> indicesOfVerticesWhoseEdgesWereCutWithCorrespondingTemporaryVertices, BoundaryVertexBuilder<TVertex, TEdge>[][] chainWithoutEntangledEdges)
        {
            // temporary vertices are one by one correctly interconnected, so then new edges of chains could be correctly deduced
            foreach (var ((chainIndex, edgeIndex), correspondingTemporaryVertices) in indicesOfVerticesWhoseEdgesWereCutWithCorrespondingTemporaryVertices)
            {
                int edgeIndexSuccessor = edgeIndex == chainWithoutEntangledEdges[chainIndex].Length - 1 ? 0 : edgeIndex + 1;
                // at first, we need to order created temporary vertices in the way they are positioned on cut edge 
                var correspondingSortedTemporaryVertices = Utils.SortVerticesCorrectly(chainWithoutEntangledEdges[chainIndex][edgeIndex], chainWithoutEntangledEdges[chainIndex][edgeIndexSuccessor], correspondingTemporaryVertices);
                var lastVertex = chainWithoutEntangledEdges[chainIndex][edgeIndex];
                // iteratively we set income and outcome neighbors of temporary vertices
                // logic of temporary vertices will ensure correct coupling of income and outcome neighbors
                for (int i = 0; i < correspondingSortedTemporaryVertices.Count - 1; i++)
                {
                    correspondingSortedTemporaryVertices[i].SetFromToVertex(lastVertex, correspondingSortedTemporaryVertices[i + 1]);
                    lastVertex = correspondingSortedTemporaryVertices[i];
                }
                correspondingSortedTemporaryVertices.Last().SetFromToVertex(lastVertex, chainWithoutEntangledEdges[chainIndex][edgeIndexSuccessor]);
            }
        }
        

        private static void ProcessTemporaryVertices(List<ChainEntanglementTemporaryVertexBuilder> allTemporaryVertices)
        {
            // most important step in resolving of chains entanglement
            // temporary vertices are one by one cut of from other ones what concludes in interconnecting of correct pairs of chain vertices, so that there were no edge crossings whatsoever
            foreach (var temporaryVertex in allTemporaryVertices)
                temporaryVertex.ProcessByDisconnecting();
        }

        private static List<BoundaryVertexBuilder<TVertex, TEdge>[]> CollectRightTurningChains(HashSet<BoundaryVertexBuilder<TVertex, TEdge>> choppedChain, int standardEdgeLength)
        {
            List<BoundaryVertexBuilder<TVertex, TEdge>[]> chains = new ();
            // while there are some non-processed vertices in "chopped" chain, components are looked for and added to list of newly created chains
            while (choppedChain.Count > 0)
            {
                List<BoundaryVertexBuilder<TVertex, TEdge>> chain = [choppedChain.First()];
                choppedChain.Remove(chain[0]);
                // if first vertex added to chain has less then two neighbors, it means it is end of the not cyclic component
                // we will cut such vertex off
                if (chain[0].BoundaryEdges.Count < 2)
                {
                    // if it has a neighbor, it is disconnected from it
                    if (chain[0].BoundaryEdges.Count == 1)
                        chain[0].BoundaryEdges.First().Key.BoundaryEdges.Remove(chain[0]);
                    continue;
                }
                var vertex = chain[0].BoundaryEdges.First().Key;
                while (vertex != chain[0])
                {
                    choppedChain.Remove(vertex);
                    // we have to check edge whether they are not too long.
                    // too long edges could be created by unraveling of the entangled chain
                    CheckWhetherLastEdgeIsNotTooLong(vertex, chain, standardEdgeLength);
                    foreach (var (neighbor, _) in vertex.BoundaryEdges)
                    {
                        if(neighbor == chain.Last()) continue;
                        chain.Add(vertex);
                        vertex = neighbor;
                        break;
                    }
                }
                // there could emerge new microscopic edges after chains copping
                RemoveMicroscopicEdges(chain);
                // if found component has less than three vertices it is not returned as chain
                if (chain.Count <= 2)
                    continue;
                // if chain is turning left, it is reversed
                if (!Utils.IsRightSideInner(chain[0], chain.Last().Position, chain[1].Position, chain.ToArray())) chain.Reverse(); 
                chains.Add(chain.ToArray());
            }
            return chains;
        }

        private static void CheckWhetherLastEdgeIsNotTooLong(BoundaryVertexBuilder<TVertex, TEdge> vertex, List<BoundaryVertexBuilder<TVertex, TEdge>> chain, int standardEdgeLength)
        {
            int countOfEdgesToBeAdded = (int)((vertex.Position - chain.Last().Position).Length() / standardEdgeLength);
            var lastVertex = chain.Last();
            for (int i = 0; i < countOfEdgesToBeAdded; i++)
            {
                BoundaryVertexBuilder<TVertex, TEdge> newVertex = new BoundaryVertexBuilder<TVertex, TEdge>( new Orienteering_ISOM_2017_2.VertexAttributes( lastVertex.Position + 1 / (float)(countOfEdgesToBeAdded + 1 - i) * (vertex.Position - lastVertex.Position)));
                vertex.BoundaryEdges.Remove(lastVertex);
                lastVertex.BoundaryEdges.Remove(vertex);
                lastVertex.BoundaryEdges[newVertex] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                newVertex.BoundaryEdges[lastVertex] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                vertex.BoundaryEdges[newVertex] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                newVertex.BoundaryEdges[vertex] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                chain.Add(newVertex);
                lastVertex = newVertex;
            }
        }
        
        private static void RemoveMicroscopicEdges(List<BoundaryVertexBuilder<TVertex, TEdge>> chain)
        {
            // removes edges that are microscopic 
            for (int i = 0; i < chain.Count; ++i)
            {
                var iP = i == 0 ? chain.Count - 1 : i - 1;
                var iS = i == chain.Count - 1 ? 0 : i + 1;
                if ((chain[iP].Position - chain[i].Position).Length() < Utils.microscopicEdgeLength)
                {
                    chain[iP].BoundaryEdges.Remove(chain[i]);
                    chain[i].BoundaryEdges.Remove(chain[iP]);
                    chain[iS].BoundaryEdges.Remove(chain[i]);
                    chain[i].BoundaryEdges.Remove(chain[iS]);
                    chain[iP].BoundaryEdges[chain[iS]] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                    chain[iS].BoundaryEdges[chain[iP]] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                    chain.RemoveAt(i);
                }
            }
        }

        private static List<BoundaryVertexBuilder<TVertex, TEdge>[]> ReverseChainsOfHoles( List<BoundaryVertexBuilder<TVertex, TEdge>[]> collectedChains)
        {
            int[] countOfOuterChainsForCollectedChains = new int[collectedChains.Count];
            for(int i = 0; i < collectedChains.Count; ++i) 
            {
                foreach (var otherChain in collectedChains)
                {
                    if (collectedChains[i] == otherChain) continue;
                    if (IsInnerVertex(collectedChains[i].First(), otherChain)) countOfOuterChainsForCollectedChains[i]++;
                }
            }

            for (int i = 0; i < collectedChains.Count; ++i)
            {
                if (countOfOuterChainsForCollectedChains[i] % 2 == 1)
                    collectedChains[i] = collectedChains[i].Reverse().ToArray();
            }
            return collectedChains;
        }
        
        private class ChainEntanglementTemporaryVertexBuilder(Orienteering_ISOM_2017_2.VertexAttributes attributesNoElev) : BoundaryVertexBuilder<TVertex, TEdge>(attributesNoElev)
        {
            private BoundaryVertexBuilder<TVertex, TEdge>? from1;
            private BoundaryVertexBuilder<TVertex, TEdge>? to1;
            private BoundaryVertexBuilder<TVertex, TEdge>? from2;
            private BoundaryVertexBuilder<TVertex, TEdge>? to2;
            public void SetFromToVertex(BoundaryVertexBuilder<TVertex, TEdge> from, BoundaryVertexBuilder<TVertex, TEdge> to)
            {
                // corresponding income and outcome edges are switched, this ensures correct coupling for future processing of vertex
                if (from1 is null)
                {
                    from1 = from;
                    to2 = to;
                }
                else if (from2 is null)
                {
                    from2 = from;
                    to1 = to;
                }
            }
            public void ProcessByDisconnecting()
            {
                if(from1 is null || from2 is null || to1 is null || to2 is null) return;
                // if some of the hold vertices are temporary, couple them with their corresponding counterparts  
                if (from1 is ChainEntanglementTemporaryVertexBuilder temporaryFrom1)
                {
                    if (temporaryFrom1.to1 == this)
                        temporaryFrom1.to1 = to1;
                    else
                        temporaryFrom1.to2 = to1;
                }
                if (to1 is ChainEntanglementTemporaryVertexBuilder temporaryTo1)
                {
                    if (temporaryTo1.from1 == this)
                        temporaryTo1.from1 = from1;
                    else
                        temporaryTo1.from2 = from1;
                }
                if (from2 is ChainEntanglementTemporaryVertexBuilder temporaryFrom2)
                {
                    if (temporaryFrom2.to1 == this)
                        temporaryFrom2.to1 = to2;
                    else
                        temporaryFrom2.to2 = to2;
                }
                if (to2 is ChainEntanglementTemporaryVertexBuilder temporaryTo2)
                {
                    if (temporaryTo2.from1 == this)
                        temporaryTo2.from1 = from2;    
                    else
                        temporaryTo2.from2 = from2;
                }
                // if both vertices are original chain vertices, couple them by edges
                if (from1 is not ChainEntanglementTemporaryVertexBuilder && to1 is not ChainEntanglementTemporaryVertexBuilder)
                {
                    from1.BoundaryEdges[to1] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                    to1.BoundaryEdges[from1] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                }
                if (from2 is not ChainEntanglementTemporaryVertexBuilder && to2 is not ChainEntanglementTemporaryVertexBuilder)
                {
                    from2.BoundaryEdges[to2] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                    to2.BoundaryEdges[from2] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                }
            }
            public override bool IsStationary { get; set; } = true;
        }
        
        #endregion
        
        #region 2 \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public static (Dictionary<(VertexBuilder<TVertex, TEdge>, VertexBuilder<TVertex, TEdge>), List<MapCoordinates>>, Dictionary<(BoundaryVertexBuilder<TVertex, TEdge>, BoundaryVertexBuilder<TVertex, TEdge>), (Orienteering_ISOM_2017_2.EdgeAttributes cutEdgeAttributes, List<(MapCoordinates, int, int)> crossSectionsWithChainsAndEdgesIndices)>)
            CutAllCrossedEdges(BoundaryVertexBuilder<TVertex, TEdge>[][] chains,
                IEditableRadiallySearchableDataStruct<VertexBuilder<TVertex, TEdge>> allVertices, int standardEdgeLength)
        {
           var (verticesOfCutNonBoundaryEdges, verticesOfCutBoundaryEdges) = Utils.FindAllCrossedEdges(chains, allVertices, standardEdgeLength, true);   
           Utils.CutAllFoundCrossedEdges(verticesOfCutNonBoundaryEdges, verticesOfCutBoundaryEdges);
           return (verticesOfCutNonBoundaryEdges, verticesOfCutBoundaryEdges);
        }
        
        #endregion
        
        #region 3 \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public static (List<VertexBuilder<TVertex, TEdge>> outerVerticesOfCutEdges, List<VertexBuilder<TVertex, TEdge>> innerVerticesOfCutEdges, List<VertexBuilder<TVertex, TEdge>> allInnerVertices) 
            FindNodesInsideThePolygonByDfs(Dictionary<(VertexBuilder<TVertex, TEdge>, VertexBuilder<TVertex, TEdge>), List<MapCoordinates>> verticesOfCutNonBoundaryEdges, Dictionary<(BoundaryVertexBuilder<TVertex, TEdge>, BoundaryVertexBuilder<TVertex, TEdge>), (Orienteering_ISOM_2017_2.EdgeAttributes, List<(MapCoordinates, int, int)>)> verticesOfCutBoundaryEdges, BoundaryVertexBuilder<TVertex, TEdge>[][] chains)
        {
            var outerVerticesOfCutEdges = new List<VertexBuilder<TVertex, TEdge>>();
            var innerVerticesOfCutEdges = new List<VertexBuilder<TVertex, TEdge>>();
            var allInnerVertices = new List<VertexBuilder<TVertex, TEdge>>();
            
            Dictionary<VertexBuilder<TVertex, TEdge>, List<(int edgeCutsCount, VertexBuilder<TVertex, TEdge> neighbor)>> verticesOfCutEdgesWithNeighbors = new ();
            // refactor dictionaries indexed by edges to dictionaries indexed by vertices
            foreach (var ((vertex1, vertex2), crossSections) in verticesOfCutNonBoundaryEdges)
            {
                if(verticesOfCutEdgesWithNeighbors.ContainsKey(vertex1)) verticesOfCutEdgesWithNeighbors[vertex1].Add((crossSections.Count, vertex2));
                else verticesOfCutEdgesWithNeighbors[vertex1] = [(crossSections.Count, vertex2)];
                if(verticesOfCutEdgesWithNeighbors.ContainsKey(vertex2)) verticesOfCutEdgesWithNeighbors[vertex2].Add((crossSections.Count, vertex1));
                else verticesOfCutEdgesWithNeighbors[vertex2] = [(crossSections.Count, vertex1)];
            }
            foreach (var ((vertex1, vertex2), (_, crossedChainEdgesIndices)) in verticesOfCutBoundaryEdges)
            {
                if(verticesOfCutEdgesWithNeighbors.ContainsKey(vertex1)) verticesOfCutEdgesWithNeighbors[vertex1].Add((crossedChainEdgesIndices.Count, vertex2));
                else verticesOfCutEdgesWithNeighbors[vertex1] = [(crossedChainEdgesIndices.Count, vertex2)];
                if(verticesOfCutEdgesWithNeighbors.ContainsKey(vertex2)) verticesOfCutEdgesWithNeighbors[vertex2].Add((crossedChainEdgesIndices.Count, vertex1));
                else verticesOfCutEdgesWithNeighbors[vertex2] = [(crossedChainEdgesIndices.Count, vertex1)];
            }
            // while there are some vertices, that were not included neither in outer nor inner vertices, repeat dfs process for their correct classification
            while(verticesOfCutEdgesWithNeighbors.Count > 0)
            {
                SortEdgesByDfs(verticesOfCutEdgesWithNeighbors,  outerVerticesOfCutEdges, innerVerticesOfCutEdges, allInnerVertices, chains);
            }
            return (outerVerticesOfCutEdges, innerVerticesOfCutEdges, allInnerVertices);
        }

        private static void SortEdgesByDfs(
            Dictionary<VertexBuilder<TVertex, TEdge>, List<(int edgeCutsCount, VertexBuilder<TVertex, TEdge> neighbor)>> verticesOfCutEdgesWithNeighbors,
            List<VertexBuilder<TVertex, TEdge>> outerVerticesOfCutEdges, List<VertexBuilder<TVertex, TEdge>> innerVerticesOfCutEdges, 
            List<VertexBuilder<TVertex, TEdge>> allInnerVertices, BoundaryVertexBuilder<TVertex, TEdge>[][] chains)
        {
            if (verticesOfCutEdgesWithNeighbors.Count > 0)
            {
                // selection of first inner vertex on which will dfs start its search
                var vertex = FindSomeInnerVertex(verticesOfCutEdgesWithNeighbors, chains);
                // if there is no such vertex, there can not be any vertex inside of polygon defined by chain
                if (vertex is null)
                {
                    outerVerticesOfCutEdges.AddRange(verticesOfCutEdgesWithNeighbors.Select(kv => kv.Key)); 
                    verticesOfCutEdgesWithNeighbors.Clear();
                    return;
                }
                Stack<VertexBuilder<TVertex, TEdge>> stack = new();
                HashSet<VertexBuilder<TVertex, TEdge>> visited = new HashSet<VertexBuilder<TVertex, TEdge>>();
                stack.Push(vertex);
                // dfs will not run out from polygon defined by chain, because edges from inner vertices to outer space were cut out by edges of chain
                int searchCount = 0;
                // while (stack.Count > 0 && (GraphCreator.Instance.processedObjectsCount != GraphCreator.Instance.debugLimit || searchCount < 7)) // for debuging
                while (stack.Count > 0)
                {
                    searchCount++;
                    vertex = stack.Pop();
                    if (visited.Contains(vertex)) continue;
                    allInnerVertices.Add(vertex);
                    visited.Add(vertex);
                    // if vertex is one of those, whose edges were cut, its classification is resolved by calling method IsInnerVertex on it
                    // then all neighboring vertices to this vertex, which shared cut edge with this vertex are classified and their cut-edge-neighbors are searched again recursively
                    if (verticesOfCutEdgesWithNeighbors.ContainsKey(vertex))
                        AreNeighborsOfVertexInnerAndProcessVertex_Recursive(vertex, true, verticesOfCutEdgesWithNeighbors, outerVerticesOfCutEdges, innerVerticesOfCutEdges, stack, visited);
                    
                    if (vertex is BoundaryVertexBuilder<TVertex, TEdge> boundaryVertex)
                    {
                        foreach (var (neighbor, _) in boundaryVertex.NonBoundaryEdges)
                            if (!visited.Contains(neighbor))
                                stack.Push(neighbor);
                        foreach (var (neighbor, _) in boundaryVertex.BoundaryEdges)
                            if (!visited.Contains(neighbor))
                                stack.Push(neighbor);
                    }
                    else if (vertex is NetVertexBuilder<TVertex, TEdge> netVertex)
                        foreach (var (neighbor,_) in netVertex.NonBoundaryEdges)
                            if (!visited.Contains(neighbor))
                                stack.Push(neighbor);

                }
            }
        }

        private static VertexBuilder<TVertex, TEdge>? FindSomeInnerVertex(Dictionary<VertexBuilder<TVertex, TEdge>, List<(int, VertexBuilder<TVertex, TEdge>)>> verticesOfCutEdgesWithNeighbors, BoundaryVertexBuilder<TVertex, TEdge>[][] chains)
        {
            foreach (var (vertex, _) in verticesOfCutEdgesWithNeighbors)
            {
                if (IsInnerVertex(vertex, chains)) return vertex;
            }
            return null;
        }

        private static void AreNeighborsOfVertexInnerAndProcessVertex_Recursive(VertexBuilder<TVertex, TEdge> vertex, bool isVertexInner,
            Dictionary<VertexBuilder<TVertex, TEdge>, List<(int edgeCutsCount, VertexBuilder<TVertex, TEdge> vertex)>> verticesOfCutEdgesWithNeighbors,
            List<VertexBuilder<TVertex, TEdge>> outerVerticesOfCutEdges, List<VertexBuilder<TVertex, TEdge>> innerVerticesOfCutEdges,
            Stack<VertexBuilder<TVertex, TEdge>> stack, HashSet<VertexBuilder<TVertex, TEdge>> visited)
        {
            // ends the recursion
            if (!verticesOfCutEdgesWithNeighbors.ContainsKey(vertex)) return;
            if (isVertexInner) 
            { 
                innerVerticesOfCutEdges.Add(vertex); 
                // if it is not yet in visited vertices, it is added to stack of ongoing dfs
                if(!visited.Contains(vertex))
                    stack.Push(vertex); 
            }
            else outerVerticesOfCutEdges.Add(vertex);
            
            
            var neighbors = verticesOfCutEdgesWithNeighbors[vertex];
            verticesOfCutEdgesWithNeighbors.Remove(vertex);
            // classify recursively neighbors of vertex
            foreach (var neighbor in neighbors)
            {
                // if count of crossings of edge between vertex and its neighbors is even, neighbor is of the same class as the vertex, if odd, it is of the oposit one
                if (neighbor.edgeCutsCount % 2 == 0) AreNeighborsOfVertexInnerAndProcessVertex_Recursive(neighbor.vertex, isVertexInner, verticesOfCutEdgesWithNeighbors, outerVerticesOfCutEdges, innerVerticesOfCutEdges, stack, visited);
                else AreNeighborsOfVertexInnerAndProcessVertex_Recursive(neighbor.vertex, !isVertexInner, verticesOfCutEdgesWithNeighbors, outerVerticesOfCutEdges, innerVerticesOfCutEdges, stack, visited);
            }
        }


        private static bool IsInnerVertex(VertexBuilder<TVertex, TEdge> vertex, BoundaryVertexBuilder<TVertex, TEdge>[] chain)
        {
            // if count of crossed chain edges by ray is odd, vertex, from which ray started is inside of polygon defined by chain
            int crossSectionsCount = 0;
            for (int i = 0; i < chain.Length - 1; i++)
                if (Utils.AreLineSegmentAndRayCrossingWithMagic(chain[i].Position, chain[i+1].Position, vertex.Position, vertex.Position + new MapCoordinates(1, 1), (chain[i - 1 >= 0 ? i  - 1 : chain.Length - 1].Position, chain[i + 2 <= chain.Length - 1 ? i + 2 : 0].Position)) ) crossSectionsCount++;
            if (Utils.AreLineSegmentAndRayCrossingWithMagic(chain.Last().Position, chain[0].Position, vertex.Position, vertex.Position + new MapCoordinates(1, 1), (chain[chain.Length - 2].Position, chain[1].Position)) ) crossSectionsCount++;
            return crossSectionsCount % 2 == 1;
        }

        private static bool IsInnerVertex(VertexBuilder<TVertex, TEdge> vertex, BoundaryVertexBuilder<TVertex, TEdge>[][] chains)
        {
            // if count of crossed chain edges by ray is odd, vertex, from which ray started is inside of multipolygon defined by chain
            int crossSectionsCount = 0;
            foreach (var chain in chains)
            {
                for (int i = 0; i < chain.Length - 1; i++)
                    if (Utils.AreLineSegmentAndRayCrossingWithMagic(chain[i].Position, chain[i + 1].Position, vertex.Position, vertex.Position + new MapCoordinates(1, 1), (chain[i - 1 >= 0 ? i - 1 : chain.Length - 1].Position, chain[i + 2 <= chain.Length - 1 ? i + 2 : 0].Position))) crossSectionsCount++;
                if (Utils.AreLineSegmentAndRayCrossingWithMagic(chain.Last().Position, chain[0].Position, vertex.Position, vertex.Position + new MapCoordinates(1, 1), (chain[chain.Length - 2].Position, chain[1].Position))) crossSectionsCount++;
            }
            return crossSectionsCount % 2 == 1;
        }
        
        #endregion
        
        #region 4 \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public static void UpdateAttributesOfInnerEdges(List<VertexBuilder<TVertex, TEdge>> innerVertices, decimal symbolCodeOfAddedObject)
        {
            // attributes of every edge of every inner vertex are updated according to symbol of object, that is inserted to the graph
            foreach (var vertex in innerVertices)
            {
                if (vertex is NetVertexBuilder<TVertex, TEdge> netVertex)
                    netVertex.Surroundings = Utils.GetUpdatedSurroundingsOfEdgeAttributes(netVertex.Surroundings, symbolCodeOfAddedObject);
                else if (vertex is BoundaryVertexBuilder<TVertex, TEdge> boundaryVertex)
                {
                    foreach (var (neighbor, oldEdgeAttributes) in boundaryVertex.NonBoundaryEdges)
                        boundaryVertex.NonBoundaryEdges[neighbor] = Utils.UpdateBothSurroundingsOfEdgeAttributes(oldEdgeAttributes, symbolCodeOfAddedObject);
                    foreach (var (neighbor, oldEdgeAttributes) in boundaryVertex.BoundaryEdges)
                        boundaryVertex.BoundaryEdges[neighbor] = Utils.UpdateBothSurroundingsOfEdgeAttributes(oldEdgeAttributes, symbolCodeOfAddedObject);
                }
            }
        }

        #endregion
        
        #region 5 \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public static BoundaryVertexBuilder<TVertex, TEdge>[][] ProcessCutBoundaryEdgesByChain(BoundaryVertexBuilder<TVertex, TEdge>[][] chains, 
            Dictionary<(BoundaryVertexBuilder<TVertex, TEdge>, BoundaryVertexBuilder<TVertex, TEdge>), (Orienteering_ISOM_2017_2.EdgeAttributes cutEdgeAttributes, List<(MapCoordinates, int, int)> crossSectionsWithChainsAndEdgesIndices)> verticesOfCutBoundaryEdges, 
            List<VertexBuilder<TVertex, TEdge>> outerVerticesOfCutEdges, decimal symbolCodeOfAddedObject)
        {
            // at first create vertex for every cross-section, then connect them correctly to the vertices of cut boundary edges and to the vertices of chain and in the end add the vertices to the chain
            var (chainVerticesOfCrossedEdgesWithTheirCrossSectionVerticesAndChainIndex, verticesOfCrossedBoundaryEdgesWithTheirCrossSectionVertices) = 
                Utils.CreateCrossSectionVertices(chains, verticesOfCutBoundaryEdges);
            ConnectNewVerticesToVerticesOfCutBoundaryEdges(verticesOfCrossedBoundaryEdgesWithTheirCrossSectionVertices, verticesOfCutBoundaryEdges, outerVerticesOfCutEdges, symbolCodeOfAddedObject);
            var listChains = chains.Select(chain => chain.ToList()).ToArray();
            Utils.ConnectAndAddNewVerticesToChain(chainVerticesOfCrossedEdgesWithTheirCrossSectionVerticesAndChainIndex, listChains);
            return listChains.Select(chain => chain.ToArray()).ToArray();
        }
        
        public static void ConnectNewVerticesToVerticesOfCutBoundaryEdges(Dictionary<(BoundaryVertexBuilder<TVertex, TEdge>, BoundaryVertexBuilder<TVertex, TEdge>), List<BoundaryVertexBuilder<TVertex, TEdge>>> verticesOfCrossedBoundaryEdgesWithTheirCrossSectionVertices, Dictionary<(BoundaryVertexBuilder<TVertex, TEdge>, BoundaryVertexBuilder<TVertex, TEdge>), (Orienteering_ISOM_2017_2.EdgeAttributes, List<(MapCoordinates, int, int)>)> verticesOfCutBoundaryEdges, List<VertexBuilder<TVertex, TEdge>> outerVerticesOfCutEdges, decimal symbolCodeOfAddedObject)
        {
            foreach(var ((vertex1, vertex2), newVertices) in verticesOfCrossedBoundaryEdgesWithTheirCrossSectionVertices)
            {
                // at first, whe have to sort cross-section vertices which corresponds to some cut boundary edge, so they could be correctly connected with vertices of this edge
                var newSortedVertices = Utils.SortVerticesCorrectly(vertex1, vertex2, newVertices);
                var outerEdgeAttributesFromV1ToV2 = verticesOfCutBoundaryEdges[(vertex1, vertex2)].Item1;
                var innerEdgeAttributesFromV1ToV2 = Utils.UpdateBothSurroundingsOfEdgeAttributes(outerEdgeAttributesFromV1ToV2, symbolCodeOfAddedObject);
                bool edgeToBeAddedIsOuter = outerVerticesOfCutEdges.Contains(vertex1);
                var lastVertex = vertex1;
                newSortedVertices.Add(vertex2);
                // we iteratively connect new vertices with each other and with vertices of cut boundary edge
                foreach (var newVertex in newSortedVertices)
                {
                    // we have to check if newly added vertex does not overlap with the one that is going to be connected with
                    // in case of overlap, last vertex is removed and is replaced by new vertex
                    // when new vertex overlap with one of the vertices of cut edge, the vertex of cut edge is removed and replaced by newly added vertex
                    BoundaryVertexBuilder<TVertex, TEdge>? overlappedVertex = newVertex.Position == lastVertex.Position ? lastVertex : null;
                    if (overlappedVertex is not null)
                    {
                        if (newVertex == vertex2) Utils.ReplaceBoundaryVertexForTheOtherOne(newVertex, overlappedVertex);
                        else Utils.ReplaceBoundaryVertexForTheOtherOne(overlappedVertex, newVertex);
                        edgeToBeAddedIsOuter = !edgeToBeAddedIsOuter;
                        lastVertex = newVertex;
                        continue;
                    }
                    // attributes of newly added edges between new vertex and last vertex are in rotation assigned inner and outer attributes
                    // this behaviour ensures correct attributes assigment to newly created edges
                    lastVertex.BoundaryEdges[newVertex] = edgeToBeAddedIsOuter ? outerEdgeAttributesFromV1ToV2 : innerEdgeAttributesFromV1ToV2;
                    newVertex.BoundaryEdges[lastVertex] = edgeToBeAddedIsOuter 
                        ? new Orienteering_ISOM_2017_2.EdgeAttributes(outerEdgeAttributesFromV1ToV2.RightSurroundings, outerEdgeAttributesFromV1ToV2.LeftSurroundings) 
                        : new Orienteering_ISOM_2017_2.EdgeAttributes(innerEdgeAttributesFromV1ToV2.RightSurroundings, innerEdgeAttributesFromV1ToV2.LeftSurroundings);
                    edgeToBeAddedIsOuter = !edgeToBeAddedIsOuter;
                    lastVertex = newVertex;
                }
            }
        }


        
        #endregion
        
        #region 6 \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public static void ConnectChainToVerticesOfCutEdgesAndOtherVerticesOfChain(BoundaryVertexBuilder<TVertex, TEdge>[][] chains, List<VertexBuilder<TVertex, TEdge>> outerVerticesOfCutEdges, List<VertexBuilder<TVertex, TEdge>> innerVerticesOfCutEdges, IEditableRadiallySearchableDataStruct<VertexBuilder<TVertex, TEdge>> allVertices, int standardEdgeLength)
            => Utils.ConnectChainVerticesToVerticesOfCutEdgesAndOtherVerticesOfChain(chains.SelectMany(chain => chain).ToArray(), outerVerticesOfCutEdges.Concat(innerVerticesOfCutEdges), allVertices, standardEdgeLength); 
        
        #endregion
        
        #region 7 \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public static void SetAttributesOfChainsEdges(BoundaryVertexBuilder<TVertex, TEdge>[][] chains, List<VertexBuilder<TVertex, TEdge>> outerVertices, decimal symbolCodeOfAddedObject)
        {
            // at first, we will obtain indices of new cross-section vertices added to chain, together with their closest outer neighbor angle-wise
            // these vertices will be used for determining of correct attribute setting of chains edges 
            var indicesAndOuterBoundaryNeighborsOfCrossSectionVertices = GetIndicesAndTheClosestOuterNeighborsOfVerticesOfCrossedBoundaryEdges(chains);
            for (int i = 0; i < indicesAndOuterBoundaryNeighborsOfCrossSectionVertices.Count; ++i)
            {
                // if count of cross-section vertices is less than 2, it means that all edges of the chain has surly the same attributes
                if (indicesAndOuterBoundaryNeighborsOfCrossSectionVertices[i].Count < 2) SetAttributesOfChainBoundaryEdgesUniformly(chains[i], outerVertices, symbolCodeOfAddedObject);
                else SetAttributesOfChainBoundaryEdgesNonUniformly(chains[i], indicesAndOuterBoundaryNeighborsOfCrossSectionVertices[i], symbolCodeOfAddedObject);
            }
        }
        
        private static List<List<(int, BoundaryVertexBuilder<TVertex, TEdge>)>> GetIndicesAndTheClosestOuterNeighborsOfVerticesOfCrossedBoundaryEdges(BoundaryVertexBuilder<TVertex, TEdge>[][] chains)
        {
            List<List<(int, BoundaryVertexBuilder<TVertex, TEdge>)>> indicesAndOuterNeighborsOfVerticesOfCrossedBoundaries = new();
            
            // we will iteratively determine the closest left-handed boundary neighbor of each vertex
            foreach(var chain in chains)
            {
                indicesAndOuterNeighborsOfVerticesOfCrossedBoundaries.Add(new List<(int, BoundaryVertexBuilder<TVertex, TEdge>)>());
                for (int j = 0; j < chain.Length; ++j)
                {
                    int iP = j == 0 ? chain.Length - 1 : j - 1;
                    int iS = j == chain.Length - 1 ? 0 : j + 1;
                    // closest left-handed neighbor is set at first to the previous chain vertex
                    // this will ensure, that found closest left-handed boundary vertex will be outer vertex  
                    var angleOfTheClosestNeighborToNextEdgeSoFar = Utils.ComputeLeftHandedAngleBetween( chain[j].Position, chain[iS].Position, chain[j].Position, chain[iP].Position);
                    var closestNeighbor = chain[iP];
                    foreach (var (neighbor, _) in chain[j].BoundaryEdges)
                    {
                        if (neighbor == chain[iS]) continue;
                        double angleOfTheNeighborToNextEdge = Utils.ComputeLeftHandedAngleBetween(chain[j].Position, chain[iS].Position, chain[j].Position, neighbor.Position);
                        if (angleOfTheNeighborToNextEdge < angleOfTheClosestNeighborToNextEdgeSoFar)
                        {
                            angleOfTheClosestNeighborToNextEdgeSoFar = angleOfTheNeighborToNextEdge;
                            closestNeighbor = neighbor;
                        }
                    }

                    // if the closest left-handed vertex is the previous chain vertex, current vertex is not added into final collection
                    if (closestNeighbor != chain[iP]) indicesAndOuterNeighborsOfVerticesOfCrossedBoundaries.Last().Add((j, closestNeighbor));
                }
            }
            return indicesAndOuterNeighborsOfVerticesOfCrossedBoundaries;
        }


        private static void SetAttributesOfChainBoundaryEdgesUniformly(BoundaryVertexBuilder<TVertex, TEdge>[] chain, List<VertexBuilder<TVertex, TEdge>> outerVertices, decimal symbolCodeOfAddedObject)
        {
            // attributes are determined from any outer net vertex
            var outerEdgeSurroundingsForWholeChain = TryFindEdgeAttributesOfSomeOuterNetVertexConnectedToChainInGivenInterval(chain, outerVertices, 0, chain.Length - 1) ?? (null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.Forest_405, null);
            Utils.SetAttributesOfChainBoundaryEdgesBasedOnGivenLeftSurroundingsOnGivenInterval(chain, outerEdgeSurroundingsForWholeChain, symbolCodeOfAddedObject, 0, chain.Length - 1);
            var innerSurroundings = Utils.GetUpdatedSurroundingsOfEdgeAttributes(outerEdgeSurroundingsForWholeChain, symbolCodeOfAddedObject);
            chain.Last().BoundaryEdges[chain[0]] = new Orienteering_ISOM_2017_2.EdgeAttributes(outerEdgeSurroundingsForWholeChain, innerSurroundings);
            chain[0].BoundaryEdges[chain.Last()] = new Orienteering_ISOM_2017_2.EdgeAttributes(innerSurroundings, outerEdgeSurroundingsForWholeChain);
        }

        private static void SetAttributesOfChainBoundaryEdgesNonUniformly(BoundaryVertexBuilder<TVertex, TEdge>[] chain,  List<(int index, BoundaryVertexBuilder<TVertex, TEdge> neighbor)> indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices, decimal symbolCodeOfAddedObject)
        {
            for (int i = 0; i < indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices.Count - 1; i++)
            {
                var outerEdgeSurroundingsForIntervalOfChain = chain[indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices[i].index].BoundaryEdges[indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices[i].neighbor].RightSurroundings;
                Utils.SetAttributesOfChainBoundaryEdgesBasedOnGivenLeftSurroundingsOnGivenInterval(chain, outerEdgeSurroundingsForIntervalOfChain, symbolCodeOfAddedObject, indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices[i].index, indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices[i+1].index);
            } 
            var outerEdgeSurroundingsForLastIntervalOfChain = chain[indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices.Last().index].BoundaryEdges[indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices.Last().neighbor].RightSurroundings;
            Utils.SetAttributesOfChainBoundaryEdgesBasedOnGivenLeftSurroundingsOnGivenInterval(chain, outerEdgeSurroundingsForLastIntervalOfChain, symbolCodeOfAddedObject, indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices.Last().index, chain.Length-1);
            Utils.SetAttributesOfChainBoundaryEdgesBasedOnGivenLeftSurroundingsOnGivenInterval(chain, outerEdgeSurroundingsForLastIntervalOfChain, symbolCodeOfAddedObject, 0, indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices[0].index);
            var innerSurroundings = Utils.GetUpdatedSurroundingsOfEdgeAttributes(outerEdgeSurroundingsForLastIntervalOfChain, symbolCodeOfAddedObject);
            chain.Last().BoundaryEdges[chain[0]] = new Orienteering_ISOM_2017_2.EdgeAttributes(outerEdgeSurroundingsForLastIntervalOfChain, innerSurroundings);
            chain[0].BoundaryEdges[chain.Last()] = new Orienteering_ISOM_2017_2.EdgeAttributes(innerSurroundings, outerEdgeSurroundingsForLastIntervalOfChain); 
        }

        private static(Orienteering_ISOM_2017_2.Grounds?, Orienteering_ISOM_2017_2.Boulders?, Orienteering_ISOM_2017_2.Stones?, Orienteering_ISOM_2017_2.Water?, Orienteering_ISOM_2017_2.VegetationAndManMade?, Orienteering_ISOM_2017_2.VegetationGoodVis?)? 
            TryFindEdgeAttributesOfSomeOuterNetVertexConnectedToChainInGivenInterval(BoundaryVertexBuilder<TVertex, TEdge>[] chain, List<VertexBuilder<TVertex, TEdge>> outerVertices, int startIndex, int endIndex)
        {
            if (startIndex >= endIndex || startIndex < 0 || endIndex >= chain.Length) return null;
            for(int i = startIndex; i <= endIndex; ++i)
                foreach (var (neighbor, edgeAttributes) in chain[i].NonBoundaryEdges)
                    if (neighbor is NetVertexBuilder<TVertex, TEdge> && outerVertices.Contains(neighbor))
                        return edgeAttributes.LeftSurroundings;
            return null;
        }
        
        #endregion

        #region 8 \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\ 
            public static void AddChainVerticesToTheGraph(BoundaryVertexBuilder<TVertex, TEdge>[][] chains, IEditableRadiallySearchableDataStruct<VertexBuilder<TVertex, TEdge>> allVertices)
                => Utils.AddChainVerticesToTheGraph(chains.SelectMany(chain => chain), allVertices);
        
        #endregion
        
        #region 9 \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public static void SetAttributesOfNonBoundaryEdgesBetweenChainAndBoundaryVertices(BoundaryVertexBuilder<TVertex, TEdge>[][] chains)
            => Utils.SetAttributesOfNonBoundaryEdgesBetweenChainVerticesAndBoundaryVertices(chains.SelectMany(chain => chain));

        #endregion 
        
    }

    private static class PathObjectsProcessing
    {
        #region 0 \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        
        public static List<BoundaryVertexBuilder<TVertex, TEdge>>? GetBlankBoundaryChain(OmapMap.Object obj, int standardEdgeLength, float minBoundaryEdgeRatio)
        {
            var segments = obj.CollectSegmentsForPath();
            var boundaryChain = Utils.GetBlankChainOfSegmentedLine(obj.TypedCoords[0].coords, segments, standardEdgeLength, minBoundaryEdgeRatio);
            if (boundaryChain.Count <= 1) return null;
            // it could happen that path has polygonal shape - it is closed segmented line
            // then we have to ensure correct cutting edges in first / last vertex of the chain
            // therefore we replace first vertex for the last one, and it will occur in chain two times - at last and first position
            if (boundaryChain[0].Position == boundaryChain.Last().Position)
            {
                boundaryChain.Last().BoundaryEdges[boundaryChain[1]] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                boundaryChain[1].BoundaryEdges[boundaryChain.Last()] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                boundaryChain[1].BoundaryEdges.Remove(boundaryChain[0]);
                boundaryChain.RemoveAt(0);
                boundaryChain.Insert(0, boundaryChain.Last());
            }
            if (boundaryChain.Count <= 1) return null;
            return boundaryChain;
        }
        
        #endregion
        
        #region 1 \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public static BoundaryVertexBuilder<TVertex, TEdge>[] AddCrossSectionVerticesIfItIsEntangled(List<BoundaryVertexBuilder<TVertex, TEdge>> potentiallyEntangledChain)
        {
            for (int i = 0; i < potentiallyEntangledChain.Count - 2; ++i)
                for (int j = i + 2; j < potentiallyEntangledChain.Count - 1; ++j)
                {
                    if (potentiallyEntangledChain[i] == potentiallyEntangledChain[j] || potentiallyEntangledChain[i] == potentiallyEntangledChain[j+1]) continue;
                    MapCoordinates? crossSectionCoords = Utils.GetLineSegmentsCrossSectionCoordsP2Q2Excluded(potentiallyEntangledChain[i].Position, potentiallyEntangledChain[i + 1].Position, potentiallyEntangledChain[j].Position, potentiallyEntangledChain[j + 1].Position)  ;
                    if (crossSectionCoords is not null)
                    {
                        var newVertex = new BoundaryVertexBuilder<TVertex, TEdge>(new Orienteering_ISOM_2017_2.VertexAttributes(crossSectionCoords.Value));
                        ConnectVertexToChain(newVertex, potentiallyEntangledChain, i, j);
                        AddVertexToChain(newVertex, potentiallyEntangledChain, ref i, ref j);
                    }
                }
            return potentiallyEntangledChain.ToArray();
        }


        private static void ConnectVertexToChain(BoundaryVertexBuilder<TVertex, TEdge> newVertex, List<BoundaryVertexBuilder<TVertex, TEdge>> chain, int firstEdgeIndex, int secondEdgeIndex)
        {
            chain[firstEdgeIndex].BoundaryEdges.Remove(chain[firstEdgeIndex+1]);
            chain[firstEdgeIndex+1].BoundaryEdges.Remove(chain[firstEdgeIndex]);
            chain[secondEdgeIndex].BoundaryEdges.Remove(chain[secondEdgeIndex+1]);
            chain[secondEdgeIndex+1].BoundaryEdges.Remove(chain[secondEdgeIndex]);
            
            chain[firstEdgeIndex].BoundaryEdges[newVertex] = new Orienteering_ISOM_2017_2.EdgeAttributes();
            newVertex.BoundaryEdges[chain[firstEdgeIndex]] = new Orienteering_ISOM_2017_2.EdgeAttributes();
            chain[firstEdgeIndex+1].BoundaryEdges[newVertex] = new Orienteering_ISOM_2017_2.EdgeAttributes();
            newVertex.BoundaryEdges[chain[firstEdgeIndex+1]] = new Orienteering_ISOM_2017_2.EdgeAttributes();
            
            chain[secondEdgeIndex].BoundaryEdges[newVertex] = new Orienteering_ISOM_2017_2.EdgeAttributes();
            newVertex.BoundaryEdges[chain[secondEdgeIndex]] = new Orienteering_ISOM_2017_2.EdgeAttributes();
            chain[secondEdgeIndex+1].BoundaryEdges[newVertex] = new Orienteering_ISOM_2017_2.EdgeAttributes();
            newVertex.BoundaryEdges[chain[secondEdgeIndex+1]] = new Orienteering_ISOM_2017_2.EdgeAttributes();
        }

        private static void AddVertexToChain(BoundaryVertexBuilder<TVertex, TEdge> newVertex, List<BoundaryVertexBuilder<TVertex, TEdge>> chain, ref int i, ref int j)
        {
            if(newVertex.Position == chain[i].Position)
                if (newVertex.Position == chain[j].Position)
                {
                    // vertex at position j at chain will never be cross-section vertex
                    chain[i].BoundaryEdges.Remove(newVertex);
                    newVertex.BoundaryEdges.Remove(chain[i]);
                    foreach (var (newVertexNeighbor, _) in newVertex.BoundaryEdges)
                    {
                        chain[i].BoundaryEdges[newVertexNeighbor] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                        newVertexNeighbor.BoundaryEdges[chain[i]] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                        newVertexNeighbor.BoundaryEdges.Remove(newVertex);
                    }
                    newVertex.BoundaryEdges.Clear();
                    foreach (var (vertexAtJInChainNeighbor, _) in chain[j].BoundaryEdges)
                    {
                        chain[i].BoundaryEdges[vertexAtJInChainNeighbor] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                        vertexAtJInChainNeighbor.BoundaryEdges[chain[i]] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                        vertexAtJInChainNeighbor.BoundaryEdges.Remove(chain[j]);
                    }
                    chain[j].BoundaryEdges.Clear();
                    chain.RemoveAt(j);
                    chain.Insert(j, chain[i]);
                }
                else if (newVertex.Position == chain[j + 1].Position)
                {
                    // vertex at position j + 1 in chain will never be cross-section vertex
                    chain[i].BoundaryEdges.Remove(newVertex);
                    newVertex.BoundaryEdges.Remove(chain[i]);
                    foreach (var (newVertexNeighbor, _) in newVertex.BoundaryEdges)
                    {
                        chain[i].BoundaryEdges[newVertexNeighbor] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                        newVertexNeighbor.BoundaryEdges[chain[i]] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                        newVertexNeighbor.BoundaryEdges.Remove(newVertex);
                    }
                    newVertex.BoundaryEdges.Clear();
                    foreach (var (vertexAtJPlusOneInChainNeighbor, _) in chain[j+1].BoundaryEdges)
                    {
                        chain[i].BoundaryEdges[vertexAtJPlusOneInChainNeighbor] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                        vertexAtJPlusOneInChainNeighbor.BoundaryEdges[chain[i]] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                        vertexAtJPlusOneInChainNeighbor.BoundaryEdges.Remove(chain[j+1]);
                    }
                    chain[j+1].BoundaryEdges.Clear();
                    chain.RemoveAt(j+1);
                    chain.Insert(++j, chain[i]);
                }
                else
                {
                    chain[i].BoundaryEdges.Remove(newVertex);
                    newVertex.BoundaryEdges.Remove(chain[i]);
                    foreach (var (newVertexNeighbor, _) in newVertex.BoundaryEdges)
                    {
                        chain[i].BoundaryEdges[newVertexNeighbor] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                        newVertexNeighbor.BoundaryEdges[chain[i]] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                        newVertexNeighbor.BoundaryEdges.Remove(newVertex);
                    }
                    newVertex.BoundaryEdges.Clear();
                    chain.Insert(++j, chain[i]);
                }
            else if (newVertex.Position == chain[j].Position)
            {
                // vertex at position j in chain will never be cross-section vertex
                chain[j].BoundaryEdges.Remove(newVertex);
                newVertex.BoundaryEdges.Remove(chain[j]);
                foreach (var (newVertexNeighbor, _) in newVertex.BoundaryEdges)
                {
                    chain[j].BoundaryEdges[newVertexNeighbor] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                    newVertexNeighbor.BoundaryEdges[chain[j]] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                    newVertexNeighbor.BoundaryEdges.Remove(newVertex);
                }
                newVertex.BoundaryEdges.Clear();
                chain.Insert(i + 1, chain[j]);
            }
            else if (newVertex.Position == chain[j + 1].Position)
            {
                // vertex at position j + 1 in chain will never be cross-section vertex
                chain[j+1].BoundaryEdges.Remove(newVertex);
                newVertex.BoundaryEdges.Remove(chain[j+1]);
                foreach (var (newVertexNeighbor, _) in newVertex.BoundaryEdges)
                {
                    chain[j+1].BoundaryEdges[newVertexNeighbor] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                    newVertexNeighbor.BoundaryEdges[chain[j+1]] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                    newVertexNeighbor.BoundaryEdges.Remove(newVertex);
                }
                newVertex.BoundaryEdges.Clear();
                chain.Insert(i+1, chain[j+1]);
                ++j;
            }
            else
            {
                chain.Insert(i + 1, newVertex);
                ++j;
                chain.Insert(++j, newVertex);
            }
        }
        
        #endregion
        
        #region 2 \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public static (Dictionary<(VertexBuilder<TVertex, TEdge>, VertexBuilder<TVertex, TEdge>), List<MapCoordinates>>, Dictionary<(BoundaryVertexBuilder<TVertex, TEdge>, BoundaryVertexBuilder<TVertex, TEdge>), (Orienteering_ISOM_2017_2.EdgeAttributes cutEdgeAttributes, List<(MapCoordinates coords, int chainIndex, int edgeIndex)> crossSectionsWithChainsAndEdgesIndices)>)
            CutAllCrossedEdges(BoundaryVertexBuilder<TVertex, TEdge>[] potentiallyMultiOccuringVerticesChain,
            IEditableRadiallySearchableDataStruct<VertexBuilder<TVertex, TEdge>> allVertices, int standardEdgeLength)
        {
           var (verticesOfCrossedNonBoundaryEdges, verticesOfCrossedBoundaryEdges) = Utils.FindAllCrossedEdges([potentiallyMultiOccuringVerticesChain], allVertices, standardEdgeLength, false);   
           Utils.CutAllFoundCrossedEdges(verticesOfCrossedNonBoundaryEdges, verticesOfCrossedBoundaryEdges);
           return (verticesOfCrossedNonBoundaryEdges, verticesOfCrossedBoundaryEdges);
        }

        #endregion
        
        #region 3 \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public static BoundaryVertexBuilder<TVertex, TEdge>[] ProcessCutBoundaryEdgesByChain(BoundaryVertexBuilder<TVertex, TEdge>[] potentiallyMultiOccuringVerticesChain, Dictionary<(BoundaryVertexBuilder<TVertex, TEdge>, BoundaryVertexBuilder<TVertex, TEdge>), (Orienteering_ISOM_2017_2.EdgeAttributes, List<(MapCoordinates, int, int)>)> verticesOfCutBoundaryEdges)
        {
            // at first create vertex for every cross-section, then connect them correctly to the vertices of cut boundary edges and to the vertices of chain and in the end add the vertices to the chain
            var listChain = potentiallyMultiOccuringVerticesChain.ToList();
            var (chainVerticesOfCrossedEdgesWithTheirCrossSectionVertices, verticesOfCrossedBoundaryEdgesWithTheirCrossSectionVertices) = 
                Utils.CreateCrossSectionVertices([potentiallyMultiOccuringVerticesChain], verticesOfCutBoundaryEdges);
            ConnectNewVerticesToVerticesOfCutBoundaryEdges(verticesOfCrossedBoundaryEdgesWithTheirCrossSectionVertices, verticesOfCutBoundaryEdges);
            Utils.ConnectAndAddNewVerticesToChain(chainVerticesOfCrossedEdgesWithTheirCrossSectionVertices, [listChain]);
            return listChain.ToArray();
            
        }
        
        public static void ConnectNewVerticesToVerticesOfCutBoundaryEdges(Dictionary<(BoundaryVertexBuilder<TVertex, TEdge>, BoundaryVertexBuilder<TVertex, TEdge>), List<BoundaryVertexBuilder<TVertex, TEdge>>> verticesOfCrossedBoundaryEdgesWithTheirCrossSectionVertices, Dictionary<(BoundaryVertexBuilder<TVertex, TEdge>, BoundaryVertexBuilder<TVertex, TEdge>), (Orienteering_ISOM_2017_2.EdgeAttributes, List<(MapCoordinates, int, int)>)> verticesOfCutBoundaryEdges)
        {
            foreach(var ((vertex1, vertex2), newVertices) in verticesOfCrossedBoundaryEdgesWithTheirCrossSectionVertices)
            {
                // at first, we have to sort cross-section vertices which corresponds to some cut boundary edge, so they could be correctly connected with vertices of this edge
                var newSortedVertices = Utils.SortVerticesCorrectly(vertex1, vertex2, newVertices);
                var edgeAttributesFromV1ToV2 = verticesOfCutBoundaryEdges[(vertex1, vertex2)].Item1;
                var lastVertex = vertex1;
                newSortedVertices.Add(vertex2);
                // we iteratively connect new vertices with each other and with vertices of cut boundary edge
                foreach (var newVertex in newSortedVertices)
                {
                    // we have to check if newly added vertex does not overlap with the one that is going to be connected with
                    // in case of overlap, last vertex is removed and is replaced by new vertex
                    // when new vertex overlap with one of the vertices of cut edge, the vertex of cut edge is removed and replaced by newly added vertex
                    BoundaryVertexBuilder<TVertex, TEdge>? overlappedVertex = newVertex.Position == lastVertex.Position ? lastVertex : null;
                    if (overlappedVertex is not null)
                    {
                        if (newVertex == vertex2) Utils.ReplaceBoundaryVertexForTheOtherOne(newVertex, overlappedVertex);
                        else Utils.ReplaceBoundaryVertexForTheOtherOne(overlappedVertex, newVertex);
                        lastVertex = newVertex;
                        continue;
                    }
                    lastVertex.BoundaryEdges[newVertex] =  edgeAttributesFromV1ToV2;
                    newVertex.BoundaryEdges[lastVertex] = new Orienteering_ISOM_2017_2.EdgeAttributes(edgeAttributesFromV1ToV2.RightSurroundings, edgeAttributesFromV1ToV2.LeftSurroundings); 
                    lastVertex = newVertex;
                }
            }
        }
        
        #endregion
        
        #region 4 \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public static void ConnectChainToVerticesOfCutEdgesAndOtherVerticesOfChain(BoundaryVertexBuilder<TVertex, TEdge>[] chain, Dictionary<(VertexBuilder<TVertex, TEdge>, VertexBuilder<TVertex, TEdge>), List<MapCoordinates>> verticesOfCutNonBoundaryEdges, Dictionary<(BoundaryVertexBuilder<TVertex, TEdge>, BoundaryVertexBuilder<TVertex, TEdge>), (Orienteering_ISOM_2017_2.EdgeAttributes, List<(MapCoordinates, int, int)>)> verticesOfCutBoundaryEdges, IEditableRadiallySearchableDataStruct<VertexBuilder<TVertex, TEdge>> allVertices, int standardEdgeLength)
        {
            HashSet<VertexBuilder<TVertex, TEdge>> verticesOfCutEdges = new();
            foreach (var ((vertex1, vertex2), _) in verticesOfCutNonBoundaryEdges) { verticesOfCutEdges.Add(vertex1); verticesOfCutEdges.Add(vertex2); }
            foreach (var ((vertex1, vertex2), _) in verticesOfCutBoundaryEdges) { verticesOfCutEdges.Add(vertex1); verticesOfCutEdges.Add(vertex2); }
            Utils.ConnectChainVerticesToVerticesOfCutEdgesAndOtherVerticesOfChain(chain, verticesOfCutEdges, allVertices, standardEdgeLength);
        }
        
        #endregion
        
        #region 5 \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        
        public static void SetAttributesOfChainsEdges(BoundaryVertexBuilder<TVertex, TEdge>[] chain, decimal symbolCodeOfAddedObject)
        {
            // at first, we will obtain indices of new cross-section vertices added to chain, together with their closest outer neighbor angle-wise
            // these vertices will be used for determining of correct attribute setting of chains edges 
            var (indicesAndLeftHandedBoundaryNeighborsOfCrossSectionVertices, leftHandedNeighborOfFirstCrossSectionVertexInOtherDirection) = GetIndicesAndTheClosestLeftHandedNeighborsOfVerticesOfCrossedBoundaryEdges(chain);
            // if count of cross-section vertices is less than 2, it means that all edges of the chain has surly the same attributes
            if (indicesAndLeftHandedBoundaryNeighborsOfCrossSectionVertices.Count < 2 )
                SetAttributesOfChainBoundaryEdgesUniformly(chain, symbolCodeOfAddedObject);
            else
                SetAttributesOfChainBoundaryEdgesNonUniformly(chain, indicesAndLeftHandedBoundaryNeighborsOfCrossSectionVertices, leftHandedNeighborOfFirstCrossSectionVertexInOtherDirection!, symbolCodeOfAddedObject);
        }
        
        private static (List<(int, BoundaryVertexBuilder<TVertex, TEdge>)>, BoundaryVertexBuilder<TVertex, TEdge>?) GetIndicesAndTheClosestLeftHandedNeighborsOfVerticesOfCrossedBoundaryEdges(BoundaryVertexBuilder<TVertex, TEdge>[] chain)
        {
            List<(int, BoundaryVertexBuilder<TVertex, TEdge>)> indicesAndOuterNeighborsOfVerticesOfCrossedBoundaries = new();
            
            // we will iteratively determine the closest left-handed boundary neighbor of each vertex
            for (int i = 1; i < chain.Length - 1; i++)
            {
                // closest left-handed neighbor is set at first to the previous chain vertex
                // this will ensure, that found closest left-handed boundary vertex will be outer vertex  
                var angleOfTheClosestNeighborToNextEdgeSoFar = Utils.ComputeLeftHandedAngleBetween(chain[i].Position, chain[i + 1].Position, chain[i].Position, chain[i - 1].Position);
                var closestNeighbor = chain[i - 1];
                foreach (var (neighbor, _) in chain[i].BoundaryEdges)
                {
                    if (neighbor == chain[i + 1]) continue;
                    double angleOfTheNeighborToNextEdge = Utils.ComputeLeftHandedAngleBetween(chain[i].Position, chain[i + 1].Position, chain[i].Position, neighbor.Position);
                    if (angleOfTheNeighborToNextEdge < angleOfTheClosestNeighborToNextEdgeSoFar)
                    {
                        angleOfTheClosestNeighborToNextEdgeSoFar = angleOfTheNeighborToNextEdge;
                        closestNeighbor = neighbor;
                    }
                }
                // if the closest left-handed vertex is the previous chain vertex, current vertex is not added into final collection
                if(closestNeighbor != chain[i - 1]) indicesAndOuterNeighborsOfVerticesOfCrossedBoundaries.Add((i, closestNeighbor));
            }
            if (indicesAndOuterNeighborsOfVerticesOfCrossedBoundaries.Count > 0)
            {
                // we have to find out, what attributes will have first interval of the chain
                // we will determine these attributes by finding the closest left-handed neighbor in opposite direction of first vertex of crossed boundaries, which was found in previous cycle
                var firstIndex = indicesAndOuterNeighborsOfVerticesOfCrossedBoundaries.First().Item1;
                var angleOfTheClosestNeighborToNextEdgeSoFar = Utils.ComputeLeftHandedAngleBetween(chain[firstIndex].Position, chain[firstIndex - 1].Position, chain[firstIndex].Position, chain[firstIndex + 1].Position);
                var closestNeighbor = chain[firstIndex + 1];
                foreach (var (neighbor, _) in chain[firstIndex].BoundaryEdges)
                {
                    if (neighbor == chain[firstIndex - 1]) continue;
                    double angleOfTheNeighborToNextEdge = Utils.ComputeLeftHandedAngleBetween(chain[firstIndex].Position, chain[firstIndex - 1].Position, chain[firstIndex].Position, neighbor.Position);
                    if (angleOfTheNeighborToNextEdge < angleOfTheClosestNeighborToNextEdgeSoFar)
                    {
                        angleOfTheClosestNeighborToNextEdgeSoFar = angleOfTheNeighborToNextEdge;
                        closestNeighbor = neighbor;
                    }
                }
                // we can see that we will leave the closest neighbor be also a next vertex from the chain, because
                // if no other closer boundary vertex is found, it means that next vertex from the chain will hold 
                // desired attributes
                return (indicesAndOuterNeighborsOfVerticesOfCrossedBoundaries, closestNeighbor);
            }
            return (indicesAndOuterNeighborsOfVerticesOfCrossedBoundaries, null);
        }
        
        private static void SetAttributesOfChainBoundaryEdgesUniformly(BoundaryVertexBuilder<TVertex, TEdge>[] chain, decimal symbolCodeOfAddedObject)
        {
            // attributes are determined from any net vertex
            var outerEdgeSurroundingsForWholeChain = TryFindEdgeAttributesOfSomeNetVertexConnectedToChainInGivenInterval(chain, 0, chain.Length - 1) ?? (null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.Forest_405, null);
            Utils.SetAttributesOfChainBoundaryEdgesBasedOnGivenLeftSurroundingsOnGivenInterval(chain, outerEdgeSurroundingsForWholeChain, symbolCodeOfAddedObject, 0, chain.Length - 1);
        }
        
        private static void SetAttributesOfChainBoundaryEdgesNonUniformly(BoundaryVertexBuilder<TVertex, TEdge>[] chain,  List<(int index, BoundaryVertexBuilder<TVertex, TEdge> neighbor)> indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices, BoundaryVertexBuilder<TVertex, TEdge>  leftHandedNeigborOfFirstCrossSectionVertexInOtherDirection, decimal symbolCodeOfAddedObject)
        {
            for (int i = 0; i < indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices.Count - 1; i++)
            {
                var outerEdgeSurroundingsForIntervalOfChain = chain[indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices[i].index].BoundaryEdges[indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices[i].neighbor].RightSurroundings;
                Utils.SetAttributesOfChainBoundaryEdgesBasedOnGivenLeftSurroundingsOnGivenInterval(chain, outerEdgeSurroundingsForIntervalOfChain, symbolCodeOfAddedObject, indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices[i].index, indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices[i+1].index);
            } 
            var outerEdgeSurroundingsForFirstIntervalOfChain = chain[indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices.First().index].BoundaryEdges[leftHandedNeigborOfFirstCrossSectionVertexInOtherDirection].RightSurroundings;
            // we can use underling method directly with chain even tough provided edge surroundings is intentioned for 
            // setting edges of chain in opposite direction - we can do this thanks to the assumption that
            // both surroundings in edge attributes of path chain are the same so there is no difference between left
            // and right side of the edge
            Utils.SetAttributesOfChainBoundaryEdgesBasedOnGivenLeftSurroundingsOnGivenInterval(chain, outerEdgeSurroundingsForFirstIntervalOfChain, symbolCodeOfAddedObject, 0, indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices.First().index);
        }
        
        private static(Orienteering_ISOM_2017_2.Grounds?, Orienteering_ISOM_2017_2.Boulders?, Orienteering_ISOM_2017_2.Stones?, Orienteering_ISOM_2017_2.Water?, Orienteering_ISOM_2017_2.VegetationAndManMade?, Orienteering_ISOM_2017_2.VegetationGoodVis?)? 
            TryFindEdgeAttributesOfSomeNetVertexConnectedToChainInGivenInterval(BoundaryVertexBuilder<TVertex, TEdge>[] chain, int startIndex, int endIndex)
        {
            if (startIndex >= endIndex || startIndex < 0 || endIndex >= chain.Length) return null;
            for(int i = startIndex; i <= endIndex; ++i)
                foreach (var (neighbor, edgeAttributes) in chain[i].NonBoundaryEdges)
                    if (neighbor is NetVertexBuilder<TVertex, TEdge>)
                        return edgeAttributes.LeftSurroundings;
            return null;
        }
        
        #endregion

        #region 6 \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public static void AddChainVerticesToTheGraph(BoundaryVertexBuilder<TVertex, TEdge>[] potentiallyMultiOccuringVerticesChain,
            IEditableRadiallySearchableDataStruct<VertexBuilder<TVertex, TEdge>> allVertices)
        {
            var chain = potentiallyMultiOccuringVerticesChain.ToHashSet();
            Utils.AddChainVerticesToTheGraph(chain, allVertices);
        }
        #endregion
        
        #region 7 \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public static void SetAttributesOfNonBoundaryEdgesBetweenChainAndBoundaryVertices(BoundaryVertexBuilder<TVertex, TEdge>[] chain)
            => Utils.SetAttributesOfNonBoundaryEdgesBetweenChainVerticesAndBoundaryVertices(chain);

        #endregion
    }

    private static class LinearObstaclesProcessing
    {
        #region 0 \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        
        public static BoundaryVertexBuilder<TVertex, TEdge>[]? GetBlankBoundaryChain(OmapMap.Object obj, int standardEdgeLength,
            float minBoundaryEdgeRatio)
        {
            var segments = obj.CollectSegmentsForPath();
            var boundaryChain = Utils.GetBlankChainOfSegmentedLine(obj.TypedCoords[0].coords, segments, standardEdgeLength, minBoundaryEdgeRatio);
            if (boundaryChain.Count <= 1) return null;
            // it could happen that linear obstacle has polygonal shape - it is closed segmented line
            // then we have to ensure correct cutting edges in first / last vertex of the chain
            // therefore we replace first vertex for last one, and it will occur in chain two times - at last and first position
            if (boundaryChain[0].Position == boundaryChain.Last().Position)
            {
                boundaryChain.Last().BoundaryEdges[boundaryChain[1]] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                boundaryChain[1].BoundaryEdges[boundaryChain.Last()] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                boundaryChain[1].BoundaryEdges.Remove(boundaryChain[0]);
                boundaryChain.RemoveAt(0);
                // if linear obstacle is around some polygon, it is better, when crossed edges by this obstacle will be the same one as edges crossed by chain of that polygon
                // so we need to ensure that "inner" side of the linear obstacle is the right one
                if (!Utils.IsRightSideInner(boundaryChain[0], boundaryChain.Last().Position, boundaryChain[1].Position, boundaryChain.ToArray()))
                { boundaryChain.Reverse(); boundaryChain.Add(boundaryChain[0]); }
                else boundaryChain.Insert(0, boundaryChain.Last());
                
            }
            if (boundaryChain.Count <= 1) return null;
            return boundaryChain.ToArray();
        }
        
        #endregion
        
        #region 1 \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        
        public static (Dictionary<(VertexBuilder<TVertex, TEdge>, VertexBuilder<TVertex, TEdge>), List<MapCoordinates>>, Dictionary<(BoundaryVertexBuilder<TVertex, TEdge>, BoundaryVertexBuilder<TVertex, TEdge>), (Orienteering_ISOM_2017_2.EdgeAttributes cutEdgeAttributes, List<(MapCoordinates, int, int)> crossSectionsWithChainsAndEdgesIndices)>)
            
            FindAllCrossedEdges(BoundaryVertexBuilder<TVertex, TEdge>[] chain, IEditableRadiallySearchableDataStruct<VertexBuilder<TVertex, TEdge>> allVertices, int standardEdgeLength) 
            => Utils.FindAllCrossedEdges([chain], allVertices, standardEdgeLength, false);
        
        #endregion
        
        #region 2 \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public static void SetLinearObstacleAttributesToAllCrossedEdges(Dictionary<(VertexBuilder<TVertex, TEdge>, VertexBuilder<TVertex, TEdge>), List<MapCoordinates>> verticesOfCrossedNonBoundaryEdges, Dictionary<(BoundaryVertexBuilder<TVertex, TEdge>, BoundaryVertexBuilder<TVertex, TEdge>), (Orienteering_ISOM_2017_2.EdgeAttributes cutEdgeAttributes, List<(MapCoordinates, int, int)> crossSectionsWithChainsAndEdgesIndices)> verticesOfCrossedBoundaryEdges, decimal symbolCodeOfAddedObject)
        {
            foreach (var ((vertex1, vertex2), _) in verticesOfCrossedNonBoundaryEdges)
            {
                if (vertex1 is BoundaryVertexBuilder<TVertex, TEdge> boundaryVertex1)
                {
                    boundaryVertex1.NonBoundaryEdges[vertex2] = Utils.UpdateLinearFeaturesOfEdgeAttributes(boundaryVertex1.NonBoundaryEdges[vertex2], symbolCodeOfAddedObject);
                    if (vertex2 is BoundaryVertexBuilder<TVertex, TEdge> boundaryVertex2)
                        boundaryVertex2.NonBoundaryEdges[boundaryVertex1] = Utils.UpdateLinearFeaturesOfEdgeAttributes(boundaryVertex2.NonBoundaryEdges[boundaryVertex1], symbolCodeOfAddedObject);
                    else if (vertex2 is NetVertexBuilder<TVertex, TEdge> netVertex2)
                        netVertex2.NonBoundaryEdges[boundaryVertex1] = Utils.GetUpdatedLinearFeaturesOfEdgeAttributes(netVertex2.NonBoundaryEdges[boundaryVertex1], netVertex2.Surroundings, netVertex2.Surroundings, symbolCodeOfAddedObject, out _, out _);
                }
                else if (vertex1 is NetVertexBuilder<TVertex, TEdge> netVertex1)
                {
                    netVertex1.NonBoundaryEdges[vertex2] = Utils.GetUpdatedLinearFeaturesOfEdgeAttributes(netVertex1.NonBoundaryEdges[vertex2], netVertex1.Surroundings, netVertex1.Surroundings, symbolCodeOfAddedObject, out _, out _);
                    if (vertex2 is BoundaryVertexBuilder<TVertex, TEdge> boundaryVertex2)
                        boundaryVertex2.NonBoundaryEdges[netVertex1] = Utils.UpdateLinearFeaturesOfEdgeAttributes(boundaryVertex2.NonBoundaryEdges[netVertex1], symbolCodeOfAddedObject);
                    else if (vertex2 is NetVertexBuilder<TVertex, TEdge> netVertex2)
                        netVertex2.NonBoundaryEdges[netVertex1] = Utils.GetUpdatedLinearFeaturesOfEdgeAttributes(netVertex2.NonBoundaryEdges[netVertex1], netVertex2.Surroundings, netVertex2.Surroundings, symbolCodeOfAddedObject, out _, out _);
                }
            }

            foreach (var ((vertex1, vertex2), _) in verticesOfCrossedBoundaryEdges)
            {
                vertex1.BoundaryEdges[vertex2] = Utils.UpdateLinearFeaturesOfEdgeAttributes(vertex1.BoundaryEdges[vertex2], symbolCodeOfAddedObject);
                vertex2.BoundaryEdges[vertex1] = Utils.UpdateLinearFeaturesOfEdgeAttributes(vertex2.BoundaryEdges[vertex1], symbolCodeOfAddedObject);
            }
        }
        
        #endregion
    }
    
    private static class Utils
    {
        #region Blank chain or shifted blank chain of segmented line retrieval \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        
        public const int microscopicEdgeLength = 100;
        public static List<BoundaryVertexBuilder<TVertex, TEdge>> GetBlankChainOfSegmentedLine(MapCoordinates firstPosition, IList<Segment> segments, int standardEdgeLength, float minBoundaryEdgeRatio)
        {
            List<BoundaryVertexBuilder<TVertex, TEdge>> chain = [new (new Orienteering_ISOM_2017_2.VertexAttributes(firstPosition))];
            // incrementally adds new vertices to chain for each segment of objects boundary 
            foreach (var segment in segments)
            {
                AddBlankChainForSegment(segment, chain, standardEdgeLength, minBoundaryEdgeRatio);
            }
            // RemoveMicroscopicEdges(chain);
            return chain;
        }


        private static void AddBlankChainForSegment(Segment segment, List<BoundaryVertexBuilder<TVertex, TEdge>> chain, int standardEdgeLength, float minBoundaryEdgeRatio)
        {
            // if segment is too small, following method will process it
            if (HandleSmallSegment(segment, chain, standardEdgeLength, minBoundaryEdgeRatio)) return;
            // remembering segments Point0
            MapCoordinates point0 = chain.Last().Position;
            // discovers count of edges to which should be segment split, so that all edges had approximately same length not bigger than standard edge length
            // edges will not have the same length
            // with increasing edge count, the variance of length of all edges will be still smaller and smaller
            int parametersCount = 5;
            // var parametrization = GetParametrizationWithUniformDistancesByGradientDescent(segment, point0, parametersCount);
            var parametrization = GetUniformParametrization(parametersCount);
            while ((segment.PositionAt(parametrization[1], point0) - point0).Length() > standardEdgeLength ||
                   (segment.PositionAt(parametrization[parametersCount / 2], point0) - segment.PositionAt(parametrization[parametersCount / 2 - 1], point0)).Length() > standardEdgeLength ||
                   (segment.PositionAt(parametrization[parametersCount - 1], point0) - segment.PositionAt(parametrization[parametersCount - 2], point0)).Length() > standardEdgeLength)
                // parametrization = GetParametrizationWithUniformDistancesByGradientDescent(segment, point0, ++parametersCount);
                parametrization = GetUniformParametrization(++parametersCount);

            // iteratively creating edges from uniformly chosen parts of segment
            for (int i = 1; i <= parametersCount - 1; ++i)
            {
                BoundaryVertexBuilder<TVertex, TEdge> newVertex = new( new Orienteering_ISOM_2017_2.VertexAttributes(segment.PositionAt(parametrization[i], point0)));
                newVertex.BoundaryEdges[chain.Last()] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                chain.Last().BoundaryEdges[newVertex] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                chain.Add(newVertex);
            }
        }

        private static bool HandleSmallSegment(Segment segment, List<BoundaryVertexBuilder<TVertex, TEdge>> chain, int standardEdgeLength, float minBoundaryEdgeRatio)
        {
            // Tries to fit small segment by as many edges as it can.
            // If more than 3 edges can be fit into the segment, return false indicating, that segment is not small one.
            if (TryGetNNewVerticesForSmallSegment(segment, 4, chain.Last().Position, standardEdgeLength, minBoundaryEdgeRatio, out var newVertices))
                return false;
            if (TryGetNNewVerticesForSmallSegment(segment, 3, chain.Last().Position, standardEdgeLength, minBoundaryEdgeRatio, out newVertices)) { }
            else if (TryGetNNewVerticesForSmallSegment(segment, 2, chain.Last().Position, standardEdgeLength, minBoundaryEdgeRatio, out newVertices)) { }
            else if (TryGetNNewVerticesForSmallSegment(segment, 1, chain.Last().Position, standardEdgeLength, minBoundaryEdgeRatio, out newVertices)) { }
            else return true;
            
            // if there is a distance between pair of vertices bigger than standard edge length, this segment can not be handled as small one, because this long edge could destroy edge cutting in following processing of object
            // although there could be some very small edges created, they are not as big problem as very long edges
            if ((newVertices[0].Position - chain.Last().Position).Length() > standardEdgeLength) return false;
            for(int i = 1; i <= newVertices.Count-1; ++i) if ((newVertices[i].Position - newVertices[i-1].Position).Length() > standardEdgeLength) return false; 
            
            foreach (var newVertex in newVertices)
            {
                newVertex.BoundaryEdges[chain.Last()] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                chain.Last().BoundaryEdges[newVertex] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                chain.Add(newVertex);
            }
            return true;
        }
        private static bool TryGetNNewVerticesForSmallSegment(Segment segment, int n, MapCoordinates point0,
            int standardEdgeLength, float minBoundaryEdgeRatio, out List<BoundaryVertexBuilder<TVertex, TEdge>> newVertices)
        {
            // double[] chordLengthParametrization = GetParametrizationWithUniformDistancesByGradientDescent(segment, point0, n+1);
            double[] chordLengthParametrization = GetUniformParametrization(n+1);
            newVertices = new ();
            for(int i = 1; i <= n; ++i)
                if ((segment.PositionAt(chordLengthParametrization[i], point0) - segment.PositionAt(chordLengthParametrization[i-1], point0)).Length() < standardEdgeLength * minBoundaryEdgeRatio) return false;
            for (int i = 1; i <= n; ++i)
                newVertices.Add(new BoundaryVertexBuilder<TVertex, TEdge>(new Orienteering_ISOM_2017_2.VertexAttributes(segment.PositionAt(chordLengthParametrization[i], point0))));
            return true;
        }


        public static List<BoundaryVertexBuilder<TVertex, TEdge>> GetBlankShiftedChainOfSegmentedLine(MapCoordinates firstPosition, IList<Segment> segments,
            int standardEdgeLength, float minBoundaryEdgeRatio, bool leftShift, int shiftSize)
        {
            var tangentVectorToFirstSegmentAt0 = segments[0].d(0, firstPosition);
            var normalVectorOfShiftSizeOfFirstSegmentAt0 = leftShift 
                ? new MapCoordinates((int)(-tangentVectorToFirstSegmentAt0.YPos * shiftSize / tangentVectorToFirstSegmentAt0.Length()), (int)(tangentVectorToFirstSegmentAt0.XPos * 125 / tangentVectorToFirstSegmentAt0.Length()))
                : new MapCoordinates((int)(tangentVectorToFirstSegmentAt0.YPos * shiftSize / tangentVectorToFirstSegmentAt0.Length()), (int)(-tangentVectorToFirstSegmentAt0.XPos * 125 / tangentVectorToFirstSegmentAt0.Length()));
            List<BoundaryVertexBuilder<TVertex, TEdge>> chain = [new (new Orienteering_ISOM_2017_2.VertexAttributes(firstPosition + normalVectorOfShiftSizeOfFirstSegmentAt0))];
            // incrementally adds new vertices to chain for each segment of objects boundary 
            foreach (var segment in segments)
            {
                AddBlankShiftedChainForSegment(segment, chain, standardEdgeLength, minBoundaryEdgeRatio, leftShift, shiftSize);
            }
            // RemoveMicroscopicEdges(chain);
            return chain;
        }

        private static void AddBlankShiftedChainForSegment(Segment segment, List<BoundaryVertexBuilder<TVertex, TEdge>> chain, int standardEdgeLength, float minBoundaryEdgeRatio, bool leftShift, int shiftSize)
        {
            // if segment is too small, following method will process it
            if (HandleSmallSegmentUsingShifting(segment, chain, standardEdgeLength, minBoundaryEdgeRatio, leftShift, shiftSize)) return;
            // remembering segments Point0
            MapCoordinates point0 = chain.Last().Position;
            // discovers count of edges to which should be segment split, so that all edges had approximately same length not bigger than standard edge length
            // edges will not have the same length
            // with increasing edge count, the variance of length of all edges will be still smaller and smaller
            int parametersCount = 5;
            // var parametrization = GetParametrizationWithUniformDistancesByGradientDescent(segment, point0, parametersCount);
            var parametrization = GetUniformParametrization(parametersCount);
            while ((segment.PositionAt(parametrization[1], point0) - point0).Length() > standardEdgeLength ||
                   (segment.PositionAt(parametrization[parametersCount / 2], point0) - segment.PositionAt(parametrization[parametersCount / 2 - 1], point0)).Length() > standardEdgeLength ||
                   (segment.PositionAt(parametrization[parametersCount - 1], point0) - segment.PositionAt(parametrization[parametersCount - 2], point0)).Length() > standardEdgeLength)
                // parametrization = GetParametrizationWithUniformDistancesByGradientDescent(segment, point0, ++parametersCount);
                parametrization = GetUniformParametrization(++parametersCount);
            // iteratively creating edges from uniformly chosen parts of segment
            for (int i = 1; i <= parametersCount - 1; ++i)
            {
                var tangentVectorToSegment = segment.d(parametrization[i], point0);
                var normalVectorOfShiftSize = leftShift 
                    ? new MapCoordinates((int)(-tangentVectorToSegment.YPos * shiftSize / tangentVectorToSegment.Length()), (int)(tangentVectorToSegment.XPos * 125 / tangentVectorToSegment.Length()))
                    : new MapCoordinates((int)(tangentVectorToSegment.YPos * shiftSize / tangentVectorToSegment.Length()), (int)(-tangentVectorToSegment.XPos * 125 / tangentVectorToSegment.Length()));
                BoundaryVertexBuilder<TVertex, TEdge> newVertex = new( new Orienteering_ISOM_2017_2.VertexAttributes(segment.PositionAt(parametrization[i], point0) + normalVectorOfShiftSize));
                newVertex.BoundaryEdges[chain.Last()] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                chain.Last().BoundaryEdges[newVertex] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                chain.Add(newVertex);
            }
        }
        
        private static bool HandleSmallSegmentUsingShifting(Segment segment, List<BoundaryVertexBuilder<TVertex, TEdge>> chain, int standardEdgeLength, float minBoundaryEdgeRatio, bool leftShift, int shiftSize)
        {
            // Tries to fit small segment by as many edges as it can.
            // If more than 3 edges can be fit into the segment, return false indicating, that segment is not small one.
            if (TryGetNNewVerticesForSmallSegmentUsingShifting(segment, 4, chain.Last().Position, standardEdgeLength, minBoundaryEdgeRatio, leftShift, shiftSize, out var newVertices))
                return false;
            if (TryGetNNewVerticesForSmallSegmentUsingShifting(segment, 3, chain.Last().Position, standardEdgeLength, minBoundaryEdgeRatio, leftShift, shiftSize, out newVertices)) { }
            else if (TryGetNNewVerticesForSmallSegmentUsingShifting(segment, 2, chain.Last().Position, standardEdgeLength, minBoundaryEdgeRatio, leftShift, shiftSize, out newVertices)) { }
            else if (TryGetNNewVerticesForSmallSegmentUsingShifting(segment, 1, chain.Last().Position, standardEdgeLength, minBoundaryEdgeRatio, leftShift, shiftSize, out newVertices)) { }
            else return true;
            
            // if there is a distance between pair of vertices bigger than standard edge length, this segment can not be handled as small one, because this long edge could destroy edge cutting in following processing of object
            // although there could be some very small edges created, they are not as big problem as very long edges
            if ((newVertices[0].Position - chain.Last().Position).Length() > standardEdgeLength) return false;
            for(int i = 1; i <= newVertices.Count-1; ++i) if ((newVertices[i].Position - newVertices[i-1].Position).Length() > standardEdgeLength) return false; 
            
            foreach (var newVertex in newVertices)
            {
                newVertex.BoundaryEdges[chain.Last()] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                chain.Last().BoundaryEdges[newVertex] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                chain.Add(newVertex);
            }
            return true;
        }
        private static bool TryGetNNewVerticesForSmallSegmentUsingShifting(Segment segment, int n, MapCoordinates point0,
            int standardEdgeLength, float minBoundaryEdgeRatio, bool leftShift, int shiftSize, out List<BoundaryVertexBuilder<TVertex, TEdge>> newVertices)
        {
            // double[] chordLengthParametrization = GetParametrizationWithUniformDistancesByGradientDescent(segment, point0, n+1);
            double[] chordLengthParametrization = GetUniformParametrization(n+1);
            List<MapCoordinates> normalVectorsOfShift = new();
            for (int i = 1; i <= n; ++i)
            {
                var tangentVectorToSegmentInINths = segment.d(chordLengthParametrization[i], point0);
                normalVectorsOfShift.Add(leftShift 
                    ? new MapCoordinates((int)(-tangentVectorToSegmentInINths.YPos * shiftSize / tangentVectorToSegmentInINths.Length()), (int)(tangentVectorToSegmentInINths.XPos * shiftSize / tangentVectorToSegmentInINths.Length()))
                    : new MapCoordinates((int)(tangentVectorToSegmentInINths.YPos * shiftSize / tangentVectorToSegmentInINths.Length()), (int)(-tangentVectorToSegmentInINths.XPos * shiftSize / tangentVectorToSegmentInINths.Length())) );
            }
            newVertices = new ();
            for(int i = 1; i <= n; ++i)
                if ((segment.PositionAt(chordLengthParametrization[i], point0) - segment.PositionAt(chordLengthParametrization[i-1], point0)).Length() < standardEdgeLength * minBoundaryEdgeRatio) return false;
            for (int i = 1; i <= n; ++i)
                newVertices.Add(new BoundaryVertexBuilder<TVertex, TEdge>(new Orienteering_ISOM_2017_2.VertexAttributes(segment.PositionAt(chordLengthParametrization[i], point0) + normalVectorsOfShift[i-1])));
            return true;
        }

        private static double[] GetUniformParametrization(int n)
        {
            double[] parameters = new double[n];
            for (int i = 0; i <= n - 1; ++i) parameters[i] = i / (double)(n - 1);
            return parameters;
        }

        private static double[] GetParametrizationWithUniformDistancesByGradientDescent(Segment segment, MapCoordinates point0, int n)
        {
            if (n <= 2) return [0, 1];
            double[] parametrization = new double[n];
            for (int i = 0; i <= n - 1; ++i) parametrization[i] = i / (double)(n - 1);

            double lR = 0.1d; //learning rate
            for (int i = 0; i <= 10; ++i)
            {
                double[] gradient = dE(parametrization, segment, point0);
                double gradientLength = Math.Sqrt(gradient.Select(x => Math.Pow(x,2)).Sum());
                for (int j = 0; j <= n - 1; ++j) gradient[j] /= gradientLength / lR; // normalization * learning rate
                for (int j = 0; j <= n - 1; ++j) parametrization[j] = Math.Min(Math.Max(parametrization[j] - gradient[j], 0), 1);
            }
            return parametrization;
        }

        private static double[] dE( double[] t, Segment segment, MapCoordinates point0)
        {
            if (t.Length <= 2) return [0, 0];
            double[] gradient = new double[t.Length];
            gradient[0] = 0;
            gradient[t.Length - 1] = 0;
            for (int i = 1; i < t.Length - 1; ++i)
            {
                (double x, double y) dL_Sti_Sti1 = dL(segment.PositionAt(t[i], point0) - segment.PositionAt(t[i-1], point0));
                MapCoordinates dSti = segment.d(t[i], point0);
                (double x, double y) dL_Sti1_Sti = dL(segment.PositionAt(t[i+1], point0) - segment.PositionAt(t[i], point0));
                MapCoordinates dS_ti = new MapCoordinates(0,0) - segment.d(t[i], point0);
                gradient[i] = 2 * ((segment.PositionAt(t[i], point0) - segment.PositionAt(t[i - 1], point0)).Length() - (segment.PositionAt(t[i + 1], point0) - segment.PositionAt(t[i], point0)).Length()) *
                              (dL_Sti_Sti1.x * dSti.XPos + dL_Sti_Sti1.y * dSti.YPos -
                               (dL_Sti1_Sti.x * dS_ti.XPos + dL_Sti1_Sti.y * dS_ti.YPos));
            }
            return gradient;
        }

        private static (double x, double y) dL(MapCoordinates vector)
        {
            return ( 2 * vector.XPos / (2 * Math.Sqrt(Math.Pow(vector.XPos, 2) + Math.Pow(vector.YPos, 2))), 
                2 * vector.YPos / (2 * Math.Sqrt(Math.Pow(vector.XPos, 2) + Math.Pow(vector.YPos, 2))));
        }
        
        private static double[] GetParametrizationWithUniformDistances(Segment segment, MapCoordinates point0, int n)
        {
            if (n <= 2) return [0, 1];
            double[] parametrization = new double[n];
            for (int i = 0; i <= n - 1; ++i) parametrization[i] = i / (double)(n - 1);
            
            List<double> distancesOfSuccessivePointsOfParametrization = new();
            double lengthsSum = 0; 
            MapCoordinates lastPoint = point0;
            for (int i = 1; i <= n-1; i++)
            {
                var currentPoint = segment.PositionAt(parametrization[i], point0);
                var chordLength = (currentPoint - lastPoint).Length();
                distancesOfSuccessivePointsOfParametrization.Add(chordLength);
                lengthsSum += chordLength;
                lastPoint = currentPoint;
            }

            double prevOldParameter = parametrization[0];
            for (int i = 1; i <= n - 1; ++i)
            {
                double currOldParameter = parametrization[i];
                parametrization[i] = parametrization[i - 1] + (parametrization[i] - prevOldParameter) * (1/(double)(n-1)/(distancesOfSuccessivePointsOfParametrization[i-1] / lengthsSum));
                    // 2 * (parametrization[i]-prevOldParameter) - distancesOfSuccessivePointsOfParametrization[i-1] / lengthsSum;
                prevOldParameter = currOldParameter;
            }
            return parametrization;
        }
        
        #endregion

        #region Sorting of vertices between vertices \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public static List<TVertexBuilder> SortVerticesCorrectly<TVertexBuilder>(BoundaryVertexBuilder<TVertex, TEdge> vertex1, BoundaryVertexBuilder<TVertex, TEdge> vertex2, List<TVertexBuilder> vertices) where TVertexBuilder : BoundaryVertexBuilder<TVertex, TEdge>
        {
            // we will preferably sort vertices based on axis, in which is edge given by vertex1 and vertex2 longer
            // order by ensures stable sort
            return vertices.OrderBy(vertex => vertex.Position, new MapCoordsComparerForSortCorrectlyMethods(
                Math.Abs(vertex1.Position.XPos - vertex2.Position.XPos) > Math.Abs(vertex1.Position.YPos - vertex2.Position.YPos),
                vertex1.Position.XPos < vertex2.Position.XPos,
                vertex1.Position.YPos < vertex2.Position.YPos )).ToList();
        }

        private class MapCoordsComparerForSortCorrectlyMethods(bool isEdgeInXAxisLonger, bool isXPosOfV1LessThenV2, bool isYPosOfV1LessThenV2) : IComparer<MapCoordinates>
        {
            public int Compare(MapCoordinates p1, MapCoordinates p2)
            {
                if(isEdgeInXAxisLonger)
                    if(isXPosOfV1LessThenV2)
                        if (isYPosOfV1LessThenV2) return p1.XPos - p2.XPos != 0 ? p1.XPos - p2.XPos : p1.YPos - p2.YPos;
                        else return p1.XPos - p2.XPos != 0 ? p1.XPos - p2.XPos : p2.YPos - p1.YPos;
                    else if (isYPosOfV1LessThenV2) return p2.XPos - p1.XPos != 0 ? p2.XPos - p1.XPos : p1.YPos - p2.YPos;
                    else return p2.XPos - p1.XPos != 0 ? p2.XPos - p1.XPos : p2.YPos - p1.YPos; 
                if(isYPosOfV1LessThenV2)
                    if (isXPosOfV1LessThenV2) return p1.YPos - p2.YPos != 0 ? p1.YPos - p2.YPos : p1.XPos - p2.XPos;
                    else return p1.YPos - p2.YPos != 0 ? p1.YPos - p2.YPos : p2.XPos - p1.XPos;
                if (isXPosOfV1LessThenV2) return p2.YPos - p1.YPos != 0 ? p2.YPos - p1.YPos : p1.XPos - p2.XPos;
                return p2.YPos - p1.YPos != 0 ? p2.YPos - p1.YPos : p2.XPos - p1.XPos;
            }
        }

        #endregion
        
        #region Is right side inner \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        
        public static bool IsRightSideInner(VertexBuilder<TVertex, TEdge> vertex, MapCoordinates prevVertexPosition, MapCoordinates nextVertexPosition, BoundaryVertexBuilder<TVertex, TEdge>[] longEnoughChain)
        {
            // in following two lines there is computed vector whose direction is in half between vector from vertex to previous vertex and vertex to next vertex
            // it is essentially previous vertex rotated around vertex by half of an angle between previous vertex and next vertex
            double angle = Utils.ComputeLeftHandedAngleBetween(vertex.Position, prevVertexPosition, vertex.Position, nextVertexPosition);
            var prevVertexPositionRotatedAroundVertexPosition = prevVertexPosition.RotateCounterClockwise(angle/2, vertex.Position);
            // if count of crossed edges by ray in direction of computed vector is odd, vertex, from which ray started had its right side turned into the polygon, so the result is true
            int crossSectionsCount = 0;
            for (int i = 1; i < longEnoughChain.Length; i++)
                if (vertex != longEnoughChain[i-1] && vertex != longEnoughChain[i] && AreLineSegmentAndRayCrossingWithMagic(longEnoughChain[i - 1].Position, longEnoughChain[i].Position, vertex.Position, prevVertexPositionRotatedAroundVertexPosition, (longEnoughChain[i - 2 >= 0 ? i - 2 : longEnoughChain.Length - 1].Position, longEnoughChain[i + 1 <= longEnoughChain.Length - 1 ? i - 2 : 0].Position))) crossSectionsCount++;
            if (vertex != longEnoughChain.Last() && vertex != longEnoughChain[0] && AreLineSegmentAndRayCrossingWithMagic(longEnoughChain.Last().Position, longEnoughChain[0].Position, vertex.Position, prevVertexPositionRotatedAroundVertexPosition, (longEnoughChain[longEnoughChain.Length - 2].Position, longEnoughChain[1].Position))) crossSectionsCount++; 
            return crossSectionsCount % 2 == 1;
        }
        
        #endregion
        
        #region Finding and cutting all crossed edges by chain \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        
        public static (Dictionary<(VertexBuilder<TVertex, TEdge>, VertexBuilder<TVertex, TEdge>), List<MapCoordinates>>, Dictionary<(BoundaryVertexBuilder<TVertex, TEdge>, BoundaryVertexBuilder<TVertex, TEdge>), (Orienteering_ISOM_2017_2.EdgeAttributes cutEdgeAttributes, List<(MapCoordinates, int, int)> crossSectionsWithChainsAndEdgesIndices)>) 
            FindAllCrossedEdges(BoundaryVertexBuilder<TVertex, TEdge>[][] chains, IEditableRadiallySearchableDataStruct<VertexBuilder<TVertex, TEdge>> allVertices, int standardEdgeLength, bool polygonalChain)
        {
            Dictionary<(VertexBuilder<TVertex, TEdge>, VertexBuilder<TVertex, TEdge>), List<MapCoordinates>> verticesOfCutNonBoundaryEdges = new();
            Dictionary<(BoundaryVertexBuilder<TVertex, TEdge>, BoundaryVertexBuilder<TVertex, TEdge>), (Orienteering_ISOM_2017_2.EdgeAttributes, List<(MapCoordinates, int, int)>)> verticesOfCutBoundaryEdges = new();
            // at first collects all edges, which are crossed by any chain
            for (int i = 0; i < chains.Length; ++i)
            {
                for (int j = 0; j < chains[i].Length - 1; ++j)
                    AddCrossedEdgesBy(chains[i][j].Position, chains[i][j + 1].Position, chains[i][j - 1 >= 0 ? j - 1 : chains[i].Length - 1].Position, chains[i][j + 2 <= chains[i].Length - 1 ? j + 2 : 0].Position, i, j, allVertices, verticesOfCutNonBoundaryEdges, verticesOfCutBoundaryEdges, standardEdgeLength);
                if (polygonalChain)
                    AddCrossedEdgesBy(chains[i].Last().Position, chains[i][0].Position, chains[i][chains[i].Length - 2].Position, chains[i][1].Position, i, chains[i].Length - 1, allVertices, verticesOfCutNonBoundaryEdges, verticesOfCutBoundaryEdges, standardEdgeLength);
            }

            return (verticesOfCutNonBoundaryEdges, verticesOfCutBoundaryEdges);
        }
        
        public static void
            CutAllFoundCrossedEdges(Dictionary<(VertexBuilder<TVertex, TEdge>, VertexBuilder<TVertex, TEdge>), List<MapCoordinates>> verticesOfCutNonBoundaryEdges, Dictionary<(BoundaryVertexBuilder<TVertex, TEdge>, BoundaryVertexBuilder<TVertex, TEdge>), (Orienteering_ISOM_2017_2.EdgeAttributes cutEdgeAttributes, List<(MapCoordinates, int, int)> crossSectionsWithChainsAndEdgesIndices)> verticesOfCutBoundaryEdges)
        {
            // at second cut out all crossed edges from the graph
            foreach (var ((vertex1, vertex2), _) in verticesOfCutNonBoundaryEdges)
            {
                if(vertex1 is BoundaryVertexBuilder<TVertex, TEdge> boundaryVertex1) boundaryVertex1.NonBoundaryEdges.Remove(vertex2);
                else if (vertex1 is NetVertexBuilder<TVertex, TEdge> netVertex1) netVertex1.NonBoundaryEdges.Remove(vertex2);
                if(vertex2 is BoundaryVertexBuilder<TVertex, TEdge> boundaryVertex2) boundaryVertex2.NonBoundaryEdges.Remove(vertex1);
                else if (vertex2 is NetVertexBuilder<TVertex, TEdge> netVertex2) netVertex2.NonBoundaryEdges.Remove(vertex1);
            }
            
            foreach (var ((vertex1, vertex2), _) in verticesOfCutBoundaryEdges)
            {
                vertex1.BoundaryEdges.Remove(vertex2);
                vertex2.BoundaryEdges.Remove(vertex1);
            }
        }


        private static void AddCrossedEdgesBy(MapCoordinates point0, MapCoordinates point1, MapCoordinates prevPoint, MapCoordinates nextPoint,int chainIndex, int edgeIndex, IEditableRadiallySearchableDataStruct<VertexBuilder<TVertex, TEdge>> allVertices, Dictionary<(VertexBuilder<TVertex, TEdge>, VertexBuilder<TVertex, TEdge>), List<MapCoordinates>> verticesOfCutNonBoundaryEdges, Dictionary<(BoundaryVertexBuilder<TVertex, TEdge>, BoundaryVertexBuilder<TVertex, TEdge>), (Orienteering_ISOM_2017_2.EdgeAttributes cutEdgeAttributes, List<(MapCoordinates, int, int)> crossSectionsWithChainsAndEdgesIndices)> verticesOfCutBoundaryEdges, int standardEdgeLength)
        {
            HashSet<VertexBuilder<TVertex, TEdge>> closeVertices = new();
            // find all vertices, which are closer than standard edge length to vertices of chain edge
            // these vertices are the only ones whose edges could be crossed by this edge
            foreach (var vertex in allVertices.FindInEuclideanDistanceFrom((point0.XPos, point0.YPos), standardEdgeLength))
                closeVertices.Add(vertex);
            foreach (var vertex in allVertices.FindInEuclideanDistanceFrom((point1.XPos, point1.YPos), standardEdgeLength))
                closeVertices.Add(vertex);
            // test all edges of found vertices, whether they are not crossed by tested edge
            HashSet<(VertexBuilder<TVertex, TEdge>, VertexBuilder<TVertex, TEdge>)> processedEdges = new();
            foreach (var closeVertex in closeVertices)
            {
                if (closeVertex is BoundaryVertexBuilder<TVertex, TEdge> closeBoundaryVertex)
                {
                    foreach (var (neighbor,_) in closeBoundaryVertex.NonBoundaryEdges)
                    {
                        if (processedEdges.Contains((neighbor, closeBoundaryVertex))) continue;
                        if (GetLineSegmentsCrossSectionCoordsWithMagic(point0, point1, closeBoundaryVertex.Position, neighbor.Position, (prevPoint, nextPoint)) is MapCoordinates crossSection)
                            AddSortedNonBoundaryVerticesTo(verticesOfCutNonBoundaryEdges, closeBoundaryVertex, neighbor, crossSection);
                        processedEdges.Add((closeBoundaryVertex, neighbor));
                    }
                    foreach (var (neighbor, _) in closeBoundaryVertex.BoundaryEdges)
                    {
                        if (processedEdges.Contains((neighbor, closeBoundaryVertex))) continue;
                        if (GetLineSegmentsCrossSectionCoordsWithMagic(point0, point1, closeBoundaryVertex.Position, neighbor.Position, (prevPoint, nextPoint)) is MapCoordinates crossSection)
                            AddSortedBoundaryVerticesTo(verticesOfCutBoundaryEdges, closeBoundaryVertex, neighbor, chainIndex, edgeIndex, crossSection);
                        processedEdges.Add((closeBoundaryVertex, neighbor));
                    }
                }
                else if (closeVertex is NetVertexBuilder<TVertex, TEdge> closeNetVertex)
                {
                    // we do not have to take care of linear features of crossed edge, because if it has some non-trivial linear features attributes
                    // we are already at the phase of processing linear obstacles, so the edge will not be cut out from graph, its features will be only augmented
                    foreach (var (neighbor, _) in closeNetVertex.NonBoundaryEdges)
                    {
                        if (processedEdges.Contains((neighbor, closeNetVertex))) continue;
                        if (GetLineSegmentsCrossSectionCoordsWithMagic(point0, point1, closeNetVertex.Position, neighbor.Position, (prevPoint, nextPoint)) is MapCoordinates crossSection)
                            AddSortedNonBoundaryVerticesTo(verticesOfCutNonBoundaryEdges, closeNetVertex, neighbor, crossSection);
                        processedEdges.Add((closeNetVertex, neighbor));
                    }
                }
            }
        }

        private static void AddSortedNonBoundaryVerticesTo(Dictionary<(VertexBuilder<TVertex, TEdge>, VertexBuilder<TVertex, TEdge>),  List<MapCoordinates>> verticesOfCutNonBoundaryEdges, VertexBuilder<TVertex, TEdge> vertex1, VertexBuilder<TVertex, TEdge> vertex2, MapCoordinates crossSectionCoords)
        {
            // dictionary is indexed by couples of vertices in specific order, first vertex of couple has always lower sum of its position axis values than the other one
            // this ensures, that edges defined by couples of vertices are in the dictionary always present only one time
            // it should be mentioned that in this sense edges are thought as not oriented
            if (vertex1.Position.XPos < vertex2.Position.XPos || (vertex1.Position.XPos == vertex2.Position.XPos && vertex1.Position.YPos < vertex2.Position.YPos))  
                if (verticesOfCutNonBoundaryEdges.ContainsKey((vertex1, vertex2))) verticesOfCutNonBoundaryEdges[(vertex1, vertex2)].Add(crossSectionCoords);
                else verticesOfCutNonBoundaryEdges[(vertex1, vertex2)] = [crossSectionCoords];
            else 
                if (verticesOfCutNonBoundaryEdges.ContainsKey((vertex2, vertex1))) verticesOfCutNonBoundaryEdges[(vertex2, vertex1)].Add(crossSectionCoords);
                else verticesOfCutNonBoundaryEdges[(vertex2, vertex1)] = [crossSectionCoords];
        }
        
        private static void AddSortedBoundaryVerticesTo(Dictionary<(BoundaryVertexBuilder<TVertex, TEdge>, BoundaryVertexBuilder<TVertex, TEdge>), (Orienteering_ISOM_2017_2.EdgeAttributes cutEdgeAttributes, List<(MapCoordinates, int, int)> crossSectionsWithChainsAndEdgesIndices)> verticesOfCutBoundaryEdges, BoundaryVertexBuilder<TVertex, TEdge> vertex1, BoundaryVertexBuilder<TVertex, TEdge> vertex2, int chainIndex, int edgeIndex, MapCoordinates crossSectionCoords)
        {
            // dictionary is indexed by couples of vertices in specific order, first vertex of couple has always lower sum of its position axis values than the other one
            // this ensures, that edges defined by couples of vertices are in the dictionary always present only one time
            // it should be mentioned that in this sense edges are thought as not oriented
            if (vertex1.Position.XPos < vertex2.Position.XPos || (vertex1.Position.XPos == vertex2.Position.XPos && vertex1.Position.YPos < vertex2.Position.YPos))
                if (verticesOfCutBoundaryEdges.ContainsKey((vertex1, vertex2))) verticesOfCutBoundaryEdges[(vertex1, vertex2)].crossSectionsWithChainsAndEdgesIndices.Add((crossSectionCoords, chainIndex,edgeIndex));
                else verticesOfCutBoundaryEdges[(vertex1, vertex2)] = (vertex1.BoundaryEdges[vertex2], new List<(MapCoordinates, int, int)>{(crossSectionCoords, chainIndex, edgeIndex)});
            else if (verticesOfCutBoundaryEdges.ContainsKey((vertex2, vertex1))) verticesOfCutBoundaryEdges[(vertex2, vertex1)].crossSectionsWithChainsAndEdgesIndices .Add((crossSectionCoords, chainIndex, edgeIndex));
                else verticesOfCutBoundaryEdges[(vertex2, vertex1)] = (vertex2.BoundaryEdges[vertex1], new List<(MapCoordinates, int, int)>{(crossSectionCoords, chainIndex, edgeIndex)});
        }
        
        #endregion
        
        #region Crossections determining and retrieving \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        
        public static MapCoordinates? GetLineSegmentsCrossSectionCoordsWithMagic(MapCoordinates p1, MapCoordinates p2, MapCoordinates q1, MapCoordinates q2, (MapCoordinates prev, MapCoordinates next) sP)
        {
            var ft = GetNumeratorAndDenominatorOfT(p1, p2, q1, q2);
            var fu = GetNumeratorAndDenominatorOfU(p1, p2, q1, q2);
            if (!IsFractionInO1IntervalAndDefined(ft.numerator, ft.denominator)) return null;
            if (!IsFractionInO1IntervalAndDefined(fu.numerator, fu.denominator)) return null;
            if (!ApplyRightMagicFor(MagicFor.TwoSegments, ft, p1, p2, fu, q1, q2, sP)) return null;
            if (ft.numerator == 0) return p1;
            if (ft.numerator == ft.denominator) return p2;
            if (fu.numerator == 0) return q1;
            if (fu.numerator == fu.denominator) return q2;
            double t = ft.numerator / (double)ft.denominator;
            return new MapCoordinates((int)(p1.XPos + t * (p2.XPos - p1.XPos)), (int)(p1.YPos + t * (p2.YPos - p1.YPos)));
        }
        
        public static MapCoordinates? GetLineSegmentsCrossSectionCoordsParallelCrossSectionIncluded(MapCoordinates p1, MapCoordinates p2, MapCoordinates q1, MapCoordinates q2)
        {
            var ft = GetNumeratorAndDenominatorOfT(p1, p2, q1, q2);
            var fu = GetNumeratorAndDenominatorOfU(p1, p2, q1, q2);
            if (!IsDefined(ft.numerator, ft.denominator))
                return ProcessParallelCrossSection(p1, p2, q1, q2);
            if (!IsFractionInO1IntervalAndDefined(ft.numerator, ft.denominator)) return null;
            if (!IsFractionInO1IntervalAndDefined(fu.numerator, fu.denominator)) return null;
            if (ft.numerator == 0) return p1;
            if (ft.numerator == ft.denominator) return p2;
            if (fu.numerator == 0) return q1;
            if (fu.numerator == fu.denominator) return q2;
            double t = ft.numerator / (double)ft.denominator;
            return new MapCoordinates((int)(p1.XPos + t * (p2.XPos - p1.XPos)), (int)(p1.YPos + t * (p2.YPos - p1.YPos)));
        }
        
        public static MapCoordinates? GetLineSegmentsCrossSectionCoordsP2Q2Excluded(MapCoordinates p1, MapCoordinates p2, MapCoordinates q1, MapCoordinates q2)
        {
            var ft = GetNumeratorAndDenominatorOfT(p1, p2, q1, q2);
            var fu = GetNumeratorAndDenominatorOfU(p1, p2, q1, q2);
            if (!IsFractionInO1IntervalAndDefined(ft.numerator, ft.denominator)) return null;
            if (!IsFractionInO1IntervalAndDefined(fu.numerator, fu.denominator)) return null;
            if (ft.numerator == ft.denominator || fu.numerator == fu.denominator) return null;
            if (ft.numerator == 0) return p1;
            if (fu.numerator == 0) return q1;
            double t = ft.numerator / (double)ft.denominator;
            return new MapCoordinates((int)(p1.XPos + t * (p2.XPos - p1.XPos)), (int)(p1.YPos + t * (p2.YPos - p1.YPos)));
        }
        
        public static bool AreLineSegmentsCrossingEndPointsExcluded(MapCoordinates p1, MapCoordinates p2, MapCoordinates q1, MapCoordinates q2)
        {
            var ft = GetNumeratorAndDenominatorOfT(p1, p2, q1, q2);
            var fu = GetNumeratorAndDenominatorOfU(p1, p2, q1, q2);
            if (!IsFractionInO1IntervalAndDefined(ft.numerator, ft.denominator)) return false;
            if (!IsFractionInO1IntervalAndDefined(fu.numerator, fu.denominator)) return false;
            if (ft.numerator == 0 || ft.numerator == ft.denominator || fu.numerator == 0 || fu.numerator == fu.denominator) return false;
            return true;
        }
        
        public static bool AreLineSegmentAndRayCrossingWithMagic(MapCoordinates ls1, MapCoordinates ls2, MapCoordinates r1, MapCoordinates r2, (MapCoordinates prev, MapCoordinates next) sP)
        {
            var ft = GetNumeratorAndDenominatorOfT(ls1, ls2, r1, r2);
            var fu = GetNumeratorAndDenominatorOfU(ls1, ls2, r1, r2);
            if (!IsFractionInO1IntervalAndDefined(ft.numerator, ft.denominator)) return false;
            if (!IsFractionGreaterOrEqualToZeroAndDefined(fu.numerator, fu.denominator)) return false;
            return ApplyRightMagicFor(MagicFor.SegmentAndRay, ft, ls1, ls2, fu, r1, r2, sP);
        }
        private enum MagicFor{TwoSegments, SegmentAndRay}
        
        private static bool ApplyRightMagicFor(MagicFor what, (int numerator, int denominator) ft, MapCoordinates p1, MapCoordinates p2, (int numerator, int denominator) fu, MapCoordinates q1, MapCoordinates q2, (MapCoordinates prev, MapCoordinates next) sP)
        {
            // magic:
            // if vertex q1 or q2 falls under (chain) edge <p1, p2>, it is processed as it felt on the right side of this edge
            if (ft.numerator == 0)
                if (fu.numerator == 0)
                    if ((p1 - sP.prev).LeftHandNormalVector() * (p2 - p1) > 0)
                        return (p1 - sP.prev).LeftHandNormalVector() * (q2 - q1) > 0 && (p2 - p1).LeftHandNormalVector() * (q2 - q1) > 0;
                    else return (p1 - sP.prev).LeftHandNormalVector() * (q2 - q1) > 0 || (p2 - p1).LeftHandNormalVector() * (q2 - q1) > 0;
                else if(what is MagicFor.TwoSegments && fu.numerator == fu.denominator)
                    if ((p1 - sP.prev).LeftHandNormalVector() * (p2 - p1) > 0)
                        return (p1 - sP.prev).LeftHandNormalVector() * (q1 - q2) > 0 && (p2 - p1).LeftHandNormalVector() * (q1 - q2) > 0;
                    else return (p1 - sP.prev).LeftHandNormalVector() * (q1 - q2) > 0 || (p2 - p1).LeftHandNormalVector() * (q1 - q2) > 0;
                else 
                    if ((p1 - sP.prev).LeftHandNormalVector() * (p2 - p1) >= 0)
                        return (((p1 - sP.prev).LeftHandNormalVector() * (q1 - q2) > 0 && (p2 - p1).LeftHandNormalVector() * (q1 - q2) > 0) &&
                                !((p1 - sP.prev).LeftHandNormalVector() * (q2 - q1) > 0 && (p2 - p1).LeftHandNormalVector() * (q2 - q1) > 0)) ||
                               (!((p1 - sP.prev).LeftHandNormalVector() * (q1 - q2) > 0 && (p2 - p1).LeftHandNormalVector() * (q1 - q2) > 0) &&
                                 ((p1 - sP.prev).LeftHandNormalVector() * (q2 - q1) > 0 && (p2 - p1).LeftHandNormalVector() * (q2 - q1) > 0));
                    else return (((p1 - sP.prev).LeftHandNormalVector() * (q1 - q2) > 0 || (p2 - p1).LeftHandNormalVector() * (q1 - q2) > 0) && 
                                 !((p1 - sP.prev).LeftHandNormalVector() * (q2 - q1) > 0 || (p2 - p1).LeftHandNormalVector() * (q2 - q1) > 0)) || 
                                (!((p1 - sP.prev).LeftHandNormalVector() * (q1 - q2) > 0 || (p2 - p1).LeftHandNormalVector() * (q1 - q2) > 0) && 
                                  ((p1 - sP.prev).LeftHandNormalVector() * (q2 - q1) > 0 || (p2 - p1).LeftHandNormalVector() * (q2 - q1) > 0));
            // if first edge (chain edge) is cut in p2, cut is processed only if the next edge is parallel with [q1,q2] edge
            // this cut would not be processed in next edge processing, because of parallelism of next edge and edge [q1, q2] 
            if (ft.numerator == ft.denominator)
                if ((sP.next - p2).RightHandNormalVector() * (q2 - q1) == 0)
                    if (fu.numerator == 0 )
                        return (sP.next - p2) * (q2 - q1) < 0 && (sP.next - p2) * (p2 - p1).RightHandNormalVector() > 0;
                    else if (what is MagicFor.TwoSegments && fu.numerator == fu.denominator )
                        return (sP.next - p2) * (q1 - q2) < 0 && (sP.next - p2) * (p2 - p1).RightHandNormalVector() > 0;
                    else return (sP.next - p2) * (p2 - p1).RightHandNormalVector() > 0;
                else return false;
            if (fu.numerator == 0)
                return (p2 - p1).RightHandNormalVector() * (q2 - q1) < 0;
            if (what is MagicFor.TwoSegments && fu.numerator == fu.denominator)
                return (p2 - p1).RightHandNormalVector() * (q1 - q2) < 0;
            return true;
        }
        private static bool ApplyLeftMagicFor(MagicFor what, (int numerator, int denominator) ft, MapCoordinates p1, MapCoordinates p2, (int numerator, int denominator) fu, MapCoordinates q1, MapCoordinates q2, (MapCoordinates prev, MapCoordinates next) sP)
        {
            // magic:
            // if vertex q1 or q2 falls under (chain) edge <p1, p2>, it is processed as it felt on the left side of this edge
            if (ft.numerator == 0)
                if (fu.numerator == 0)
                    if ((p1 - sP.prev).RightHandNormalVector() * (p2 - p1) > 0)
                        return (p1 - sP.prev).RightHandNormalVector() * (q2 - q1) > 0 && (p2 - p1).RightHandNormalVector() * (q2 - q1) > 0;
                    else return (p1 - sP.prev).RightHandNormalVector() * (q2 - q1) > 0 || (p2 - p1).RightHandNormalVector() * (q2 - q1) > 0;
                else if(what is MagicFor.TwoSegments && fu.numerator == fu.denominator)
                    if ((p1 - sP.prev).RightHandNormalVector() * (p2 - p1) > 0)
                        return (p1 - sP.prev).RightHandNormalVector() * (q1 - q2) > 0 && (p2 - p1).RightHandNormalVector() * (q1 - q2) > 0;
                    else return (p1 - sP.prev).RightHandNormalVector() * (q1 - q2) > 0 || (p2 - p1).RightHandNormalVector() * (q1 - q2) > 0;
                else 
                    if ((p1 - sP.prev).RightHandNormalVector() * (p2 - p1) >= 0)
                        return (((p1 - sP.prev).RightHandNormalVector() * (q1 - q2) > 0 && (p2 - p1).RightHandNormalVector() * (q1 - q2) > 0) &&
                                !((p1 - sP.prev).RightHandNormalVector() * (q2 - q1) > 0 && (p2 - p1).RightHandNormalVector() * (q2 - q1) > 0)) ||
                               (!((p1 - sP.prev).RightHandNormalVector() * (q1 - q2) > 0 && (p2 - p1).RightHandNormalVector() * (q1 - q2) > 0) &&
                                 ((p1 - sP.prev).RightHandNormalVector() * (q2 - q1) > 0 && (p2 - p1).RightHandNormalVector() * (q2 - q1) > 0));
                    else return (((p1 - sP.prev).RightHandNormalVector() * (q1 - q2) > 0 || (p2 - p1).RightHandNormalVector() * (q1 - q2) > 0) && 
                                 !((p1 - sP.prev).RightHandNormalVector() * (q2 - q1) > 0 || (p2 - p1).RightHandNormalVector() * (q2 - q1) > 0)) || 
                                (!((p1 - sP.prev).RightHandNormalVector() * (q1 - q2) > 0 || (p2 - p1).RightHandNormalVector() * (q1 - q2) > 0) && 
                                  ((p1 - sP.prev).RightHandNormalVector() * (q2 - q1) > 0 || (p2 - p1).RightHandNormalVector() * (q2 - q1) > 0));
            // if first edge (chain edge) is cut in p2, cut is processed only if the next edge is parallel with [q1,q2] edge
            // this cut would not be processed in next edge processing, because of parallelism of next edge and edge [q1, q2] 
            if (ft.numerator == ft.denominator)
                if ((sP.next - p2).LeftHandNormalVector() * (q2 - q1) == 0)
                    if (fu.numerator == 0 )
                        return (sP.next - p2) * (q2 - q1) < 0 && (sP.next - p2) * (p2 - p1).LeftHandNormalVector() > 0;
                    else if (what is MagicFor.TwoSegments && fu.numerator == fu.denominator )
                        return (sP.next - p2) * (q1 - q2) < 0 && (sP.next - p2) * (p2 - p1).LeftHandNormalVector() > 0;
                    else return (sP.next - p2) * (p2 - p1).LeftHandNormalVector() > 0;
                else return false;
            if (fu.numerator == 0)
                return (p2 - p1).LeftHandNormalVector() * (q2 - q1) < 0;
            if (what is MagicFor.TwoSegments && fu.numerator == fu.denominator)
                return (p2 - p1).LeftHandNormalVector() * (q1 - q2) < 0;
            return true;
        }
        
        private static MapCoordinates? ProcessParallelCrossSection(MapCoordinates p1, MapCoordinates p2, MapCoordinates q1, MapCoordinates q2)
        {
            if ((q2 - p1 != new MapCoordinates(0, 0) ? q2 - p1 : q1 - p1) * (p2 - p1).LeftHandNormalVector() == 0)
            {
                var sortedCoordinates = SortCoordsCorrectly(p1, p2, [p1, p2, q1, q2]);
                if (sortedCoordinates[0] == p1 || sortedCoordinates[0] == p2)
                    if (sortedCoordinates[1] == q1 || sortedCoordinates[1] == q2)
                        return (sortedCoordinates[1] + sortedCoordinates[2]) / 2;
                    else return null;
                if (sortedCoordinates[1] == p1 || sortedCoordinates[1] == p2)
                    return (sortedCoordinates[1] + sortedCoordinates[2]) / 2;
                return null;
            }
            return null;
        }
        
        private static List<MapCoordinates> SortCoordsCorrectly(MapCoordinates p1, MapCoordinates p2, List<MapCoordinates> coords) 
        {
            // we will preferably sort vertices based on axis, in which is edge given by vertex1 and vertex2 longer
            // order by ensures stable sort
            return coords.OrderBy(pos => pos, new MapCoordsComparerForSortCorrectlyMethods(
                Math.Abs(p1.XPos - p2.XPos) > Math.Abs(p1.YPos - p2.YPos),
                p1.XPos < p2.XPos,
                p1.YPos < p2.YPos )).ToList();
        }
            
        private static (int numerator , int denominator) GetNumeratorAndDenominatorOfT(MapCoordinates p1, MapCoordinates p2, MapCoordinates p3, MapCoordinates p4)
            => ((p1.XPos - p3.XPos) * (p3.YPos - p4.YPos) - (p1.YPos - p3.YPos) * (p3.XPos - p4.XPos),
                (p1.XPos - p2.XPos) * (p3.YPos - p4.YPos) - (p1.YPos - p2.YPos) * (p3.XPos - p4.XPos));
        private static (int numerator, int denominator) GetNumeratorAndDenominatorOfU(MapCoordinates p1, MapCoordinates p2, MapCoordinates p3, MapCoordinates p4)
            => (- ((p1.XPos - p2.XPos) * (p1.YPos - p3.YPos) - (p1.YPos - p2.YPos) * (p1.XPos - p3.XPos)),
                   (p1.XPos - p2.XPos) * (p3.YPos - p4.YPos) - (p1.YPos - p2.YPos) * (p3.XPos - p4.XPos));

        private static bool IsDefined(int numerator, int denominator)
            => denominator != 0;
        private static bool IsFractionInO1IntervalAndDefined(int numerator, int denominator)
            => (numerator >= 0 && denominator > 0 && denominator >= numerator) || (numerator <= 0 && denominator < 0 && denominator <= numerator);
        private static bool IsFractionGreaterOrEqualToZeroAndDefined(int numerator, int denominator)
            => (numerator >= 0 && denominator > 0) || (numerator <= 0 && denominator < 0);
        
        #endregion

        #region Attributes updating \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public static Orienteering_ISOM_2017_2.EdgeAttributes UpdateLinearFeaturesOfEdgeAttributes( Orienteering_ISOM_2017_2.EdgeAttributes edge, decimal symbolCodeOfAddedObject)
        {
            var newLinearFeatures = GetUpdatedLinearFeaturesOfEdgeAttributes(edge.LineFeatures,  edge.LeftSurroundings, edge.RightSurroundings, symbolCodeOfAddedObject, out var newSurroundings, out var newSecondSurroundings);
            return new Orienteering_ISOM_2017_2.EdgeAttributes(newSurroundings, newLinearFeatures, newSecondSurroundings);
        }
        
        public static Orienteering_ISOM_2017_2.EdgeAttributes UpdateBothSurroundingsOfEdgeAttributes(Orienteering_ISOM_2017_2.EdgeAttributes edge, decimal symbolCodeOfAddedObject)
        {
            // both left and right surroundings of edge attributes are updated
            var newLeftSurroundings = GetUpdatedSurroundingsOfEdgeAttributes(edge.LeftSurroundings, symbolCodeOfAddedObject); 
            var newRightSurroundings = GetUpdatedSurroundingsOfEdgeAttributes(edge.RightSurroundings, symbolCodeOfAddedObject); 
            return new Orienteering_ISOM_2017_2.EdgeAttributes(newLeftSurroundings, newRightSurroundings);
        }
        
        public static (Orienteering_ISOM_2017_2.Grounds? ground, Orienteering_ISOM_2017_2.Boulders? boulders, Orienteering_ISOM_2017_2.Stones? stones, Orienteering_ISOM_2017_2.Water? water, Orienteering_ISOM_2017_2.VegetationAndManMade? vegetationAndManMade, Orienteering_ISOM_2017_2.VegetationGoodVis? vegetationGoodVis)
            GetUpdatedSurroundingsOfEdgeAttributes((Orienteering_ISOM_2017_2.Grounds? ground, Orienteering_ISOM_2017_2.Boulders? boulders, Orienteering_ISOM_2017_2.Stones? stones, Orienteering_ISOM_2017_2.Water? water, Orienteering_ISOM_2017_2.VegetationAndManMade? vegetationAndManMade, Orienteering_ISOM_2017_2.VegetationGoodVis? vegetationGoodVis) surroundings, decimal symbolCodeOfAddedObject)
            => symbolCodeOfAddedObject switch 
            {
                403 => (null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.RoughOpenLand_403, null),
                404 or 404.1m => (null , null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.RoughOpenLandWithTrees_404, null),
                413.1m => (null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.RoughOrchard_413, null),
                213 => (null, null, Orienteering_ISOM_2017_2.Stones.SandyGround_213, null, null, null),
                414.1m => (null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.RoughVineyard_414, null),
                401 => (null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.OpenLand_401, null),
                402 or 402.1m => (null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.OpenLandWithTrees_402, null),
                413 => (null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.Orchard_413, null),
                412 => (Orienteering_ISOM_2017_2.Grounds.CultivatedLand_412, null, null, null, null, null),
                414 => (null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.Vineyard_414, null),
                407 => new Func<(Orienteering_ISOM_2017_2.Grounds? ground, Orienteering_ISOM_2017_2.Boulders? boulders, Orienteering_ISOM_2017_2.Stones? stones, Orienteering_ISOM_2017_2.Water? water, Orienteering_ISOM_2017_2.VegetationAndManMade? vegetationAndManMade, Orienteering_ISOM_2017_2.VegetationGoodVis? vegetationGoodVis)>(() => {
                        if (surroundings.ground == Orienteering_ISOM_2017_2.Grounds.CultivatedLand_412 ||
                            surroundings.stones == Orienteering_ISOM_2017_2.Stones.SandyGround_213)
                            return surroundings;
                        return (null, null, null, null, surroundings.vegetationAndManMade, Orienteering_ISOM_2017_2.VegetationGoodVis.VegetationSlowGoodVis_407); })(),
                409 => new Func<(Orienteering_ISOM_2017_2.Grounds? ground, Orienteering_ISOM_2017_2.Boulders? boulders, Orienteering_ISOM_2017_2.Stones? stones, Orienteering_ISOM_2017_2.Water? water, Orienteering_ISOM_2017_2.VegetationAndManMade? vegetationAndManMade, Orienteering_ISOM_2017_2.VegetationGoodVis? vegetationGoodVis)>(() => {
                        if (surroundings.ground == Orienteering_ISOM_2017_2.Grounds.CultivatedLand_412 ||
                            surroundings.stones == Orienteering_ISOM_2017_2.Stones.SandyGround_213)
                            return surroundings;
                        return (null, null, null, null, surroundings.vegetationAndManMade, Orienteering_ISOM_2017_2.VegetationGoodVis.VegetationWalkGoodVis_409); })(),
                406 or 406.1m => (null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.VegetationSlow_406, null),
                408 or 408.1m or 408.2m => (null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.VegetationWalk_408, null),
                410 or 410.1m or 410.2m or 410.3m or 410.4m =>    (null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.VegetationFight_410, null),
                214 => (null, null, Orienteering_ISOM_2017_2.Stones.BareRock_214, null, null, null),
                405 => (null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.Forest_405, null),
                310 => new Func<(Orienteering_ISOM_2017_2.Grounds? ground, Orienteering_ISOM_2017_2.Boulders? boulders, Orienteering_ISOM_2017_2.Stones? stones, Orienteering_ISOM_2017_2.Water? water, Orienteering_ISOM_2017_2.VegetationAndManMade? vegetationAndManMade, Orienteering_ISOM_2017_2.VegetationGoodVis? vegetationGoodVis)>(() => {
                        if (surroundings.ground == Orienteering_ISOM_2017_2.Grounds.CultivatedLand_412 ||
                            surroundings.stones == Orienteering_ISOM_2017_2.Stones.SandyGround_213 ||
                            surroundings.stones == Orienteering_ISOM_2017_2.Stones.BareRock_214)
                            return surroundings;
                        return (null, null, null, Orienteering_ISOM_2017_2.Water.IndistinctMarsh_310, surroundings.vegetationAndManMade, surroundings.vegetationGoodVis); })(),
                308 => new Func<(Orienteering_ISOM_2017_2.Grounds? ground, Orienteering_ISOM_2017_2.Boulders? boulders, Orienteering_ISOM_2017_2.Stones? stones, Orienteering_ISOM_2017_2.Water? water, Orienteering_ISOM_2017_2.VegetationAndManMade? vegetationAndManMade, Orienteering_ISOM_2017_2.VegetationGoodVis? vegetationGoodVis)>(() => {
                        if (surroundings.ground == Orienteering_ISOM_2017_2.Grounds.CultivatedLand_412 ||
                            surroundings.stones == Orienteering_ISOM_2017_2.Stones.SandyGround_213 ||
                            surroundings.stones == Orienteering_ISOM_2017_2.Stones.BareRock_214)
                            return surroundings;
                        return (null, null, null, Orienteering_ISOM_2017_2.Water.Marsh_308, surroundings.vegetationAndManMade, surroundings.vegetationGoodVis); })(),
                302 or 302.1m or 302.5m => (null, null, null, Orienteering_ISOM_2017_2.Water.ShallowBodyOfWater_302, null, null),
                113 => new Func<(Orienteering_ISOM_2017_2.Grounds? ground, Orienteering_ISOM_2017_2.Boulders? boulders, Orienteering_ISOM_2017_2.Stones? stones, Orienteering_ISOM_2017_2.Water? water, Orienteering_ISOM_2017_2.VegetationAndManMade? vegetationAndManMade, Orienteering_ISOM_2017_2.VegetationGoodVis? vegetationGoodVis)>(() => {
                        if (surroundings.ground == Orienteering_ISOM_2017_2.Grounds.CultivatedLand_412 ||
                            surroundings.stones == Orienteering_ISOM_2017_2.Stones.SandyGround_213 ||
                            surroundings.stones == Orienteering_ISOM_2017_2.Stones.BareRock_214 ||
                            surroundings.water == Orienteering_ISOM_2017_2.Water.ShallowBodyOfWater_302)
                            return surroundings;
                        return (Orienteering_ISOM_2017_2.Grounds.BrokenGround_113, null, null, surroundings.water, surroundings.vegetationAndManMade, surroundings.vegetationGoodVis); })(),
                114 => new Func<(Orienteering_ISOM_2017_2.Grounds? ground, Orienteering_ISOM_2017_2.Boulders? boulders, Orienteering_ISOM_2017_2.Stones? stones, Orienteering_ISOM_2017_2.Water? water, Orienteering_ISOM_2017_2.VegetationAndManMade? vegetationAndManMade, Orienteering_ISOM_2017_2.VegetationGoodVis? vegetationGoodVis)>(() => {
                        if (surroundings.ground == Orienteering_ISOM_2017_2.Grounds.CultivatedLand_412 ||
                            surroundings.stones == Orienteering_ISOM_2017_2.Stones.SandyGround_213 ||
                            surroundings.stones == Orienteering_ISOM_2017_2.Stones.BareRock_214 ||
                            surroundings.water == Orienteering_ISOM_2017_2.Water.ShallowBodyOfWater_302 ||
                            surroundings.vegetationAndManMade == Orienteering_ISOM_2017_2.VegetationAndManMade.OpenLand_401 ||
                            surroundings.vegetationAndManMade == Orienteering_ISOM_2017_2.VegetationAndManMade.OpenLandWithTrees_402 ||
                            surroundings.vegetationAndManMade == Orienteering_ISOM_2017_2.VegetationAndManMade.Orchard_413 ||
                            surroundings.vegetationAndManMade == Orienteering_ISOM_2017_2.VegetationAndManMade.Vineyard_414)
                            return surroundings;
                        return (Orienteering_ISOM_2017_2.Grounds.VeryBrokenGround_114, null, null, null, surroundings.vegetationAndManMade, surroundings.vegetationGoodVis); })(),
                210 => new Func<(Orienteering_ISOM_2017_2.Grounds? ground, Orienteering_ISOM_2017_2.Boulders? boulders, Orienteering_ISOM_2017_2.Stones? stones, Orienteering_ISOM_2017_2.Water? water, Orienteering_ISOM_2017_2.VegetationAndManMade? vegetationAndManMade, Orienteering_ISOM_2017_2.VegetationGoodVis? vegetationGoodVis)>( () => {
                        if (surroundings.ground == Orienteering_ISOM_2017_2.Grounds.CultivatedLand_412 || 
                            surroundings.stones == Orienteering_ISOM_2017_2.Stones.SandyGround_213 ||
                            surroundings.stones == Orienteering_ISOM_2017_2.Stones.BareRock_214 ||
                            surroundings.water == Orienteering_ISOM_2017_2.Water.ShallowBodyOfWater_302 ||
                            surroundings.vegetationAndManMade == Orienteering_ISOM_2017_2.VegetationAndManMade.OpenLand_401 ||
                            surroundings.vegetationAndManMade == Orienteering_ISOM_2017_2.VegetationAndManMade.OpenLandWithTrees_402 ||
                            surroundings.vegetationAndManMade == Orienteering_ISOM_2017_2.VegetationAndManMade.Orchard_413 ||
                            surroundings.vegetationAndManMade == Orienteering_ISOM_2017_2.VegetationAndManMade.Vineyard_414)
                            return surroundings;
                        if (surroundings.ground == Orienteering_ISOM_2017_2.Grounds.VeryBrokenGround_114)
                            return (null, null, Orienteering_ISOM_2017_2.Stones.StonyGroundSlow_210, surroundings.water, surroundings.vegetationAndManMade, surroundings.vegetationGoodVis);
                        return (surroundings.ground, null, Orienteering_ISOM_2017_2.Stones.StonyGroundSlow_210, surroundings.water, surroundings.vegetationAndManMade, surroundings.vegetationGoodVis); })(),
                211 => new Func<(Orienteering_ISOM_2017_2.Grounds? ground, Orienteering_ISOM_2017_2.Boulders? boulders, Orienteering_ISOM_2017_2.Stones? stones, Orienteering_ISOM_2017_2.Water? water, Orienteering_ISOM_2017_2.VegetationAndManMade? vegetationAndManMade, Orienteering_ISOM_2017_2.VegetationGoodVis? vegetationGoodVis)>(() => {
                        if ( surroundings.ground == Orienteering_ISOM_2017_2.Grounds.VeryBrokenGround_114 ||
                            surroundings.ground == Orienteering_ISOM_2017_2.Grounds.CultivatedLand_412 || 
                            surroundings.stones == Orienteering_ISOM_2017_2.Stones.SandyGround_213 ||
                            surroundings.stones == Orienteering_ISOM_2017_2.Stones.BareRock_214 ||
                            surroundings.water == Orienteering_ISOM_2017_2.Water.ShallowBodyOfWater_302 ||
                            surroundings.vegetationAndManMade == Orienteering_ISOM_2017_2.VegetationAndManMade.OpenLand_401 ||
                            surroundings.vegetationAndManMade == Orienteering_ISOM_2017_2.VegetationAndManMade.OpenLandWithTrees_402 ||
                            surroundings.vegetationAndManMade == Orienteering_ISOM_2017_2.VegetationAndManMade.Orchard_413 ||
                            surroundings.vegetationAndManMade == Orienteering_ISOM_2017_2.VegetationAndManMade.Vineyard_414)
                            return surroundings;
                        return (surroundings.ground, null, Orienteering_ISOM_2017_2.Stones.StonyGroundWalk_211, surroundings.water, surroundings.vegetationAndManMade, surroundings.vegetationGoodVis); })(),
                212 => new Func<(Orienteering_ISOM_2017_2.Grounds? ground, Orienteering_ISOM_2017_2.Boulders? boulders, Orienteering_ISOM_2017_2.Stones? stones, Orienteering_ISOM_2017_2.Water? water, Orienteering_ISOM_2017_2.VegetationAndManMade? vegetationAndManMade, Orienteering_ISOM_2017_2.VegetationGoodVis? vegetationGoodVis)>(() => {
                        if (surroundings.ground == Orienteering_ISOM_2017_2.Grounds.VeryBrokenGround_114 ||
                            surroundings.ground == Orienteering_ISOM_2017_2.Grounds.CultivatedLand_412 || 
                            surroundings.stones == Orienteering_ISOM_2017_2.Stones.SandyGround_213 ||
                            surroundings.stones == Orienteering_ISOM_2017_2.Stones.BareRock_214 ||
                            surroundings.water == Orienteering_ISOM_2017_2.Water.ShallowBodyOfWater_302 ||
                            surroundings.vegetationAndManMade == Orienteering_ISOM_2017_2.VegetationAndManMade.OpenLand_401 ||
                            surroundings.vegetationAndManMade == Orienteering_ISOM_2017_2.VegetationAndManMade.OpenLandWithTrees_402 ||
                            surroundings.vegetationAndManMade == Orienteering_ISOM_2017_2.VegetationAndManMade.Orchard_413 ||
                            surroundings.vegetationAndManMade == Orienteering_ISOM_2017_2.VegetationAndManMade.Vineyard_414)
                            return surroundings;
                        return (surroundings.ground, null, Orienteering_ISOM_2017_2.Stones.StonyGroundFight_212, surroundings.water, surroundings.vegetationAndManMade, surroundings.vegetationGoodVis); })(),
                208 => new Func<(Orienteering_ISOM_2017_2.Grounds? ground, Orienteering_ISOM_2017_2.Boulders? boulders, Orienteering_ISOM_2017_2.Stones? stones, Orienteering_ISOM_2017_2.Water? water, Orienteering_ISOM_2017_2.VegetationAndManMade? vegetationAndManMade, Orienteering_ISOM_2017_2.VegetationGoodVis? vegetationGoodVis)>(() => {
                        if (surroundings.ground == Orienteering_ISOM_2017_2.Grounds.CultivatedLand_412 || 
                            surroundings.stones == Orienteering_ISOM_2017_2.Stones.SandyGround_213 ||
                            surroundings.stones == Orienteering_ISOM_2017_2.Stones.BareRock_214 ||
                            surroundings.water == Orienteering_ISOM_2017_2.Water.ShallowBodyOfWater_302)
                            return surroundings;
                        return (surroundings.ground, Orienteering_ISOM_2017_2.Boulders.BoulderField_208, surroundings.stones, surroundings.water, surroundings.vegetationAndManMade, surroundings.vegetationGoodVis); })(),
                209 => new Func<(Orienteering_ISOM_2017_2.Grounds? ground, Orienteering_ISOM_2017_2.Boulders? boulders, Orienteering_ISOM_2017_2.Stones? stones, Orienteering_ISOM_2017_2.Water? water, Orienteering_ISOM_2017_2.VegetationAndManMade? vegetationAndManMade, Orienteering_ISOM_2017_2.VegetationGoodVis? vegetationGoodVis)>(() => {
                        if (surroundings.ground == Orienteering_ISOM_2017_2.Grounds.CultivatedLand_412 || 
                            surroundings.stones == Orienteering_ISOM_2017_2.Stones.SandyGround_213 ||
                            surroundings.stones == Orienteering_ISOM_2017_2.Stones.BareRock_214 ||
                            surroundings.water == Orienteering_ISOM_2017_2.Water.ShallowBodyOfWater_302 ||
                            surroundings.vegetationAndManMade == Orienteering_ISOM_2017_2.VegetationAndManMade.OpenLand_401 ||
                            surroundings.vegetationAndManMade == Orienteering_ISOM_2017_2.VegetationAndManMade.OpenLandWithTrees_402 ||
                            surroundings.vegetationAndManMade == Orienteering_ISOM_2017_2.VegetationAndManMade.Orchard_413 ||
                            surroundings.vegetationAndManMade == Orienteering_ISOM_2017_2.VegetationAndManMade.Vineyard_414)
                            return surroundings;
                        return (null, Orienteering_ISOM_2017_2.Boulders.DenseBoulderField_209, null, surroundings.water, surroundings.vegetationAndManMade, null); })(),
                501 or 501.1m => (null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.PavedArea_501, null),
                
                520 => (null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.AreaThatShallNotBeEntered_520, null),
                307 or 307.1m => new Func<(Orienteering_ISOM_2017_2.Grounds? ground, Orienteering_ISOM_2017_2.Boulders? boulders, Orienteering_ISOM_2017_2.Stones? stones, Orienteering_ISOM_2017_2.Water? water, Orienteering_ISOM_2017_2.VegetationAndManMade? vegetationAndManMade, Orienteering_ISOM_2017_2.VegetationGoodVis? vegetationGoodVis)>(() => {
                        if (surroundings.vegetationAndManMade == Orienteering_ISOM_2017_2.VegetationAndManMade.AreaThatShallNotBeEntered_520 ||
                            surroundings.vegetationAndManMade == Orienteering_ISOM_2017_2.VegetationAndManMade.PavedArea_501)
                            return surroundings;
                        return (null, null, null, Orienteering_ISOM_2017_2.Water.UncrossableMarsh_307, null, null); })(),
                301 or 301.1m or 301.2m or 301.3m => new Func<(Orienteering_ISOM_2017_2.Grounds? ground, Orienteering_ISOM_2017_2.Boulders? boulders, Orienteering_ISOM_2017_2.Stones? stones, Orienteering_ISOM_2017_2.Water? water, Orienteering_ISOM_2017_2.VegetationAndManMade? vegetationAndManMade, Orienteering_ISOM_2017_2.VegetationGoodVis? vegetationGoodVis)>(() => {
                        if (surroundings.vegetationAndManMade == Orienteering_ISOM_2017_2.VegetationAndManMade.PavedArea_501)
                            return surroundings;
                        return (null, null, null, Orienteering_ISOM_2017_2.Water.UncrossableBodyOfWater_301, null, null); })(),
                206 => (null, Orienteering_ISOM_2017_2.Boulders.GiganticBoulder_206, null, null, null, null),
                521 or 521.2m or 521.3m => (null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.Building_521, null),
                520.2m => (null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.AreaThatShallNotBeEntered_520, null),
                _ => surroundings
            };
        
        public static (Orienteering_ISOM_2017_2.NaturalLinearObstacles? naturalLinearObstacle, Orienteering_ISOM_2017_2.Paths? path, Orienteering_ISOM_2017_2.ManMadeLinearObstacles? manMadeLinearObstacle)
            GetUpdatedLinearFeaturesOfEdgeAttributes((Orienteering_ISOM_2017_2.NaturalLinearObstacles? naturalLinearObstacle, Orienteering_ISOM_2017_2.Paths? path, Orienteering_ISOM_2017_2.ManMadeLinearObstacles? manMadeLinearObstacle)  linearFeaturs, (Orienteering_ISOM_2017_2.Grounds? ground, Orienteering_ISOM_2017_2.Boulders? boulders, Orienteering_ISOM_2017_2.Stones? stones, Orienteering_ISOM_2017_2.Water? water, Orienteering_ISOM_2017_2.VegetationAndManMade? vegetationAndManMade, Orienteering_ISOM_2017_2.VegetationGoodVis? vegetationGoodVis) surroundings, (Orienteering_ISOM_2017_2.Grounds? ground, Orienteering_ISOM_2017_2.Boulders? boulders, Orienteering_ISOM_2017_2.Stones? stones, Orienteering_ISOM_2017_2.Water? water, Orienteering_ISOM_2017_2.VegetationAndManMade? vegetationAndManMade, Orienteering_ISOM_2017_2.VegetationGoodVis? vegetationGoodVis) secondSurroundings, decimal symbolCodeOfAddedObject, out (Orienteering_ISOM_2017_2.Grounds? ground, Orienteering_ISOM_2017_2.Boulders? boulders, Orienteering_ISOM_2017_2.Stones? stones, Orienteering_ISOM_2017_2.Water? water, Orienteering_ISOM_2017_2.VegetationAndManMade? vegetationAndManMade, Orienteering_ISOM_2017_2.VegetationGoodVis? vegetationGoodVis) newSurroundings, out (Orienteering_ISOM_2017_2.Grounds? ground, Orienteering_ISOM_2017_2.Boulders? boulders, Orienteering_ISOM_2017_2.Stones? stones, Orienteering_ISOM_2017_2.Water? water, Orienteering_ISOM_2017_2.VegetationAndManMade? vegetationAndManMade, Orienteering_ISOM_2017_2.VegetationGoodVis? vegetationGoodVis) newSecondSurroundings)
        {
            var updatedLineFeatures = symbolCodeOfAddedObject switch 
            {
                508 => (null, Orienteering_ISOM_2017_2.Paths.NarrowRide_508, null),
                508.2m => new Func<(Orienteering_ISOM_2017_2.NaturalLinearObstacles? naturalLinearObstacle, Orienteering_ISOM_2017_2.Paths? path, Orienteering_ISOM_2017_2.ManMadeLinearObstacles? manMadeLinearObstacle)>(() => {
                    surroundings = GetSurroundingsForEdgeWithNarrowRide(Orienteering_ISOM_2017_2.VegetationAndManMade.Forest_405, surroundings);
                    secondSurroundings = GetSurroundingsForEdgeWithNarrowRide(Orienteering_ISOM_2017_2.VegetationAndManMade.Forest_405, secondSurroundings);
                    return (null, Orienteering_ISOM_2017_2.Paths.NarrowRide_508, null); })(),
                508.3m => new Func<(Orienteering_ISOM_2017_2.NaturalLinearObstacles? naturalLinearObstacle, Orienteering_ISOM_2017_2.Paths? path, Orienteering_ISOM_2017_2.ManMadeLinearObstacles? manMadeLinearObstacle)>(() => {
                    surroundings = GetSurroundingsForEdgeWithNarrowRide(Orienteering_ISOM_2017_2.VegetationAndManMade.VegetationSlow_406, surroundings);
                    secondSurroundings = GetSurroundingsForEdgeWithNarrowRide(Orienteering_ISOM_2017_2.VegetationAndManMade.VegetationSlow_406, secondSurroundings);
                    return (null, Orienteering_ISOM_2017_2.Paths.NarrowRide_508, null); })(),
                508.4m => new Func<(Orienteering_ISOM_2017_2.NaturalLinearObstacles? naturalLinearObstacle, Orienteering_ISOM_2017_2.Paths? path, Orienteering_ISOM_2017_2.ManMadeLinearObstacles? manMadeLinearObstacle)>(() => {
                    surroundings = GetSurroundingsForEdgeWithNarrowRide(Orienteering_ISOM_2017_2.VegetationAndManMade.VegetationWalk_408, surroundings);
                    secondSurroundings = GetSurroundingsForEdgeWithNarrowRide(Orienteering_ISOM_2017_2.VegetationAndManMade.VegetationWalk_408, secondSurroundings);
                    return (null, Orienteering_ISOM_2017_2.Paths.NarrowRide_508, null); })(),
                508.1m => new Func<(Orienteering_ISOM_2017_2.NaturalLinearObstacles? naturalLinearObstacle, Orienteering_ISOM_2017_2.Paths? path, Orienteering_ISOM_2017_2.ManMadeLinearObstacles? manMadeLinearObstacle)>(() => { 
                    surroundings = GetSurroundingsForEdgeWithNarrowRide(Orienteering_ISOM_2017_2.VegetationAndManMade.OpenLand_401, surroundings);
                    secondSurroundings = GetSurroundingsForEdgeWithNarrowRide(Orienteering_ISOM_2017_2.VegetationAndManMade.OpenLand_401, secondSurroundings);
                    return (null, Orienteering_ISOM_2017_2.Paths.NarrowRide_508, null); })(),
                507 => (null, Orienteering_ISOM_2017_2.Paths.LessDistinctSmallFootpath_507 ,null),
                506 => (null, Orienteering_ISOM_2017_2.Paths.SmallFootpath_506, null),
                505 => (null, Orienteering_ISOM_2017_2.Paths.Footpath_505, null),
                504 => (null, Orienteering_ISOM_2017_2.Paths.VehicleTrack_504, null),
                503 => (null, Orienteering_ISOM_2017_2.Paths.Road_503, null),
                502 or 502.2m =>  (null, Orienteering_ISOM_2017_2.Paths.WideRoad_502, null),
                532 => (null, Orienteering_ISOM_2017_2.Paths.Stairway_532,null),
                104 or 104.2m => (Orienteering_ISOM_2017_2.NaturalLinearObstacles.EarthBank_104, linearFeaturs.path, linearFeaturs.manMadeLinearObstacle),
                105 => (Orienteering_ISOM_2017_2.NaturalLinearObstacles.EarthWall_105, linearFeaturs.path, linearFeaturs.manMadeLinearObstacle),
                107 => (Orienteering_ISOM_2017_2.NaturalLinearObstacles.ErosionGully_107, linearFeaturs.path, linearFeaturs.manMadeLinearObstacle),
                305 => (Orienteering_ISOM_2017_2.NaturalLinearObstacles.SmallCrossableWatercourse_305, linearFeaturs.path, linearFeaturs.manMadeLinearObstacle),
                304 => (Orienteering_ISOM_2017_2.NaturalLinearObstacles.CrossableWatercourse_304, linearFeaturs.path, linearFeaturs.manMadeLinearObstacle),
                201 or 201.3m => (ImapssableCliff_201: Orienteering_ISOM_2017_2.NaturalLinearObstacles.ImpassableCliff_201, linearFeaturs.path, linearFeaturs.manMadeLinearObstacle),
                202 or 202.2m => (Orienteering_ISOM_2017_2.NaturalLinearObstacles.Cliff_202, linearFeaturs.path, linearFeaturs.manMadeLinearObstacle),
                215 => (linearFeaturs.naturalLinearObstacle, linearFeaturs.path, Orienteering_ISOM_2017_2.ManMadeLinearObstacles.Trench_215),
                513 => (linearFeaturs.naturalLinearObstacle, linearFeaturs.path, Orienteering_ISOM_2017_2.ManMadeLinearObstacles.Wall_513),   
                515 => (linearFeaturs.naturalLinearObstacle, linearFeaturs.path, Orienteering_ISOM_2017_2.ManMadeLinearObstacles.ImpassableWall_515),   
                516 => (linearFeaturs.naturalLinearObstacle, linearFeaturs.path, Orienteering_ISOM_2017_2.ManMadeLinearObstacles.Fence_516),     
                518 => (linearFeaturs.naturalLinearObstacle, linearFeaturs.path, Orienteering_ISOM_2017_2.ManMadeLinearObstacles.ImpassableFence_518), 
                528 => (linearFeaturs.naturalLinearObstacle, linearFeaturs.path, Orienteering_ISOM_2017_2.ManMadeLinearObstacles.ProminentLineFeature_528),
                529 => (linearFeaturs.naturalLinearObstacle, linearFeaturs.path, Orienteering_ISOM_2017_2.ManMadeLinearObstacles.ImpassableProminentLineFeature_529),
                _ => linearFeaturs
            };
            newSurroundings = surroundings;
            newSecondSurroundings = secondSurroundings;
            return updatedLineFeatures;
        }

        private static (Orienteering_ISOM_2017_2.Grounds? ground, Orienteering_ISOM_2017_2.Boulders? boulders, Orienteering_ISOM_2017_2.Stones? stones, Orienteering_ISOM_2017_2.Water? water, Orienteering_ISOM_2017_2.VegetationAndManMade? vegetationAndManMade, Orienteering_ISOM_2017_2.VegetationGoodVis? vegetationGoodVis)
            GetSurroundingsForEdgeWithNarrowRide(Orienteering_ISOM_2017_2.VegetationAndManMade narrowRideSurroundings, (Orienteering_ISOM_2017_2.Grounds? ground, Orienteering_ISOM_2017_2.Boulders? boulders, Orienteering_ISOM_2017_2.Stones? stones, Orienteering_ISOM_2017_2.Water? water, Orienteering_ISOM_2017_2.VegetationAndManMade? vegetationAndManMade, Orienteering_ISOM_2017_2.VegetationGoodVis? vegetationGoodVis) originalSurroundings)
        {
            if (!(originalSurroundings.ground == Orienteering_ISOM_2017_2.Grounds.CultivatedLand_412 ||
                  originalSurroundings.boulders == Orienteering_ISOM_2017_2.Boulders.GiganticBoulder_206 ||
                  originalSurroundings.stones == Orienteering_ISOM_2017_2.Stones.SandyGround_213 ||
                  originalSurroundings.water == Orienteering_ISOM_2017_2.Water.UncrossableMarsh_307 ||
                  originalSurroundings.water == Orienteering_ISOM_2017_2.Water.ShallowBodyOfWater_302 ||
                  originalSurroundings.vegetationAndManMade == Orienteering_ISOM_2017_2.VegetationAndManMade.PavedArea_501 ||
                  originalSurroundings.vegetationAndManMade == Orienteering_ISOM_2017_2.VegetationAndManMade.AreaThatShallNotBeEntered_520 ||
                  originalSurroundings.vegetationAndManMade == Orienteering_ISOM_2017_2.VegetationAndManMade.Building_521)) 
                return (originalSurroundings.ground, originalSurroundings.boulders, originalSurroundings.stones, originalSurroundings.water, narrowRideSurroundings, null);
            return originalSurroundings;
        }
        
        #endregion
        
        #region Computing of left handed angle between two edges \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        
        public static double ComputeLeftHandedAngleBetween(MapCoordinates v1s, MapCoordinates v1e, MapCoordinates v2s, MapCoordinates v2e)
        {
            var v1 = v1e - v1s;
            var v2 = v2e - v2s;
            var cosValue = (v1 * v2)/(v1.Length()*v2.Length());
            double angle = Math.Acos(cosValue < -1? -1 : cosValue > 1 ? 1 : cosValue);
            // if scalar product of vector 2 and right side normal vector of vector 1 is positive, computed angle is inverted 
            var v1RightNorm = v1.RightHandNormalVector(); 
            return v1RightNorm * v2 > 0 ? 2*Math.PI - angle : angle;
        }
        
        #endregion
        
        #region Cross-section of chain and boundary edges vertices retrieving and their connection to chain vertices \\\
        
        public static (Dictionary<(BoundaryVertexBuilder<TVertex, TEdge>, BoundaryVertexBuilder<TVertex, TEdge>), (List<BoundaryVertexBuilder<TVertex, TEdge>>, int)>, Dictionary<(BoundaryVertexBuilder<TVertex, TEdge>, BoundaryVertexBuilder<TVertex, TEdge>), List<BoundaryVertexBuilder<TVertex, TEdge>>>) 
            CreateCrossSectionVertices(BoundaryVertexBuilder<TVertex, TEdge>[][] chains, Dictionary<(BoundaryVertexBuilder<TVertex, TEdge>, BoundaryVertexBuilder<TVertex, TEdge>), (Orienteering_ISOM_2017_2.EdgeAttributes, List<(MapCoordinates, int, int)>)> verticesOfCutBoundaryEdges)
        {
            Dictionary<(BoundaryVertexBuilder<TVertex, TEdge>, BoundaryVertexBuilder<TVertex, TEdge>), (List<BoundaryVertexBuilder<TVertex, TEdge>>, int)> chainVerticesOfCrossedEdgesWithTheirCrossSectionVerticesAndChainIndex = new();
            Dictionary<(BoundaryVertexBuilder<TVertex, TEdge>, BoundaryVertexBuilder<TVertex, TEdge>), List<BoundaryVertexBuilder<TVertex, TEdge>>> verticesOfCrossedBoundaryEdgesWithTheirCrossSectionVertices = new();
            // process every boundary vertex couple whose edge was cut by the chain
            foreach (var ((vertex1, vertex2), (_, crossSectionsWithChainsAndEdgesIndices)) in verticesOfCutBoundaryEdges)
            {
                foreach (var (crossSectionCoords, crossedChainIndex, crossedEdgeIndex) in crossSectionsWithChainsAndEdgesIndices)
                {
                    var chainVertex1 = chains[crossedChainIndex][crossedEdgeIndex];
                    // crossed chain edge index will never be equal to chain.Count - 1, if chain is segmented line and not polygon
                    var chainVertex2 = crossedEdgeIndex == chains[crossedChainIndex].Length - 1 ? chains[crossedChainIndex][0] : chains[crossedChainIndex][crossedEdgeIndex + 1];
                    // creation of cross-section vertex and its addition to list of added vertices and list of added vertices for given cut boundary edge and chain edge
                    var newVertex = new BoundaryVertexBuilder<TVertex, TEdge>(new Orienteering_ISOM_2017_2.VertexAttributes(crossSectionCoords));
                    if (chainVerticesOfCrossedEdgesWithTheirCrossSectionVerticesAndChainIndex.ContainsKey((chainVertex1, chainVertex2))) chainVerticesOfCrossedEdgesWithTheirCrossSectionVerticesAndChainIndex[(chainVertex1, chainVertex2)].Item1.Add(newVertex);
                    else chainVerticesOfCrossedEdgesWithTheirCrossSectionVerticesAndChainIndex[(chainVertex1, chainVertex2)] = ([newVertex], crossedChainIndex);
                    if (verticesOfCrossedBoundaryEdgesWithTheirCrossSectionVertices.ContainsKey((vertex1, vertex2))) verticesOfCrossedBoundaryEdgesWithTheirCrossSectionVertices[(vertex1, vertex2)].Add(newVertex);
                    else verticesOfCrossedBoundaryEdgesWithTheirCrossSectionVertices[(vertex1, vertex2)] = [newVertex];
                }
            }
            return (chainVerticesOfCrossedEdgesWithTheirCrossSectionVerticesAndChainIndex, verticesOfCrossedBoundaryEdgesWithTheirCrossSectionVertices);
        }
        


        public static void ConnectAndAddNewVerticesToChain( Dictionary<(BoundaryVertexBuilder<TVertex, TEdge>, BoundaryVertexBuilder<TVertex, TEdge>), (List<BoundaryVertexBuilder<TVertex, TEdge>>, int)> chainVerticesOfCrossedEdgesWithTheirCrossSectionVerticesAndChainIndex, List<BoundaryVertexBuilder<TVertex, TEdge>>[] chains)
        {
            foreach (var ((vertex1, vertex2), (newVertices, chainIndex)) in chainVerticesOfCrossedEdgesWithTheirCrossSectionVerticesAndChainIndex)
            {
                // at first, we have to sort cross-section vertices so they could be correctly connected to the chain vertices
                var newSortedVertices = SortVerticesCorrectly(vertex1, vertex2, newVertices);
                ConnectNewVerticesToChain(newSortedVertices,  vertex1, vertex2);
                AddNewVerticesToChain(newSortedVertices, vertex1, vertex2, chains, chainIndex);
            }
        }

        private static void ConnectNewVerticesToChain(List<BoundaryVertexBuilder<TVertex, TEdge>> sortedVertices, BoundaryVertexBuilder<TVertex, TEdge> vertex1, BoundaryVertexBuilder<TVertex, TEdge> vertex2)
        {
            // at first, edge between two crossed chain vertices must be removed
            vertex1.BoundaryEdges.Remove(vertex2);
            vertex2.BoundaryEdges.Remove(vertex1);
            var lastVertex = vertex1;
            // then new vertices are iterattively connected to chain vertices 
            foreach (var newVertex in sortedVertices)
            {
                lastVertex.BoundaryEdges[newVertex] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                newVertex.BoundaryEdges[lastVertex] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                lastVertex = newVertex;
            }
            lastVertex.BoundaryEdges[vertex2] = new Orienteering_ISOM_2017_2.EdgeAttributes();
            vertex2.BoundaryEdges[lastVertex] = new Orienteering_ISOM_2017_2.EdgeAttributes();
        }

        private static void AddNewVerticesToChain(List<BoundaryVertexBuilder<TVertex, TEdge>> sortedVertices, BoundaryVertexBuilder<TVertex, TEdge> vertex1, BoundaryVertexBuilder<TVertex, TEdge> vertex2, List<BoundaryVertexBuilder<TVertex, TEdge>>[] chains, int chainIndex)
        {
            int vertexIndex = chains[chainIndex].IndexOf(vertex1); 
            var lastVertex = vertex1;
            // newly created cross-section vertices are added to the chain
            foreach (var vertex in sortedVertices)
            {
                
                // we have to check if newly added vertex does not overlap with the one that is going to be connected with
                // in case of overlap, the new vertex is disconnected from the chain and is not added to the chain
                var overlappedVertex = vertex.Position == lastVertex.Position 
                        ? lastVertex : vertex.Position == vertex2.Position 
                            ? vertex2 : null;
                if (overlappedVertex is not null)
                {
                    vertex.BoundaryEdges.Remove(overlappedVertex);
                    overlappedVertex.BoundaryEdges.Remove(vertex); 
                    ReplaceBoundaryVertexForTheOtherOne(vertex, overlappedVertex);
                    if (overlappedVertex == vertex2 && lastVertex != vertex2)
                    {
                        lastVertex = vertex2;
                        vertexIndex++;
                    }
                }
                else
                    chains[chainIndex].Insert(++vertexIndex, vertex);
            }
        }
        
        public static void ReplaceBoundaryVertexForTheOtherOne(BoundaryVertexBuilder<TVertex, TEdge> vertex, BoundaryVertexBuilder<TVertex, TEdge> theOtherOne)
        {
            // all edges of the boundary vertex are added to the other one, so if the other one has some same edge as vertex, it is overwritten
            foreach (var (neighbor, edgeAttributesWithNeighbor) in vertex.NonBoundaryEdges)
            {
                if (neighbor == vertex) continue; // TODO: resolve issue with self-pointing edges
                theOtherOne.NonBoundaryEdges[neighbor] = edgeAttributesWithNeighbor;
                if (neighbor is BoundaryVertexBuilder<TVertex, TEdge> boundaryNeighbor)
                {
                    boundaryNeighbor.NonBoundaryEdges[theOtherOne] = boundaryNeighbor.NonBoundaryEdges[vertex];
                    boundaryNeighbor.NonBoundaryEdges.Remove(vertex);
                }
                else if (neighbor is NetVertexBuilder<TVertex, TEdge> netNeighbor)
                {
                    netNeighbor.NonBoundaryEdges[theOtherOne] = netNeighbor.NonBoundaryEdges[vertex];
                    netNeighbor.NonBoundaryEdges.Remove(vertex);
                }
            }
            vertex.NonBoundaryEdges.Clear();
            foreach (var (neighbor, edgeAttributesWithNeighbor) in vertex.BoundaryEdges)
            {
                if (neighbor == vertex) continue; // TODO: resolve issue with self-pointing edges
                theOtherOne.BoundaryEdges[neighbor] = edgeAttributesWithNeighbor;
                neighbor.BoundaryEdges[theOtherOne] = neighbor.BoundaryEdges[vertex];
                neighbor.BoundaryEdges.Remove(vertex);
            }
            vertex.BoundaryEdges.Clear();
        }
        
        #endregion
        
        #region Connecting of chain vertices to vertices of cut edges and to itself by non boundary edges \\\\\\\\\\\\\\
        public static void ConnectChainVerticesToVerticesOfCutEdgesAndOtherVerticesOfChain(BoundaryVertexBuilder<TVertex, TEdge>[] chainVertices, IEnumerable<VertexBuilder<TVertex, TEdge>> verticesOfCutEdges, IEditableRadiallySearchableDataStruct<VertexBuilder<TVertex, TEdge>> allVertices, int standardEdgeLength)
        {
            // connectable vertices consists of chain vertices and all vertices of edges, which were cut by chain
            IEditableRadiallySearchableDataStruct<VertexBuilder<TVertex, TEdge>> connectableVertices = new RadiallySearchableKdTree<VertexBuilder<TVertex, TEdge>>(chainVertices.Concat(verticesOfCutEdges), vertexBuilder => (vertexBuilder.Position.XPos, vertexBuilder.Position.YPos));
            
            // at first, we have to find all boundary vertices in standard edge length distance from vertices of chain
            // these boundary vertices will also include chain vertices
            // we will need them so we could test, whether newly added edges does not cross some boundary edge of previously added objects or currently processed object
            var closeBoundaryVerticesToChainsVertices = FindCloseBoundaryVerticesToChainVertices(chainVertices, allVertices, standardEdgeLength);
            for (int i = 0; i < chainVertices.Length; ++i)
            {
                var closeVertices = connectableVertices.FindInEuclideanDistanceFrom((chainVertices[i].Position.XPos, chainVertices[i].Position.YPos), standardEdgeLength);
                foreach (var closeVertex in closeVertices)
                    // we do not connect vertices by non boundary edges to vertices which are connected to current vertex by boundary edge already 
                    if (closeVertex is not BoundaryVertexBuilder<TVertex, TEdge> boundaryCloseVertex || !chainVertices[i].BoundaryEdges.ContainsKey(boundaryCloseVertex))
                        // check, if created edge does not cut any boundary edge 
                        if (DoesNotCrossSomeBoundaryEdge(chainVertices[i], closeVertex, closeBoundaryVerticesToChainsVertices[i]))
                            if (closeVertex is NetVertexBuilder<TVertex, TEdge> closeNetVertex)
                                ConnectBoundaryVertexAndNetVertex(chainVertices[i], closeNetVertex);
                            else if (closeVertex is BoundaryVertexBuilder<TVertex, TEdge> closeBoundaryVertex && !chainVertices[i].BoundaryEdges.ContainsKey(closeBoundaryVertex))
                                ConnectTwoBoundaryVerticesWithNonBoundaryEdge(chainVertices[i], closeBoundaryVertex);
            }
            
        }
        
        private static List<List<BoundaryVertexBuilder<TVertex, TEdge>>> FindCloseBoundaryVerticesToChainVertices(BoundaryVertexBuilder<TVertex, TEdge>[] chainVertices, IEditableRadiallySearchableDataStruct<VertexBuilder<TVertex, TEdge>> allVertices,  int standardEdgeLength)
        {
            List<List<BoundaryVertexBuilder<TVertex, TEdge>>> closeBoundaryVerticesToChainsVertices = new List<List<BoundaryVertexBuilder<TVertex, TEdge>>>();
            IRadiallySearchableDataStruct<BoundaryVertexBuilder<TVertex, TEdge>> searchableChain = new RadiallySearchableKdTree<BoundaryVertexBuilder<TVertex, TEdge>>(chainVertices, vertexBuilder => (vertexBuilder.Position.XPos, vertexBuilder.Position.YPos));
            foreach (var chainVertex in chainVertices)
            {
                var allCloseVerticesToChainVertex = allVertices.FindInEuclideanDistanceFrom((chainVertex.Position.XPos, chainVertex.Position.YPos), standardEdgeLength);
                closeBoundaryVerticesToChainsVertices.Add(allCloseVerticesToChainVertex.OfType<BoundaryVertexBuilder<TVertex, TEdge>>().ToList());
                var allCloseChainVerticesToChainVertex = searchableChain.FindInEuclideanDistanceFrom((chainVertex.Position.XPos, chainVertex.Position.YPos), standardEdgeLength);
                closeBoundaryVerticesToChainsVertices.Last().AddRange(allCloseChainVerticesToChainVertex);
            }
            return closeBoundaryVerticesToChainsVertices;
        }

        private static bool DoesNotCrossSomeBoundaryEdge(BoundaryVertexBuilder<TVertex, TEdge> chainVertex, VertexBuilder<TVertex, TEdge> closeVertex, List<BoundaryVertexBuilder<TVertex, TEdge>> closeBoundaryVerticesToBeChecked)
        {
            foreach (var closeBoundaryVertex in closeBoundaryVerticesToBeChecked)
            {
                // if close boundary vertex is tested close vertex or tested chain vertex, we do not test them
                if (closeVertex == closeBoundaryVertex || chainVertex == closeBoundaryVertex) continue;
                foreach (var boundaryNeighborOfCloseBoundaryVertex in closeBoundaryVertex.BoundaryEdges.Keys)
                {
                    // same think holds in case of neighbor of close boundary vertex
                    if (closeVertex == boundaryNeighborOfCloseBoundaryVertex || chainVertex == boundaryNeighborOfCloseBoundaryVertex) continue;
                    if (AreLineSegmentsCrossingEndPointsExcluded( closeBoundaryVertex.Position, boundaryNeighborOfCloseBoundaryVertex.Position, chainVertex.Position, closeVertex.Position )) return false;
                }
            }
            return true;
        }

        private static void ConnectBoundaryVertexAndNetVertex(BoundaryVertexBuilder<TVertex, TEdge> boundaryVertex, NetVertexBuilder<TVertex, TEdge> netVertex)
        {
            // net vertices are connected by edges whose attributes are correctly selected based on attributes of another edge of net vertex
            // here is used the assumption, that edges of net vertices have same attributes
            boundaryVertex.NonBoundaryEdges[netVertex] = new Orienteering_ISOM_2017_2.EdgeAttributes(netVertex.Surroundings);
            netVertex.NonBoundaryEdges[boundaryVertex] = (null, null, null);
        }

        private static void ConnectTwoBoundaryVerticesWithNonBoundaryEdge(BoundaryVertexBuilder<TVertex, TEdge> boundaryVertex1, BoundaryVertexBuilder<TVertex, TEdge> boundaryVertex2)
        {
            // edges between boundary edges will be assigned with blank attributes
            // correct attributes will be assigned to these edges in the future
            boundaryVertex1.NonBoundaryEdges[boundaryVertex2] = new Orienteering_ISOM_2017_2.EdgeAttributes();
            boundaryVertex2.NonBoundaryEdges[boundaryVertex1] = new Orienteering_ISOM_2017_2.EdgeAttributes();
        }
        
        #endregion
        
        #region Setting of chain edges \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        
        public static void SetAttributesOfChainBoundaryEdgesBasedOnGivenLeftSurroundingsOnGivenInterval(BoundaryVertexBuilder<TVertex, TEdge>[] chain, (Orienteering_ISOM_2017_2.Grounds?, Orienteering_ISOM_2017_2.Boulders?, Orienteering_ISOM_2017_2.Stones?, Orienteering_ISOM_2017_2.Water?, Orienteering_ISOM_2017_2.VegetationAndManMade?, Orienteering_ISOM_2017_2.VegetationGoodVis?) outerSurroundings, decimal symbolCodeOfAddedObject, int startIndex, int endIndex)
            => SetLeftSurroundingsUpdatedRightSurroundingsAndUpdatedLinearFeaturesOfChainBoundaryEdgesOnGivenInterval(chain, outerSurroundings, (null, null, null), symbolCodeOfAddedObject, startIndex, endIndex);
        
        //private for the time being
        private static void SetLeftSurroundingsUpdatedRightSurroundingsAndUpdatedLinearFeaturesOfChainBoundaryEdgesOnGivenInterval(BoundaryVertexBuilder<TVertex, TEdge>[] chain, (Orienteering_ISOM_2017_2.Grounds?, Orienteering_ISOM_2017_2.Boulders?, Orienteering_ISOM_2017_2.Stones?, Orienteering_ISOM_2017_2.Water?, Orienteering_ISOM_2017_2.VegetationAndManMade?, Orienteering_ISOM_2017_2.VegetationGoodVis?) leftSurroundings, (Orienteering_ISOM_2017_2.NaturalLinearObstacles? naturalLinearObstacle, Orienteering_ISOM_2017_2.Paths? path, Orienteering_ISOM_2017_2.ManMadeLinearObstacles? manMadeLinearObstacle) linearFeatures, decimal symbolCodeOfAddedObject, int startIndex, int endIndex)
        {
            if (startIndex >= endIndex || startIndex < 0 || endIndex >= chain.Length) return;
            var rightSurroundings = GetUpdatedSurroundingsOfEdgeAttributes(leftSurroundings, symbolCodeOfAddedObject);
            var linearFeaturesInDirection = GetUpdatedLinearFeaturesOfEdgeAttributes(linearFeatures, leftSurroundings, rightSurroundings, symbolCodeOfAddedObject, out leftSurroundings, out rightSurroundings);
            var linearFeaturesInOppositeDirection = GetUpdatedLinearFeaturesOfEdgeAttributes(linearFeatures, rightSurroundings, leftSurroundings, symbolCodeOfAddedObject, out rightSurroundings, out leftSurroundings);
            
            chain[startIndex].BoundaryEdges[chain[startIndex + 1]] = new Orienteering_ISOM_2017_2.EdgeAttributes(leftSurroundings, rightSurroundings);
            for (int i = startIndex + 1; i < endIndex; ++i)
            {
                chain[i].BoundaryEdges[chain[i + 1]] = new Orienteering_ISOM_2017_2.EdgeAttributes( leftSurroundings, linearFeaturesInDirection, rightSurroundings);
                chain[i].BoundaryEdges[chain[i - 1]] = new Orienteering_ISOM_2017_2.EdgeAttributes( rightSurroundings, linearFeaturesInOppositeDirection, leftSurroundings);
            }
            chain[endIndex].BoundaryEdges[chain[endIndex - 1]] = new Orienteering_ISOM_2017_2.EdgeAttributes(rightSurroundings, leftSurroundings); 
        }
        
        #endregion
        
        #region Setting non boundary attributes between chain and boundary vertices \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        
        public static void SetAttributesOfNonBoundaryEdgesBetweenChainVerticesAndBoundaryVertices( IEnumerable<BoundaryVertexBuilder<TVertex, TEdge>> chainVertices)
        {
            foreach (var chainVertex in chainVertices)
            {
                SetAttributesOfNonBoundaryEdgesDirectedToBoundaryVertices(chainVertex);
            }
        }

        private static void SetAttributesOfNonBoundaryEdgesDirectedToBoundaryVertices(BoundaryVertexBuilder<TVertex, TEdge> chainVertex)
        {
            foreach (var (neighbor, edgeAttributes) in chainVertex.NonBoundaryEdges)
            {
                if (neighbor is BoundaryVertexBuilder<TVertex, TEdge> boundaryNeighbor && edgeAttributes == new Orienteering_ISOM_2017_2.EdgeAttributes())
                {
                    // if (chainVertex.Position == new MapCoordinates(776100, 2535)){} // for debugging
                    var attributesOfTheClosestLefHandedEdge = FindAttributesOfClosestLeftHandedBoundaryEdgeOfChainVertexTo(boundaryNeighbor, chainVertex);
                    chainVertex.NonBoundaryEdges[boundaryNeighbor] = new Orienteering_ISOM_2017_2.EdgeAttributes(attributesOfTheClosestLefHandedEdge.RightSurroundings);
                    boundaryNeighbor.NonBoundaryEdges[chainVertex] = new Orienteering_ISOM_2017_2.EdgeAttributes(attributesOfTheClosestLefHandedEdge.RightSurroundings);
                }
            }
        }

        private static Orienteering_ISOM_2017_2.EdgeAttributes FindAttributesOfClosestLeftHandedBoundaryEdgeOfChainVertexTo(
            VertexBuilder<TVertex, TEdge> neighbor, BoundaryVertexBuilder<TVertex, TEdge> chainVertex)
        {
            var angleOfTheClosestLefHandedBoundaryNeighborSoFar = 10.0; // greater than 2 * Math.Pi
            // by default set to forest attributes, will be surly overwritten 
            Orienteering_ISOM_2017_2.EdgeAttributes attributesOfTheClosestLeftHandedEdge = new Orienteering_ISOM_2017_2.EdgeAttributes((null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.Forest_405, null));
            foreach (var (boundaryNeighbor, edgeAttributes) in chainVertex.BoundaryEdges)
            {
                var angleOfTheCurrentBoundaryNeighbor = ComputeLeftHandedAngleBetween(chainVertex.Position, neighbor.Position, chainVertex.Position, boundaryNeighbor.Position);
                if (angleOfTheCurrentBoundaryNeighbor < angleOfTheClosestLefHandedBoundaryNeighborSoFar)
                {
                    attributesOfTheClosestLeftHandedEdge = edgeAttributes;
                    angleOfTheClosestLefHandedBoundaryNeighborSoFar = angleOfTheCurrentBoundaryNeighbor;
                }
            }
            return attributesOfTheClosestLeftHandedEdge;
        }
        
        #endregion
        
        #region Adding chain to the graph collection \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        
        public static void AddChainVerticesToTheGraph(IEnumerable<BoundaryVertexBuilder<TVertex, TEdge>> chain, IEditableRadiallySearchableDataStruct<VertexBuilder<TVertex, TEdge>> allVertices)
        {
            foreach (var chainVertex in chain)
            {
                if (allVertices.TryFindAtPositionOf(chainVertex, out var foundOverlappedVertex))
                {
                    ResolveAndDisconnectOverlappedGraphVertex(chainVertex, foundOverlappedVertex);
                    allVertices.Delete(foundOverlappedVertex);
                }
                allVertices.Insert(chainVertex);
            }
        }

        private static void ResolveAndDisconnectOverlappedGraphVertex(BoundaryVertexBuilder<TVertex, TEdge> chainVertex, VertexBuilder<TVertex, TEdge> overlappedVertex)
        {
            // we disconnect all overlapped vertex edges, and we add them to the chain vertex (if the chain vertex does not contain given edge already)
            if (overlappedVertex is BoundaryVertexBuilder<TVertex, TEdge> overlappedBoundaryVertex)
            {
                foreach (var (overlappedBoundaryVertexNeighbor, edgeAttributes) in overlappedBoundaryVertex.BoundaryEdges)
                {
                    if (overlappedBoundaryVertexNeighbor == overlappedBoundaryVertex) continue; // TODO: resolve issue with self-pointing edges
                    if (!chainVertex.BoundaryEdges.ContainsKey(overlappedBoundaryVertexNeighbor))
                    {
                        chainVertex.BoundaryEdges[overlappedBoundaryVertexNeighbor] = edgeAttributes;
                        overlappedBoundaryVertexNeighbor.BoundaryEdges[chainVertex] = overlappedBoundaryVertexNeighbor.BoundaryEdges[overlappedBoundaryVertex];
                    }
                    overlappedBoundaryVertexNeighbor.BoundaryEdges.Remove(overlappedBoundaryVertex);
                }
                overlappedBoundaryVertex.BoundaryEdges.Clear();
                foreach (var (overlappedBoundaryVertexNeighbor, edgeAttributes) in overlappedBoundaryVertex.NonBoundaryEdges)
                {
                    if (overlappedBoundaryVertexNeighbor == overlappedBoundaryVertex) continue; // TODO: resolve issue with self-pointing edges
                    if (overlappedBoundaryVertexNeighbor is BoundaryVertexBuilder<TVertex, TEdge> overlappedBoundaryVertexBoundaryNeighbor)
                    {
                        if (!chainVertex.NonBoundaryEdges.ContainsKey(overlappedBoundaryVertexBoundaryNeighbor))
                        {
                            chainVertex.NonBoundaryEdges[overlappedBoundaryVertexBoundaryNeighbor] = edgeAttributes;
                            overlappedBoundaryVertexBoundaryNeighbor.NonBoundaryEdges[chainVertex] = overlappedBoundaryVertexBoundaryNeighbor.NonBoundaryEdges[overlappedBoundaryVertex];
                        }
                        overlappedBoundaryVertexBoundaryNeighbor.NonBoundaryEdges.Remove(overlappedBoundaryVertex);
                    }
                    else if (overlappedBoundaryVertexNeighbor is NetVertexBuilder<TVertex, TEdge> overlappedBoundaryVertexNetNeighbor)
                    {
                        if (!chainVertex.NonBoundaryEdges.ContainsKey(overlappedBoundaryVertexNetNeighbor))
                        {
                            chainVertex.NonBoundaryEdges[overlappedBoundaryVertexNetNeighbor] = edgeAttributes;
                            overlappedBoundaryVertexNetNeighbor.NonBoundaryEdges[chainVertex] = edgeAttributes.LineFeatures;
                        }
                        overlappedBoundaryVertexNetNeighbor.NonBoundaryEdges.Remove(overlappedBoundaryVertex);
                    }
                }
                overlappedBoundaryVertex.NonBoundaryEdges.Clear();
            }
            else if (overlappedVertex is NetVertexBuilder<TVertex, TEdge> overlappedNetVertex)
            {
                foreach (var (overlappedNetVertexNeighbor, linearFeatures) in overlappedNetVertex.NonBoundaryEdges)
                {
                    if (overlappedNetVertexNeighbor == overlappedNetVertex) continue; // TODO: resolve issue with self-pointing edges
                    if (overlappedNetVertexNeighbor is BoundaryVertexBuilder<TVertex, TEdge> overlappedNetVertexBoundaryNeighbor)
                    {
                        if (!chainVertex.NonBoundaryEdges.ContainsKey(overlappedNetVertexBoundaryNeighbor))
                        {
                            chainVertex.NonBoundaryEdges[overlappedNetVertexBoundaryNeighbor] = new Orienteering_ISOM_2017_2.EdgeAttributes(overlappedNetVertex.Surroundings, linearFeatures);
                            overlappedNetVertexBoundaryNeighbor.NonBoundaryEdges[chainVertex] = overlappedNetVertexBoundaryNeighbor.NonBoundaryEdges[overlappedNetVertex];
                        }
                        overlappedNetVertexBoundaryNeighbor.NonBoundaryEdges.Remove(overlappedNetVertex);
                    }
                    else if (overlappedNetVertexNeighbor is NetVertexBuilder<TVertex, TEdge> overlappedNetVertexNetNeighbor)
                    {
                        if (!chainVertex.NonBoundaryEdges.ContainsKey(overlappedNetVertexNetNeighbor))
                        {
                            chainVertex.NonBoundaryEdges[overlappedNetVertexNetNeighbor] = new Orienteering_ISOM_2017_2.EdgeAttributes(overlappedNetVertex.Surroundings, linearFeatures);
                            overlappedNetVertexNetNeighbor.NonBoundaryEdges[chainVertex] = linearFeatures;
                        }
                        overlappedNetVertexNetNeighbor.NonBoundaryEdges.Remove(overlappedNetVertex);
                    }
                }
                overlappedNetVertex.NonBoundaryEdges.Clear();
            }
        }
        
        #endregion
    }
}
    

public abstract class VertexBuilder<TVertex, TEdge>(Orienteering_ISOM_2017_2.VertexAttributes attributesNoElev) 
    where TVertex : IBuildableVertex<TEdge>, IInitializableVertex<TEdge, Orienteering_ISOM_2017_2.VertexAttributes>, new()
    where TEdge : IEdge, IInitializableEdge<TVertex, Orienteering_ISOM_2017_2.EdgeAttributes>, new()
{
    public Orienteering_ISOM_2017_2.VertexAttributes AttributesNoElev = attributesNoElev;

    // public abstract HashSet<VertexBuilder> NonBoundaryEdges { get; }
    public TVertex? BuiltVertex { get; private set; }

    public MapCoordinates Position => AttributesNoElev.Position;
    public abstract bool IsStationary { get; set; }
    
    public TVertex Build((IElevData elevData, GeoCoordinates referencePoint, int scale, double declination)? withElevation = null)
    {
        if (BuiltVertex is not null) return BuiltVertex;
        if (withElevation is not null)
        {
            // map and its graph representation are rotated to north magnetic pole. To compensate this rotation while retrieving corresponding elevation data, we must rotate position clockwise with declination (which is angular difference between north magnetic and north pole).
            double? elevation = withElevation.Value.elevData.GetElevation(Position.RotateCounterClockwise(-double.DegreesToRadians(withElevation.Value.declination)), withElevation.Value.referencePoint, withElevation.Value.scale);
            BuiltVertex = new TVertex{ Attributes = new Orienteering_ISOM_2017_2.VertexAttributes(Position,elevation is not null ? elevation.Value : 0), OutgoingEdges = [] };
        }
        else
            BuiltVertex = new TVertex{ Attributes = AttributesNoElev, OutgoingEdges = [] };
        return BuiltVertex;
    }

    public abstract void ConnectAfterBuild();
}

public class BoundaryVertexBuilder<TVertex, TEdge>(Orienteering_ISOM_2017_2.VertexAttributes attributesNoElev) : VertexBuilder<TVertex, TEdge>(attributesNoElev)
    where TVertex : IBuildableVertex<TEdge>, IInitializableVertex<TEdge, Orienteering_ISOM_2017_2.VertexAttributes>, new()
    where TEdge : IEdge, IInitializableEdge<TVertex, Orienteering_ISOM_2017_2.EdgeAttributes>, new()
{
    
    // public override HashSet<VertexBuilder> NonBoundaryEdges => NonBoundaryAttributeEdges.Keys.ToHashSet();
    public Dictionary<VertexBuilder<TVertex, TEdge>, Orienteering_ISOM_2017_2.EdgeAttributes> NonBoundaryEdges { get; } = new ();
    public Dictionary<BoundaryVertexBuilder<TVertex, TEdge>, Orienteering_ISOM_2017_2.EdgeAttributes> BoundaryEdges { get; } = new ();
    public override bool IsStationary { get; set;} = true;
    // public bool LeftSideIsOuter { get; set; } -> YES
    public override void ConnectAfterBuild()
    {
        if (BuiltVertex is null) return;
        foreach (var (builder, edgeAttributes) in BoundaryEdges)
        {
            TVertex? vertex = builder.BuiltVertex; 
            if (vertex is null) continue;
            BuiltVertex.AddEdge(new (){Attributes = edgeAttributes, To = vertex});
        }
        foreach (var (builder, edgeAttributes) in NonBoundaryEdges)
        {
            TVertex? vertex = builder.BuiltVertex; 
            if (vertex is null) continue;
            BuiltVertex.AddEdge(new (){Attributes = edgeAttributes, To = vertex});
        }
    }
}

public class NetVertexBuilder<TVertex, TEdge>(Orienteering_ISOM_2017_2.VertexAttributes attributesNoElev): VertexBuilder<TVertex,TEdge>(attributesNoElev)
    where TVertex : IBuildableVertex<TEdge>, IInitializableVertex<TEdge, Orienteering_ISOM_2017_2.VertexAttributes>, new()
    where TEdge : IEdge, IInitializableEdge<TVertex, Orienteering_ISOM_2017_2.EdgeAttributes>, new()
{
    public Dictionary<VertexBuilder<TVertex, TEdge>, (Orienteering_ISOM_2017_2.NaturalLinearObstacles?, Orienteering_ISOM_2017_2.Paths?, Orienteering_ISOM_2017_2.ManMadeLinearObstacles?)> NonBoundaryEdges { get; } = new();

    public (Orienteering_ISOM_2017_2.Grounds?, Orienteering_ISOM_2017_2.Boulders?, Orienteering_ISOM_2017_2.Stones?, Orienteering_ISOM_2017_2.Water?, Orienteering_ISOM_2017_2.VegetationAndManMade?, Orienteering_ISOM_2017_2.VegetationGoodVis?)
        Surroundings = (null, null, null, null, null, null);

    public override void ConnectAfterBuild()
    {
         if (BuiltVertex is null) return;
         foreach (var (builder, linearFeatures) in NonBoundaryEdges)
         {
             TVertex? vertex = builder.BuiltVertex;
             if (vertex is null) continue;
             BuiltVertex.AddEdge(new(){Attributes = new Orienteering_ISOM_2017_2.EdgeAttributes(Surroundings, linearFeatures), To = vertex});
         }       
    }

    public override bool IsStationary { get; set;} = false;
}

public interface IBuildableVertex<TEdge> : IVertex 
    where TEdge : IEdge
{
    void AddEdge(TEdge edge);
}

public interface IInitializableVertex<TEdge, TVertexAttributes>
    where TEdge: IEdge
    where TVertexAttributes : IVertexAttributes
{
    public IList<TEdge> OutgoingEdges { protected get; init; }
    public TVertexAttributes Attributes { get; init;}
}

public interface IInitializableEdge<TVertex, TEdgeAttributes>
    where TVertex : IVertex
    where TEdgeAttributes : IEdgeAttributes
{
    public TEdgeAttributes Attributes { get; init; } 
    public TVertex To { get; init; } 
}


