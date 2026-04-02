using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Microsoft.CodeAnalysis;

namespace AgenticDotnetConsole
{
    public class CodeRunner
    {
        public static string Run(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);

            // ✅ ADD THIS HERE — replaces your old refs array
            var assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);

            var refs = Directory.GetFiles(assemblyPath, "*.dll")
                .Select(dll => MetadataReference.CreateFromFile(dll))
                .ToList();

            // Continue as before
            var compilation = CSharpCompilation.Create(
                "GeneratedProgram",
                new[] { tree },
                refs,
                new CSharpCompilationOptions(OutputKind.ConsoleApplication)
            );

            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                return string.Join("\n", result.Diagnostics);
            }

            ms.Seek(0, SeekOrigin.Begin);
            var assembly = Assembly.Load(ms.ToArray());

            using var sw = new StringWriter();
            Console.SetOut(sw);

            assembly.EntryPoint.Invoke(null, new object[] { new string[0] });

            return sw.ToString();
        }

    }
}
