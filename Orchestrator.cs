using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace AgenticDotnetConsole
{
    public class Orchestrator
    {
        private readonly OllamaClient _ollama;
        private readonly ToolRegistry _tools;

        public Orchestrator(OllamaClient ollama, ToolRegistry tools)
        {
            _ollama = ollama;
            _tools = tools;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("Enter your task:");
            var task = Console.ReadLine();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Console.WriteLine("\n=== Planner Agent ===\n");
            var planJson = await _ollama.PlanAsync(task);
            var json= PlannerSanitizer.Sanitize(planJson);
            var steps = JsonSerializer.Deserialize<List<PlanStep>>(json, options) ?? new List<PlanStep>();
            var contextBuilder = new StringBuilder();

            foreach (var step in steps)
            {
                if (step.Type == "tool" && step.Tool != null)

                {
                    var tool = _tools.Get(step.Tool);
                    if (tool == null)
                    {
                        Console.WriteLine($"[Planner requested unknown Tool {step.Tool}.");
                        continue;
                    }
                    Console.WriteLine($"[Tool:{step.Tool}");
                    var result = await tool.ExecuteAsync(step.Args ?? new Dictionary<string, string>());
                    Console.WriteLine($"[Tool Result]: {result}");
                    contextBuilder.AppendLine($"[Tool: {step.Tool}]");
                    contextBuilder.AppendLine(result);
                    contextBuilder.AppendLine();
                }
                else if (step.Type == "code" && !string.IsNullOrWhiteSpace(step.Description))
                {
                    Console.WriteLine($"[Code Step]: {step.Description}");
                    var coderPrompt = $"""
                        You are a coding agent.

                        Context from previous tool calls:
                        {contextBuilder}

                        Implement this step as a complete C# console program:

                        {step.Description}

                        Rules:
                        - Return ONLY valid C# code.
                        - Must include a static void Main.
                        - Must create new methods as needed.
                        """;
                    var _rawCode = await _ollama.GenerateAsync(coderPrompt);
                    Console.WriteLine("\n=== Initial Generated Code ===\n");
                    Console.WriteLine(_rawCode);
                    Console.WriteLine("\n=== Reviewer Agent Fixing Code ===\n");
                    var reviewerCode = await _ollama.ReviewAsync(_rawCode);
                    Console.WriteLine("\n=== Running Code ===\n");
                    var sanitizedCode = ProgramSanitizer.Sanitize(reviewerCode);
                    var output = CodeRunner.Run(sanitizedCode);
                    contextBuilder.AppendLine("[Code Output]");
                    contextBuilder.AppendLine(output);
                    contextBuilder.AppendLine();

                }
            }
            
        }
    }
}
