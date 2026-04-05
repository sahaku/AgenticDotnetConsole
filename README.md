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