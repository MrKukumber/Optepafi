namespace Optepafi.Models.SearchingAlgorithmMan;

public record struct Leg(Coordinates Start, Coordinates Finish){}
public record struct Coordinates(int XPos, int YPos){} 