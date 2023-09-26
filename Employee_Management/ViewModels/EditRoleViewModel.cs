using System.ComponentModel.DataAnnotations;

namespace Employee_Management.ViewModels
{
    public class EditRoleViewModel
    {

        //initialize Users list otherwise it will throw erorr
        public EditRoleViewModel()
        {
            Users = new List<string>(); //initialized collection list here
        }
        public string Id { get;set; }

        [Required(ErrorMessage="Role name is required")]
        public string  RoleName { get;set; }

        public List<string> Users { get; set; } //coollection property need to be initialized
    }
}
