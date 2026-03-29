using Group_Project_2.ApiLibrary.Models;
using Group_Project_2.ApiLibrary.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Group_Project_2.Pages
{
    public class NBATeamsModel : PageModel
    {
        private readonly SportsApiService _service;

        public List<NBATeam> Teams { get; set; } = new();

        public NBATeamsModel(SportsApiService service)
        {
            _service = service;
        }

        public async Task OnGetAsync()
        {
            Teams = await _service.GetNBATeamsAsync();
        }
    }
}