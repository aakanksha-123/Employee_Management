using System.ComponentModel.DataAnnotations;

namespace Employee_Management.ViewModels
{
    public class CreateRoleViewModel
    {

        [Required]
        public string? RoleName { get; set; }
    }
}
