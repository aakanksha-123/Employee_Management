//using Employee_Management.Utilities;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore.Query.Internal;
//using System.ComponentModel.DataAnnotations;

//namespace Employee_Management.ViewModels
//{
//    public class RegisterViewModel
//    {
//        [Required]
//        [EmailAddress]
//        [Remote(action: "IsEmailInUse", controller: "Account")]
//        [ValidEmailDomain(allowedDomain:"pragimtech.com",ErrorMessage ="email domain must be pragimtech.com")]
//        public string? Email { get; set; }

//        [Required]
//        [DataType(DataType.Password)]
//        public string? Password { get; set; }

//        [DataType(DataType.Password)]
//        [Display(Name = "Confirmed Password")]
//        [Compare("Password",ErrorMessage ="Password and confirmation password do not match")]
//        public string? ConfirmPassword { get;set; }

//    }
//}
using Employee_Management.Utilities; // Custom validation attribute
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Employee_Management.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailInUse", controller: "Account")] // Remote validation
        [ValidEmailDomain(allowedDomain: "pragimtech.com", ErrorMessage = "Email domain must be pragimtech.com")]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match")]
        public string? ConfirmPassword { get; set; }

        public string? City { get; set; }    
    }
}
