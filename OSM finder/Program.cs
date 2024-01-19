using System;
using System.Collections.Generic;

namespace OSM_finder;

class Programm
{
    static void Main()
    {
        /*
         * Firstly the Reader, creates an internal storage of all the necessary osm data.
         * Secondly the adjacency matrix is created.
         * Thirdly the graph structure is initialized.
         */
        
        Console.WriteLine("Provice path of osm: ");
        string location = Console.ReadLine()!;
        Console.WriteLine(location);
        Console.WriteLine("Provide location where to save path.json:");
        string save = Console.ReadLine()!;
        
        
        GtfsReader reader = new GtfsReader(@location);
        Dictionary<string, Dictionary<string, double>> vertices = reader.GetVertices();
        Graph railGraph = new OsmGraph(vertices).GetGraph();

        while (true)
        {
            Console.WriteLine("Provide start id: ");
            string start = Console.ReadLine() ?? throw new InvalidOperationException();
            if (start == "q") break; 
            Console.WriteLine("Provide destination id: ");
            string destination = Console.ReadLine() ?? throw new InvalidOperationException();
            if (destination == "q") break; 
            List<string> path = railGraph.shortest_path(start, destination);
            reader.Write(path, save);
        }
    }
}