using Group_Project_2.ApiLibrary.Models;
using Group_Project_2.ApiLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Group_Project_2.Pages
{
    public class NFLTeamDetailsModel : PageModel
    {
        private readonly SportsApiService _service;
        public NFLTeam? Team { get; set; }
        public List<NFLPlayer> Players { get; set; } = new();
        public NFLStanding? Standing { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Key { get; set; }

        public NFLTeamDetailsModel(SportsApiService service) => _service = service;

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrWhiteSpace(Key)) return RedirectToPage("./NFLTeams");

            Team = await _service.GetNFLTeamByKeyAsync(Key.Trim());
            if (Team == null) return Page();

            Players = await _service.GetNFLPlayersByTeamAsync(Key.Trim());
            var standings = await _service.GetNFLStandingsAsync();
            if (standings != null)
            {
                Standing = standings.FirstOrDefault(s => s.Team.Equals(Key, StringComparison.OrdinalIgnoreCase));
            }
            return Page();
        }

        public IActionResult OnPostFavoriteTeam(string teamKey, string fullTeamName)
        {
            var record = teamKey;
            var existing = HttpContext.Session.GetString("FavoriteTeams") ?? "";
            var list = existing.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();

            if (!list.Contains(record))
            {
                list.Add(record);
                HttpContext.Session.SetString("FavoriteTeams", string.Join("|", list));
                TempData["UserMessage"] = $"{fullTeamName} added to favorites!";
            }
            return RedirectToPage(new { Key = teamKey });
        }

        public IActionResult OnPostFavoritePlayer(string playerName, string teamKey)
        {
            var record = $"{playerName.Replace(" ", ",")},{teamKey}";
            var existing = HttpContext.Session.GetString("FavoritePlayers") ?? "";
            var list = existing.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();

            if (!list.Contains(record))
            {
                list.Add(record);
                HttpContext.Session.SetString("FavoritePlayers", string.Join("|", list));
                TempData["UserMessage"] = $"{playerName} added to favorites!";
            }
            return RedirectToPage(new { Key = teamKey });
        }
    }
}