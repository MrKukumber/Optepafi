using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using Optepafi.Models.MapMan.MapInterfaces;
using Optepafi.Models.MapMan.Maps;
using Optepafi.Models.SearchingAlgorithmMan;
using Optepafi.Models.Utils;

namespace Optepafi.Models.MapMan.MapRepresentatives;

//empty coords list is not valid solution, there is always at least one coordinate tuple in objects shape/position 
//TODO: comment
public sealed class OmapMapRepresentative : IMapRepresentative<OmapMap>
{
    public static OmapMapRepresentative Instance { get; } = new OmapMapRepresentative();
    
    public string Extension { get; } = "omap";
    public string MapFormatName { get; } = "OMAP";
    public OmapMap? CreateMapFrom((Stream mapStream,string path) input, CancellationToken? cancellationToken, out MapManager.MapCreationResult creationResult)
    {
        var result = OmapMapParser.Parse(input, cancellationToken, out OmapMap? map);
        switch (result)
        {
            case OmapMapParser.ParseResult.Ok: creationResult = MapManager.MapCreationResult.Ok; break;
            case OmapMapParser.ParseResult.Cancelled: creationResult = MapManager.MapCreationResult.Cancelled; break;
            case OmapMapParser.ParseResult.Incomplete: creationResult = MapManager.MapCreationResult.Incomplete; break;
            default: creationResult = MapManager.MapCreationResult.UnableToParse; break;
        }
        return map;
    }

    public List<Leg>? GetDefaultTrackFrom(OmapMap map)
    {
        if (map.Objects.TryGetValue(799, out var simpleOrienteeringCourses) && simpleOrienteeringCourses.Count > 0)
        {
            int i = 0;
            List<Leg> track = new();
            while (i + 1 < simpleOrienteeringCourses[0].TypedCoords.Length)
            {
                track.Add(new Leg(simpleOrienteeringCourses[0].TypedCoords[i].Item1, 
                    simpleOrienteeringCourses[0].TypedCoords[++i].Item1));
            }
            return track;
        }
        return null;
    }
}

public static class OmapMapParser
{
    private static int _cancellationCheckInterval = 10000;
    public enum ParseResult {Ok, Cancelled, Incomplete, UnsupportedVersion, XmlError, OmapError}
    public static ParseResult Parse((Stream mapStream,string path) input, CancellationToken? cancellationToken, out OmapMap? map)
    {
        map = null;
        using (XmlReader reader = XmlReader.Create(input.mapStream))
        { 
            int scale;
            GeoCoordinates geoLocation;
            bool isGeoLocated;
            Dictionary<int, OmapMap.Symbol> symbols;
            Dictionary<decimal, List<OmapMap.Object>> objects;
            (MapCoordinates tc, MapCoordinates bc, MapCoordinates lc, MapCoordinates rc) extremeCoords;

            int readsSinceLastCancelCheck = 0;
            
            var res = TryGetVersion(reader, cancellationToken, ref readsSinceLastCancelCheck, out int version);
            if (res is not ParseResult.Ok) return res;
            if (version != 9) return ParseResult.UnsupportedVersion;

            res = TryGetScale(reader, cancellationToken, ref readsSinceLastCancelCheck, out scale);
            if (res is not ParseResult.Ok) return res;

            res = TryGetGeoLocation(reader, cancellationToken, ref readsSinceLastCancelCheck, out isGeoLocated, out geoLocation);
            if (res is not ParseResult.Ok) return res;

            res = TryGetSymbols(reader, cancellationToken, ref readsSinceLastCancelCheck, out symbols);
            if (res is not ParseResult.Ok) return res;
            
            res = TryGetAllObjects(reader, symbols, cancellationToken, ref readsSinceLastCancelCheck, out objects, out extremeCoords);
            if (res is not ParseResult.Ok && res is not ParseResult.Incomplete) return res; 
            
            List<OmapMap.Symbol> mapSymbols = symbols.Select(kv => kv.Value).ToList();
            
            map = isGeoLocated 
                ? new InnerGeoLocatedOmapMap(scale, mapSymbols, objects, geoLocation, (new(extremeCoords.lc.XPos, extremeCoords.bc.YPos), new(extremeCoords.rc.XPos, extremeCoords.tc.YPos))) 
                    { FilePath = input.path, FileName = Path.GetFileName(input.path) } 
                : map = new InnerOmapMap(scale, mapSymbols, objects, (new(extremeCoords.lc.XPos, extremeCoords.bc.YPos), new(extremeCoords.rc.XPos, extremeCoords.tc.YPos))) 
                    { FilePath = input.path, FileName = Path.GetFileName(input.path) };
            return res;
        }
    }
    
