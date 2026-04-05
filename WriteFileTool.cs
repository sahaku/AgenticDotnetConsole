using System;
using System.Collections.Generic;
using System.Text;

namespace AgenticDotnetConsole
{
    internal class WriteFileTool : ITool
    {
        public string Name => "write_file";

        public async Task<string> ExecuteAsync(Dictionary<string, string> args)
        {
            if (!args.TryGetValue("path", out var path))
                throw new ArgumentException("Missing 'path' argument for write_file.");

            if (!args.TryGetValue("content", out var content))
            {
                throw new ArgumentException("Missing 'content' argument for write_file.");
            }

            if (!File.Exists(path))
                throw new FileNotFoundException($"File not found: {path}");

            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            await File.WriteAllTextAsync(path, content);
            return $"Wrote file:{path}";
        }
    }
}
