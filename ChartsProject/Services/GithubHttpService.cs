using ChartsProject.Models;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using ChartsProject.Utilities;

namespace ChartsProject.Services
{
    public sealed class GithubHttpService : IDisposable
    {

        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public GithubHttpService(
            HttpClient httpClient,
            ILogger<GithubHttpService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        public async Task<OperationResult<string>> RetrieveStargazersCount(string owner, string repo)
        {

            try
            {
                JsonElement response = await _httpClient.GetFromJsonAsync<JsonElement>($"repos/{owner}/{repo}",
                    new JsonSerializerOptions(JsonSerializerDefaults.Web));
                JsonElement stargazersCount = response.GetProperty("stargazers_count");
                _logger.LogTrace($"The base address is: {_httpClient.BaseAddress}");
                return OperationResult<string>.Resolve(stargazersCount.ToString());
            }catch(Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex.Message}");
                return OperationResult<string>.Reject(ex.Message, ex);
            }
        }
    }
}
