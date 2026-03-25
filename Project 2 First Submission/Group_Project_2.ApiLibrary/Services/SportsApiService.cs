using Group_Project_2.ApiLibrary.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Numerics;
using System.Linq;

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

        public async Task<NFLTeam?> GetNFLTeamByKeyAsync(string key)
        {
            var teams = await GetNFLTeamsAsync();
            return teams.FirstOrDefault(t => t.Key == key);
        }

        public async Task<List<NFLPlayer>> GetNFLPlayersByTeamAsync(string key)
        {
            string url = "https://api.sportsdata.io/v3/nfl/scores/json/Players";
            string json = await GetJsonAsync(url);

            var players = JsonConvert.DeserializeObject<List<NFLPlayer>>(json)
                          ?? new List<NFLPlayer>();

            return players
                .Where(p => p.Team != null && p.Team.Equals(key, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public async Task<NBATeam?> GetNBATeamByKeyAsync(string key)
        {
            var teams = await GetNBATeamsAsync();

            return teams.FirstOrDefault(t =>
                t.Key != null &&
                t.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<List<NBAPlayer>> GetNBAPlayersByTeamAsync(string key)
        {
            string url = "https://api.sportsdata.io/v3/nba/scores/json/Players";
            string json = await GetJsonAsync(url);

            var players = JsonConvert.DeserializeObject<List<NBAPlayer>>(json)
                          ?? new List<NBAPlayer>();

            return players
                .Where(p => p.Team != null &&
                            p.Team.Equals(key, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}