    private static ParseResult TryGetVersion(XmlReader reader, CancellationToken? cancellationToken, ref int readsSinceLastCancelCheck, out int version)
        {
            version = 0;
            try
            {
                while (reader.Read())
                {
                    if (IsCancellationRequested(cancellationToken, ref readsSinceLastCancelCheck)) return ParseResult.Cancelled;
                    if (reader.NodeType is XmlNodeType.Element &&
                        reader.Name == "map" &&
                        int.TryParse(reader.GetAttribute("version"), out version)) 
                        return ParseResult.Ok;
                }
                return ParseResult.OmapError;
            }
            catch (XmlException)
            {
                return ParseResult.XmlError;
            }
        }
    
    private static ParseResult TryGetScale(XmlReader reader, CancellationToken? cancellationToken, ref int readsSinceLastCancelCheck, out int scale)
    {
        scale = 0;
        try
        {
            while (reader.Read())
            {
                if (IsCancellationRequested(cancellationToken, ref readsSinceLastCancelCheck)) return ParseResult.Cancelled;
                if (reader.NodeType is XmlNodeType.Element && reader.Name == "georeferencing" &&
                    int.TryParse(reader.GetAttribute("scale"), out scale)) 
                    return ParseResult.Ok;
            }
            return ParseResult.OmapError;
        }
        catch (XmlException)
        {
            return ParseResult.XmlError;
        }
    }

    private static ParseResult TryGetGeoLocation(XmlReader reader, CancellationToken? cancellationToken, ref int readsSinceLastCancelCheck, out bool isGeoLocated, out GeoCoordinates location)
    {
        location = new GeoCoordinates();
        isGeoLocated = false;
        try
        {
            while (reader.Read())
            {
                if (IsCancellationRequested(cancellationToken, ref readsSinceLastCancelCheck)) return ParseResult.Cancelled;
                if (reader.NodeType is XmlNodeType.Element && reader.Name == "projected_crs")
                {
                    if (reader.GetAttribute("id") != "Local") break;
                    return ParseResult.Ok;
                }
                if (reader.NodeType is XmlNodeType.EndElement && reader.Name == "georeferencing") return ParseResult.OmapError;
            }

            while (reader.Read())
            {
                if (IsCancellationRequested(cancellationToken, ref readsSinceLastCancelCheck)) return ParseResult.Cancelled;
                if (reader.NodeType is XmlNodeType.Element && reader.Name == "ref_point_deg")
                {
                    if (float.TryParse(reader.GetAttribute("lat"), new CultureInfo("en-US"),out float lat) &&
                        float.TryParse(reader.GetAttribute("lon"), new CultureInfo("en-US"), out float lon))
                    {
                        location = new GeoCoordinates(lon, lat);
                        isGeoLocated = true;
                        return ParseResult.Ok;
                    }
                    return ParseResult.OmapError;
                }
                if (reader.NodeType is XmlNodeType.EndElement && reader.Name == "georeferencing") return ParseResult.OmapError;
            }
            return ParseResult.OmapError;
        }
        catch (XmlException) { return ParseResult.XmlError; }
    }

    private static ParseResult TryGetSymbols(XmlReader reader, CancellationToken? cancellationToken, ref int readsSinceLastCancelCheck, out Dictionary<int, OmapMap.Symbol> symbols)
    {
        symbols = new();
        try
        {
            while (reader.Read())
            {
                if (IsCancellationRequested(cancellationToken, ref readsSinceLastCancelCheck)) return ParseResult.Cancelled;
                if (reader.NodeType is XmlNodeType.Element && reader.Name == "symbol")
                {
                    if (int.TryParse(reader.GetAttribute("id"), out int id) &&
                        decimal.TryParse(reader.GetAttribute("code"), new CultureInfo("en-US"),out decimal code))
                        symbols[id] = new OmapMap.Symbol(code);
                }
                if (reader.NodeType is XmlNodeType.EndElement && reader.Name == "symbols") return ParseResult.Ok;
            }
            return ParseResult.OmapError;
        }
        catch (XmlException)
        {
            return ParseResult.XmlError;
        }
    }

