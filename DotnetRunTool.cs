using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AgenticDotnetConsole
{
    public class DotnetRunTool:ITool
    {
        public string Name => "dotnet_run";

        public async Task<string> ExecuteAsync(Dictionary<string, string> args)
        {
            if (!args.TryGetValue("path", out var path))
                throw new ArgumentException("Missing 'path' argument.");

            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException($"Directory not found: {path}");

            var psi = new ProcessStartInfo("dotnet", "run --no-build")
            {
                WorkingDirectory = path,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var process = Process.Start(psi);

            var output = await process.StandardOutput.ReadToEndAsync();
            var error = await process.StandardError.ReadToEndAsync();

            return output + error;
        }
    }
}

