using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using static System.Net.WebRequestMethods;


namespace AgenticDotnetConsole
{
    public class OllamaClient
    {
        private readonly HttpClient _http;
        public OllamaClient()
        {
            _http = new HttpClient { BaseAddress = new Uri("http://localhost:11434") };
        }

        public async Task<string> GenerateAsync(string prompt)
        {
            var request = new
            {
                model = "llama3.1",
                prompt = prompt,
                stream = false
            };

            var response = await _http.PostAsJsonAsync("/api/generate", request);
            var json = await response.Content.ReadFromJsonAsync<OllamaResponse>();

            return json?.response ?? "";
        }

        public async Task<string> ReviewAsync(string code)
        {
           var prompt = $"""
            You are a senior C# code reviewer.

            Your job:
            - Fix syntax errors
            - Ensure the code compiles
            - Ensure it contains: static void Main(string[] args)
            - Ensure all required namespaces are included
            - Remove unsafe or disallowed operations
            - Return ONLY valid C# code. No explanations.

            Here is the code to review:

            {code}
            """;

            var request = new
            {
                model = "llama3.1",
                prompt = prompt,
                stream = false
            };

            var response = await _http.PostAsJsonAsync("/api/generate", request);
            var json = await response.Content.ReadFromJsonAsync<OllamaResponse>();

            return json?.response ?? "";
        }

        public async Task<string> PlanAsync(string task)
        {
            var appPath = AppContext.BaseDirectory;
            var schemaPath = Path.Combine(appPath, "planner.schema.json");
            var schema =System.IO.File.ReadAllText(schemaPath);

            var prompt = $$"""
                You are a senior software planner for a C# console application.

                Available tools:
                - read_file
                - run_code
                - write_file
                - search_repo
                - create_project
                - dotnet_build
                - dotnet_run

                OUTPUT RULES (MANDATORY):
                - Return ONLY a JSON array. No text before or after.
                - Do NOT restate, rewrite, summarize, or mention the JSON schema.
                - Do NOT explain your reasoning.
                - Do NOT describe the plan.
                - Do NOT include conversational text.
                - Do NOT wrap the array in any object.
                - Do NOT add any fields other than: type, tool, args, description.
                - All field names and values MUST be lowercase exactly as in the schema.
                - TOOL steps MUST have an empty description.
                - CODE steps MUST describe WHAT to compute, not HOW to implement it.
                - Do NOT describe what a tool does.

                CRITICAL WORKFLOW RULES:
                1. If a TOOL step is used for an operation, do NOT create a CODE step for that same operation.

                2. For pure numeric computation tasks (NOT C# code generation):
                   - Use ONLY a single CODE step.
                   - Do NOT call read_file, write_file, or run_code unless the user explicitly provides a file path or explicitly asks to execute code.
                   - Do NOT invent file paths.

                3. A CODE step MUST describe the entire computation.
                4. Do NOT add extra steps unless the task explicitly requires repository search.

                CRITICAL WORKFLOW RULES FOR .NET PROJECTS:
                1. If the task involves creating or modifying C# code, ALWAYS ensure a project exists.

                2. If the project does not exist, the FIRST step MUST be a create_project tool call.

                3. When choosing a project name:
                   - Infer a short lowercase identifier from the task.
                   - Remove spaces and punctuation.
                   - Append "app" if needed.
                   - Examples: "calculatorapp", "todoapp", "fileprocessorapp".

                4. After creating the project, write new code files using write_file.

                5. Only call read_file when modifying an existing file.

                6. Never call read_file before write_file unless the file already exists.

                7. After writing or modifying files, ALWAYS call dotnet_build.

                8. After building, call dotnet_run.

                9. Never invent file paths. Use the pattern: <projectname>/<filename>.cs

                10. All C# source code MUST be generated inside write_file steps, not inside CODE steps.

                11. CODE steps are ONLY for pure computation tasks, NOT for generating C# source code.

                Follow this JSON schema exactly:
                {{schema}}

                Task:
                {{task}}

                Return ONLY the JSON array. No commentary.
                """;

            var request = new
            {
                model = "llama3.1",
                prompt = prompt,
                stream = false
            };

            var response = await _http.PostAsJsonAsync("/api/generate", request);

            var json = await response.Content.ReadFromJsonAsync<OllamaResponse>();

            return json?.response ?? "";
        }


    }

    public class OllamaResponse
    {
        public string response { get; set; }
    }
}
