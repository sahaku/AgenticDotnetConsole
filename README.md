# AgenticDotnetConsole
A tiny AI‑powered coding agent that turns natural language into real, runnable .NET code — all inside a console app.
# 📘 Agentic .NET Console
An agentic development environment powered by Ollama, a structured planner, an orchestrator, and a suite of tools that allow natural‑language instructions to generate, modify, build, and run .NET applications autonomously.

This project demonstrates how LLM‑driven tool‑calling can be used to create real, executable C# applications from plain English instructions.
# 🚀 Features
Natural‑language → C# project generation

Automatic project creation (dotnet new console)

Automatic file creation & modification

Automatic build & run (dotnet build, dotnet run)

Pure computation tasks handled separately

Deterministic planner rules for safe, predictable execution

Workspace‑based project isolation

Extensible tool registry
# 🧩 Architecture Overview
The system follows a clear pipeline:

1. User Input
You type a natural‑language instruction such as:
2. Planner
  The planner converts the instruction into a JSON plan using strict rules:
  Create project if missing
  Write files
  Build 
  Run
  Avoid unnecessary read/write
  Avoid mixing computation with code generation
3. Orchestrator
  Executes each step in order:
  Calls the correct tool
  Passes arguments
  Streams output back to the user
4. Tools
Each tool performs a real action:
