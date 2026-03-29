using Group_Project_2.ApiLibrary.Models;
using Group_Project_2.ApiLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Group_Project_2.Pages
{
    public class NBAPlayerDetailsModel : PageModel
    {
        private readonly SportsApiService _service;

        public List<NBAPlayerSeasonStat> Stats { get; set; } = new();

 
        [BindProperty(SupportsGet = true)]
        public int PlayerId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? FirstName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? LastName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Position { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Height { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Weight { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Number { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? TeamKey { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? TeamName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? TeamCity { get; set; }

        // Combines first + last name for display
        public string FullName => $"{FirstName ?? ""} {LastName ?? ""}".Trim();

        // Constructor
        public NBAPlayerDetailsModel(SportsApiService service)
        {
            _service = service;
        }

        // Runs when page loads
        public async Task OnGetAsync()
        {
            // Always reset stats first
            Stats = new List<NBAPlayerSeasonStat>();

            // use PlayerId (fast + accurate)
            if (PlayerId > 0)
            {
                Stats = await _service.GetNBAPlayerSeasonStatsAsync(PlayerId);
                return;
            }

            if (!string.IsNullOrWhiteSpace(FirstName) || !string.IsNullOrWhiteSpace(LastName))
            {
                Stats = await _service.GetNBAPlayerStatsAsync(
                    FirstName ?? "",
                    LastName ?? ""
                );
            }
        }
    }
}