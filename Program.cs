using AgenticDotnetConsole;

var ollama = new OllamaClient();

var tools = new ToolRegistry(new ITool[]
{
    new RunCodeTool(),
    new ReadFileTool(),
    new WriteFileTool(),
    new SearchRepoTool(root: "."),

    // NEW TOOLS
    new CreateProjectTool(),
    new DotnetBuildTool(),
    new DotnetRunTool()
});

var orchestrator = new Orchestrator(ollama, tools);
await orchestrator.RunAsync();
