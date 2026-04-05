using AgenticDotnetConsole;
using System.Diagnostics;

public class CreateProjectTool : ITool
{
    public string Name => "create_project";

    public async Task<string> ExecuteAsync(Dictionary<string, string> args)
    {
        if (!args.TryGetValue("name", out var name))
            throw new ArgumentException("Missing 'name' argument.");

        args.TryGetValue("path", out var path);
        path ??= Directory.GetCurrentDirectory();

        var outputDir = Path.Combine(path, name);

        if (Directory.Exists(outputDir))
            return $"Project {name} already exists at {outputDir}.";

        var psi = new ProcessStartInfo(
            "dotnet",
            $"new console -n {name} --framework net10.0 --output \"{outputDir}\""
        )
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        var process = Process.Start(psi);

        var output = await process.StandardOutput.ReadToEndAsync();
        var error = await process.StandardError.ReadToEndAsync();

        return output + error;
    }

}
