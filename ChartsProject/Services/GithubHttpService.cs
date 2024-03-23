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

        public async Task<List<SimpleChart>> GetAsyncSimpleChartList()
        {
            string? httpServicesName = _configuration["GitHubServiceClient"];
            using HttpClient httpClient = _httpClientFactory.CreateClient(httpServicesName ?? "");

            try
            {
                string owner = "vercel";
                string repo = "next.js";
                var response = await httpClient.GetFromJsonAsync<dynamic>($"/repos/{owner}/{repo}", new JsonSerializerOptions(JsonSerializerDefaults.Web));
                if (response is null)
                {
                    throw new Exception("The request failed!!");
                }
                else
                {
                    _logger.LogInformation("Response retrieved correctly");
                    Console.WriteLine(response);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("It has occurs an error: {Error}: ", ex);
            }

            return [];
        } 

        public async Task<OperationResult<int>> RetrieveStargazersCount(string owner, string repo)
        {
            string? httpClientName = _configuration["GitHubServiceClient"] ?? "";
            using HttpClient client = _httpClientFactory.CreateClient(httpClientName);

            try
            {
                int stargazersCount = await client.GetFromJsonAsync<int>($"/repos/{owner}/{repo}",
                    new JsonSerializerOptions(JsonSerializerDefaults.Web));
                return OperationResult<int>.Resolve(stargazersCount);
            }catch(Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex.Message}");
                return OperationResult<int>.Reject(ex.Message, ex);
            }
        }
    }
}
