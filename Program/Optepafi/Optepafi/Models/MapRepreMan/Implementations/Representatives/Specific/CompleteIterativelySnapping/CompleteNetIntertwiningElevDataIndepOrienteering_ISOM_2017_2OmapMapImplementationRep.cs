using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Avalonia;
using DynamicData;
using DynamicData.Kernel;
using Optepafi.Models.GraphicsMan.Objects.Map;
using Optepafi.Models.MapMan;
using Optepafi.Models.MapMan.MapFormats;
using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.MapRepreMan.Configurations;
using Optepafi.Models.MapRepreMan.Graphs.Specific;
using Optepafi.Models.MapRepreMan.Implementations.Specific.CompleteIterativelySnapping;
using Optepafi.Models.MapRepreMan.Utils;
using Optepafi.Models.TemplateMan.Templates;
using Optepafi.Models.Utils;
using Optepafi.Models.Utils.Configurations;
using Optepafi.Models.Utils.Shapes.Segments;
using Optepafi.ViewModels.Main;
using Splat.ApplicationPerformanceMonitoring;

namespace Optepafi.Models.MapRepreMan.Implementations.Representatives.Specific.CompleteIterativelySnapping;

public class CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationRep : 
    ElevDataIndepImplementationRep<Orienteering_ISOM_2017_2, OmapMap, OmapMap, CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation, CompleteNetIntertwiningMapRepreConfiguration, ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex, ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge, Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>
{
    public static CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationRep Instance { get; } = new();
    private CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationRep(){ }

    public override Orienteering_ISOM_2017_2 UsedTemplate { get; } = Orienteering_ISOM_2017_2.Instance;
    public override IMapFormat<OmapMap> UsedMapFormat { get; } = OmapMapRepresentative.Instance;

    public override CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation CreateImplementation(Orienteering_ISOM_2017_2 template, OmapMap map,
        CompleteNetIntertwiningMapRepreConfiguration configuration, IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken)
    {
        return GraphCreator.Instance.Create(map, configuration, progress, cancellationToken);
    }


    private class GraphCreator
    {
        private const int microspocicEdgeLength = 20;
        public static GraphCreator Instance { get; } = new();
        private GraphCreator() { }
        public CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation Create(OmapMap map,
            CompleteNetIntertwiningMapRepreConfiguration configuration, IProgress<MapRepreConstructionReport>? progress,
            CancellationToken? cancellationToken)
        {
            List<NetVertexBuilder> netVertices = CreateNet(map, configuration);
            if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return new CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation(new RadiallySearchableKdTree<ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex>(v => (v.Attributes.Position.XPos, v.Attributes.Position.YPos)), map.Scale);
            
            IEditableRadiallySearchableDataStruct<VertexBuilder> vertexBuilders = new RadiallySearchableKdTree<VertexBuilder>(netVertices, vb => (vb.Position.XPos, vb.Position.YPos));
            int allObjectsCount = 0;
            foreach (var crossablePolygonalSymbolCode in OrderedCrossablePolygonalSymbolsCodes)
                if (map.Symbols.Contains(new OmapMap.Symbol(crossablePolygonalSymbolCode)))
                    allObjectsCount += map.Objects[crossablePolygonalSymbolCode].Count;
            foreach (var pathSymbolCode in OrderedPathsSymbolsCodes)
                if (map.Symbols.Contains(new OmapMap.Symbol(pathSymbolCode)))
                    allObjectsCount += map.Objects[pathSymbolCode].Count;
            foreach (var linearobstacleSymbolCode in OrderedLinearObstacleSymbolsCodes)
                if (map.Symbols.Contains(new OmapMap.Symbol(linearobstacleSymbolCode)))
                    allObjectsCount += map.Objects[linearobstacleSymbolCode].Count;
            foreach (var uncrossablePolygonalSymbolCode in OrderedUncrossablePolygonalSymbolsCodes)
                if (map.Symbols.Contains(new OmapMap.Symbol(uncrossablePolygonalSymbolCode)))
                    allObjectsCount += map.Objects[uncrossablePolygonalSymbolCode].Count;
            // Console.WriteLine(allObjectsCount);
            int processedObjectsCount = 0;
            foreach (var crossablePolygonalSymbolCode in OrderedCrossablePolygonalSymbolsCodes)
                if (map.Symbols.Contains(new OmapMap.Symbol(crossablePolygonalSymbolCode)))
                    foreach (var obj in map.Objects[crossablePolygonalSymbolCode])
                    {
                        ProcessCrossablePolygonalObject(obj, crossablePolygonalSymbolCode, vertexBuilders, configuration, cancellationToken);
                        if (progress is not null && ++processedObjectsCount % (allObjectsCount/100) == 0) progress.Report(new MapRepreConstructionReport(processedObjectsCount/(float)allObjectsCount));
                        if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return new CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation(new RadiallySearchableKdTree<ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex>(v => (v.Attributes.Position.XPos, v.Attributes.Position.YPos)), map.Scale);
                    }
            foreach (var pathSymbolCode in OrderedPathsSymbolsCodes)
                if (map.Symbols.Contains(new OmapMap.Symbol(pathSymbolCode)))
                    foreach (var obj in map.Objects[pathSymbolCode])
                    {
                        ProcessPathObject(obj, pathSymbolCode, vertexBuilders, configuration, cancellationToken);
                        if (progress is not null && ++processedObjectsCount % (allObjectsCount/100) == 0) progress.Report(new MapRepreConstructionReport(processedObjectsCount/(float)allObjectsCount));
                        if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return new CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation(new RadiallySearchableKdTree<ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex>(v => (v.Attributes.Position.XPos, v.Attributes.Position.YPos)), map.Scale);
                    }
            foreach (var linearObstacleSymbolCode in OrderedLinearObstacleSymbolsCodes)
                if (map.Symbols.Contains(new OmapMap.Symbol(linearObstacleSymbolCode)))
                    foreach (var obj in map.Objects[linearObstacleSymbolCode])
                    {
                        ProcessLinearObstacleObject(obj, linearObstacleSymbolCode, vertexBuilders, configuration, cancellationToken);
                        if (progress is not null && ++processedObjectsCount % (allObjectsCount/100) == 0) progress.Report(new MapRepreConstructionReport(processedObjectsCount/(float)allObjectsCount));
                        if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return new CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation(new RadiallySearchableKdTree<ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex>(v => (v.Attributes.Position.XPos, v.Attributes.Position.YPos)), map.Scale);
                    }
            foreach (var uncrossablePolygonalSymbolCode in OrderedUncrossablePolygonalSymbolsCodes)
                if (map.Symbols.Contains(new OmapMap.Symbol(uncrossablePolygonalSymbolCode)))
                    foreach (var obj in map.Objects[uncrossablePolygonalSymbolCode])
                    {
                        ProcessUncrossablePolygonalObject(obj, uncrossablePolygonalSymbolCode, vertexBuilders, configuration, cancellationToken);
                        if (progress is not null && ++processedObjectsCount % (allObjectsCount/100) == 0) progress.Report(new MapRepreConstructionReport(processedObjectsCount/(float)allObjectsCount));
                        if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return new CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation(new RadiallySearchableKdTree<ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex>(v => (v.Attributes.Position.XPos, v.Attributes.Position.YPos)), map.Scale);
                    }
            RadiallySearchableKdTree<BuildableVertex> vertices = new RadiallySearchableKdTree<BuildableVertex>(vertexBuilders.Select(vb => vb.Build()), v => (v.Attributes.Position.XPos, v.Attributes.Position.YPos));
            foreach (var vertexBuilder in vertexBuilders) vertexBuilder.ConnectAfterBuild(); 
            return new CompleteNetIntertwiningElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation(vertices, map.Scale);
        }
        
        private List<decimal> OrderedCrossablePolygonalSymbolsCodes =
        [
            403, 404, 404.1m, 413.1m, 213, 414.1m, 401, 402, 402.1m, 413, 412, 414, 407, 409, 406, 406.1m, 408, 408.1m, 408.2m,
            410, 410.1m, 410.2m, 410.3m, 410.4m, 214, 405, 302, 302.1m, 302.5m, 308, 310, 501, 501.1m, 113, 114, 210, 205, 211, 209, 212
        ];

        private List<decimal> OrderedPathsSymbolsCodes =
        [
            502, 502.2m, 503, 504, 505, 506, 507, 508, 508.1m, 508.2m, 508.3m, 508.4m, 532 
        ];
        
        private List<decimal> OrderedLinearObstacleSymbolsCodes =
        [
            104, 105, 107, 304, 201, 201.3m, 202, 202.2m, 215, 513, 515, 516, 518, 528, 529
        ];

        private List<decimal> OrderedUncrossablePolygonalSymbolsCodes =
        [
            520, 307, 307.1m, 301, 301.1m, 301.3m, 521, 521.3m, 206, 520.2m
        ];
        

        private List<NetVertexBuilder> CreateNet(OmapMap map, CompleteNetIntertwiningMapRepreConfiguration configuration) 
            => configuration.typeOfNet.AllValues[configuration.typeOfNet.IndexOfSelectedValue] switch
        {
            CompleteNetIntertwiningMapRepreConfiguration.NetTypesEnumeration.Triangular => CreateTriangularNet(map, configuration),
            _ => throw new InvalidEnumArgumentException()
        };

        private List<NetVertexBuilder> CreateTriangularNet(OmapMap map, CompleteNetIntertwiningMapRepreConfiguration configuration)
        {
            (int left, int top, int right, int bottom) boundaries = (map.WesternmostCoords.XPos, map.NorthernmostCoords.YPos, map.EasternmostCoords.XPos, map.SouthernmostCoords.YPos);
            int edgeLength = configuration.standardEdgeLength.Value;
            int colls = (boundaries.right - boundaries.left)/edgeLength + 1;
            int rows = (int)((boundaries.top - boundaries.bottom)/(Math.Sqrt(3)*edgeLength/2)) + 1;
            List<NetVertexBuilder> vertices = new ();
            List<NetVertexBuilder> lastRow = new ();
            List<NetVertexBuilder> currentRow = new ();
            (Orienteering_ISOM_2017_2.Grounds?, Orienteering_ISOM_2017_2.Boulders?, Orienteering_ISOM_2017_2.Stones?, Orienteering_ISOM_2017_2.Water?, Orienteering_ISOM_2017_2.VegetationAndManMade?, Orienteering_ISOM_2017_2.VegetationGoodVis?)justForest =  (null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.Forest_405, null);

            for (int coll = 0; coll < colls; ++coll)
                lastRow.Add(new NetVertexBuilder( new Orienteering_ISOM_2017_2.VertexAttributes( new MapCoordinates(boundaries.left + coll * edgeLength, boundaries.top)))
                    {Surroundings = justForest});

            for (int row = 1; row < rows; ++row)
            {
                for (int coll = 0; coll < colls - 1; ++coll)
                {
                    NetVertexBuilder vertex = new NetVertexBuilder(new Orienteering_ISOM_2017_2.VertexAttributes(new MapCoordinates( boundaries.left + edgeLength/2 + coll * edgeLength , (int) (boundaries.top - row * Math.Sqrt(3) * edgeLength / 2))))
                        {Surroundings = justForest};
                    vertex.NonBoundaryEdges.Add(lastRow[coll]);
                    lastRow[coll].NonBoundaryEdges.Add(vertex) ;
                    vertex.NonBoundaryEdges.Add(lastRow[coll + 1]) ;
                    lastRow[coll + 1].NonBoundaryEdges.Add(vertex) ;
                    lastRow[coll + 1].NonBoundaryEdges.Add(lastRow[coll]);
                    lastRow[coll].NonBoundaryEdges.Add(lastRow[coll + 1]);
                    currentRow.Add(vertex);
                }
                
                vertices.AddRange(lastRow);
                lastRow.Clear();
                lastRow.AddRange(currentRow);
                currentRow.Clear();
                ++row;
                
                NetVertexBuilder leftVertex = new NetVertexBuilder(new Orienteering_ISOM_2017_2.VertexAttributes(new MapCoordinates(boundaries.left, (int) (boundaries.top - row * Math.Sqrt(3) * edgeLength / 2))))
                    {Surroundings = justForest};
                leftVertex.NonBoundaryEdges.Add(lastRow[0]);
                currentRow.Add(leftVertex);
                for (int coll = 1; coll < colls - 1; ++coll)
                {
                    NetVertexBuilder vertex =  new NetVertexBuilder(new Orienteering_ISOM_2017_2.VertexAttributes(new MapCoordinates( boundaries.left + coll * edgeLength , (int) (boundaries.top - row * Math.Sqrt(3) * edgeLength / 2)))) 
                        {Surroundings = justForest};
                    vertex.NonBoundaryEdges.Add(lastRow[coll - 1]);
                    lastRow[coll - 1].NonBoundaryEdges.Add(vertex);
                    vertex.NonBoundaryEdges.Add(lastRow[coll]);
                    lastRow[coll].NonBoundaryEdges.Add(vertex);
                    lastRow[coll].NonBoundaryEdges.Add(lastRow[coll-1]);
                    lastRow[coll - 1].NonBoundaryEdges.Add(lastRow[coll]);
                    currentRow.Add(vertex);
                }
                NetVertexBuilder rightVertex = new NetVertexBuilder(new Orienteering_ISOM_2017_2.VertexAttributes(new MapCoordinates(boundaries.left + (colls - 1) * edgeLength, (int) (boundaries.top - row * Math.Sqrt(3) * edgeLength / 2))))
                    {Surroundings = justForest};
                rightVertex.NonBoundaryEdges.Add(lastRow[colls - 2]);
                currentRow.Add(rightVertex);
                
                vertices.AddRange(lastRow);
                lastRow.Clear();
                lastRow.AddRange(currentRow);
                currentRow.Clear();
            }
            
            for (int coll = 1; coll < colls; ++coll)
            {
                lastRow[coll].IsStationary = true;
                lastRow[coll].NonBoundaryEdges.Add(lastRow[coll - 1]);
                lastRow[coll - 1].NonBoundaryEdges.Add(lastRow[coll]);
            }
            vertices.AddRange(lastRow);
            return vertices;
        }
        
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void ProcessCrossablePolygonalObject(OmapMap.Object obj, decimal symbolCode, IEditableRadiallySearchableDataStruct<VertexBuilder> allVertices,
            CompleteNetIntertwiningMapRepreConfiguration configuration, CancellationToken? cancellationToken)
        {
            var potentiallyEntangledChain = GetBlankBoundaryChainOfPolygonalObject(obj, configuration.standardEdgeLength.Value, configuration.minBoundaryEdgeRatio.Value);
            if (potentiallyEntangledChain is null) return;
            var chains = SplitChainToMoreIfItIsEntangledAndMakeThemTurnRight(potentiallyEntangledChain);
            foreach(var chain in chains)
            {
                var (verticesOfCutNonBoundaryEdges, verticesOfCutBoundaryEdges) = CutAllCrossedEdges(chain, allVertices, configuration.standardEdgeLength.Value, true);
                var (outerVerticesOfCutEdges, innerVerticesOfCutEdges, allInnerVertices) = FindNodesInsideThePolygonByDfs(verticesOfCutNonBoundaryEdges, verticesOfCutBoundaryEdges, chain); 
                UpdateAttributesOfInnerEdges(allInnerVertices, symbolCode);

                var chainListForNewVerticesAddition = chain.ToList();
                ProcessCutBoundaryEdgesByChain(chainListForNewVerticesAddition, verticesOfCutBoundaryEdges, outerVerticesOfCutEdges, symbolCode);
                var chainEnrichedByNewCrossSectionVertices = chainListForNewVerticesAddition.ToArray();

                ConnectChainToVerticesOfCutEdgesAndOtherVerticesOfChain(chainEnrichedByNewCrossSectionVertices, outerVerticesOfCutEdges, innerVerticesOfCutEdges, allVertices, configuration.standardEdgeLength.Value, true);
                SetAttributesOfChainsEdges(chainEnrichedByNewCrossSectionVertices, outerVerticesOfCutEdges, symbolCode);
                SetAttributesOfNonBoundaryEdgesBetweenChainAndBoundaryVertices(chainEnrichedByNewCrossSectionVertices);
                AddChainVerticesToTheGraph(chainEnrichedByNewCrossSectionVertices, allVertices);
            }
            //TODO:
        }
        
        private void ProcessPathObject(OmapMap.Object obj, decimal symbolCode, IEditableRadiallySearchableDataStruct<VertexBuilder> allVertices,
            CompleteNetIntertwiningMapRepreConfiguration configuration, CancellationToken? cancellationToken)
        {
            //TODO:
        }
        
        private void ProcessLinearObstacleObject(OmapMap.Object obj, decimal symbolCode, IEditableRadiallySearchableDataStruct<VertexBuilder> allVertices,
            CompleteNetIntertwiningMapRepreConfiguration configuration, CancellationToken? cancellationToken)
        {
            //TODO:
        }
        
        private void ProcessUncrossablePolygonalObject(OmapMap.Object obj, decimal symbolCode, IEditableRadiallySearchableDataStruct<VertexBuilder> allVertices,
            CompleteNetIntertwiningMapRepreConfiguration configuration, CancellationToken? cancellationToken)
        {
            //TODO:
        }

        // 0 ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        private BoundaryVertexBuilder[]? GetBlankBoundaryChainOfPolygonalObject(OmapMap.Object obj, int standardEdgeLength, float minBoundaryEdgeRatio)
        {
            var segments = obj.CollectSegments();
            // last and first vertices will be on the same position
            var boundaryChain = GetBlankChainOfSegmentedLine(segments.Last().LastPoint, segments, standardEdgeLength, minBoundaryEdgeRatio);
            if (boundaryChain.Count <= 2) return null;
            //connects last and second vertex of chain for polygonal shape, removes first one
            boundaryChain.Last().BoundaryEdges[boundaryChain[1]] = new Orienteering_ISOM_2017_2.EdgeAttributes();
            boundaryChain[1].BoundaryEdges[boundaryChain.Last()] = new Orienteering_ISOM_2017_2.EdgeAttributes();
            boundaryChain[1].BoundaryEdges.Remove(boundaryChain[0]);
            boundaryChain.RemoveAt(0);
            return boundaryChain.ToArray();
        }

        private IList<VertexBuilder> GetBlankChainOfLinearObject(OmapMap.Object obj, int standardEdgeLength, float minBoundaryEdgeRatio)
        {
            //TODO: uses GetBlankBoundaryChainOfSegmentedLine
            throw new NotImplementedException();
        }
        
        private (IList<VertexBuilder>,IList<VertexBuilder>) GetBlankBoundaryChainOfLineObject()
        {
            //TODO:
            throw new NotImplementedException();
        }
        
        private List<BoundaryVertexBuilder> GetBlankChainOfSegmentedLine(MapCoordinates firstPosition, IList<Segment> segments, int standardEdgeLength, float minBoundaryEdgeRatio)
        {
            List<BoundaryVertexBuilder> chain = [new (new Orienteering_ISOM_2017_2.VertexAttributes(firstPosition))];
            // incrementally adds new vertices to chain for each segment of objects boundary 
            foreach (var segment in segments)
            {
                AddBlankChainForSegment(segment, chain, standardEdgeLength, minBoundaryEdgeRatio);
            }
            // removes edges that are microscopic 
            for (int i = 1; i < chain.Count; ++i)
            {
                if ((chain[i-1].Position - chain[i].Position).Length() < microspocicEdgeLength)
                {
                    chain[i-1].BoundaryEdges.Remove(chain[i]);
                    chain[i].BoundaryEdges.Remove(chain[i-1]);
                    if (i + 1 < chain.Count)
                    {
                        chain[i + 1].BoundaryEdges.Remove(chain[i]);
                        chain[i].BoundaryEdges.Remove(chain[i + 1]);
                        chain[i - 1].BoundaryEdges[chain[i + 1]] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                        chain[i + 1].BoundaryEdges[chain[i - 1]] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                    }
                    chain.RemoveAt(i);
                }
            }
            return chain;
        }

        private void AddBlankChainForSegment(Segment segment, List<BoundaryVertexBuilder> chain, int standardEdgeLength, float minBoundaryEdgeRatio)
        {
            // if segment is too small, following method will process it
            if (HandleSmallSegment(segment, chain, standardEdgeLength, minBoundaryEdgeRatio)) return;
            // remembering segments Point0
            MapCoordinates point0 = chain.Last().Position;
            // discovers count of edges to which should be segment split, so that all edges had approximately same length not bigger than standard edge length
            // edges will not have the same length
            // with increasing edge count, the variance of length of all edges will be still smaller and smaller
            int edgesCount = 3;
            while ((segment.PositionAt(1 / (double)++edgesCount, point0) - point0).Length() > standardEdgeLength) { }
            // iteratively creating edges from uniformly chosen parts of segment
            for (int i = 1; i <= edgesCount; ++i)
            {
                BoundaryVertexBuilder newVertex = new( new Orienteering_ISOM_2017_2.VertexAttributes(segment.PositionAt(i / (double)edgesCount, point0)));
                newVertex.BoundaryEdges[chain.Last()] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                chain.Last().BoundaryEdges[newVertex] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                chain.Add(newVertex);
            }
        }

        private bool HandleSmallSegment(Segment segment, List<BoundaryVertexBuilder> chain, int standardEdgeLength, float minBoundaryEdgeRatio)
        {
            BoundaryVertexBuilder[]? newVertices;
            // Tries to fit small segment by as many edges as it can.
            // If more than 3 edges can be fit into the segment, return false indicating, that segment is not small one.
            if ((segment.PositionAt(0.25, chain.Last().Position) - chain.Last().Position).Length() < standardEdgeLength * minBoundaryEdgeRatio ||
                (segment.PositionAt(0.5, chain.Last().Position) - segment.PositionAt(0.25, chain.Last().Position)).Length() < standardEdgeLength * minBoundaryEdgeRatio ||
                (segment.PositionAt(0.75, chain.Last().Position) - segment.PositionAt(0.5, chain.Last().Position)).Length() < standardEdgeLength * minBoundaryEdgeRatio ||
                (segment.LastPoint - segment.PositionAt(0.75, chain.Last().Position)).Length() < standardEdgeLength * minBoundaryEdgeRatio)
                if ((segment.PositionAt(1 / (double)3, chain.Last().Position) - chain.Last().Position).Length() < standardEdgeLength * minBoundaryEdgeRatio ||
                    (segment.PositionAt(2 / (double)3, chain.Last().Position) - segment.PositionAt(1 / (double)3, chain.Last().Position)).Length() < standardEdgeLength * minBoundaryEdgeRatio ||
                    (segment.LastPoint - segment.PositionAt(2 / (double)3, chain.Last().Position)).Length() < standardEdgeLength * minBoundaryEdgeRatio)
                    if ((segment.PositionAt(0.5, chain.Last().Position) - chain.Last().Position).Length() < standardEdgeLength * minBoundaryEdgeRatio ||
                        (segment.LastPoint - segment.PositionAt(0.5, chain.Last().Position)).Length() < standardEdgeLength * minBoundaryEdgeRatio)
                        newVertices = [
                            new BoundaryVertexBuilder(new Orienteering_ISOM_2017_2.VertexAttributes(segment.LastPoint))
                        ];        
                    else newVertices = [ 
                            new BoundaryVertexBuilder( new Orienteering_ISOM_2017_2.VertexAttributes( segment.PositionAt(1 / (double)2, chain.Last().Position))), 
                            new BoundaryVertexBuilder( new Orienteering_ISOM_2017_2.VertexAttributes(segment.LastPoint))
                        ];
                else newVertices = [
                        new BoundaryVertexBuilder( new Orienteering_ISOM_2017_2.VertexAttributes(segment.PositionAt(1 / (double)3, chain.Last().Position))), 
                        new BoundaryVertexBuilder( new Orienteering_ISOM_2017_2.VertexAttributes(segment.PositionAt(2 / (double)3, chain.Last().Position))),
                        new BoundaryVertexBuilder( new Orienteering_ISOM_2017_2.VertexAttributes(segment.LastPoint))
                    ];
            else return false;
            foreach (var newVertex in newVertices)
            {
                newVertex.BoundaryEdges[chain.Last()] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                chain.Last().BoundaryEdges[newVertex] = new Orienteering_ISOM_2017_2.EdgeAttributes();
                chain.Add(newVertex);
            }
            return true;
        }
                                
        // 1 ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        private List<BoundaryVertexBuilder[]> SplitChainToMoreIfItIsEntangledAndMakeThemTurnRight(BoundaryVertexBuilder[] potentiallyEntangledChain)
        {
            var(indicesOfCutEdgesWithCorrespondingTemporaryVertices, allTemporaryVertices) = CreateTemporaryVertices(potentiallyEntangledChain);
            // if no cross-sections of edges are found, chain is not entangled so we can return it whole
            // we just have to check its orientation
            if (indicesOfCutEdgesWithCorrespondingTemporaryVertices.Count == 0)
                if (!IsRightSideInner(potentiallyEntangledChain[0], potentiallyEntangledChain.Last().Position, potentiallyEntangledChain[1].Position, potentiallyEntangledChain)) 
                    return [potentiallyEntangledChain.Reverse().ToArray()];
                else return [potentiallyEntangledChain];
            DisconnectCrossingEdgesInChain(potentiallyEntangledChain, indicesOfCutEdgesWithCorrespondingTemporaryVertices);
            InterconnectTemporaryVertices(indicesOfCutEdgesWithCorrespondingTemporaryVertices, potentiallyEntangledChain);
            ProcessTemporaryVertices(allTemporaryVertices);
            return CollectRightTurningChains(potentiallyEntangledChain.ToHashSet());
        }

        private (Dictionary<int, List<ChainEntanglementTemporaryVertexBuilder>>, List<ChainEntanglementTemporaryVertexBuilder>) CreateTemporaryVertices(BoundaryVertexBuilder[] potentiallyEntangledChain)
        {
            Dictionary<int, List<ChainEntanglementTemporaryVertexBuilder>> indicesOfCutEdgesWithCorrespondingTemporaryVertices = new();
            List<ChainEntanglementTemporaryVertexBuilder> allCreatedTemporaryVertices = new();
            // For each cross-section of chain edges is created new temporary vertex
            // this vertex is then added to lists of created temporary vertices which are hold in dictionary under the indices of crossed edges
            for (int i = 0; i < potentiallyEntangledChain.Length - 2; ++i)
                for (int j = i + 2; j < potentiallyEntangledChain.Length; ++j)
                {
                    if (i == 0 && j == potentiallyEntangledChain.Length - 1) continue;
                    int js = j == potentiallyEntangledChain.Length - 1 ? 0 : j + 1;
                    MapCoordinates? crossSectionCoords = GetLineSegmentsCrossSectionCoords(potentiallyEntangledChain[i].Position, potentiallyEntangledChain[i + 1].Position, potentiallyEntangledChain[j].Position, potentiallyEntangledChain[js].Position)  ;
                    if (crossSectionCoords is not null)
                    {
                        var newTemporaryVertex = new ChainEntanglementTemporaryVertexBuilder( new Orienteering_ISOM_2017_2.VertexAttributes(crossSectionCoords.Value));
                        if (indicesOfCutEdgesWithCorrespondingTemporaryVertices.ContainsKey(i)) indicesOfCutEdgesWithCorrespondingTemporaryVertices[i].Add(newTemporaryVertex);
                        else indicesOfCutEdgesWithCorrespondingTemporaryVertices[i] = [newTemporaryVertex];
                        if (indicesOfCutEdgesWithCorrespondingTemporaryVertices.ContainsKey(j)) indicesOfCutEdgesWithCorrespondingTemporaryVertices[j].Add(newTemporaryVertex);
                        else indicesOfCutEdgesWithCorrespondingTemporaryVertices[j] = [newTemporaryVertex];
                        allCreatedTemporaryVertices.Add(newTemporaryVertex);
                    }
                }
            return (indicesOfCutEdgesWithCorrespondingTemporaryVertices, allCreatedTemporaryVertices);
        }

        private MapCoordinates? GetLineSegmentsCrossSectionCoords(MapCoordinates p1, MapCoordinates p2, MapCoordinates q1, MapCoordinates q2)
        {
            var ft = GetNumeratorAndDenominatorOfT(p1, p2, q1, q2);
            var fu = GetNumeratorAndDenominatorOfU(p1, p2, q1, q2);
            if (!IsFractionInO1IntervalAndDefined(ft.numerator, ft.denominator)) return null;
            if (!IsFractionInO1IntervalAndDefined(fu.numerator, fu.denominator)) return null;
            //TODO: osetrenie pripadov, kedy t,u = 0,1
            if(ft.numerator == ft.denominator || fu.numerator == fu.denominator || ft.numerator == 0 || fu.numerator == 0)Console.WriteLine("ERROR!!!!!");
            double t = ft.numerator / (double)ft.denominator;
            return new MapCoordinates((int)(p1.XPos + t * (p2.XPos - p1.XPos)), (int)(p1.YPos + t * (p2.YPos - p1.YPos)));
        }
        
            // if (TryComputeCrossSection(p1, p2, q1, q2, out var coords))
            // {
                // whole code works under the assumption that p1, p2, q1 and q2 never equals x. It resolves issues with vertices right upon edges. !!!!!!!!!
                // if (p1.XPos < p2.XPos && p1.XPos <= coords.x && coords.x <= p2.XPos && q1.XPos < q2.XPos && q1.XPos <= coords.x && coords.x <= q2.XPos) return new MapCoordinates((int)coords.x, (int)coords.y);
                // if (p1.XPos < p2.XPos && p1.XPos <= coords.x && coords.x <= p2.XPos && q1.XPos > q2.XPos && q1.XPos >= coords.x && coords.x >= q2.XPos) return new MapCoordinates((int)coords.x, (int)coords.y);;
                // if (p1.XPos > p2.XPos && p1.XPos >= coords.x && coords.x >= p2.XPos && q1.XPos < q2.XPos && q1.XPos <= coords.x && coords.x <= q2.XPos) return new MapCoordinates((int)coords.x, (int)coords.y);;
                // if (p1.XPos > p2.XPos && p1.XPos >= coords.x && coords.x >= p2.XPos && q1.XPos > q2.XPos && q1.XPos >= coords.x && coords.x >= q2.XPos) return new MapCoordinates((int)coords.x, (int)coords.y);;
            // }
            // return null;

        // private bool TryComputeCrossSection(MapCoordinates p1, MapCoordinates p2, MapCoordinates q1, MapCoordinates q2, out (double x, double y) coords)
        // {
            // if (p1 == p2 || q1 == q2) return false;
            // double a = (p2.YPos - p1.YPos) / (double)(p2.XPos - p1.XPos) * p1.XPos + p1.YPos;
            // double b = (p2.YPos - p1.YPos) / (double)(p2.XPos - p1.XPos);
            // double c = (q2.YPos - q1.YPos) / (double)(q2.XPos - q1.XPos) * q1.XPos + q1.YPos;
            // double d = (q2.YPos - q1.YPos) / (double)(q2.XPos - q1.XPos);
            // happens, when lines are parallel to each other
            // if ( -0.00000001 < d - b && d - b < 0.00000001) return false;
            // coords.x = (a - c) / (d - b);
            // coords.y = a + b * coords.x(;
        // }
            
        private (int numerator , int denominator) GetNumeratorAndDenominatorOfT(MapCoordinates p1, MapCoordinates p2, MapCoordinates q1, MapCoordinates q2)
            => ((p1.XPos - q1.XPos) * (q1.YPos - q2.YPos) - (p1.YPos - q1.YPos) * (q1.XPos - q2.XPos),
                (p1.XPos - p2.XPos) * (q1.YPos - q2.YPos) - (p1.YPos - p2.YPos) * (q1.XPos - q2.XPos));
        private (int numerator, int denominator) GetNumeratorAndDenominatorOfU(MapCoordinates p1, MapCoordinates p2, MapCoordinates q1, MapCoordinates q2)
            => (- ((p1.XPos - p2.XPos) * (p1.YPos - q1.YPos) - (p1.YPos - p2.YPos) * (p1.XPos - q1.XPos)),
                   (p1.XPos - p2.XPos) * (q1.YPos - q2.YPos) - (p1.YPos - p2.YPos) * (q1.XPos - q2.XPos));

        private bool IsFractionInO1IntervalAndDefined(int numerator, int denominator)
            => (numerator >= 0 && denominator > 0 && denominator >= numerator) || (numerator <= 0 && denominator < 0 && denominator <= numerator);

        private bool IsFractionGreaterOrEqualToZeroAndDefined(int numerator, int denominator)
            => (numerator >= 0 && denominator > 0) || (numerator <= 0 && denominator < 0);
        
        private void DisconnectCrossingEdgesInChain(BoundaryVertexBuilder[] potentiallyEntangledChain, Dictionary<int, List<ChainEntanglementTemporaryVertexBuilder>> indicesOfVerticesWhoseEdgesWereCutWithCorrespondingTemporaryVertices)
        {
            // every edge, that was crossing some other edge will be removed from chain
            foreach (var index in indicesOfVerticesWhoseEdgesWereCutWithCorrespondingTemporaryVertices.Keys)
            {
                int indexSuccessor = index == potentiallyEntangledChain.Length - 1 ? 0 : index + 1; 
                potentiallyEntangledChain[index].BoundaryEdges.Remove(potentiallyEntangledChain[indexSuccessor]);
                potentiallyEntangledChain[indexSuccessor].BoundaryEdges.Remove(potentiallyEntangledChain[index]);
            }
        }

        private void InterconnectTemporaryVertices( Dictionary<int, List<ChainEntanglementTemporaryVertexBuilder>> indicesOfVerticesWhoseEdgesWereCutWithCorrespondingTemporaryVertices, BoundaryVertexBuilder[] chainWithoutEntangledEdges)
        {
            // temporary vertices are one by one correctly interconnected, so then new edges of chains could be correctly deduced
            foreach (var (chainVertexIndex, correspondingTemporaryVertices) in indicesOfVerticesWhoseEdgesWereCutWithCorrespondingTemporaryVertices)
            {
                int chainVertexIndexSuccessor = chainVertexIndex == chainWithoutEntangledEdges.Length - 1 ? 0 : chainVertexIndex + 1;
                // at first, we need to order created temporary vertices in the way they are positioned on cut edge 
                SortVerticesCorrectly(chainWithoutEntangledEdges[chainVertexIndex], chainWithoutEntangledEdges[chainVertexIndexSuccessor], correspondingTemporaryVertices);
                var lastVertex = chainWithoutEntangledEdges[chainVertexIndex];
                // iteratively we set income and outcome neighbors of temporary vertices
                // logic of temporary vertices will ensure correct coupling of income and outcome neighbors
                for (int i = 0; i < correspondingTemporaryVertices.Count - 1; i++)
                {
                    correspondingTemporaryVertices[i].SetFromToVertex(lastVertex, correspondingTemporaryVertices[i + 1]);
                    lastVertex = correspondingTemporaryVertices[i];
                }
                correspondingTemporaryVertices.Last().SetFromToVertex(lastVertex, chainWithoutEntangledEdges[chainVertexIndexSuccessor]);
            }
        }
        
        private void SortVerticesCorrectly<TVertexBuilder>(BoundaryVertexBuilder vertex1, BoundaryVertexBuilder vertex2, List<TVertexBuilder> vertices) where TVertexBuilder : BoundaryVertexBuilder
        {
            // we will sort vertices based on axis, in which is edge given by vertex1 and vertex2 longer
            if (Math.Abs(vertex1.Position.XPos - vertex2.Position.XPos) > Math.Abs(vertex1.Position.YPos - vertex2.Position.YPos))
            {
                if (vertex1.Position.XPos < vertex2.Position.XPos) vertices.Sort((v1, v2) => v1.Position.XPos - v2.Position.XPos);
                else vertices.Sort((v1, v2) => v2.Position.XPos - v1.Position.XPos);
            }
            else
            {
                if (vertex1.Position.YPos < vertex2.Position.YPos) vertices.Sort((v1, v2) => v1.Position.YPos - v2.Position.YPos);
                else vertices.Sort((v1, v2) => v2.Position.YPos - v1.Position.YPos);
            }
        }

        private void ProcessTemporaryVertices(List<ChainEntanglementTemporaryVertexBuilder> allTemporaryVertices)
        {
            // most important step in resolving of chains entanglement
            // temporary vertices are one by one cut of from other ones what concludes in interconnecting of correct pairs of chain vertices, so that there were no edge crossings whatsoever
            foreach (var temporaryVertex in allTemporaryVertices)
                temporaryVertex.ProcessByDisconnecting();
        }

        private List<BoundaryVertexBuilder[]> CollectRightTurningChains(HashSet<BoundaryVertexBuilder> choppedChain)
        {
            List<BoundaryVertexBuilder[]> chains = new ();
            // while there are some non-processed vertices in "chopped" chain, components are looked for and added to list of newly created chains
            while (choppedChain.Count > 0)
            {
                List<BoundaryVertexBuilder> chain = [choppedChain.First()];
                choppedChain.Remove(chain[0]);
                // if first vertex added to chain has less then two neighbors, it means it is part of component with less than 3 vertices 
                // such components are not returned as chains
                if (chain[0].BoundaryEdges.Count < 2)
                {
                    // if it is part of component of size two, second vertex must be removed from chopped chain as well
                    if (chain[0].BoundaryEdges.Count == 1) 
                        choppedChain.Remove(chain[0].BoundaryEdges.First().Key);
                    continue;
                }
                var vertex = chain[0].BoundaryEdges.First().Key;
                while (vertex != chain[0])
                {
                    choppedChain.Remove(vertex);
                    foreach (var (neighbor, _) in vertex.BoundaryEdges)
                    {
                        if(neighbor == chain.Last()) continue;
                        chain.Add(vertex);
                        vertex = neighbor;
                        break;
                    }
                }
                // if chain is turning left, it is reversed
                if (!IsRightSideInner(chain[0], chain.Last().Position, chain[1].Position, chain.ToArray())) chain.Reverse(); 
                chains.Add(chain.ToArray());
            }
            return chains;
        }
        private bool IsRightSideInner(VertexBuilder vertex, MapCoordinates prevVertexPosition, MapCoordinates nextVertexPosition, BoundaryVertexBuilder[] longEnoughChain)
        {
            int crossesCount = 0;
            // in following two lines there is computed vector whose direction is in half between vector from vertex to previous vertex and vertex to next vertex
            // it is essentially previous vertex rotated around vertex by half of an angle between previous vertex and next vertex
            double angle = ComputeLeftHandedAngleBetween(vertex.Position, prevVertexPosition, vertex.Position, nextVertexPosition);
            var prevVertexPositionRotatedAroundVertexPosition = prevVertexPosition.Rotate(angle/2, vertex.Position);
            // if count of crossed edges by ray in direction of computed vector is odd, vertex, from which ray started had its right side turned into the polygon, so the result is true
            for (int i = 1; i < longEnoughChain.Length; i++)
                if (vertex != longEnoughChain[i-1] && vertex != longEnoughChain[i] && AreLineSegmentAndRayCrossing(longEnoughChain[i - 1].Position, longEnoughChain[i].Position, vertex.Position, prevVertexPositionRotatedAroundVertexPosition )) ++crossesCount;
            if (vertex != longEnoughChain.Last() && vertex != longEnoughChain[0] && AreLineSegmentAndRayCrossing(longEnoughChain.Last().Position, longEnoughChain[0].Position, vertex.Position, prevVertexPositionRotatedAroundVertexPosition)) ++crossesCount;
            return crossesCount % 2 == 1;
        }
        
        private bool AreLineSegmentAndRayCrossing(MapCoordinates ls1, MapCoordinates ls2, MapCoordinates r1, MapCoordinates r2)
        {
            var ft = GetNumeratorAndDenominatorOfT(ls1, ls2, r1, r2);
            var fu = GetNumeratorAndDenominatorOfU(ls1, ls2, r1, r2);
            if (!IsFractionInO1IntervalAndDefined(ft.numerator, ft.denominator)) return false;
            if (!IsFractionGreaterOrEqualToZeroAndDefined(fu.numerator, fu.denominator)) return false;
            //TODO: osetrenie pripadov, kedy t = 0,1 a u = 0
            if(ft.numerator == ft.denominator || fu.numerator == fu.denominator || ft.numerator == 0 || fu.numerator == 0)Console.WriteLine("ERROR!!!!!");
            return true;
        }
            // if (ls0 == ls1 && ls1 == r0 && r0 == r1) return true;
            // if (ls0 == ls1 && r0 == r1) return false;
            // if (ls0 == ls1) return ls0.YPos == (int)((r1.YPos - r0.YPos) / (double)(r1.XPos - r1.YPos) * (ls0.XPos - r0.XPos) + r0.YPos) && 
                                 // ((r0.XPos < r1.XPos && r0.XPos < ls0.XPos) || (r0.XPos > r1.XPos && r0.XPos > ls0.XPos));
            // if (r0 == r1) return r0.YPos == (int)((ls1.YPos - ls0.YPos) / (double)(ls1.XPos - ls1.YPos) * (r0.XPos - ls0.XPos) + ls0.YPos) && 
                                 // ((ls0.XPos < ls1.XPos && ls0.XPos < r0.XPos && r0.XPos <= ls1.XPos) || (ls0.XPos > ls1.XPos && ls0.XPos > r0.XPos && r0.XPos >= ls1.XPos));
                                 
            // if (TryComputeCrossSection(ls0, ls1, r0, r1, out var coords))
            // {
                // whole code works under the assumption that p1, p2, q1 and q2 (almost) never equals to computed coords. It resolves issues with vertices being right over edges. !!!!!!!!!
                // test bellow ensures, that line segment and ray are crossing at their ranges
                // if (ls0.XPos < ls1.XPos && ls0.XPos <= coords.x && coords.x <= ls1.XPos && r0.XPos < r1.XPos && r0.XPos <= coords.x) return true;
                // if (ls0.XPos < ls1.XPos && ls0.XPos <= coords.x && coords.x <= ls1.XPos && r0.XPos > r1.XPos && r0.XPos >= coords.x) return true;
                // if (ls0.XPos > ls1.XPos && ls0.XPos >= coords.x && coords.x >= ls1.XPos && r0.XPos < r1.XPos && r0.XPos <= coords.x) return true;
                // if (ls0.XPos > ls1.XPos && ls0.XPos >= coords.x && coords.x >= ls1.XPos && r0.XPos > r1.XPos && r0.XPos >= coords.x) return true;
            // }
            // return false;

         private class ChainEntanglementTemporaryVertexBuilder(Orienteering_ISOM_2017_2.VertexAttributes attributes) : BoundaryVertexBuilder(attributes)
         {
             private BoundaryVertexBuilder? from1;
             private BoundaryVertexBuilder? to1;
             private BoundaryVertexBuilder? from2;
             private BoundaryVertexBuilder? to2;
             public void SetFromToVertex(BoundaryVertexBuilder from, BoundaryVertexBuilder to)
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
                     temporaryFrom1.to1 = to1;
                 }
                 if (to1 is ChainEntanglementTemporaryVertexBuilder temporaryTo1)
                 {
                    temporaryTo1.from1 = from1;    
                 }
                 if (from2 is ChainEntanglementTemporaryVertexBuilder temporaryFrom2)
                 {
                     temporaryFrom2.to2 = to2;
                 }
                 if (to2 is ChainEntanglementTemporaryVertexBuilder temporaryTo2)
                 {
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
         
        
        // 2 ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        private (Dictionary<(VertexBuilder, VertexBuilder), int>, Dictionary<(BoundaryVertexBuilder, BoundaryVertexBuilder), (Orienteering_ISOM_2017_2.EdgeAttributes cutEdgeAttributes, List<int> crossedChainEdgesIndeces)>) 
            CutAllCrossedEdges(BoundaryVertexBuilder[] chain, IEditableRadiallySearchableDataStruct<VertexBuilder> allVertices, int standardEdgeLength, bool polygonalChain)
        {
            Dictionary<(VertexBuilder, VertexBuilder),  int> verticesOfCutNonBoundaryEdges = new();
            Dictionary<(BoundaryVertexBuilder, BoundaryVertexBuilder), (Orienteering_ISOM_2017_2.EdgeAttributes, List<int>)> verticesOfCutBoundaryEdges = new();
            // at first collects all edges, which are crossed by this chain
            for (int i = 0; i < chain.Length - 1; ++i)
                AddCutEdgesBy(chain[i].Position, chain[i + 1].Position, i, allVertices, verticesOfCutNonBoundaryEdges, verticesOfCutBoundaryEdges, standardEdgeLength);
            if (polygonalChain)
                AddCutEdgesBy(chain.Last().Position, chain[0].Position, chain.Length - 1, allVertices, verticesOfCutNonBoundaryEdges, verticesOfCutBoundaryEdges, standardEdgeLength);
            
            // at second cut out all crossed edges from the graph
            foreach (var ((vertex1, vertex2), _) in verticesOfCutNonBoundaryEdges)
            {
                if(vertex1 is BoundaryVertexBuilder boundaryVertex1) boundaryVertex1.NonBoundaryEdges.Remove(vertex2);
                else if (vertex1 is NetVertexBuilder netVertex1) netVertex1.NonBoundaryEdges.Remove(vertex2);
                if(vertex2 is BoundaryVertexBuilder boundaryVertex2) boundaryVertex2.NonBoundaryEdges.Remove(vertex1);
                else if (vertex2 is NetVertexBuilder netVertex2) netVertex2.NonBoundaryEdges.Remove(vertex1);
            }
            
            foreach (var ((vertex1, vertex2), _) in verticesOfCutBoundaryEdges)
            {
                vertex1.BoundaryEdges.Remove(vertex2);
                vertex2.BoundaryEdges.Remove(vertex1);
            }
            return (verticesOfCutNonBoundaryEdges, verticesOfCutBoundaryEdges);
        }

        private void AddCutEdgesBy(MapCoordinates point0, MapCoordinates point1, int edgeIndex, IEditableRadiallySearchableDataStruct<VertexBuilder> allVertices, Dictionary<(VertexBuilder, VertexBuilder), int> verticesOfCutNonBoundaryEdges, Dictionary<(BoundaryVertexBuilder, BoundaryVertexBuilder), (Orienteering_ISOM_2017_2.EdgeAttributes cutEdgeAttributes, List<int> crossedChainEdgesIndeces)> verticesOfCutBoundaryEdges, int standardEdgeLength)
        {
            HashSet<VertexBuilder> closeVertices = new();
            // find all vertices, which are closer than standard edge length to vertices of chain edge
            // these vertices are the only ones whose that could be crossed by this edge
            foreach (var vertex in allVertices.FindInEuclideanDistanceFrom((point0.XPos, point0.YPos), standardEdgeLength))
                closeVertices.Add(vertex);
            foreach (var vertex in allVertices.FindInEuclideanDistanceFrom((point1.XPos, point1.YPos), standardEdgeLength))
                closeVertices.Add(vertex);
            // test all edges of found vertices, wheter they are not crossed by tested edge
            HashSet<(VertexBuilder, VertexBuilder)> processedEdgesAlready = new();
            foreach (var vertex in closeVertices)
            {

                if (vertex is BoundaryVertexBuilder boundaryVertex)
                {
                    foreach (var (neighbor,_) in boundaryVertex.NonBoundaryEdges)
                    {
                        if ((boundaryVertex.Position.XPos + boundaryVertex.Position.YPos < neighbor.Position.XPos + neighbor.Position.YPos && processedEdgesAlready.Contains((boundaryVertex, neighbor))) || processedEdgesAlready.Contains((neighbor, boundaryVertex))) continue;
                        if (AreLineSegmentsCrossing(point0, point1, boundaryVertex.Position, neighbor.Position))
                            AddSortedNonBoundaryVerticesTo(verticesOfCutNonBoundaryEdges, boundaryVertex, neighbor);
                        processedEdgesAlready.Add(boundaryVertex.Position.XPos + boundaryVertex.Position.YPos < neighbor.Position.XPos + neighbor.Position.YPos ? (boundaryVertex, neighbor) : (neighbor, boundaryVertex));
                    }
                    foreach (var (neighbor, _) in boundaryVertex.BoundaryEdges)
                    {
                        if ((boundaryVertex.Position.XPos + boundaryVertex.Position.YPos < neighbor.Position.XPos + neighbor.Position.YPos && processedEdgesAlready.Contains((boundaryVertex, neighbor))) || processedEdgesAlready.Contains((neighbor, boundaryVertex))) continue;
                        if (AreLineSegmentsCrossing(point0, point1, boundaryVertex.Position, neighbor.Position))
                            AddSortedBoundaryVerticesTo(verticesOfCutBoundaryEdges, boundaryVertex, neighbor, edgeIndex);
                        processedEdgesAlready.Add( boundaryVertex.Position.XPos + boundaryVertex.Position.YPos < neighbor.Position.XPos + neighbor.Position.YPos ? (boundaryVertex, neighbor) : (neighbor, boundaryVertex));
                    }
                }
                else if (vertex is NetVertexBuilder netVertex)
                {
                    foreach (var neighbor in netVertex.NonBoundaryEdges)
                    {
                        if ((netVertex.Position.XPos + netVertex.Position.YPos < neighbor.Position.XPos + neighbor.Position.YPos && processedEdgesAlready.Contains((netVertex, neighbor))) || processedEdgesAlready.Contains((neighbor, netVertex))) continue;
                        if (AreLineSegmentsCrossing(point0, point1, netVertex.Position, neighbor.Position))
                            AddSortedNonBoundaryVerticesTo(verticesOfCutNonBoundaryEdges, netVertex, neighbor);
                        processedEdgesAlready.Add(netVertex.Position.XPos + netVertex.Position.YPos < neighbor.Position.XPos + neighbor.Position.YPos ? (netVertex, neighbor) : (neighbor, netVertex));
                    }
                }

            }

        }

        private bool AreLineSegmentsCrossing(MapCoordinates p1, MapCoordinates p2, MapCoordinates q1, MapCoordinates q2)
        {
            var ft = GetNumeratorAndDenominatorOfT(p1, p2, q1, q2);
            var fu = GetNumeratorAndDenominatorOfU(p1, p2, q1, q2);
            if (!IsFractionInO1IntervalAndDefined(ft.numerator, ft.denominator)) return false;
            if (!IsFractionInO1IntervalAndDefined(fu.numerator, fu.denominator)) return false;
            //TODO: osetrenie pripadov, kedy t,u = 0,1
            if(ft.numerator == ft.denominator || fu.numerator == fu.denominator || ft.numerator == 0 || fu.numerator == 0)Console.WriteLine("ERROR!!!!!");
            return true;
        }
        
            // if (p1 == p2 && p2 == q1 && q1 == q2) return true;
            // if (p1 == p2 && q1 == q2) return false;
            // if (p1 == p2) return p1.YPos == (int)((q2.YPos - q1.YPos) / (double)(q2.XPos - q2.YPos) * (p1.XPos - q1.XPos) + q1.YPos) && 
                                 // ((q1.XPos < q2.XPos && q1.XPos < p1.XPos && p1.XPos < q2.XPos) || (q1.XPos > q2.XPos && q1.XPos > p1.XPos && p1.XPos > q2.XPos));
            // if (q1 == q2) return q1.YPos == (int)((p2.YPos - p1.YPos) / (double)(p2.XPos - p2.YPos) * (q1.XPos - p1.XPos) + p1.YPos) && 
                                 // ((p1.XPos < p2.XPos && p1.XPos < q1.XPos && q1.XPos <= p2.XPos) || (p1.XPos > p2.XPos && p1.XPos > q1.XPos && q1.XPos >= p2.XPos));
                                 
            // if (TryComputeCrossSection(p1, p2, q1, q2, out var coords))
            // {
                // whole code works under the assumption that p1, p2, q1 and q2 never equals x. It resolves issues with vertices right upon edges. !!!!!!!!!
                // test bellow ensures, that line segments are crossing at their ranges
                // if (p1.XPos < p2.XPos && p1.XPos <= coords.x && coords.x <= p2.XPos && q1.XPos < q2.XPos && q1.XPos <= coords.x && coords.x <= q2.XPos) return true;
                // if (p1.XPos < p2.XPos && p1.XPos <= coords.x && coords.x <= p2.XPos && q1.XPos > q2.XPos && q1.XPos >= coords.x && coords.x >= q2.XPos) return true;
                // if (p1.XPos > p2.XPos && p1.XPos >= coords.x && coords.x >= p2.XPos && q1.XPos < q2.XPos && q1.XPos <= coords.x && coords.x <= q2.XPos) return true;
                // if (p1.XPos > p2.XPos && p1.XPos >= coords.x && coords.x >= p2.XPos && q1.XPos > q2.XPos && q1.XPos >= coords.x && coords.x >= q2.XPos) return true;
            // }
            // return false;

        private void AddSortedNonBoundaryVerticesTo(Dictionary<(VertexBuilder, VertexBuilder),  int> verticesOfCutNonBoundaryEdges, VertexBuilder vertex1, VertexBuilder vertex2)
        {
            // dictionary is indexed by couples of vertices in specific order, first vertex of couple has always lower sum of its position axis values than the other one
            // this ensures, that edges defined by couples of vertices are in the dictionary always present only one time
            // it should be mentioned that in this sense edges are thought as not oriented
            if (vertex1.Position.XPos + vertex1.Position.YPos < vertex2.Position.XPos + vertex2.Position.YPos)
                if (verticesOfCutNonBoundaryEdges.ContainsKey((vertex1, vertex2))) ++verticesOfCutNonBoundaryEdges[(vertex1, vertex2)];
                else verticesOfCutNonBoundaryEdges[(vertex1, vertex2)] = 1;
            else
                if (verticesOfCutNonBoundaryEdges.ContainsKey((vertex2, vertex1))) ++verticesOfCutNonBoundaryEdges[(vertex2, vertex1)];
                else verticesOfCutNonBoundaryEdges[(vertex2, vertex1)] = 1;
        }
        
        private void AddSortedBoundaryVerticesTo( Dictionary<(BoundaryVertexBuilder, BoundaryVertexBuilder), (Orienteering_ISOM_2017_2.EdgeAttributes cutEdgeAttributes, List<int> crossedChainEdgesIndices)> verticesOfCutBoundaryEdges, BoundaryVertexBuilder vertex1, BoundaryVertexBuilder vertex2, int edgeIndex)
        {
            // dictionary is indexed by couples of vertices in specific order, first vertex of couple has always lower sum of its position axis values than the other one
            // this ensures, that edges defined by couples of vertices are in the dictionary always present only one time
            // it should be mentioned that in this sense edges are thought as not oriented
            if (vertex1.Position.XPos + vertex1.Position.YPos < vertex2.Position.XPos + vertex2.Position.YPos)
                if (verticesOfCutBoundaryEdges.ContainsKey((vertex1, vertex2))) verticesOfCutBoundaryEdges[(vertex1, vertex2)].crossedChainEdgesIndices.Add(edgeIndex);
                else verticesOfCutBoundaryEdges[(vertex1, vertex2)] = (vertex1.BoundaryEdges[vertex2], [edgeIndex]);
            else
                if (verticesOfCutBoundaryEdges.ContainsKey((vertex2, vertex1))) verticesOfCutBoundaryEdges[(vertex2, vertex1)].crossedChainEdgesIndices.Add(edgeIndex);
                else verticesOfCutBoundaryEdges[(vertex2, vertex1)] = (vertex2.BoundaryEdges[vertex1], [edgeIndex]);
        }
        
        // 3 ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        private (List<VertexBuilder> outerVerticesOfCutEdges, List<VertexBuilder> innerVerticesOfCutEdges, List<VertexBuilder> allInnerVertices) 
            FindNodesInsideThePolygonByDfs(Dictionary<(VertexBuilder, VertexBuilder), int> verticesOfCutNonBoundaryEdges, Dictionary<(BoundaryVertexBuilder, BoundaryVertexBuilder), (Orienteering_ISOM_2017_2.EdgeAttributes, List<int>)> verticesOfCutBoundaryEdges, BoundaryVertexBuilder[] chain)
        {
            var outerVerticesOfCutEdges = new List<VertexBuilder>();
            var innerVerticesOfCutEdges = new List<VertexBuilder>();
            var allInnerVertices = new List<VertexBuilder>();
            
            Dictionary<VertexBuilder, List<(int edgeCutsCount, VertexBuilder neighbor)>> verticesOfCutEdgesWithNeighbors = new ();
            // refactor dictionaries indexed by edges to dictionaries indexed by vertices
            foreach (var ((vertex1, vertex2), edgeCutsCount) in verticesOfCutNonBoundaryEdges)
            {
                if(verticesOfCutEdgesWithNeighbors.ContainsKey(vertex1)) verticesOfCutEdgesWithNeighbors[vertex1].Add((edgeCutsCount, vertex2));
                else verticesOfCutEdgesWithNeighbors[vertex1] = [(edgeCutsCount, vertex2)];
                if(verticesOfCutEdgesWithNeighbors.ContainsKey(vertex2)) verticesOfCutEdgesWithNeighbors[vertex2].Add((edgeCutsCount, vertex1));
                else verticesOfCutEdgesWithNeighbors[vertex2] = [(edgeCutsCount, vertex1)];
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
                SortEdgesByDfs(verticesOfCutEdgesWithNeighbors,  outerVerticesOfCutEdges, innerVerticesOfCutEdges, allInnerVertices, chain);
            }
            return (outerVerticesOfCutEdges, innerVerticesOfCutEdges, allInnerVertices);
        }

        private void SortEdgesByDfs(
            Dictionary<VertexBuilder, List<(int edgeCutsCount, VertexBuilder neighbor)>> verticesOfCutEdgesWithNeighbors,
            List<VertexBuilder> outerVerticesOfCutEdges, List<VertexBuilder> innerVerticesOfCutEdges, 
            List<VertexBuilder> allInnerVertices, BoundaryVertexBuilder[] chain)
        {
            if (verticesOfCutEdgesWithNeighbors.Count > 0)
            {
                // selection of first inner vertex on which will dfs start its search
                var vertex = FindSomeInnerVertex(verticesOfCutEdgesWithNeighbors, chain);
                // if there is no such vertex, there can not be any vertex inside of polygon defined by chain
                if (vertex is null)
                {
                    outerVerticesOfCutEdges.AddRange(verticesOfCutEdgesWithNeighbors.Select(kv => kv.Key)); 
                    return;
                }
                Stack<VertexBuilder> stack = new();
                HashSet<VertexBuilder> visited = new HashSet<VertexBuilder>();
                stack.Push(vertex);
                // dfs will not run out from polygon defined by chain, because edges from inner vertices to outer space were cut out by edges of chain
                while (stack.Count > 0)
                {
                    vertex = stack.Pop();
                    if (visited.Contains(vertex)) continue;
                    allInnerVertices.Add(vertex);
                    visited.Add(vertex);
                    // if vertex is one of those, whose edges were cut, its classification is resolved by calling method IsInnerVertex on it
                    // then all neighboring vertices to this vertex, which shared cut edge with this vertex are classified and their cut-edge-neighbors are searched again recursively
                    if (verticesOfCutEdgesWithNeighbors.ContainsKey(vertex))
                        AreNeighborsOfVertexInnerAndProcessVertex_Recursive(vertex, true, verticesOfCutEdgesWithNeighbors, outerVerticesOfCutEdges, innerVerticesOfCutEdges, stack, visited);
                    
                    if (vertex is BoundaryVertexBuilder boundaryVertex)
                    {
                        foreach (var (neighbor, _) in boundaryVertex.NonBoundaryEdges)
                            if (!visited.Contains(neighbor))
                                stack.Push(neighbor);
                        foreach (var (neighbor, _) in boundaryVertex.BoundaryEdges)
                            if (!visited.Contains(neighbor))
                                stack.Push(neighbor);
                    }
                    else if (vertex is NetVertexBuilder netVertex)
                        foreach (var neighbor in netVertex.NonBoundaryEdges)
                            if (!visited.Contains(neighbor))
                                stack.Push(neighbor);
                        
                }
            }
        }

        private VertexBuilder? FindSomeInnerVertex(Dictionary<VertexBuilder, List<(int, VertexBuilder)>> verticesOfCutEdgesWithNeighbors, BoundaryVertexBuilder[] chain)
        {
            foreach (var (vertex, _) in verticesOfCutEdgesWithNeighbors)
            {
                if (IsInnerVertex(vertex, chain)) return vertex;
            }
            return null;
        }

        private void AreNeighborsOfVertexInnerAndProcessVertex_Recursive(VertexBuilder vertex, bool isVertexInner,
            Dictionary<VertexBuilder, List<(int edgeCutsCount, VertexBuilder vertex)>> verticesOfCutEdgesWithNeighbors,
            List<VertexBuilder> outerVerticesOfCutEdges, List<VertexBuilder> innerVerticesOfCutEdges,
            Stack<VertexBuilder> stack, HashSet<VertexBuilder> visited)
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


        private bool IsInnerVertex(VertexBuilder vertex, BoundaryVertexBuilder[] chain)
        {
            int crossesCount = 0;
            // if count of crossed chain edges by ray is odd, vertex, from which ray started is inside of polygon defined by chain
            for (int i = 1; i < chain.Length; i++)
                if (AreLineSegmentAndRayCrossing(chain[i - 1].Position, chain[i].Position, vertex.Position, vertex.Position + new MapCoordinates(1, 1))) ++crossesCount;
            if (AreLineSegmentAndRayCrossing(chain.Last().Position, chain[0].Position, vertex.Position, vertex.Position + new MapCoordinates(1, 1))) ++crossesCount;
            return crossesCount % 2 == 1;
        }

        
        // 4 ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void UpdateAttributesOfInnerEdges(List<VertexBuilder> innerVertices, decimal symbolCodeOfAddedObject)
        {
            // attributes of every edge of every inner vertex are updated according to symbol of object, that is inserted to the graph
            foreach (var vertex in innerVertices)
            {
                if (vertex is NetVertexBuilder netVertex)
                    netVertex.Surroundings = GetUpdatedSurroundingsOfEdgeAttributes(netVertex.Surroundings, symbolCodeOfAddedObject);
                else if (vertex is BoundaryVertexBuilder boundaryVertex)
                {
                    foreach (var (neighbor, oldEdgeAttributes) in boundaryVertex.NonBoundaryEdges)
                        boundaryVertex.NonBoundaryEdges[neighbor] = GetUpdatedInnerEdgeAttributes(oldEdgeAttributes, symbolCodeOfAddedObject);
                    foreach (var (neighbor, oldEdgeAttributes) in boundaryVertex.BoundaryEdges)
                        boundaryVertex.BoundaryEdges[neighbor] = GetUpdatedInnerEdgeAttributes(oldEdgeAttributes, symbolCodeOfAddedObject);
                }
            }
        }

        private Orienteering_ISOM_2017_2.EdgeAttributes GetUpdatedInnerEdgeAttributes( Orienteering_ISOM_2017_2.EdgeAttributes edge, decimal symbolCodeOfAddedObject)
        {
            // both surroundings and second surroundings of edge attributes are updated
            UpdateSurroundingsOfEdgeAttributes(ref edge, symbolCodeOfAddedObject);
            UpdateSecondSurroundingsOfEdgeAttributes(ref edge, symbolCodeOfAddedObject);
            return edge;
        }

        private void UpdateSurroundingsOfEdgeAttributes(ref Orienteering_ISOM_2017_2.EdgeAttributes edge, decimal symbolCodeOfAddedObject)
            => edge = new Orienteering_ISOM_2017_2.EdgeAttributes( GetUpdatedSurroundingsOfEdgeAttributes(edge.Surroundings, symbolCodeOfAddedObject), edge.LineFeatures, edge.SecondSurroundings);
        
        private void UpdateSecondSurroundingsOfEdgeAttributes(ref Orienteering_ISOM_2017_2.EdgeAttributes edge, decimal symbolCodeOfAddedObject)
            => edge = new Orienteering_ISOM_2017_2.EdgeAttributes(edge.Surroundings, edge.LineFeatures, GetUpdatedSurroundingsOfEdgeAttributes(edge.SecondSurroundings, symbolCodeOfAddedObject));
        private void UpdateLinearFeaturesOfEdgeAttributes(ref Orienteering_ISOM_2017_2.EdgeAttributes edge, decimal symbolCodeOfAddedObject)
            => edge = new Orienteering_ISOM_2017_2.EdgeAttributes(edge.Surroundings, GetUpdatedLinearFeatureOfEdgeAttributes(edge.LineFeatures, symbolCodeOfAddedObject), edge.SecondSurroundings);

        private (Orienteering_ISOM_2017_2.Grounds? ground, Orienteering_ISOM_2017_2.Boulders? boulders, Orienteering_ISOM_2017_2.Stones? stones, Orienteering_ISOM_2017_2.Water? water, Orienteering_ISOM_2017_2.VegetationAndManMade? vegetationAndManMade, Orienteering_ISOM_2017_2.VegetationGoodVis? vegetationGoodVis)
            GetUpdatedSurroundingsOfEdgeAttributes((Orienteering_ISOM_2017_2.Grounds? ground, Orienteering_ISOM_2017_2.Boulders? boulders, Orienteering_ISOM_2017_2.Stones? stones, Orienteering_ISOM_2017_2.Water? water, Orienteering_ISOM_2017_2.VegetationAndManMade? vegetationAndManMade, Orienteering_ISOM_2017_2.VegetationGoodVis? vegetationGoodVis) surroundings, decimal symbolCodeOfAddedObject)
        {
            switch (symbolCodeOfAddedObject)
            {
                case 403:
                    return (surroundings.ground, surroundings.boulders, surroundings.stones, surroundings.water, Orienteering_ISOM_2017_2.VegetationAndManMade.RoughOpenLand_403, surroundings.vegetationGoodVis);
                //TODO:
                default:
                    // Console.WriteLine($"Symbol code {symbolCodeOfAddedObject} is not handled in method for updating of edge attributes surrounding property.");
                    return (surroundings.ground, surroundings.boulders, surroundings.stones, surroundings.water, surroundings.vegetationAndManMade, surroundings.vegetationGoodVis);
            }
        }

        private (Orienteering_ISOM_2017_2.NaturalLinearObstacles? naturalLinearObstacle, Orienteering_ISOM_2017_2.Paths? path, Orienteering_ISOM_2017_2.ManMadeLinearObstacles? manMadeLinearObstacle)
            GetUpdatedLinearFeatureOfEdgeAttributes((Orienteering_ISOM_2017_2.NaturalLinearObstacles? naturalLinearObstacle, Orienteering_ISOM_2017_2.Paths? path, Orienteering_ISOM_2017_2.ManMadeLinearObstacles? manMadeLinearObstacle) linearFeaturs, decimal symbolCodeOfAddedObject)
        {
            switch (symbolCodeOfAddedObject)
            {
                //TODO:
                default:
                    // Console.WriteLine($"Symbol code {symbolCodeOfAddedObject} is not handled in method for updating of edge attributes linear features property.");
                    return (linearFeaturs.naturalLinearObstacle, linearFeaturs.path, linearFeaturs.manMadeLinearObstacle);
            }
            
        }
        
        
        // 5 ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void ProcessCutBoundaryEdgesByChain(List<BoundaryVertexBuilder> chain, 
            Dictionary<(BoundaryVertexBuilder, BoundaryVertexBuilder), (Orienteering_ISOM_2017_2.EdgeAttributes cutEdgeAttributes, List<int> crossedChainEdgesIndices)> verticesOfCutBoundaryEdges, 
            List<VertexBuilder> outerVerticesOfCutEdges, decimal symbolCodeOfAddedObject)
        {
            // at first create vertex for every cross-section, then connect them correctly to the vertices of cut boundary edges and to the vertices of chain and in the end add the vertices to the chain
            var (chainVerticesOfCrossedEdgesWithTheirCrossSectionVertices, verticesOfCrossedBoundaryEdgesWithTheirCrossSectionVertices, allNewVertices) = 
                CreateCrossSectionVertices(chain, verticesOfCutBoundaryEdges);
            ConnectNewVerticesToVerticesOfCutBoundaryEdges(verticesOfCrossedBoundaryEdgesWithTheirCrossSectionVertices, verticesOfCutBoundaryEdges, outerVerticesOfCutEdges, symbolCodeOfAddedObject, allNewVertices);
            ConnectAndAddNewVerticesToChain(chainVerticesOfCrossedEdgesWithTheirCrossSectionVertices, chain, allNewVertices);
        }

        private (Dictionary<(BoundaryVertexBuilder, BoundaryVertexBuilder), List<BoundaryVertexBuilder>>, Dictionary<(BoundaryVertexBuilder, BoundaryVertexBuilder), List<BoundaryVertexBuilder>>, HashSet<BoundaryVertexBuilder>) 
            CreateCrossSectionVertices( List<BoundaryVertexBuilder> chain, Dictionary<(BoundaryVertexBuilder, BoundaryVertexBuilder), (Orienteering_ISOM_2017_2.EdgeAttributes, List<int>)> verticesOfCutBoundaryEdges)
        {
            Dictionary<(BoundaryVertexBuilder, BoundaryVertexBuilder), List<BoundaryVertexBuilder>> chainVerticesOfCrossedEdgesWithTheirCrossSectionVertices = new();
            Dictionary<(BoundaryVertexBuilder, BoundaryVertexBuilder), List<BoundaryVertexBuilder>> verticesOfCrossedBoundaryEdgesWithTheirCrossSectionVertices = new();
            HashSet<BoundaryVertexBuilder> allNewVertices = new();
            // process every boundary vertex couple whose edge was cut by the chain
            foreach (var ((vertex1, vertex2), (_, crossedChainEdgesIndices)) in verticesOfCutBoundaryEdges)
            {
                foreach (var crossedChainEdgeIndex in crossedChainEdgesIndices)
                {
                    var chainVertex1 = chain[crossedChainEdgeIndex];
                    var chainVertex2 = crossedChainEdgeIndex == chain.Count - 1 ? chain[0] : chain[crossedChainEdgeIndex + 1];
                    // retrieves coordinates of chain edge and cut boundary edge
                    // this coords should be retrieved successfully, because this cross-section was found once already , when cutting of boundary edge occured
                    MapCoordinates? crossSectionCoords = GetLineSegmentsCrossSectionCoords(chainVertex1.Position, chainVertex2.Position,  vertex1.Position, vertex2.Position);
                    if (crossSectionCoords is not null)
                    {
                        // creation of cross-section vertex and its addition to list of added vertices and list of added vertices for given cut boundary edge and chain edge
                        var newVertex = new BoundaryVertexBuilder(new Orienteering_ISOM_2017_2.VertexAttributes(crossSectionCoords.Value));
                        if (chainVerticesOfCrossedEdgesWithTheirCrossSectionVertices.ContainsKey((chainVertex1, chainVertex2))) chainVerticesOfCrossedEdgesWithTheirCrossSectionVertices[(chainVertex1, chainVertex2)].Add(newVertex);
                        else chainVerticesOfCrossedEdgesWithTheirCrossSectionVertices[(chainVertex1, chainVertex2)] = [newVertex];
                        if (verticesOfCrossedBoundaryEdgesWithTheirCrossSectionVertices.ContainsKey((vertex1, vertex2))) verticesOfCrossedBoundaryEdgesWithTheirCrossSectionVertices[(vertex1, vertex2)].Add(newVertex);
                        else verticesOfCrossedBoundaryEdgesWithTheirCrossSectionVertices[(vertex1, vertex2)] = [newVertex];
                        allNewVertices.Add(newVertex);
                    }
                }
            }
            return (chainVerticesOfCrossedEdgesWithTheirCrossSectionVertices, verticesOfCrossedBoundaryEdgesWithTheirCrossSectionVertices, allNewVertices);
        }

        private void ConnectNewVerticesToVerticesOfCutBoundaryEdges(Dictionary<(BoundaryVertexBuilder, BoundaryVertexBuilder), List<BoundaryVertexBuilder>> verticesOfCrossedBoundaryEdgesWithTheirCrossSectionVertices, Dictionary<(BoundaryVertexBuilder, BoundaryVertexBuilder), (Orienteering_ISOM_2017_2.EdgeAttributes, List<int>)> verticesOfCutBoundaryEdges, List<VertexBuilder> outerVerticesOfCutEdges, decimal symbolCodeOfAddedObject, HashSet<BoundaryVertexBuilder> allNewVertices)
        {
            foreach(var ((vertex1, vertex2), newVertices) in verticesOfCrossedBoundaryEdgesWithTheirCrossSectionVertices)
            {
                // at first, whe have to sort cross-section vertices which corresponds to some cut boundary edge, so they could be correctly connected with vertices of this edge
                SortVerticesCorrectly(vertex1, vertex2, newVertices);
                var outerEdgeAttributesFromV1ToV2 = verticesOfCutBoundaryEdges[(vertex1, vertex2)].Item1;
                var innerEdgeAttributesFromV1ToV2 = GetUpdatedInnerEdgeAttributes(outerEdgeAttributesFromV1ToV2, symbolCodeOfAddedObject);
                bool edgeToBeAddedIsOuter = outerVerticesOfCutEdges.Contains(vertex1);
                var lastVertex = vertex1;
                // we iteratively connect new vertices with each other and with vertices of cut boundary edge
                foreach (var newVertex in newVertices)
                {
                    // we have to check if newly added vertex does not overlap with the one that is going to be connected with
                    // in case of overlap, last vertex is removed and is replaced by new vertex
                    // when new vertex overlap with one of the vertices of cut edge, the vertex of cut edge is removed and replaced by newly added vertex
                    BoundaryVertexBuilder? overlappedVertex = newVertex.Position == lastVertex.Position 
                        ? lastVertex 
                        : newVertex.Position == vertex2.Position 
                            ? vertex2 
                            : null;
                    if (overlappedVertex is not null)
                    {
                        foreach (var (neighbor, edgeAttributesWithNeighbor) in overlappedVertex.NonBoundaryEdges)
                        {
                            newVertex.NonBoundaryEdges[neighbor] = edgeAttributesWithNeighbor;
                            if (neighbor is BoundaryVertexBuilder boundaryNeighbor)
                            {
                                boundaryNeighbor.NonBoundaryEdges[newVertex] = boundaryNeighbor.NonBoundaryEdges[overlappedVertex];
                                boundaryNeighbor.NonBoundaryEdges.Remove(overlappedVertex);
                            }
                            else if (neighbor is NetVertexBuilder netNeighbor)
                            {
                                netNeighbor.NonBoundaryEdges.Add(newVertex);
                                netNeighbor.NonBoundaryEdges.Remove(overlappedVertex);
                            }
                            overlappedVertex.NonBoundaryEdges.Remove(neighbor);
                        }
                        foreach (var (neighbor, edgeAttributesWithNeighbor) in overlappedVertex.BoundaryEdges)
                        {
                            newVertex.BoundaryEdges[neighbor] = edgeAttributesWithNeighbor;
                            neighbor.BoundaryEdges[newVertex] = neighbor.BoundaryEdges[overlappedVertex];
                            neighbor.BoundaryEdges.Remove(overlappedVertex);
                            overlappedVertex.BoundaryEdges.Remove(neighbor);
                        }
                        allNewVertices.Remove(overlappedVertex);
                        lastVertex = newVertex;
                        continue;
                    }
                    // attributes of newly added edges between new vertex and last vertex are in rotation assigned inner and outer attributes
                    // this behaviour ensures correct attributes assigment to newly created edges
                    lastVertex.BoundaryEdges[newVertex] = edgeToBeAddedIsOuter ? outerEdgeAttributesFromV1ToV2 : innerEdgeAttributesFromV1ToV2;
                    newVertex.BoundaryEdges[lastVertex] = edgeToBeAddedIsOuter 
                        ? new Orienteering_ISOM_2017_2.EdgeAttributes(outerEdgeAttributesFromV1ToV2.SecondSurroundings, outerEdgeAttributesFromV1ToV2.Surroundings) 
                        : new Orienteering_ISOM_2017_2.EdgeAttributes(innerEdgeAttributesFromV1ToV2.SecondSurroundings, innerEdgeAttributesFromV1ToV2.Surroundings);
                    edgeToBeAddedIsOuter = !edgeToBeAddedIsOuter;
                    lastVertex = newVertex;
                }
                if (lastVertex != vertex2)
                {
                    lastVertex.BoundaryEdges[vertex2] = edgeToBeAddedIsOuter ? outerEdgeAttributesFromV1ToV2 : innerEdgeAttributesFromV1ToV2;
                    vertex2.BoundaryEdges[lastVertex] = edgeToBeAddedIsOuter 
                        ? new Orienteering_ISOM_2017_2.EdgeAttributes(outerEdgeAttributesFromV1ToV2.SecondSurroundings, outerEdgeAttributesFromV1ToV2.Surroundings) 
                        : new Orienteering_ISOM_2017_2.EdgeAttributes(innerEdgeAttributesFromV1ToV2.SecondSurroundings, innerEdgeAttributesFromV1ToV2.Surroundings);
                }
            }
        }


        private void ConnectAndAddNewVerticesToChain( Dictionary<(BoundaryVertexBuilder, BoundaryVertexBuilder), List<BoundaryVertexBuilder>> chainVerticesOfCrossedEdgesWithTheirCrossSectionVertices, List<BoundaryVertexBuilder> chain, HashSet<BoundaryVertexBuilder> allnewVertices)
        {
            foreach (var ((vertex1, vertex2), newVertices) in chainVerticesOfCrossedEdgesWithTheirCrossSectionVertices)
            {
                // at first, we have to sort cross-section vertices so they could be correctly connected to the chain vertices
                SortVerticesCorrectly(vertex1, vertex2, newVertices);
                ConnectNewVerticesToChain(newVertices, vertex1, vertex2);
                AddNewVerticesToChain(newVertices, vertex1, vertex2, chain, allnewVertices);
            }
        }

        private void ConnectNewVerticesToChain(List<BoundaryVertexBuilder> sortedVertices, BoundaryVertexBuilder vertex1, BoundaryVertexBuilder vertex2)
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

        private void AddNewVerticesToChain(List<BoundaryVertexBuilder> sortedVertices, BoundaryVertexBuilder vertex1, BoundaryVertexBuilder vertex2, List<BoundaryVertexBuilder> chain, HashSet<BoundaryVertexBuilder> allNewVertices)
        {
            int chainIndex = chain.IndexOf(vertex1); 
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
                    foreach (var (neighbor, edgeAttributesWithNeighbor) in vertex.NonBoundaryEdges)
                    {
                        overlappedVertex.NonBoundaryEdges[neighbor] = edgeAttributesWithNeighbor;
                        if (neighbor is BoundaryVertexBuilder boundaryNeighbor)
                        {
                            boundaryNeighbor.NonBoundaryEdges[overlappedVertex] = boundaryNeighbor.NonBoundaryEdges[vertex];
                            boundaryNeighbor.NonBoundaryEdges.Remove(vertex);
                        }
                        else if (neighbor is NetVertexBuilder netNeighbor)
                        {
                            netNeighbor.NonBoundaryEdges.Add(overlappedVertex);
                            netNeighbor.NonBoundaryEdges.Remove(vertex);
                        }
                        vertex.NonBoundaryEdges.Remove(neighbor);
                    }
                    foreach (var (neighbor, edgeAttributesWithNeighbor) in vertex.BoundaryEdges)
                    {
                        overlappedVertex.BoundaryEdges[neighbor] = edgeAttributesWithNeighbor;
                        neighbor.BoundaryEdges[overlappedVertex] = neighbor.BoundaryEdges[vertex];
                        neighbor.BoundaryEdges.Remove(vertex);
                        vertex.BoundaryEdges.Remove(neighbor);
                    }
                    if (overlappedVertex == vertex2)
                    {
                        lastVertex = vertex2;
                        chainIndex++;
                    }
                    allNewVertices.Remove(vertex);
                }
                else
                    chain.Insert(++chainIndex, vertex);
            }
        }

        
        // 6 ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void ConnectChainToVerticesOfCutEdgesAndOtherVerticesOfChain(BoundaryVertexBuilder[] chain, List<VertexBuilder> outerVerticesOfCutEdges, List<VertexBuilder> innerVerticesOfCutEdges, IEditableRadiallySearchableDataStruct<VertexBuilder> allVertices, int standardEdgeLength, bool polygonalChain)
        {
            // connectable vertices consists of chain vertices and outer and inner vertices of edges, which were cut by chain
            IEditableRadiallySearchableDataStruct<VertexBuilder> connectableVertices = new RadiallySearchableKdTree<VertexBuilder>(chain.Concat(outerVerticesOfCutEdges).Concat(innerVerticesOfCutEdges), vertexBuilder => (vertexBuilder.Position.XPos, vertexBuilder.Position.YPos));
            // at first, we have to find all boundary vertices in standard edge length distance from vertices of chain
            // these boundary vertices will also include chain vertices
            // we will need them so we could test, whether newly added edges does not cross some boundary edge of previously added objects
            var closeBoundaryVerticesToChainsVertices = FindCloseBoundaryVerticesToChain(chain, allVertices, standardEdgeLength);
            for (int i = 0; i < chain.Length; ++i)
            {
                var closeVertices = connectableVertices.FindInEuclideanDistanceFrom((chain[i].Position.XPos, chain[i].Position.YPos), standardEdgeLength);
                int iP = i == 0 ? chain.Length - 1 : i - 1;
                int iS = i == chain.Length - 1 ? 0 : i + 1;
                foreach (var closeVertex in closeVertices)
                    // we do not connect vertices by non boundary edges to vertices which are connected to current vertex by boundary edge already 
                    if (closeVertex is not BoundaryVertexBuilder boundaryCloseVertex || !chain[i].BoundaryEdges.ContainsKey(boundaryCloseVertex))
                    {
                        // check, if created edge does not cut any boundary edge 
                        if (DoesNotCrossSomeBoundaryEdge(chain[i], closeVertex, closeBoundaryVerticesToChainsVertices[i]))
                        {
                            if (closeVertex is NetVertexBuilder closeNetVertex)
                            {
                                ConnectBoundaryVertexAndNetVertex(chain[i], closeNetVertex);
                            }
                            else if (closeVertex is BoundaryVertexBuilder closeBoundaryVertex && !chain[i].BoundaryEdges.ContainsKey(closeBoundaryVertex))
                            {
                                ConnectTwoBoundaryVerticesWithNonBoundaryEdge(chain[i], closeBoundaryVertex);
                            }
                        }
                    }
            }
        }

        private List<List<BoundaryVertexBuilder>> FindCloseBoundaryVerticesToChain(BoundaryVertexBuilder[] chain, IEditableRadiallySearchableDataStruct<VertexBuilder> allVertices,  int standardEdgeLength)
        {
            List<List<BoundaryVertexBuilder>> closeBoundaryVerticesToChainsVertices = new List<List<BoundaryVertexBuilder>>();
            IRadiallySearchableDataStruct<BoundaryVertexBuilder> searchableChain = new RadiallySearchableKdTree<BoundaryVertexBuilder>(chain, vertexBuilder => (vertexBuilder.Position.XPos, vertexBuilder.Position.YPos));
            foreach (var chainVertex in chain)
            {
                var allCloseVerticesToChainVertex = allVertices.FindInEuclideanDistanceFrom((chainVertex.Position.XPos, chainVertex.Position.YPos), standardEdgeLength);
                closeBoundaryVerticesToChainsVertices.Add(allCloseVerticesToChainVertex.OfType<BoundaryVertexBuilder>().ToList());
                var allCloseChainVerticesToChainVertex = searchableChain.FindInEuclideanDistanceFrom((chainVertex.Position.XPos, chainVertex.Position.YPos), standardEdgeLength);
                closeBoundaryVerticesToChainsVertices.Last().AddRange(allCloseChainVerticesToChainVertex);
            }
            return closeBoundaryVerticesToChainsVertices;
        }

        private bool DoesNotCrossSomeBoundaryEdge(BoundaryVertexBuilder chainVertex, VertexBuilder closeVertex, List<BoundaryVertexBuilder> closeBoundaryVerticesToBeChecked)
        {
            foreach (var closeBoundaryVertex in closeBoundaryVerticesToBeChecked)
            {
                // if close boundary vertex is tested close vertex or tested chain vertex, we do not test them
                if (closeVertex == closeBoundaryVertex || chainVertex == closeBoundaryVertex) continue;
                foreach (var neighborOfCloseBoundaryVertex in closeBoundaryVertex.BoundaryEdges.Keys)
                {
                    // same think holds in case of neighbor of close boundary vertex
                    if (closeVertex == neighborOfCloseBoundaryVertex || chainVertex == neighborOfCloseBoundaryVertex) continue;
                    if (AreLineSegmentsCrossing(chainVertex.Position, closeVertex.Position, closeBoundaryVertex.Position, neighborOfCloseBoundaryVertex.Position)) return false;
                }
            }
            return true;
        }

        private void ConnectBoundaryVertexAndNetVertex(BoundaryVertexBuilder boundaryVertex, NetVertexBuilder netVertex)
        {
            // net vertices are connected by edges whose attributes are correctly selected based on attributes of another edge of net vertex
            // here is used the assumption, that edges of net vertices have same attributes
            boundaryVertex.NonBoundaryEdges[netVertex] = new Orienteering_ISOM_2017_2.EdgeAttributes(netVertex.Surroundings);
            netVertex.NonBoundaryEdges.Add(boundaryVertex);
        }

        private void ConnectTwoBoundaryVerticesWithNonBoundaryEdge(BoundaryVertexBuilder boundaryVertex1, BoundaryVertexBuilder boundaryVertex2)
        {
            // edges between boundary edges will be assigned with blank attributes
            // correct attributes will be assigned to these edges in the future
            boundaryVertex1.NonBoundaryEdges[boundaryVertex2] = new Orienteering_ISOM_2017_2.EdgeAttributes();
            boundaryVertex2.NonBoundaryEdges[boundaryVertex1] = new Orienteering_ISOM_2017_2.EdgeAttributes();
        }
        
        // 7 ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void SetAttributesOfChainsEdges(BoundaryVertexBuilder[] chain, List<VertexBuilder> outerVertices, decimal symbolCodeOfAddedObject)
        {
            // at first, we will obtain indices of new cross-section vertices added to chain, together with their closest outer neighbor angle-wise
            // these vertices will be used for determining of correct attribute setting of chains edges 
            var indicesAndOuterBoundaryNeighborsOfCrossSectionVertices = GetIndicesAndTheClosestOuterNeighborsOfVerticesOfCrossedBoundaryEdges(chain);
            // if count of cross-section vertices is less than 2, it means that all edges of the chain has surly the same attributes
            if (indicesAndOuterBoundaryNeighborsOfCrossSectionVertices.Count < 2)
                SetAttributesOfChainBoundaryEdgesUniformly(chain, outerVertices, symbolCodeOfAddedObject);
            else
                SetAttributesOfChainBoundaryEdgesNonUniformly(chain, indicesAndOuterBoundaryNeighborsOfCrossSectionVertices, symbolCodeOfAddedObject);
        }
        
        private List<(int, BoundaryVertexBuilder)> GetIndicesAndTheClosestOuterNeighborsOfVerticesOfCrossedBoundaryEdges(BoundaryVertexBuilder[] chain)
        {
            List<(int, BoundaryVertexBuilder)> indicesAndOuterNeighborsOfVerticesOfCrossedBoundaries = new();
            
            // we will iteratively determine the closest left-handed boundary neighbor of each vertex
            for (int i = 0; i < chain.Length; i++)
            {
                int iP = i == 0 ? chain.Length - 1 : i - 1; 
                int iS = i == chain.Length - 1 ? 0 : i + 1;
                // closest left-handed neighbor is set at first to the previous chain vertex
                // this will ensure, that found closest left-handed boundary vertex will be outer vertex  
                var angleOfTheClosestNeighborToNextEdgeSoFar = ComputeLeftHandedAngleBetween(chain[i].Position, chain[iS].Position, chain[i].Position, chain[iP].Position);
                var closestNeighbor = chain[iP];
                foreach (var (neighbor, _) in chain[i].BoundaryEdges)
                {
                    if (neighbor == chain[iS]) continue;
                    double angleOfTheNeighborToNextEdge = ComputeLeftHandedAngleBetween(chain[i].Position, chain[iS].Position, chain[i].Position, neighbor.Position);
                    if (angleOfTheNeighborToNextEdge < angleOfTheClosestNeighborToNextEdgeSoFar)
                    {
                        angleOfTheClosestNeighborToNextEdgeSoFar = angleOfTheNeighborToNextEdge;
                        closestNeighbor = neighbor;
                    }
                }
                // if the closest left-handed vertex is the previous chain vertex, current vertex is not added into final collection
                if(closestNeighbor != chain[iP]) indicesAndOuterNeighborsOfVerticesOfCrossedBoundaries.Add((i, closestNeighbor));
            }
            return indicesAndOuterNeighborsOfVerticesOfCrossedBoundaries;
        }

        private double ComputeLeftHandedAngleBetween(MapCoordinates v1s, MapCoordinates v1e, MapCoordinates v2s, MapCoordinates v2e)
        {
            var v1 = v1e - v1s;
            var v2 = v2e - v2s;
            double angle = Math.Acos((v1.XPos * v2.XPos + v1.YPos * v2.YPos)/(v1.Length()*v2.Length()));
            // if scalar product of vector 2 and right side normal vector of vector 1 is positive, computed angle is inverted 
            var v1RightNorm = new MapCoordinates(v1.YPos, -v1.XPos); 
            return v1RightNorm.XPos * v2.XPos + v1RightNorm.YPos * v2.YPos > 0 ? 2*Math.PI - angle : angle;
        }

        private void SetAttributesOfChainBoundaryEdgesUniformly(BoundaryVertexBuilder[] chain, List<VertexBuilder> outerVertices, decimal symbolCodeOfAddedObject)
        {
            // attributes are determined from any outer 
            var outerEdgeSurroundingsForWholeChain = TryFindEdgeAttributesOfSomeOuterNetVertexConnectedToChainInGivenInterval(chain, outerVertices, 0, chain.Length - 1) ?? (null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.Forest_405, null);
            SetAttributesOfChainBoundaryEdgesBasedOnGivenOuterSurroundingsOnGivenInterval(chain, outerEdgeSurroundingsForWholeChain, symbolCodeOfAddedObject, 0, chain.Length - 1);
            var innerSurroundings = GetUpdatedSurroundingsOfEdgeAttributes(outerEdgeSurroundingsForWholeChain, symbolCodeOfAddedObject);
            chain.Last().BoundaryEdges[chain[0]] = new Orienteering_ISOM_2017_2.EdgeAttributes(outerEdgeSurroundingsForWholeChain, innerSurroundings);
            chain[0].BoundaryEdges[chain.Last()] = new Orienteering_ISOM_2017_2.EdgeAttributes(innerSurroundings, outerEdgeSurroundingsForWholeChain); 
        }

        private void SetAttributesOfChainBoundaryEdgesNonUniformly(BoundaryVertexBuilder[] chain,  List<(int index, BoundaryVertexBuilder neighbor)> indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices, decimal symbolCodeOfAddedObject)
        {
            for (int i = 0; i < indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices.Count - 1; i++)
            {
                var outerEdgeSurroundingsForIntervalOfChain = chain[indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices[i].index].BoundaryEdges[indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices[i].neighbor].SecondSurroundings;
                SetAttributesOfChainBoundaryEdgesBasedOnGivenOuterSurroundingsOnGivenInterval(chain, outerEdgeSurroundingsForIntervalOfChain, symbolCodeOfAddedObject, indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices[i].index, indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices[i+1].index);
            } 
            var outerEdgeSurroundingsForLastIntervalOfChain = chain[indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices.Last().index].BoundaryEdges[indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices.Last().neighbor].SecondSurroundings;
            SetAttributesOfChainBoundaryEdgesBasedOnGivenOuterSurroundingsOnGivenInterval(chain, outerEdgeSurroundingsForLastIntervalOfChain, symbolCodeOfAddedObject, indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices.Last().index, chain.Length-1);
            SetAttributesOfChainBoundaryEdgesBasedOnGivenOuterSurroundingsOnGivenInterval(chain, outerEdgeSurroundingsForLastIntervalOfChain, symbolCodeOfAddedObject, 0, indicesAndClosestOuterBoundaryNeighborsOfCrossSectionVertices[0].index);
            var innerSurroundings = GetUpdatedSurroundingsOfEdgeAttributes(outerEdgeSurroundingsForLastIntervalOfChain, symbolCodeOfAddedObject);
            chain.Last().BoundaryEdges[chain[0]] = new Orienteering_ISOM_2017_2.EdgeAttributes(outerEdgeSurroundingsForLastIntervalOfChain, innerSurroundings);
            chain[0].BoundaryEdges[chain.Last()] = new Orienteering_ISOM_2017_2.EdgeAttributes(innerSurroundings, outerEdgeSurroundingsForLastIntervalOfChain); 
        }

        private(Orienteering_ISOM_2017_2.Grounds?, Orienteering_ISOM_2017_2.Boulders?, Orienteering_ISOM_2017_2.Stones?, Orienteering_ISOM_2017_2.Water?, Orienteering_ISOM_2017_2.VegetationAndManMade?, Orienteering_ISOM_2017_2.VegetationGoodVis?)? 
            TryFindEdgeAttributesOfSomeOuterNetVertexConnectedToChainInGivenInterval(BoundaryVertexBuilder[] chain, List<VertexBuilder> outerVertices, int startIndex, int endIndex)
        {
            if (startIndex >= endIndex || startIndex < 0 || endIndex >= chain.Length) return null;
            for(int i = startIndex; i <= endIndex; ++i)
                foreach (var (neighbor, edgeAttributes) in chain[i].NonBoundaryEdges)
                    if (neighbor is NetVertexBuilder && outerVertices.Contains(neighbor))
                        return edgeAttributes.Surroundings;
            return null;
        }

        private void SetAttributesOfChainBoundaryEdgesBasedOnGivenOuterSurroundingsOnGivenInterval( BoundaryVertexBuilder[] chain, (Orienteering_ISOM_2017_2.Grounds?, Orienteering_ISOM_2017_2.Boulders?, Orienteering_ISOM_2017_2.Stones?, Orienteering_ISOM_2017_2.Water?, Orienteering_ISOM_2017_2.VegetationAndManMade?, Orienteering_ISOM_2017_2.VegetationGoodVis?) outerSurroundings, decimal symbolCodeOfAddedObject, int startIndex, int endIndex)
        {
            if (startIndex >= endIndex || startIndex < 0 || endIndex >= chain.Length) return;
            var innerSurroundings = GetUpdatedSurroundingsOfEdgeAttributes(outerSurroundings, symbolCodeOfAddedObject);
            
            chain[startIndex].BoundaryEdges[chain[startIndex + 1]] = new Orienteering_ISOM_2017_2.EdgeAttributes(outerSurroundings, innerSurroundings);
            for (int i = startIndex + 1; i < endIndex; ++i)
            {
                chain[i].BoundaryEdges[chain[i - 1]] = new Orienteering_ISOM_2017_2.EdgeAttributes( innerSurroundings, outerSurroundings);
                chain[i].BoundaryEdges[chain[i + 1]] = new Orienteering_ISOM_2017_2.EdgeAttributes( outerSurroundings, innerSurroundings);
            }
            chain[endIndex].BoundaryEdges[chain[endIndex - 1]] = new Orienteering_ISOM_2017_2.EdgeAttributes(innerSurroundings, outerSurroundings); 
        }

        // 8 ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void SetAttributesOfNonBoundaryEdgesBetweenChainAndBoundaryVertices( BoundaryVertexBuilder[] chain)
        {
            foreach (var chainVertex in chain)
            {
                SetAttributesOfNonBoundaryEdgesDirectedToBoundaryVertices(chainVertex);
            }
        }

        private void SetAttributesOfNonBoundaryEdgesDirectedToBoundaryVertices(BoundaryVertexBuilder chainVertex)
        {
            foreach (var (neighbor, edgeAttributes) in chainVertex.NonBoundaryEdges)
            {
                if (neighbor is BoundaryVertexBuilder boundaryNeighbor && edgeAttributes == new Orienteering_ISOM_2017_2.EdgeAttributes())
                {
                    var attributesOfTheClosesLefHandedEdge = FindAttributesOfClosestBoundaryEdgeOfChainVertexTo(boundaryNeighbor, chainVertex);
                    chainVertex.NonBoundaryEdges[boundaryNeighbor] = new Orienteering_ISOM_2017_2.EdgeAttributes(attributesOfTheClosesLefHandedEdge.SecondSurroundings);
                    boundaryNeighbor.NonBoundaryEdges[chainVertex] = new Orienteering_ISOM_2017_2.EdgeAttributes(attributesOfTheClosesLefHandedEdge.SecondSurroundings);
                }
            }
        }

        private Orienteering_ISOM_2017_2.EdgeAttributes FindAttributesOfClosestBoundaryEdgeOfChainVertexTo(
            VertexBuilder neighbor, BoundaryVertexBuilder chainVertex)
        {
            var angleOfTheClosestLefHandedBoundaryNeighborSoFar = 2 * Math.PI;
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
        
        // 9 ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void AddChainVerticesToTheGraph(BoundaryVertexBuilder[] chain, IEditableRadiallySearchableDataStruct<VertexBuilder> allVertices)
        {
            foreach (var chainVertex in chain)
            {
                if (allVertices.TryFindAt(chainVertex, out var foundVertex))
                {
                    ResolveOverlappingOfChainVertexWithVertexInGraph(chainVertex, foundVertex);
                    allVertices.Delete(foundVertex);
                }
                allVertices.Insert(chainVertex);
            }
        }

        private void ResolveOverlappingOfChainVertexWithVertexInGraph(BoundaryVertexBuilder chainVertex, VertexBuilder overlappedVertex)
        {
            if (overlappedVertex is BoundaryVertexBuilder overlappedBoundaryVertex)
            {
                foreach (var (overlappedBoundaryVertexNeighbor, edgeAttributes) in overlappedBoundaryVertex.BoundaryEdges)
                {
                    if (!chainVertex.BoundaryEdges.ContainsKey(overlappedBoundaryVertexNeighbor))
                    {
                        chainVertex.BoundaryEdges[overlappedBoundaryVertexNeighbor] = edgeAttributes;
                        overlappedBoundaryVertexNeighbor.BoundaryEdges[chainVertex] = overlappedBoundaryVertexNeighbor.BoundaryEdges[overlappedBoundaryVertex];
                    }
                    overlappedBoundaryVertex.BoundaryEdges.Remove(overlappedBoundaryVertexNeighbor);
                    overlappedBoundaryVertexNeighbor.BoundaryEdges.Remove(overlappedBoundaryVertex);
                }
                foreach (var (overlappedBoundaryVertexNeighbor,_) in overlappedBoundaryVertex.NonBoundaryEdges)
                {
                    if (overlappedBoundaryVertexNeighbor is BoundaryVertexBuilder overlappedBoundaryVertexBoundaryNeighbor)
                        overlappedBoundaryVertexBoundaryNeighbor.NonBoundaryEdges.Remove(overlappedBoundaryVertex);
                    else if (overlappedBoundaryVertexNeighbor is NetVertexBuilder overlappedBoundaryVertexNetNeighbor) 
                        overlappedBoundaryVertexNetNeighbor.NonBoundaryEdges.Remove(overlappedBoundaryVertex);
                    overlappedBoundaryVertex.NonBoundaryEdges.Remove(overlappedBoundaryVertexNeighbor);
                }
            }
            else if (overlappedVertex is NetVertexBuilder overlappedNetVertex)
                foreach (var overlappedNetVertexNeighbor in overlappedNetVertex.NonBoundaryEdges)
                {
                    if (overlappedNetVertexNeighbor is BoundaryVertexBuilder overlappedNetVertexBoundaryNeighbor) 
                        overlappedNetVertexBoundaryNeighbor.NonBoundaryEdges.Remove(overlappedVertex);
                    else if (overlappedNetVertexNeighbor is NetVertexBuilder overlappedNetVertexNetNeighbor) 
                        overlappedNetVertexNetNeighbor.NonBoundaryEdges.Remove(overlappedVertex);
                    overlappedNetVertex.NonBoundaryEdges.Remove(overlappedNetVertexNeighbor);
                }
        }
    }
    

    private abstract class VertexBuilder(Orienteering_ISOM_2017_2.VertexAttributes attributes)
    {
        public Orienteering_ISOM_2017_2.VertexAttributes Attributes = attributes;

        // public abstract HashSet<VertexBuilder> NonBoundaryEdges { get; }
        public BuildableVertex? BuiltVertex { get; private set; }

        public MapCoordinates Position => Attributes.Position;
        public abstract bool IsStationary { get; set; }

        public BuildableVertex Build()
        {
            if (BuiltVertex is not null) return BuiltVertex;
            BuiltVertex = new BuildableVertex(Attributes);
            return BuiltVertex;
        }

        public abstract void ConnectAfterBuild();
    }

    private class BoundaryVertexBuilder(Orienteering_ISOM_2017_2.VertexAttributes attributes) : VertexBuilder(attributes)
    {
        
        // public override HashSet<VertexBuilder> NonBoundaryEdges => NonBoundaryAttributeEdges.Keys.ToHashSet();
        public Dictionary<VertexBuilder, Orienteering_ISOM_2017_2.EdgeAttributes> NonBoundaryEdges { get; } = new ();
        public Dictionary<BoundaryVertexBuilder, Orienteering_ISOM_2017_2.EdgeAttributes> BoundaryEdges { get; } = new ();
        public override bool IsStationary { get; set;} = true;
        // public bool LeftSideIsOuter { get; set; } -> YES
        public override void ConnectAfterBuild()
        {
            if (BuiltVertex is null) return;
            foreach (var (builder, edgeAttributes) in BoundaryEdges)
            {
                BuildableVertex? vertex = builder.BuiltVertex; 
                if (vertex is null) continue;
                BuiltVertex.AddEdge(new (edgeAttributes, vertex));
            }
            foreach (var (builder, edgeAttributes) in NonBoundaryEdges)
            {
                BuildableVertex? vertex = builder.BuiltVertex; 
                if (vertex is null) continue;
                BuiltVertex.AddEdge(new (edgeAttributes, vertex));
            }
        }
    }

    private class NetVertexBuilder(Orienteering_ISOM_2017_2.VertexAttributes attributes): VertexBuilder(attributes)
    {
        public HashSet<VertexBuilder> NonBoundaryEdges { get; } = new();

        public (Orienteering_ISOM_2017_2.Grounds?, Orienteering_ISOM_2017_2.Boulders?, Orienteering_ISOM_2017_2.Stones?, Orienteering_ISOM_2017_2.Water?, Orienteering_ISOM_2017_2.VegetationAndManMade?, Orienteering_ISOM_2017_2.VegetationGoodVis?)
            Surroundings = (null, null, null, null, null, null);

        public override void ConnectAfterBuild()
        {
            if (BuiltVertex is null) return;
            foreach (var builder in NonBoundaryEdges)
            {
                BuildableVertex? vertex = builder.BuiltVertex;
                if (vertex is null) continue;
                BuiltVertex.AddEdge(new (new Orienteering_ISOM_2017_2.EdgeAttributes(Surroundings), vertex));
            }
        }

        public override bool IsStationary { get; set;} = false;
    }

    private class BuildableVertex : ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Vertex
    {
        public BuildableVertex(Orienteering_ISOM_2017_2.VertexAttributes attributes) : base(attributes, new List<ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge>()) { }
        public void AddEdge(ICompleteNetIntertwiningGraph<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>.Edge edge) 
            => _outgoingWeightedEdges[edge] = null;
    }

}

