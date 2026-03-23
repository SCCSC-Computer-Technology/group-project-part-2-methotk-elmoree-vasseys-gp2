using Group_Project_2.ApiLibrary.Models;
using Newtonsoft.Json;
using System.Net.Http;

namespace Group_Project_2.ApiLibrary.Services
{
    public class SportsApiService
    {
        private readonly HttpClient _client;

        private const string ApiKey = "f6d43bd8239c4fe7a38abcac1c0cb30c";
        private const string FixedYear = "2025";
        private const string FixedNFLSeason = "2025REG";

        public SportsApiService(HttpClient client)
        {
            _client = client;
        }

        private void SetApiKeyHeader()
        {
            const string headerName = "Ocp-Apim-Subscription-Key";

            if (_client.DefaultRequestHeaders.Contains(headerName))
                _client.DefaultRequestHeaders.Remove(headerName);

            _client.DefaultRequestHeaders.Add(headerName, ApiKey);
        }

        private async Task<string> GetJsonAsync(string url)
        {
            SetApiKeyHeader();

            var response = await _client.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"API ERROR: {response.StatusCode}");

            return body;
        }

        // ================= NFL =================

        public async Task<List<NFLTeam>> GetNFLTeamsAsync()
        {
            string url = "https://api.sportsdata.io/v3/nfl/scores/json/Teams";
            string json = await GetJsonAsync(url);
            return JsonConvert.DeserializeObject<List<NFLTeam>>(json) ?? new List<NFLTeam>();
        }

        public async Task<List<NFLStanding>> GetNFLStandingsAsync()
        {
            string url = $"https://api.sportsdata.io/v3/nfl/scores/json/Standings/{FixedNFLSeason}";
            string json = await GetJsonAsync(url);
            return JsonConvert.DeserializeObject<List<NFLStanding>>(json) ?? new List<NFLStanding>();
        }

        // ================= NBA =================

        public async Task<List<NBATeam>> GetNBATeamsAsync()
        {
            string url = "https://api.sportsdata.io/v3/nba/scores/json/Teams";
            string json = await GetJsonAsync(url);
            return JsonConvert.DeserializeObject<List<NBATeam>>(json) ?? new List<NBATeam>();
        }

        public async Task<List<NBAStanding>> GetNBAStandingsAsync()
        {
            string url = $"https://api.sportsdata.io/v3/nba/scores/json/Standings/{FixedYear}";
            string json = await GetJsonAsync(url);
            return JsonConvert.DeserializeObject<List<NBAStanding>>(json) ?? new List<NBAStanding>();
        }
    }
}