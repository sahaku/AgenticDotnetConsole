using System;
using System.Collections.Generic;
using System.Text;

namespace AgenticDotnetConsole
{
    public class PlanStep
    {
        public string Type { get; set; } // "tool" | "code"
        public string? Tool {  get; set; } //when Type=="tool", the name of the tool to use 
        public Dictionary<string, string> Args { get; set; } = new();//tool args
        public string? Description { get; set; } //when Type=="code", a description of the code to write 

    }
}
