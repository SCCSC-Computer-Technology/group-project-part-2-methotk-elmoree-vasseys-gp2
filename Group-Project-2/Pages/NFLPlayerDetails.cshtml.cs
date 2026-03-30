using Group_Project_2.ApiLibrary.Models;
using Group_Project_2.ApiLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Group_Project_2.Pages
{
    public class NFLPlayerDetailsModel : PageModel
    {
        private readonly SportsApiService _service;

        public List<NFLPlayerSeasonStat> Stats { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? FirstName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? LastName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Position { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Number { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Height { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Weight { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? TeamKey { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? TeamName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? TeamCity { get; set; }

        public string FullName => $"{FirstName ?? ""} {LastName ?? ""}".Trim();

        public NFLPlayerDetailsModel(SportsApiService service) => _service = service;

        public async Task OnGetAsync()
        {
            if (!string.IsNullOrEmpty(TeamKey) && !string.IsNullOrEmpty(Number))
            {
                Stats = await _service.GetNFLPlayerStatsAsync(TeamKey, Number);

                if (Stats != null && Stats.Any())
                {
                    var firstStat = Stats.First();
                    Position ??= firstStat.Position;
                }
            }
        }
    }
}