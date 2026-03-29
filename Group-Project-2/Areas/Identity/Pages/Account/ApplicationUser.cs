using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Group_Project_2.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [PersonalData]
        [MaxLength(50)]
        public string? LastName { get; set; }
    }
}