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

        public async Task OnGetAsync()
        {
            if (string.IsNullOrEmpty(Key))
                return;

           
            Team = await _service.GetNFLTeamByKeyAsync(Key);
            
            Players = await _service.GetNFLPlayersByTeamAsync(Key);

            var standings = await _service.GetNFLStandingsAsync();

            Standing = standings.FirstOrDefault(s =>
                s.Team != null &&
                s.Team.Equals(Key, StringComparison.OrdinalIgnoreCase));
        }
    }
}