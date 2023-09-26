using System.ComponentModel.DataAnnotations;

namespace Employee_Management.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50,ErrorMessage="Name cannot exceed more than 50 characters")]
        public string? Name { get; set; }
        [Display(Name="Office Email")]
       [ RegularExpression(@"^[A-Za-z0-9](([a-zA-Z0-9,=\.!\-#|\$%\^&\*\+/\?_`\{\}~]+)*)@(?:[0-9a-zA-Z-]+\.)+[a-zA-Z]{2,9}$",ErrorMessage="Invalid email format")]
        [Required]
        public string? Email { get; set; }
        [Required]
        public Dept? Department { get; set; }

        public string? Photopat{ get; set; }
       
    }
}
