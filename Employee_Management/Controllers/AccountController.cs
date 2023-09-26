//using Employee_Management.ViewModels;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;

//namespace Employee_Management.Controllers
//{
//    public class AccountController : Controller
//    {
//        private readonly UserManager<IdentityUser> userManager;
//        private readonly SignInManager<IdentityUser> signInManager;

//        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
//        {
//            this.userManager = userManager;
//            this.signInManager = signInManager;

//        }

//        //logout sathi httpost
//        [HttpPost]
//        public async Task<IActionResult> Logout()
//        {
//            await signInManager.SignOutAsync(); //if user login then we need to signout for this
//            return RedirectToAction("index", "home");

//        }




//        //
//        [HttpGet]
//        [AllowAnonymous]
//        public IActionResult Register()
//        {
//            return View();
//        }
//        [HttpPost]
//        [AllowAnonymous]
//        public async Task<IActionResult> Register(RegisterViewModel model) //asynchronous
//        { //chek if the incoming model is valid?
//            if (ModelState.IsValid) //checking if the RegisterViewModel model is valid or not?
//            {

//                //created new identity user object
//                var user = new IdentityUser
//                {
//                    UserName = model.Email,
//                    Email = model.Email
//                };
//                var result = await userManager.CreateAsync(user, model.Password);

//                if (result.Succeeded) //if result succeeded
//                {
//                    await signInManager.SignInAsync(user, isPersistent: false);//go to index if user created suceefully at top right we see gmail login of our name
//                    return RedirectToAction("Index", "Home");
//                }

//                foreach (var error in result.Errors)
//                {
//                    ModelState.AddModelError("", error.Description);
//                }
//            }
//            return View(model);
//        }







//        /// HttpGet and HttpPost  methods for Login

//        [HttpGet]
//        [AllowAnonymous]
//        public IActionResult Login()
//        {
//            return View();
//        }


//        //check the provided email by the user is already sed or not?

//        [AcceptVerbs("Get","Post")]
//        [AllowAnonymous]
//        public async Task<ActionResult> IsEmailInUse(string email)//jquery validate methid use to call serverside==IEmailInUse
//        {
//            //take the email type by the user on email field check if alredy any other user also use the same email
//         var user= await userManager.FindByEmailAsync(email);

//            //if user null means the email is unique
//            if(user == null)
//            {
//                return Json(true); //jquery expect the json result
//            }
//            else
//            {
//                return Json($"Email {email} is already in used");
//            }

//        }






//        [HttpPost]
//        [AllowAnonymous]
//        public async Task<IActionResult> Login(LoginviewModel model,string returnUrl)
//        { //chek if the incoming model is valid?
//            if (ModelState.IsValid)
//            {

//                var result = await signInManager.PasswordSignInAsync(model.Email,
//                                                                     model.Password,
//                                                                     model.RememberMe,
//                                                                     false);

//                if (result.Succeeded)
//                if(!string.IsNullOrEmpty(returnUrl)){
//                        return LocalRedirect(returnUrl);

//                    } else {
//                //if succeded then redirect to homecontroller index action method executed
//                return RedirectToAction("Index", "Home");
//            }
//                //no need
//                //foreach (var error in result.Errors)
//                //{
//                //    ModelState.AddModelError("", error.Description);
//                //}
//                ModelState.AddModelError(string.Empty, "Invalid Login attempt");
//            }
//            return View(model);
//        }

//    }
//}


using Employee_Management.Models;
using Employee_Management.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Employee_Management.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    City = model.City
                };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if(signInManager.IsSignedIn(User) && User.IsInRole("Admin")){ 
                        return RedirectToAction("ListUsers", "Administration");

                }
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginviewModel model, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    model.RememberMe,
                    false
                );

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        
    }
}

