using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using OSM_finder.Structures;

namespace OSM_finder.IO;

public static class RouteJson
{
    public static void WriteToFile(List<Node> nodes, string location)
    {
        string json = "data=" + JsonSerializer.Serialize(nodes);
        File.WriteAllText(@location, json);
    }

    public static JsonDocument GetJsonPath(List<Node> nodes)
    {
        return JsonSerializer.SerializeToDocument(nodes);
    }
}