    private static ParseResult TryGetAllObjects(XmlReader reader, 
        Dictionary<int, OmapMap.Symbol> symbols, CancellationToken? cancellationToken, ref int readsSinceLastCancelCheck, out Dictionary<decimal, List<OmapMap.Object>> objects, 
         out (MapCoordinates tc, MapCoordinates bc, MapCoordinates lc, MapCoordinates rc) extremeCoords)
    {
        objects = new();
        foreach (var symbol in symbols) if (!objects.ContainsKey(symbol.Value.Code)) objects[symbol.Value.Code] = new List<OmapMap.Object>();
        extremeCoords = (new MapCoordinates(0, int.MinValue), new MapCoordinates(0, int.MaxValue), new MapCoordinates(int.MaxValue,0), new MapCoordinates(int.MinValue, 0));
        try
        {
            while (reader.Read())
            {
                if (IsCancellationRequested(cancellationToken, ref readsSinceLastCancelCheck)) return ParseResult.Cancelled;
                if (reader.NodeType is XmlNodeType.Element && reader.Name == "part" &&
                    reader.GetAttribute("name") == "default part")
                    break;
                if (reader.NodeType is XmlNodeType.EndElement && reader.Name == "parts")
                    return ParseResult.OmapError;
            }

            bool complete = true;
            while (reader.Read())
            {
                if (IsCancellationRequested(cancellationToken, ref readsSinceLastCancelCheck)) return ParseResult.Cancelled;
                
                if (reader.NodeType is XmlNodeType.Element && reader.Name == "object")
                {
                    if (int.TryParse(reader.GetAttribute("symbol"), out int id) && symbols.ContainsKey(id))
                    {
                        bool firstgetRotationFound = float.TryParse(reader.GetAttribute("rotation"), CultureInfo.InvariantCulture, out float rotation);
                        switch (TryGetCoords(reader, cancellationToken, ref readsSinceLastCancelCheck, out var typedCoords, out (MapCoordinates tc, MapCoordinates bc, MapCoordinates lc, MapCoordinates rc) objectsExtremCoords))
                        {
                            case ParseResult.Ok:
                                if (!firstgetRotationFound)
                                {
                                    var secondGetRotationResult = TryGetRotation2(reader, cancellationToken, ref readsSinceLastCancelCheck, out rotation);
                                    if (secondGetRotationResult is not ParseResult.Ok) return secondGetRotationResult;
                                }
                                objects[symbols[id].Code].Add(new OmapMap.Object(typedCoords, rotation));
                                
                                extremeCoords.tc = objectsExtremCoords.tc.YPos > extremeCoords.tc.YPos ? objectsExtremCoords.tc : extremeCoords.tc;
                                extremeCoords.bc = objectsExtremCoords.bc.YPos < extremeCoords.bc.YPos ? objectsExtremCoords.bc : extremeCoords.bc;
                                extremeCoords.lc = objectsExtremCoords.lc.XPos < extremeCoords.lc.XPos ? objectsExtremCoords.lc : extremeCoords.lc;
                                extremeCoords.rc = objectsExtremCoords.rc.XPos > extremeCoords.rc.XPos ? objectsExtremCoords.rc : extremeCoords.rc;
                                break;
                            case ParseResult.OmapError:
                                complete = false;
                                break;
                            case ParseResult.Cancelled:
                                return ParseResult.Cancelled;
                        }
                    }
                    else complete = false;
                }
                if (reader.NodeType is XmlNodeType.EndElement && reader.Name == "part")
                    return complete ? ParseResult.Ok : ParseResult.Incomplete;
            }
            return ParseResult.OmapError;
        }
        catch (XmlException) { return ParseResult.XmlError; }
    }
    
    private static ParseResult TryGetCoords(XmlReader reader, CancellationToken? cancellationToken, ref int readsSinceLastCancelCheck, 
        out (MapCoordinates, byte)[] typedCoords, out (MapCoordinates tc, MapCoordinates bc, MapCoordinates lc, MapCoordinates rc) extremeCoords)
    {
        typedCoords = [];
        extremeCoords = new();
        while (reader.Read())
        {
            if (IsCancellationRequested(cancellationToken, ref readsSinceLastCancelCheck)) return ParseResult.Cancelled;

            if (reader.NodeType is XmlNodeType.Element && reader.Name == "coords")
            {
                if (reader.Read() && reader.NodeType is XmlNodeType.Text)
                {
                    if (IsCancellationRequested(cancellationToken, ref readsSinceLastCancelCheck)) return ParseResult.Cancelled;
                    if (TryParseCoords(reader.Value, cancellationToken, ref readsSinceLastCancelCheck, out typedCoords, out extremeCoords))
                        return ParseResult.Ok;
                    if (cancellationToken is not null && cancellationToken.Value.IsCancellationRequested) return ParseResult.Cancelled;
                }
                return ParseResult.OmapError;
            }

            if (reader.NodeType is XmlNodeType.EndElement && reader.Name == "coords")
            {
                return ParseResult.OmapError;
            }
        }
        return ParseResult.OmapError;
    }

