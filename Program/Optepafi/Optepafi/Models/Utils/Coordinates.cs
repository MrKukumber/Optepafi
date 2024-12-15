using System;
using ExCSS;

namespace Optepafi.Models.Utils;

/// <summary>
/// Represents position on map relative to (hopefully geo-referenced) origin. This coordinate system is the one in which application logic communicates.
/// 
/// Values of coordinates are measured in maps micrometers. Point (1000, 0) is therefore in 1 millimeter distance from the origin on map.  
/// If maps scale is 1:10000, point (1,0) is in the real world positioned in 1 cm distance from the origin of the map. In this manner we should be able to wrap whole Globe to the map of this scale or smaller.  
/// This coordinate system is meant for maps that are not implicitly represented in geographic coordinate system.  
/// Every map representation can use its own coordinate system for describing maps. However, it should contain conversion mechanism either to <c>MapCoordinate</c> system or to <see cref="GeoCoordinates"/> system, so it could effectively communicate with other parts of application.  
/// </summary>
/// <param name="XPos">Position on horizontal axis.</param>
/// <param name="YPos">Position on vertical axis.</param>
public record struct MapCoordinates(int XPos, int YPos)
{
    public static MapCoordinates operator -(MapCoordinates coordinate1, MapCoordinates coordinate2)
    {
        return new MapCoordinates(coordinate1.XPos - coordinate2.XPos, coordinate1.YPos - coordinate2.YPos);
    }
    public static MapCoordinates operator +(MapCoordinates coordinate1, MapCoordinates coordinate2)
    {
        return new MapCoordinates(coordinate1.XPos + coordinate2.XPos, coordinate1.YPos + coordinate2.YPos);
    }
    public static MapCoordinates operator *(MapCoordinates coordinate, double scalar)
    {
        return new MapCoordinates((int)(coordinate.XPos * scalar), (int)(coordinate.YPos * scalar));
    }
    public static MapCoordinates operator *(double scalar, MapCoordinates coordinate)
    {
        return new MapCoordinates((int)(coordinate.XPos * scalar), (int)(coordinate.YPos * scalar));
    }

    public double Length() => Math.Sqrt(Math.Pow(XPos, 2) + Math.Pow(YPos, 2));

    //TODO: comment
    public MapCoordinates Rotate(double angle, MapCoordinates center)
    {
        int translatedX = XPos - center.XPos;
        int translatedY = YPos - center.YPos;
        int rotatedTransposedX = (int)(translatedX * Math.Cos(angle) - translatedY * Math.Sin(angle));
        int rotatedTransposedY = (int)(translatedX * Math.Sin(angle) + translatedY * Math.Cos(angle));
        return new MapCoordinates(rotatedTransposedX + center.XPos, rotatedTransposedY + center.YPos);
    }
    
    //TODO: comment
    public GeoCoordinates ToGeoCoordinates(GeoCoordinates referencePoint, int scale)
    {
        int earthRadInM = 6_371_000;
        
        double xInM = XPos * (scale / 1_000_000.0);
        double yInM = YPos * (scale / 1_000_000.0);

        double overflowingLatitude = double.RadiansToDegrees(yInM / earthRadInM ) + referencePoint.Latitude ;
        double overflowingLongitude = double.RadiansToDegrees(2 * Math.Asin(
                                          Math.Sin(xInM / (2 * earthRadInM)) 
                                          / Math.Cos(double.DegreesToRadians(referencePoint.Latitude)))) 
                                      + referencePoint.Longitude; 
        
        double longitude = overflowingLongitude > 180 
            ?  overflowingLongitude % 360 - 360
            : overflowingLongitude < -180 
                ? 360 - (-overflowingLongitude) % 360
                : overflowingLongitude;
        double latitude;
        if (overflowingLatitude > 90)
            if (overflowingLatitude % 360 > 270)
                latitude = overflowingLatitude - 360;
            else
            {
                latitude = 180 - overflowingLatitude;
                longitude = longitude >= 0 ? longitude - 180 : longitude + 180;
            }
        else if (overflowingLatitude < -90)
            if ((-overflowingLatitude) % 360 > 270)
                latitude = 360 - (-overflowingLatitude);
            else
            {
                latitude = - overflowingLatitude - 180;
                longitude = longitude >= 0 ? longitude - 180 : longitude + 180;
            }
        else
            latitude = overflowingLatitude;
                
        
        return new GeoCoordinates(longitude, latitude);
    }
}


/// <summary>
/// Represents coordinate of geographic coordinate system (GCS).
/// 
/// Values of this struct represent longitude and latitude values of the coordinate.  
/// This coordinate system is meant for maps that implicitly use GCS for representation of world features positions.  
/// Every map representation can use its own coordinate system for describing maps. However, it should contain conversion mechanism either to <c>GeoCoordinate</c> system or to <see cref="MapCoordinates"/> system, so it could effectively communicate with other parts of application.  
/// </summary>
/// <param name="Longitude">Longitude of coordinate.</param>
/// <param name="Latitude"> Latitude of coordinate.</param>
public record struct GeoCoordinates(double Longitude, double Latitude)
{
    
    //TODO: comment
    public MapCoordinates ToMapCoordinates(GeoCoordinates referencePoint, int scale)
    {
        int earthRadInM = 6_371_000;
        
        double distXInM = 2 * earthRadInM * Math.Asin(
            Math.Cos(double.DegreesToRadians(referencePoint.Latitude)) * 
            Math.Sin(double.DegreesToRadians(Longitude - referencePoint.Longitude)/2));
        double distYInM = (double.DegreesToRadians(Latitude - referencePoint.Latitude)) * earthRadInM;
        int distX = (int)(distXInM * (1_000_000.0 / scale));
        int distY = (int)(distYInM * (1_000_000.0 / scale));

        int x = Math.Abs(Longitude - referencePoint.Longitude) <= 180 ? distX : -distX;
        int y = distY;
        
        return new MapCoordinates(x, y);
    }    
}

        // double latitude = overflowingLatitude > 90
            // ? overflowingLatitude % 360 > 270
                // ? overflowingLatitude - 360
                // : 180 - overflowingLatitude
            // : overflowingLatitude < -90
                // ? (-overflowingLatitude) % 360 > 270
                    // ? 360 - (-overflowingLatitude)
                    // : overflowingLatitude - 180
                // : overflowingLatitude;
