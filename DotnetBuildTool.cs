using System;
using System.Collections.Generic;
using System.Text;
using AgenticDotnetConsole;
using System.Diagnostics;


namespace AgenticDotnetConsole
{
   
    public class DotnetBuildTool : ITool
    {
        public string Name => "dotnet_build";

        public async Task<string> ExecuteAsync(Dictionary<string, string> args)
        {
            if (!args.TryGetValue("path", out var path))
                throw new ArgumentException("Missing 'path' argument.");

            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException($"Directory not found: {path}");

            var psi = new ProcessStartInfo("dotnet", "build")
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
