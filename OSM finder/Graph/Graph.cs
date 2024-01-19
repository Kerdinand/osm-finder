using System;
using System.Collections.Generic;

namespace OSM_finder;

public class Graph
{
    // Adjacency Matrix
    private readonly Dictionary<string, Dictionary<string, double>> _vertices = new Dictionary<string, Dictionary<string, double>>();
    private List<string> _path = null!;
    public void add_vertex(string name, Dictionary<string, double> edges)
    {
        _vertices[name] = edges;
    }

    public List<string> shortest_path(string start, string finish)
    {

        if (!_vertices.ContainsKey(start) || !_vertices.ContainsKey(finish)) throw new ArgumentException();
        
        var previous = new Dictionary<string, string>();
        var distances = new Dictionary<string, double>();
        var nodes = new PriorityQueue<string, double>();

        _path = null!;

        foreach (var vertex in _vertices)
        {
            if (vertex.Key == start)
            {
                distances[vertex.Key] = 0;
            }
            else
            {
                distances[vertex.Key] = double.MaxValue;
            }
        }
        nodes.Enqueue(start, 0);
        while (nodes.Count != 0)
        {
            var smallest = nodes.Dequeue();
            if (smallest == finish)
            {
                _path = new List<string>();
                while (previous.ContainsKey(smallest))
                {
                    _path.Add(smallest);
                    smallest = previous[smallest];
                }
                break;
            }
            if (Math.Abs(distances[smallest] - double.MaxValue) < 0.000000000000001) break;
            foreach (var neighbor in _vertices[smallest])
            {
                var alt = distances[smallest] + neighbor.Value;
                if (alt < distances[neighbor.Key])
                {
                    distances[neighbor.Key] = alt;
                    previous[neighbor.Key] = smallest;
                    nodes.Enqueue(neighbor.Key, alt);
                }
            }
        }
        return _path;
    }

    public List<string> GetPath()
    {
        return _path;
    }

}