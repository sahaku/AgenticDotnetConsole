using System;
using System.Collections.Generic;
using System.Text;

namespace AgenticDotnetConsole
{
    public class ProgramSanitizer
    {
       public static string Sanitize(string code)
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
    }
}
