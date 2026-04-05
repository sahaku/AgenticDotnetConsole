using System;
using System.Collections.Generic;
using System.Text;

namespace AgenticDotnetConsole
{
    public class RunCodeTool : ITool
    {
        public string Name => "run_code";
        public async Task<string> ExecuteAsync(Dictionary<string, string> args)
        {
            if (!args.TryGetValue("code", out var code))
                throw new ArgumentException("Missing 'code' argument for run_code.");
            var sanitized = ProgramSanitizer.Sanitize(code); // reuse your Sanitize var sanitizedCode = ProgramSan
            var output = CodeRunner.Run(sanitized);
            return await Task.FromResult(output);
        }
    }
}
