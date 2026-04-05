using System;
using System.Collections.Generic;
using System.Text;

namespace AgenticDotnetConsole
{
    public class SearchRepoTool:ITool
    {
        public string Name => "search_repo";
        private readonly string _root;

        public SearchRepoTool(string root)
        {
            _root = root;
        }

        public async Task<string> ExecuteAsync(Dictionary<string, string> args)
        {
            if(!args.TryGetValue("query", out var query))
                throw new ArgumentException("Missing 'query' argument for search_repo.");

            var files=Directory.GetFiles(_root, "*.cs", SearchOption.AllDirectories);
            var matches = files.Where(f => File.ReadAllText(f).Contains(query, StringComparison.OrdinalIgnoreCase)).Take(10);
            var result = string.Join("\n", matches);
            return await Task.FromResult(result);
        }
    }
}
