namespace OSM_finder;

public class OsmGraph
{
    private readonly Graph _graph = new Graph();

    public OsmGraph(Dictionary<string, Dictionary<string, double>> vertices)
    {
        foreach (var variable in vertices)
        {
            _graph.add_vertex(variable.Key, variable.Value);
        }
        Console.WriteLine("Graph created!");
    }

    public Graph GetGraph()
    {
        return _graph;
    }
}