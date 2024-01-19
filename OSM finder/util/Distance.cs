using System;
using System.Xml;
using OSM_finder.Structures;

namespace OSM_finder.util;

public static class Distance
{
    private static double CalculateDistance(double sLatitude, double sLongitude, double eLatitude, double eLongitude)
    {
        double radiansOverDegrees = (Math.PI / 180.0);
        double sLatitudeRadians = sLatitude * radiansOverDegrees;
        double sLongitudeRadians = sLongitude * radiansOverDegrees;
        double eLatitudeRadians = eLatitude * radiansOverDegrees;
        double eLongitudeRadians = eLongitude * radiansOverDegrees;

        double dLongitude = eLongitudeRadians - sLongitudeRadians;
        double dLatitude = eLatitudeRadians - sLatitudeRadians;

        double result1 = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) + Math.Cos(sLatitudeRadians) * Math.Cos(eLatitudeRadians) * Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);

        // Using 6371 as the radius of the earth in kilometers
        double result2 = 6371.0 * 2.0 * Math.Atan2(Math.Sqrt(result1), Math.Sqrt(1.0 - result1));

        return result2;
    }

    public static double CalculateDistance(Node node1, Node node2)
    {
        return CalculateDistance(node1.Lat, node1.Lon, node2.Lat, node2.Lon);
    }

    public static double CalculateDistance(XmlNode node1, XmlNode node2)
    {
        return CalculateDistance(double.Parse(node1.Attributes!["lat"]!.Value),
            double.Parse(node1.Attributes["lon"]!.Value), double.Parse(node2.Attributes!["lat"]!.Value),
            double.Parse(node2.Attributes["lon"]!.Value));
    }
}