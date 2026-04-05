using System;
using System.Collections.Generic;
using System.Text;

namespace AgenticDotnetConsole
{
    public interface ITool
    {
        string Name { get; }
        Task<string> ExecuteAsync(Dictionary<string, string> args);
    }
}
