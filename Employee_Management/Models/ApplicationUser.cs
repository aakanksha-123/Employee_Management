using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee_Management.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string? City { get; set; }


        //public object? Users { get; set; }
        [NotMapped]
        public ICollection<User> Users { get; set; }
    }
}
