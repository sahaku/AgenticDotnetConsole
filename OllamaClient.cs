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
    }

    public class OllamaResponse
    {
        public string response { get; set; }
    }
}
