using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using DynamicData.Kernel;
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
using Splat.ApplicationPerformanceMonitoring;

namespace Optepafi.Models.MapRepreMan.Implementations.Representatives.Specific.CompleteIterativelySnapping;

public class CompleteSnappingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationRep : 
    ElevDataIndepImplementationRep<Orienteering_ISOM_2017_2, OmapMap, OmapMap, CompleteSnappingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation, CompleteSnappingMapRepreConfiguration, ICompleteSnappingGraph.Vertex<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>, ICompleteSnappingGraph.Edge<Orienteering_ISOM_2017_2.EdgeAttributes, Orienteering_ISOM_2017_2.VertexAttributes>, Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>
{
    public static CompleteSnappingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationRep Instance { get; } = new();
    private CompleteSnappingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementationRep(){ }

    public override Orienteering_ISOM_2017_2 UsedTemplate { get; } = Orienteering_ISOM_2017_2.Instance;
    public override IMapFormat<OmapMap> UsedMapFormat { get; } = OmapMapRepresentative.Instance;

    public override CompleteSnappingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation CreateImplementation(Orienteering_ISOM_2017_2 template, OmapMap map,
        CompleteSnappingMapRepreConfiguration configuration, IProgress<MapRepreConstructionReport>? progress, CancellationToken? cancellationToken)
    {
        return GraphCreator.Create(map, configuration, progress, cancellationToken);
    }


    private static class GraphCreator
    {
        public static CompleteSnappingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation Create(OmapMap map,
            CompleteSnappingMapRepreConfiguration configuration, IProgress<MapRepreConstructionReport>? progress,
            CancellationToken? cancellationToken)
        {
            List<VertexBuilder> vertexBuildersInNet = CreateNet(map, configuration);
            if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return new CompleteSnappingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation(new RadiallySearchableKdTree<ICompleteSnappingGraph.Vertex<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>>(v => (v.Attributes.Position.XPos, v.Attributes.Position.YPos)), configuration);
            
            IEditableRadiallySearchableDataStruct<VertexBuilder> vertexBuilders = new RadiallySearchableKdTree<VertexBuilder>(vertexBuildersInNet, vb => (vb.Position.XPos, vb.Position.YPos));
            int allObjectsCount = map.Objects.Values.Sum(x => x.Count);
            int processedObjectsCount = 0;
            foreach (var crossablePolygonalSymbolCode in OrderedCrossablePolygonalSymbolsCodes)
                if (map.Symbols.Contains(new OmapMap.Symbol(crossablePolygonalSymbolCode)))
                    foreach (var obj in map.Objects[crossablePolygonalSymbolCode])
                    {
                        ProcessCrossablePolygonalObject(obj, vertexBuilders, configuration, cancellationToken);
                        if (progress is not null && ++processedObjectsCount % (allObjectsCount/100) == 0) progress.Report(new MapRepreConstructionReport(processedObjectsCount/(float)allObjectsCount));
                        if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return new CompleteSnappingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation(new RadiallySearchableKdTree<ICompleteSnappingGraph.Vertex<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>>(v => (v.Attributes.Position.XPos, v.Attributes.Position.YPos)), configuration);
                    }
            foreach (var pathSymbolCode in OrderedPathsSymbolsCodes)
                if (map.Symbols.Contains(new OmapMap.Symbol(pathSymbolCode)))
                    foreach (var obj in map.Objects[pathSymbolCode])
                    {
                        ProcessPathObject(obj, vertexBuilders, configuration, cancellationToken);
                        if (progress is not null && ++processedObjectsCount % (allObjectsCount/100) == 0) progress.Report(new MapRepreConstructionReport(processedObjectsCount/(float)allObjectsCount));
                        if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return new CompleteSnappingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation(new RadiallySearchableKdTree<ICompleteSnappingGraph.Vertex<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>>(v => (v.Attributes.Position.XPos, v.Attributes.Position.YPos)), configuration);
                    }
            foreach (var linearObstacleSymbolCode in OrderedLinearObstacleSymbolsCodes)
                if (map.Symbols.Contains(new OmapMap.Symbol(linearObstacleSymbolCode)))
                    foreach (var obj in map.Objects[linearObstacleSymbolCode])
                    {
                        ProcessLinearObstacleObject(obj, vertexBuilders, configuration, cancellationToken);
                        if (progress is not null && ++processedObjectsCount % (allObjectsCount/100) == 0) progress.Report(new MapRepreConstructionReport(processedObjectsCount/(float)allObjectsCount));
                        if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return new CompleteSnappingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation(new RadiallySearchableKdTree<ICompleteSnappingGraph.Vertex<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>>(v => (v.Attributes.Position.XPos, v.Attributes.Position.YPos)), configuration);
                    }
            foreach (var uncrossablePolygonalSymbolCode in OrderedUncrossablePolygonalSymbolsCodes)
                if (map.Symbols.Contains(new OmapMap.Symbol(uncrossablePolygonalSymbolCode)))
                    foreach (var obj in map.Objects[uncrossablePolygonalSymbolCode])
                    {
                        ProcessUncrossablePolygonalObject(obj, vertexBuilders, configuration, cancellationToken);
                        if (progress is not null && ++processedObjectsCount % (allObjectsCount/100) == 0) progress.Report(new MapRepreConstructionReport(processedObjectsCount/(float)allObjectsCount));
                        if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return new CompleteSnappingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation(new RadiallySearchableKdTree<ICompleteSnappingGraph.Vertex<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>>(v => (v.Attributes.Position.XPos, v.Attributes.Position.YPos)), configuration);
                    }
            RadiallySearchableKdTree<BuildableVertex> vertices = new RadiallySearchableKdTree<BuildableVertex>(vertexBuilders.Select(vb => vb.Build()), v => (v.Attributes.Position.XPos, v.Attributes.Position.YPos));
            return new CompleteSnappingElevDataIndepOrienteering_ISOM_2017_2OmapMapImplementation(vertices, configuration);
        }
        
        private static List<decimal> OrderedCrossablePolygonalSymbolsCodes =
        [
            403, 404, 404.1m, 413.1m, 213, 414.1m, 401, 402, 402.1m, 413, 412, 414, 407, 409, 406, 406.1m, 408, 408.1m, 408.2m,
            410, 410.1m, 410.2m, 410.3m, 410.4m, 214, 405, 302, 302.1m, 302.5m, 308, 310, 501, 501.1m, 113, 114, 210, 205, 211, 209, 212
        ];

        private static List<decimal> OrderedPathsSymbolsCodes =
        [
            502, 502.2m, 503, 504, 505, 506, 507, 508, 508.1m, 508.2m, 508.3m, 508.4m, 532 
        ];
        
        private static List<decimal> OrderedLinearObstacleSymbolsCodes =
        [
            104, 105, 107, 304, 201, 201.3m, 202, 202.2m, 215, 513, 515, 516, 518, 528, 529
        ];

        private static List<decimal> OrderedUncrossablePolygonalSymbolsCodes =
        [
            520, 307, 307.1m, 301, 301.1m, 301.3m, 521, 521.3m, 206, 520.2m
        ];

        private static List<VertexBuilder> CreateNet(OmapMap map, CompleteSnappingMapRepreConfiguration configuration) => configuration.typeOfNet.AllValues[configuration.typeOfNet.IndexOfSelectedValue] switch
        {
            CompleteSnappingMapRepreConfiguration.NetTypesEnumeration.Triangular => CreateTriangularNet(map, configuration),
            _ => throw new InvalidEnumArgumentException()
        };

        private static List<VertexBuilder> CreateTriangularNet(OmapMap map, CompleteSnappingMapRepreConfiguration configuration)
        {
            (int left, int top, int right, int bottom) boundaries = (map.WesternmostCoords.XPos, map.NorthernmostCoords.YPos, map.EasternmostCoords.XPos, map.SouthernmostCoords.YPos);
            int edgeLength = configuration.averageEdgeLength.Value;
            int colls = (boundaries.right - boundaries.left)/edgeLength + 1;
            int rows = (int)((boundaries.top - boundaries.bottom)/(Math.Sqrt(3)*edgeLength)/2) + 1;
            List<VertexBuilder> vertices = new List<VertexBuilder>();
            List<VertexBuilder> lastRow = new List<VertexBuilder>();
            List<VertexBuilder> currentRow = new List<VertexBuilder>();
            Orienteering_ISOM_2017_2.EdgeAttributes justForest = new Orienteering_ISOM_2017_2.EdgeAttributes(
                (null, null, null, null, Orienteering_ISOM_2017_2.VegetationAndManMade.Forest_405, null), 
                (null, null, null));

            for (int coll = 0; coll < colls; ++coll)
                lastRow.Add(new VertexBuilder( new Orienteering_ISOM_2017_2.VertexAttributes(new MapCoordinates(
                    boundaries.left + coll *  edgeLength, 
                    boundaries.top)), true, true));

            for (int row = 1; row < rows; ++row)
            {
                for (int coll = 0; coll < colls - 1; ++coll)
                {
                    VertexBuilder vertex = new VertexBuilder(new Orienteering_ISOM_2017_2.VertexAttributes(new MapCoordinates(
                        boundaries.left + edgeLength/2 + coll * edgeLength ,
                        (int) (boundaries.top - row * Math.Sqrt(3) * edgeLength / 2))), false);
                    vertex.NeighboringVertexBuilders[lastRow[coll]] = justForest;
                    lastRow[coll].NeighboringVertexBuilders[vertex] = justForest;
                    vertex.NeighboringVertexBuilders[lastRow[coll + 1]] = justForest;
                    lastRow[coll + 1].NeighboringVertexBuilders[vertex] = justForest;
                    lastRow[coll + 1].NeighboringVertexBuilders[lastRow[coll]] = justForest;
                    lastRow[coll].NeighboringVertexBuilders[lastRow[coll + 1]] = justForest;
                    currentRow.Add(vertex);
                }
                
                vertices.AddRange(lastRow);
                lastRow.Clear();
                lastRow.AddRange(currentRow);
                currentRow.Clear();
                ++row;
                
                VertexBuilder leftVertex = new VertexBuilder(new Orienteering_ISOM_2017_2.VertexAttributes(new MapCoordinates(boundaries.left, (int) (boundaries.top - row * Math.Sqrt(3) * edgeLength / 2))), true, true);
                leftVertex.NeighboringVertexBuilders[lastRow[0]] = justForest;
                currentRow.Add(leftVertex);
                for (int coll = 1; coll < colls - 1; ++coll)
                {
                    VertexBuilder vertex =  new VertexBuilder(new Orienteering_ISOM_2017_2.VertexAttributes(new MapCoordinates(
                        boundaries.left + coll * edgeLength ,
                        (int) (boundaries.top - row * Math.Sqrt(3) * edgeLength / 2))), false);
                    vertex.NeighboringVertexBuilders[lastRow[coll - 1]] = justForest;
                    lastRow[coll - 1].NeighboringVertexBuilders[vertex] = justForest;
                    vertex.NeighboringVertexBuilders[lastRow[coll]] = justForest;
                    lastRow[coll].NeighboringVertexBuilders[vertex] = justForest;
                    lastRow[coll].NeighboringVertexBuilders[lastRow[coll-1]] = justForest;
                    lastRow[coll - 1].NeighboringVertexBuilders[lastRow[coll]] = justForest;
                    currentRow.Add(vertex);
                }
                VertexBuilder rightVertex = new VertexBuilder(new Orienteering_ISOM_2017_2.VertexAttributes(new MapCoordinates(boundaries.left + (colls - 1) * edgeLength, (int) (boundaries.top - row * Math.Sqrt(3) * edgeLength / 2))), true, true);
                rightVertex.NeighboringVertexBuilders[lastRow[colls - 2]] = justForest;
                currentRow.Add(rightVertex);
                
                vertices.AddRange(lastRow);
                lastRow.Clear();
                lastRow.AddRange(currentRow);
                currentRow.Clear();
            }
            
            for (int coll = 1; coll < colls; ++coll)
            {
                lastRow[coll].IsStationary = true;
                lastRow[coll].IsBoundary = true;
                lastRow[coll].NeighboringVertexBuilders[lastRow[coll - 1]] = justForest;
                lastRow[coll - 1].NeighboringVertexBuilders[lastRow[coll]] = justForest;
            }
            vertices.AddRange(lastRow);
            return vertices;
        }

        private static void ProcessCrossablePolygonalObject(OmapMap.Object obj, IEditableRadiallySearchableDataStruct<VertexBuilder> vertexBuilders,
            CompleteSnappingMapRepreConfiguration configuration, CancellationToken? cancellationToken)
        {
            
        }
        
        private static void ProcessPathObject(OmapMap.Object obj, IEditableRadiallySearchableDataStruct<VertexBuilder> vertexBuilders,
            CompleteSnappingMapRepreConfiguration configuration, CancellationToken? cancellationToken)
        {
            
        }
        
        private static void ProcessLinearObstacleObject(OmapMap.Object obj, IEditableRadiallySearchableDataStruct<VertexBuilder> vertexBuilders,
            CompleteSnappingMapRepreConfiguration configuration, CancellationToken? cancellationToken)
        {
            
        }
        
        private static void ProcessUncrossablePolygonalObject(OmapMap.Object obj, IEditableRadiallySearchableDataStruct<VertexBuilder> vertexBuilders,
            CompleteSnappingMapRepreConfiguration configuration, CancellationToken? cancellationToken)
        {
            
        }
    }

    private class VertexBuilder(Orienteering_ISOM_2017_2.VertexAttributes attributes, bool isStationary, bool isBoundary = false)
    {
        public Orienteering_ISOM_2017_2.VertexAttributes Attributes = attributes;

        public Dictionary<VertexBuilder, Orienteering_ISOM_2017_2.EdgeAttributes> NeighboringVertexBuilders = new ();

        private BuildableVertex? _builtVertex;

        public MapCoordinates Position => Attributes.Position; 
        public bool IsStationary { get; set; } = isStationary;
        public bool IsBoundary { get; set; } = isBoundary;

        public BuildableVertex Build()
        {
            if (_builtVertex is not null) return _builtVertex;
            _builtVertex = new BuildableVertex(Attributes);
            foreach (var (builder, edgeAttributes) in NeighboringVertexBuilders)
            {
                BuildableVertex vertex = builder.Build(); 
                _builtVertex.AddEdge(new ICompleteSnappingGraph.Edge<Orienteering_ISOM_2017_2.EdgeAttributes, Orienteering_ISOM_2017_2.VertexAttributes>(edgeAttributes, vertex));
            }
            return _builtVertex;
        }
    }

    private class BuildableVertex : ICompleteSnappingGraph.Vertex<Orienteering_ISOM_2017_2.VertexAttributes, Orienteering_ISOM_2017_2.EdgeAttributes>
    {
        public BuildableVertex(Orienteering_ISOM_2017_2.VertexAttributes attributes) : base(attributes, new List<ICompleteSnappingGraph.Edge<Orienteering_ISOM_2017_2.EdgeAttributes, Orienteering_ISOM_2017_2.VertexAttributes>>()) { }
        public void AddEdge(ICompleteSnappingGraph.Edge<Orienteering_ISOM_2017_2.EdgeAttributes, Orienteering_ISOM_2017_2.VertexAttributes> edge) 
            => _outgoingWeightedEdges[edge] = null;
    }

}

