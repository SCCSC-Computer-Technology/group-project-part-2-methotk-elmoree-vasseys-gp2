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

        public NFLTeamDetailsModel(SportsApiService service)
        {
            _service = service;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrWhiteSpace(Key))
            {
                return RedirectToPage("./NFLTeams");
            }

            var cleanKey = Key.Trim();

            // Get team
            Team = await _service.GetNFLTeamByKeyAsync(cleanKey);

            if (Team == null)
            {
                return NotFound();
            }

            // Get players
            Players = await _service.GetNFLPlayersByTeamAsync(cleanKey);

            // Get standings
            var standings = await _service.GetNFLStandingsAsync();

            if (standings != null)
            {
                Standing = standings.FirstOrDefault(s =>
                    !string.IsNullOrEmpty(s.Team) &&
                    s.Team.Trim().Equals(cleanKey, StringComparison.OrdinalIgnoreCase));
            }

            return Page();
        }
    }
}