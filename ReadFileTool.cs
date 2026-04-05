using System;
using System.Collections.Generic;
using System.Text;

namespace AgenticDotnetConsole
{
    public class ReadFileTool : ITool
    {
        public string Name => "read_file";

        public async Task<string> ExecuteAsync(Dictionary<string, string> args)
        {
            if (!args.TryGetValue("path", out var path))
                throw new ArgumentException("Missing 'path' argument for write_file.");



            if (!File.Exists(path))
                throw new FileNotFoundException($"File not found: {path}");

            return await File.ReadAllTextAsync(path);

        }

    }
}
