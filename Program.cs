using AgenticDotnetConsole;


var ollama = new OllamaClient();

Console.WriteLine("Enter your task:");
var task = Console.ReadLine();

var prompt = $"""
You are a coding agent. Write a complete C# console program that performs this task:

Task: {task}

Rules:
- Return ONLY valid C# code.
- Must include a static void Main.
""";

var code = await ollama.GenerateAsync(prompt);

Console.WriteLine("\n=== Generated Code ===\n");
Console.WriteLine(code);

Console.WriteLine("\n=== Running Code ===\n");
code=Sanitize(code);
var output = CodeRunner.Run(code);

Console.WriteLine(output);


static string Sanitize(string code)
{
    // Remove markdown fences
    code = code
        .Replace("```csharp", "")
        .Replace("```cs", "")
        .Replace("```", "")
        .Trim();

    // Remove everything before the first line of actual C#
    int firstUsing = code.IndexOf("using ");
    int firstClass = code.IndexOf("class ");
    int start = -1;

    if (firstUsing >= 0 && firstClass >= 0)
        start = Math.Min(firstUsing, firstClass);
    else
        start = Math.Max(firstUsing, firstClass);

    if (start > 0)
        code = code.Substring(start);

    // Remove everything after the last closing brace
    int lastBrace = code.LastIndexOf('}');
    if (lastBrace > -1)
        code = code.Substring(0, lastBrace + 1);

    return code.Trim();
}



