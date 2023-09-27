using Employee_Management.Models;
using Employee_Management.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace Employee_Management.Controllers
{
    //role based authorization
    [Authorize(Roles = "Admin")]
    [AllowAnonymous]

    /*  [Authorize(Roles = "Admin")]*/ //must be member of both role=Student121 and user=User
    public class AdministrationController : Controller

    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<AdministrationController> logger;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ILogger<AdministrationController> logger)
        {

            this.roleManager = roleManager;
            this.userManager = userManager;
            this.logger = logger;

        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id={userId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRolesViewModel>();
            foreach (var role in roleManager.Roles) //get all roles
            {
                var userRolesViewModel = new UserRolesViewModel //created the instance of UserRolesViewModel class
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }
                model.Add(userRolesViewModel);
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string userId)
        {
            // ViewBag.userId = userId;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id={userId} cannot be found";
                return View("NotFound");
            }
            var roles = await userManager.GetRolesAsync(user); // Corrected variable name to "roles"
            var result = await userManager.RemoveFromRolesAsync(user, roles); // Use "roles" variable

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user from existing roles");
                return View(model);
            }

            result = await userManager.AddToRolesAsync(user, model.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            return RedirectToAction("EditUser", new { Id = userId });
        }


        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with ID={userId} cannot be found";
                return View("NotFound");

            }
            var existingUserClaims = await userManager.GetClaimsAsync(user);
            var model = new UserClaimsViewModel //model pass
            {
                UserId = userId

            };

            foreach (Claim claim in ClaimsStore.AlliClaims)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type,
                };

                if (existingUserClaims.Any(c => c.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }
                model.Claims.Add(userClaim);
            }
            return View(model);//passing the model
        }

        [HttpPost]
        //public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
        //{
        //    var user = await userManager.FindByIdAsync(model.UserId);
        //    if (user == null)
        //    {
        //        ViewBag.ErrorMessage = $"User with id={model.UserId} cannot be found";
        //        return View("NotFound");
        //    }
        //    var claims = await userManager.GetClaimsAsync(user);
        //    var result = await userManager.RemoveClaimAsync(user, claims);
        //    if (!result.Succeeded)
        //    {
        //        ModelState.AddModelError("", "cannot remove user existing claims");
        //        return View(model);
        //    }
        //    result = await userManager.AddClaimAsync(user,
        //        model.Claims.Where(c => c.IsSelected).Select(c => new Claim(c.ClaimType, c.ClaimType
        //        ));
        //    if (!result.Succeeded)
        //    {
        //        ModelState.AddModelError("", "Canno add selected claims to user");
        //        return View(model);
        //    }
        //    //Redirect to a success page or another appropriate action
        //    return RedirectToAction("EditUser", new { Id = model.UserId }); }


        ////}

     

  [HttpPost]

        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)

        {

            var user = await userManager.FindByIdAsync(model.UserId);

            if (user == null)

            {

                ViewBag.ErrorMessage = $"User with Id = {model.UserId} cannot be found";

                return View("NotFound");

            }



            var claims = await userManager.GetClaimsAsync(user);

            var result = await userManager.RemoveClaimsAsync(user, claims);



            if (!result.Succeeded)

            {

                ModelState.AddModelError("", "Cannot Remove User Existing Claim");

                return View(model);

            }



            result = await userManager.AddClaimsAsync(user, model.Claims.Select(y => new Claim(y.ClaimType, y.isSelected ? "true" : "false")));

            if (!result.Succeeded)

            {

                ModelState.AddModelError("", "Cannot add existing Claim to the user");

                return View(model);

            }



            return RedirectToAction("EditUser", new { Id = model.UserId });

        }







        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)  //not found user in database
            {
                ViewBag.ErrorMessage = $"User with Id={id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("ListUsers");
            }
        }




        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)  //not found user in database
            {
                ViewBag.ErrorMessage = $"Role User with Id={id} cannot be found";
                return View("NotFound");
            }
            else
            {

                try
                {
                    throw new Exception("Test Exception");
                    var result = await roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");

                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View("ListRoles");
                }catch(DbUpdateException ex)
                {
                    logger.LogError($"Error deleting role{ex}");
                    @ViewBag.ErrorTitle = $"{role.Name} role is in use";
                    @ViewBag.ErrorMessage = $"{role.Name} role cannot be deleted as there are users" + $" in this role.If you want to delete this role plz remove the users from" +
                              $"the role and then try to delete";
                    return View("Error");
                }
            }
        }










        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ListUsers()// to retrieve the list of all users
        {
            var users = userManager.Users;
            return View(users);
        }




        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            {
                if (ModelState.IsValid)
                {
                    IdentityRole identityRole = new IdentityRole
                    {
                        Name = model.RoleName
                    };

                    IdentityResult result = await roleManager.CreateAsync(identityRole);
                    if (result.Succeeded)
                    {
                        //we have to redirect
                        //return RedirectToAction("Index", "Home");
                        //AcceptVerbsAttribute now we have to redirect to new view that is == ListRoles.cshtml
                        return RedirectToAction("ListRoles", "Administration");
                    }
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                }
                return View(model);
            }
        }

        //to show the list of roles
        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        //[HttpGet]
        //public async Task<IActionResult> EditRole(string id)
        //{
        //    var role = await roleManager.FindByIdAsync(id);

        //    if (role == null)
        //    {
        //        ViewBag.ErrorMessage = $"Role with Id={id} canno be found";
        //        return View("NotFound");
        //    }


        //    var model = new EditRoleViewModel
        //    {
        //        Id = role.Id,
        //        RoleName = role.Name

        //    };

        //    foreach (var user in userManager.Users)
        //    {
        //        if (await userManager.IsInRoleAsync(user, role.Name))
        //        {
        //            model.Users.Add(user.UserName);
        //        }
        //        return View(model);
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id={id} cannot be found";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model); // This line should be outside the foreach loop
        }









        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id={model.Id} canno be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                //update into databse usig updateasync method
                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
            // no need of local view model instance
            // var model = new EditRoleViewModel
            // {
            //     Id = role.Id,
            //     RoleName = role.Name

            // };
            // no need
            //foreach (var user in userManager.Users)
            // {
            //     if (await userManager.IsInRoleAsync(user, role.Name))
            //     {
            //         model.Users.Add(user.UserName);
            //     }
            //return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id={roleId} canno be found";
                return View("NotFound");
            }
            var model = new List<UserRoleViewModel>();
            foreach (var user in userManager.Users)
            {
                var UserRoleViewModel = new UserRoleViewModel
                {
                    userID = user.Id,
                    userName = user.UserName
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    UserRoleViewModel.isSelected = true;
                }
                else
                {
                    UserRoleViewModel.isSelected = false;
                }
                model.Add(UserRoleViewModel);
            }
            return View(model);
        }





        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id={roleId} cannot be found";
                return View("NotFound");
            }




            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].userID);

                if (user == null)
                {
                    ViewBag.ErrorMessage = $"User with Id={model[i].userID} cannot be found";
                    return View("NotFound");
                }

                IdentityResult result = null;

                if (model[i].isSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].isSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (!result.Succeeded)
                {
                    // Handle the error, if needed.
                    // You can return an error view or log the error.
                }
            }

            return RedirectToAction("EditRole", new { Id = roleId });
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id={id} cannot be found";
                return View("NotFound");
            }
            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                City = user.City,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = (List<string>)userRoles
            };
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id={model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.City = model.City;
                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }
    }

}






