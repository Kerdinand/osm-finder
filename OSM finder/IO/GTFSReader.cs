using System.Globalization;
using System.Xml;
using OSM_finder.IO;
using OSM_finder.Structures;
using OSM_finder.util;

namespace OSM_finder;

public class GtfsReader
{
    private readonly Dictionary<String, Node> _nodes = new Dictionary<String, Node>();
    private readonly Dictionary<string, Dictionary<string, double>> _vertices = new Dictionary<string, Dictionary<string, double>>();
    public GtfsReader(String location)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(location);
        foreach (XmlElement element in doc.DocumentElement!.ChildNodes)
        {
            if (element.Name == "node")
            {
                _nodes.Add(element.Attributes["id"]!.Value,
                    new Node(element.Attributes["id"]!.Value,
                        double.Parse(element.Attributes["lat"]!.Value, CultureInfo.InvariantCulture.NumberFormat),
                        double.Parse(element.Attributes["lon"]!.Value, CultureInfo.InvariantCulture.NumberFormat)));
                _vertices[element.Attributes["id"]!.Value] = new Dictionary<string, double>();
            }
        }
        Console.WriteLine("Finished with nodes");
        foreach (XmlElement element in doc.DocumentElement.ChildNodes)
        {
            if (element.Name == "way")
            {
                int speed = 100;
                for (int i = 0; i < element.ChildNodes.Count; i++)
                {
                    if (element.ChildNodes[i]!.Name == "tag" &&
                        element.ChildNodes[i]!.Attributes!["k"]!.Value == "maxspeed")
                    {
                        int.TryParse(element.ChildNodes[i]!.Attributes!["v"]!.Value,out speed);
                    }
                }
                
                for (int i = 0; i < element.ChildNodes.Count; i++)
                {
                    if (element.ChildNodes[i]!.Name != "nd") break;
                    _nodes[element.ChildNodes[i]!.Attributes!["ref"]!.Value].Vmax = speed;
                    _nodes[element.ChildNodes[i]!.Attributes!["ref"]!.Value].NumberOfWays++;
                }
            }
        }
        foreach (XmlElement element in doc.DocumentElement.ChildNodes)
        {
            if (element.Name == "way")
            {
                for (int i = 1; i < element.ChildNodes.Count; i++)
                {
                    if (element.ChildNodes[i]!.Name != "nd") break;
                    string start = element.ChildNodes[i - 1]!.Attributes!["ref"]!.Value;
                    string end = element.ChildNodes[i]!.Attributes!["ref"]!.Value;
                    double weight = Distance.CalculateDistance(_nodes[start], _nodes[end]) / Math.Max(_nodes[start].Vmax, _nodes[end].Vmax);
                    _vertices[start][end] = weight;
                    _vertices[end][start] = weight;
                }
            }
        }
        Console.WriteLine("Done with the edges, Ready to parse into Graph");

    }

    public Dictionary<string, Dictionary<string, double>> GetVertices()
    {
        return _vertices;
    }

    public void Write(List<string> path, string location)
    {
        List<Node> result = new List<Node>();
        foreach (var variable in path)
        {
            result.Add(_nodes[variable]);
        }

        RouteJson.WriteToFile(result, location);
    }
}