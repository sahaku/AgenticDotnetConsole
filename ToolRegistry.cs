using System;
using System.Collections.Generic;
using System.Text;

namespace AgenticDotnetConsole
{
    public class ToolRegistry
    {
        private readonly Dictionary<string, ITool> _tools;
        public ToolRegistry(IEnumerable<ITool> tools)
        {
            _tools = tools.ToDictionary(t => t.Name, StringComparer.OrdinalIgnoreCase);
        }

        public ITool? Get(string name) => _tools.TryGetValue(name, out var tool) ? tool : null;
    }
}
