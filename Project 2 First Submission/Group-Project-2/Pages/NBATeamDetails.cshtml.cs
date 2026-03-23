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
            if (string.IsNullOrWhiteSpace(Key))
            {
                return RedirectToPage("./NBATeams");
            }

            Team = await _service.GetNBATeamByKeyAsync(Key);

            if (Team == null)
            {
                return NotFound();
            }

            Players = await _service.GetNBAPlayersByTeamAsync(Key);

            var standings = await _service.GetNBAStandingsAsync();

            if (standings != null)
            {
                Standing = standings.FirstOrDefault(s =>
                    !string.IsNullOrEmpty(s.Key) &&
                    s.Key.Trim().Equals(Key.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            return Page();
        }
    }
}