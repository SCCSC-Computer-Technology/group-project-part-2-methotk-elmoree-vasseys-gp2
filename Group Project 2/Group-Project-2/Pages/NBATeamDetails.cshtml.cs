using Group_Project_2.ApiLibrary.Models;
using Group_Project_2.ApiLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Group_Project_2.Pages
{
    public class NBATeamDetailsModel : PageModel
    {
        private readonly SportsApiService _service;

        public NBATeam? Team { get; set; }

        public List<NBAPlayer> Players { get; set; } = new();

        public NBAStanding? Standing { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Key { get; set; }

        public NBATeamDetailsModel(SportsApiService service)
        {
            _service = service;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // If no team key -> redirect back to team list
            if (string.IsNullOrWhiteSpace(Key))
            {
                return RedirectToPage("./NBATeams");
            }

            // Clean the key
            var cleanKey = Key.Trim();

            // Get team info using key
            Team = await _service.GetNBATeamByKeyAsync(cleanKey);

            // If team not found -> show 404 page
            if (Team == null)
            {
                return NotFound();
            }

            // Get all players on this team
            Players = await _service.GetNBAPlayersByTeamAsync(cleanKey);

            var standings = await _service.GetNBAStandingsAsync();

            if (standings != null)
            {
                Standing = standings.FirstOrDefault(s =>
                    !string.IsNullOrEmpty(s.Key) &&
                    s.Key.Trim().Equals(cleanKey, StringComparison.OrdinalIgnoreCase));
            }

            return Page();
        }
    }
}