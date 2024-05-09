namespace Optepafi.Models.SearchingAlgorithmMan;

public record struct Leg(MapCoordinate Start, MapCoordinate Finish){}
public record struct MapCoordinate(int XPos, int YPos){} 