    private static ParseResult TryGetRotation2(XmlReader reader, CancellationToken? cancellationToken, ref int readsSinceLastCancelCheck, out float rotation)
    {
        rotation = 0;
        while (reader.Read())
        {
            if (IsCancellationRequested(cancellationToken, ref readsSinceLastCancelCheck)) return ParseResult.Cancelled;
            if (reader.NodeType is XmlNodeType.Element && reader.Name == "pattern" &&
                float.TryParse(reader.GetAttribute("rotation"), CultureInfo.InvariantCulture,out rotation))
                return ParseResult.Ok;
            if (reader.NodeType is XmlNodeType.EndElement && reader.Name == "object")
                return ParseResult.Ok;
        }
        return ParseResult.OmapError;
    }

    private static bool TryParseCoords(string strAllCoords, CancellationToken? cancellationToken, ref int readsOrParsesSinceLastCheck,
        out (MapCoordinates, byte)[] typedCoords, out (MapCoordinates tc, MapCoordinates bc, MapCoordinates lc, MapCoordinates rc) extremeCoords)
    {
        typedCoords = [];
        extremeCoords = (new MapCoordinates(0, int.MinValue), new MapCoordinates(0, int.MaxValue), new MapCoordinates(int.MaxValue,0), new MapCoordinates(int.MinValue, 0));
        
        List<(MapCoordinates, byte)> coordsList = [];
        int i = 0;
        StringBuilder strBuilder = new();
        while(i < strAllCoords.Length)
        {
            string strCoord = GetWordTill(strAllCoords, ';', ref i, strBuilder);
            
            if (IsCancellationRequested(cancellationToken, ref readsOrParsesSinceLastCheck)) return false;
            int x; int y; byte type;
            
            int j = 0;
            while (!int.TryParse(GetWordTill(strCoord, ' ', ref j, strBuilder), out x))
                if (j >= strCoord.Length || !IsCancellationRequested(cancellationToken, ref readsOrParsesSinceLastCheck)) return false;
            while (!int.TryParse(GetWordTill(strCoord, ' ', ref j, strBuilder), out y))
                if (j >= strCoord.Length || !IsCancellationRequested(cancellationToken, ref readsOrParsesSinceLastCheck)) return false;
            while (!byte.TryParse(GetWordTill(strCoord, ' ', ref j, strBuilder), out type) && j < strCoord.Length && !IsCancellationRequested(cancellationToken, ref readsOrParsesSinceLastCheck)){}

            if (x == -14731 && y == 8283)
                Console.WriteLine();
            y = -y; //y-axis values are saved in omap files other way than we use them in map coordinates
            if (y > extremeCoords.tc.YPos) extremeCoords.tc = new MapCoordinates(x, y);
            if (y < extremeCoords.bc.YPos) extremeCoords.bc = new MapCoordinates(x, y);
            if (x < extremeCoords.lc.XPos) extremeCoords.lc = new MapCoordinates(x, y);
            if (x > extremeCoords.rc.XPos) extremeCoords.rc = new MapCoordinates(x, y);
            coordsList.Add((new MapCoordinates(x,y), type));
        }
        if (coordsList.Count == 0) return false; //empty coords list is not valid
        typedCoords = coordsList.ToArray();
        return true;
    }

    private static string GetWordTill(string str, char delimeter, ref int i, StringBuilder builder)
    {
        builder.Clear();
        while (i < str.Length && str[i] != delimeter)
        {
            builder.Append(str[i++]);
        }
        if (i < str.Length) i++;
        return builder.ToString();
    }

    private static bool IsCancellationRequested( CancellationToken? cancellationToken, ref int repeatsSinceLastCancelCheck)
    {
        if (++repeatsSinceLastCancelCheck % _cancellationCheckInterval == _cancellationCheckInterval - 1 &&
            cancellationToken is not null && cancellationToken.Value.IsCancellationRequested)
            return true;
        return false;
    }

