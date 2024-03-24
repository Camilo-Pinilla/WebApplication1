using ChartsProject.Models;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using ChartsProject.Utilities;

namespace ChartsProject.Services
{
    public sealed class GithubHttpService
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public GithubHttpService(
            IHttpClientFactory httpClientFactory,
            ILogger<GithubHttpService> logger,
            IConfiguration configuration)
        {

            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<OperationResult<string>> RetrieveStargazersCount(string owner, string repo)
        {
            string? httpClientName = _configuration["GitHubServiceClient"] ?? "";
            using HttpClient client = _httpClientFactory.CreateClient(httpClientName);

            try
            {
                JsonElement response = await client.GetFromJsonAsync<JsonElement>($"/repos/{owner}/{repo}",
                    new JsonSerializerOptions(JsonSerializerDefaults.Web));
                JsonElement stargazersCount = response.GetProperty("stargazers_count");
  
                return OperationResult<string>.Resolve(stargazersCount.ToString());
            }catch(Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex.Message}");
                return OperationResult<string>.Reject(ex.Message, ex);
            }
        }
    }
}
