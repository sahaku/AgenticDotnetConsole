using System;
using System.Collections.Generic;
using System.Text;

namespace AgenticDotnetConsole
{
    using System.Text.Json;
    using System.Text.Json.Nodes;

    public static class PlannerSanitizer
    {
        public static string Sanitize(string json)
        {
            JsonNode? root;

            try
            {
                root = JsonNode.Parse(json);
            }
            catch
            {
                // If the model returned garbage, wrap it in an empty array
                return "[]";
            }

            if (root is not JsonArray arr)
                return "[]";

            foreach (var item in arr)
            {
                if (item is not JsonObject obj)
                    continue;

                // Normalize "type"
                if (!obj.TryGetPropertyValue("type", out var typeNode))
                    obj["type"] = "code"; // fallback

                var type = obj["type"]?.ToString()?.ToLowerInvariant();

                // Normalize "tool"
                if (type == "tool")
                {
                    if (!obj.ContainsKey("tool"))
                        obj["tool"] = "run_code"; // default tool

                    // Remove invalid tools
                    var tool = obj["tool"]?.ToString();
                    var allowed = new[] { "search_repo", "read_file", "write_file", "run_code" };
                    if (!allowed.Contains(tool))
                        obj["tool"] = "run_code";
                }
                else
                {
                    obj["tool"] = null;
                }

                // Normalize "args"
                if (!obj.ContainsKey("args") || obj["args"] is not JsonObject)
                {
                    obj["args"] = new JsonObject();
                }
                else
                {
                    // Convert all values to strings
                    var argsObj = (JsonObject)obj["args"]!;
                    var pairs = argsObj.ToList(); // KeyValuePair<string, JsonNode?>

                    foreach (var pair in pairs)
                    {
                        var key = pair.Key;
                        var val = pair.Value;

                        if (val is JsonArray arrVal)
                        {
                            argsObj[key] = string.Join(",", arrVal.Select(v => v?.ToString() ?? ""));
                        }
                        else
                        {
                            argsObj[key] = val?.ToString() ?? "";
                        }
                    }

                }

                // Normalize "description"
                if (!obj.ContainsKey("description"))
                    obj["description"] = "";
            }

            return arr.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
        }
    }

}
