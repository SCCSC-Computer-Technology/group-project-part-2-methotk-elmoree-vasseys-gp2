using Group_Project_2.ApiLibrary.Models;
using Newtonsoft.Json;

namespace Group_Project_2.ApiLibrary.Services
{
    public class SportsApiService
    {
        private readonly HttpClient _client;

        private const string ApiKey = "8c7b4074a3d2420d953d9e872b124d19";
        private const string FixedYear = "2025";
        private const string FixedNFLSeason = "2025REG";

        public SportsApiService(HttpClient client)
        {
            _client = client;
        }

        private void SetApiKeyHeader()
        {
            const string headerName = "Ocp-Apim-Subscription-Key";

            if (!_client.DefaultRequestHeaders.Contains(headerName))
            {
                _client.DefaultRequestHeaders.Add(headerName, ApiKey);
            }
        }

        private async Task<string> GetJsonAsync(string url)
        {
            SetApiKeyHeader();

            var response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API ERROR: {response.StatusCode}");
            }

            return await response.Content.ReadAsStringAsync();
        }

        private async Task<List<T>> GetListAsync<T>(string url)
        {
            try
            {
                string json = await GetJsonAsync(url);
                return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LIST ERROR ({typeof(T).Name}): {ex.Message}");
                return new List<T>();
            }
        }

        private async Task<T?> GetObjectAsync<T>(string url) where T : class
        {
            try
            {
                string json = await GetJsonAsync(url);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OBJECT ERROR ({typeof(T).Name}): {ex.Message}");
                return null;
            }
        }

        // ================= NFL =================

        public async Task<List<NFLTeam>> GetNFLTeamsAsync()
        {
            return await GetListAsync<NFLTeam>(
                "https://api.sportsdata.io/v3/nfl/scores/json/Teams");
        }

        public async Task<List<NFLStanding>> GetNFLStandingsAsync()
        {
            return await GetListAsync<NFLStanding>(
                $"https://api.sportsdata.io/v3/nfl/scores/json/Standings/{FixedNFLSeason}");
        }

        public async Task<NFLTeam?> GetNFLTeamByKeyAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return null;

            var teams = await GetNFLTeamsAsync();

            string incomingKey = key.Trim();

            return teams.FirstOrDefault(t =>
                !string.IsNullOrWhiteSpace(t.Key) &&
                t.Key.Trim().Equals(incomingKey, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<List<NFLPlayer>> GetNFLPlayersByTeamAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return new List<NFLPlayer>();

            var players = await GetListAsync<NFLPlayer>(
                "https://api.sportsdata.io/v3/nfl/scores/json/Players");

            string incomingKey = key.Trim();

            return players
                .Where(p =>
                    !string.IsNullOrWhiteSpace(p.Team) &&
                    p.Team.Trim().Equals(incomingKey, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // ================= NFL PLAYER STATS =================

        public async Task<List<NFLPlayerSeasonStat>> GetNFLAllPlayerSeasonStatsAsync()
        {
            return await GetListAsync<NFLPlayerSeasonStat>(
                $"https://api.sportsdata.io/v3/nfl/stats/json/PlayerSeasonStats/{FixedNFLSeason}");
        }

        public async Task<List<NFLPlayerSeasonStat>> GetNFLPlayerStatsAsync(string teamKey, string number)
        {
            if (string.IsNullOrWhiteSpace(teamKey) || string.IsNullOrWhiteSpace(number))
                return new List<NFLPlayerSeasonStat>();

            var allStats = await GetNFLAllPlayerSeasonStatsAsync();

            string incomingKey = teamKey.Trim();
            string incomingNumber = number.Trim();

            return allStats
                .Where(s =>
                    !string.IsNullOrWhiteSpace(s.Team) &&
                    s.Team.Trim().Equals(incomingKey, StringComparison.OrdinalIgnoreCase) &&
                    s.Number.ToString().Trim().Equals(incomingNumber, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // ================= NBA =================

        public async Task<List<NBATeam>> GetNBATeamsAsync()
        {
            return await GetListAsync<NBATeam>(
                "https://api.sportsdata.io/v3/nba/scores/json/Teams");
        }

        public async Task<List<NBAStanding>> GetNBAStandingsAsync()
        {
            return await GetListAsync<NBAStanding>(
                $"https://api.sportsdata.io/v3/nba/scores/json/Standings/{FixedYear}");
        }

        public async Task<NBATeam?> GetNBATeamByKeyAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return null;

            var teams = await GetNBATeamsAsync();

            string incomingKey = key.Trim();

            return teams.FirstOrDefault(t =>
                !string.IsNullOrWhiteSpace(t.Key) &&
                t.Key.Trim().Equals(incomingKey, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<List<NBAPlayer>> GetNBAPlayersByTeamAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return new List<NBAPlayer>();

            var players = await GetListAsync<NBAPlayer>(
                "https://api.sportsdata.io/v3/nba/scores/json/Players");

            string incomingKey = key.Trim();

            return players
                .Where(p =>
                    !string.IsNullOrWhiteSpace(p.Team) &&
                    p.Team.Trim().Equals(incomingKey, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public async Task<List<NBAPlayerSeasonStat>> GetNBAPlayerSeasonStatsAsync(int playerId)
        {
            if (playerId <= 0)
                return new List<NBAPlayerSeasonStat>();

            var stat = await GetObjectAsync<NBAPlayerSeasonStat>(
                $"https://api.sportsdata.io/v3/nba/stats/json/PlayerSeasonStatsByPlayer/{FixedYear}/{playerId}");

            if (stat == null)
                return new List<NBAPlayerSeasonStat>();

            return new List<NBAPlayerSeasonStat> { stat };
        }

        public async Task<List<NBAPlayerSeasonStat>> GetNBAPlayerStatsAsync(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName))
                return new List<NBAPlayerSeasonStat>();

            string fullName = $"{firstName} {lastName}".Trim();

            var players = await GetListAsync<NBAPlayer>(
                "https://api.sportsdata.io/v3/nba/scores/json/Players");

            var player = players.FirstOrDefault(p =>
                $"{p.FirstName} {p.LastName}".Trim()
                    .Equals(fullName, StringComparison.OrdinalIgnoreCase));

            if (player == null)
                return new List<NBAPlayerSeasonStat>();

            return await GetNBAPlayerSeasonStatsAsync(player.PlayerID);
        }
    }
}
