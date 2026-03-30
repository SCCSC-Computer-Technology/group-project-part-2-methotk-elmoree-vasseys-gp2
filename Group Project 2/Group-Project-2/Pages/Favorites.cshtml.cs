using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Group_Project_2.Pages
{
    public class FavoritesModel : PageModel
    {
        public List<string> FavoriteTeams { get; set; } = new();
        public List<string> FavoritePlayers { get; set; } = new();

        public void OnGet()
        {
            LoadFavorites();
        }

        public IActionResult OnPostRemove(string type, string value)
        {
            LoadFavorites();

            if (type == "Team")
            {
                FavoriteTeams.Remove(value);
                HttpContext.Session.SetString("FavoriteTeams", string.Join(",", FavoriteTeams));
            }
            else if (type == "Player")
            {
                FavoritePlayers.Remove(value);
                HttpContext.Session.SetString("FavoritePlayers", string.Join("|", FavoritePlayers));
            }

            return RedirectToPage();
        }

        private void LoadFavorites()
        {
            var teams = HttpContext.Session.GetString("FavoriteTeams");
            var players = HttpContext.Session.GetString("FavoritePlayers");

            FavoriteTeams.Clear();
            FavoritePlayers.Clear();

            if (!string.IsNullOrEmpty(teams))
            {
                FavoriteTeams = teams.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            if (!string.IsNullOrEmpty(players))
            {
                FavoritePlayers = players.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();
            }
        }
    }
}