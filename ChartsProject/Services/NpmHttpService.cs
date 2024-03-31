using ChartsProject.Utilities;
using System.Globalization;
using System.Text.Json;

namespace ChartsProject.Services
{
    public class NpmHttpService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public NpmHttpService(HttpClient httpClient, ILogger<NpmHttpService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        public async Task<OperationResult<dynamic>> RetrieveDownloadRecords(string package1, string package2, string package3)
        {
            try
            {
                JsonElement response1 = await _httpClient.GetFromJsonAsync<JsonElement>($"{package1}",
                    new JsonSerializerOptions(JsonSerializerDefaults.Web));

                JsonElement response2 = await _httpClient.GetFromJsonAsync<JsonElement>($"{package2}",
                    new JsonSerializerOptions(JsonSerializerDefaults.Web));

                JsonElement response3 = await _httpClient.GetFromJsonAsync<JsonElement>($"{package3}",
                    new JsonSerializerOptions(JsonSerializerDefaults.Web));

                var dataList = DataFormatter(response1, response2, response3);

                return OperationResult<dynamic>.Resolve(dataList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex.Message}");
                return OperationResult<dynamic>.Reject(ex.Message, ex);
            }
        }

        private List<dynamic> DataFormatter(JsonElement json1, JsonElement json2, JsonElement json3)
        {

            //date format
            string dateFormat = "dd-MMM-yy";
            //lists
            List<dynamic> result = new();


            for (int i = 0; i < 365; i++)
            {
                JsonElement defaultPackageName = json1.GetProperty("package");
                var defaultDownloadsContent = json1.GetProperty("downloads").EnumerateArray().ToList();

                //json1
                DateTime date = defaultDownloadsContent[i].GetProperty("day").GetDateTime();
                int downloads = defaultDownloadsContent[i].GetProperty("downloads").GetInt32();
                string parsedDate = date.ToString(dateFormat, CultureInfo.InvariantCulture);

                List<dynamic> tempList = [parsedDate, defaultPackageName, downloads];
                result.Add(tempList);

                //json2
                defaultPackageName = json2.GetProperty("package");
                defaultDownloadsContent = json2.GetProperty("downloads").EnumerateArray().ToList();
                date = defaultDownloadsContent[i].GetProperty("day").GetDateTime();
                downloads = defaultDownloadsContent[i].GetProperty("downloads").GetInt32();
                parsedDate = date.ToString(dateFormat, CultureInfo.InvariantCulture);

                List<dynamic> tempList2 = [parsedDate, defaultPackageName, downloads];
                result.Add(tempList2);

                //json3
                defaultPackageName = json3.GetProperty("package");
                defaultDownloadsContent = json3.GetProperty("downloads").EnumerateArray().ToList();
                date = defaultDownloadsContent[i].GetProperty("day").GetDateTime();
                downloads = defaultDownloadsContent[i].GetProperty("downloads").GetInt32();
                parsedDate = date.ToString(dateFormat, CultureInfo.InvariantCulture);

                List<dynamic> tempList3 = [parsedDate, defaultPackageName, downloads];
                result.Add(tempList3);
            }
            return result;

        }
    }
}