    private class InnerOmapMap(int scale, List<OmapMap.Symbol> symbols, Dictionary<decimal, List<OmapMap.Object>> objects, (MapCoordinates blc, MapCoordinates trc) boundingRectCorners) : OmapMap
    {
        public override int Scale => scale;

        public override List<Symbol> Symbols => symbols;
        public override Dictionary<decimal, List<Object>> Objects => objects;
        
        public override MapCoordinates BottomLeftBoundingCorner { get; } = boundingRectCorners.blc;
        public override MapCoordinates TopRightBoundingCorner { get; } = boundingRectCorners.trc;

        public override IMap GetPartitionOfSize(int size, CancellationToken? cancellationToken, out bool wholeMapReturned)
        {
            var (newSymbols, newObjects) = GetNewSymbolsAndObjects(size, cancellationToken, symbols, objects, out wholeMapReturned);
            return new InnerOmapMap(Scale, newSymbols, newObjects, (BottomLeftBoundingCorner, TopRightBoundingCorner))
            { FilePath = FilePath, FileName = FileName };
        }
    }

    private class InnerGeoLocatedOmapMap(int scale,   List<OmapMap.Symbol> symbols, Dictionary<decimal, List<OmapMap.Object>> objects, 
        GeoCoordinates representativeLocation, (MapCoordinates blc, MapCoordinates trc) boundingRectCorners) : GeoLocatedOmapMap
    {
        public override GeoCoordinates RepresentativeLocation => representativeLocation;
        public override MapCoordinates BottomLeftBoundingCorner { get; } = boundingRectCorners.blc;
        public override MapCoordinates TopRightBoundingCorner { get; } = boundingRectCorners.trc;

        public override int Scale => scale;
        public override List<Symbol> Symbols => symbols;
        public override Dictionary<decimal, List<Object>> Objects => objects;
        
        public override IMap GetPartitionOfSize(int size, CancellationToken? cancellationToken, out bool wholeMapReturned)
        {
            var (newSymbols, newObjects) = GetNewSymbolsAndObjects(size, cancellationToken, symbols, objects, out wholeMapReturned);
            return new InnerGeoLocatedOmapMap(Scale, newSymbols, newObjects, representativeLocation, (BottomLeftBoundingCorner, TopRightBoundingCorner))
            { FilePath = FilePath, FileName = FileName };
        }
    }

    private static (List<OmapMap.Symbol>, Dictionary<decimal, List<OmapMap.Object>>) GetNewSymbolsAndObjects(int size, CancellationToken? cancellationToken, List<OmapMap.Symbol> symbols, Dictionary<decimal, List<OmapMap.Object>> objects, out bool wholeMapReturned)
    {
        List<OmapMap.Symbol> newSymbols = new ();
        foreach (var symbol in symbols) newSymbols.Add(symbol);
        
        Dictionary<decimal, List<OmapMap.Object>> newObjects = new ();
        foreach (var symbol in newSymbols) if (!newObjects.ContainsKey(symbol.Code)) newObjects[symbol.Code] = new List<OmapMap.Object>();
        
        if (0 >= symbols.Count)
        {
            wholeMapReturned = true;
            return (newSymbols, newObjects);
        }

        int sinceLastCancelCheck = 0;
        int currentSymbolIndex = 0;
        int currentObjectIndex = 0;
        while (--size >= -1)
        {
            if (sinceLastCancelCheck >= _cancellationCheckInterval)
            {
                if (cancellationToken is not null && !cancellationToken.Value.IsCancellationRequested) { wholeMapReturned = false; return (newSymbols, newObjects);}
                sinceLastCancelCheck = 0;
            }

            if (currentObjectIndex >= objects[symbols[currentSymbolIndex].Code].Count)
            {
                currentObjectIndex = 0;
                if (++currentSymbolIndex == symbols.Count) { wholeMapReturned = true; return (newSymbols, newObjects); }
                while (objects[symbols[currentSymbolIndex].Code].Count == 0)
                    if (++currentSymbolIndex == symbols.Count) { wholeMapReturned = true; return (newSymbols, newObjects); }
            }
            if (size >= 0) newObjects[symbols[currentSymbolIndex].Code].Add(objects[symbols[currentSymbolIndex].Code][currentObjectIndex++]);
        }
        wholeMapReturned = false;
        return (newSymbols, newObjects);
    }
}