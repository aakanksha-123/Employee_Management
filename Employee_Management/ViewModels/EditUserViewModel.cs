using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Employee_Management.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel() //INITIALIZE TO AVOID NULL EXCEPTION REFERANCE ERROR
        {
            Claims=new List<string>();
            Roles = new List<string>();
        }

      public string Id { get; set; }

        [Required]
        [EmailAddress] 
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        public string City { get; set; }

        public List<string> Roles { get; set;}


        public IList<string> Claims { get; set;}
    }
}
