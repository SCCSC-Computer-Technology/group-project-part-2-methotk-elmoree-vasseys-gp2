using Group_Project_2.ApiLibrary.Models;
using Group_Project_2.ApiLibrary.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Group_Project_2.Pages
{
    public class NFLTeamsModel : PageModel
    {
        private readonly SportsApiService _service;

        public List<NFLTeam> Teams { get; set; } = new();

        public NFLTeamsModel(SportsApiService service)
        {
            _service = service;
        }

        public async Task OnGetAsync()
        {
            Teams = await _service.GetNFLTeamsAsync();
        }
    }
}