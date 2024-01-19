namespace OSM_finder.Structures;

public class Node
{
    public string Id { get; }
    public double Lat { get; }
    public double Lon { get; }
    public int NumberOfWays = 0;
    public double Vmax { get; set; } = 100.0;

    public Node(string id, double lat, double lon)
    {
        this.Id = id;
        this.Lat = lat;
        this.Lon = lon;
    }

    public override string ToString()
    {
        return Id;
    }
}