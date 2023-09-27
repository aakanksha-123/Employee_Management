using System.Security.Claims;

namespace Employee_Management.ViewModels
{
    public class UserClaim
    {
        internal bool isSelected;

        public string ClaimType { get; set; }
        public bool IsSelected { get; set; }
        public ClaimsIdentity? ClaimValue { get;  set; }
    }
}
