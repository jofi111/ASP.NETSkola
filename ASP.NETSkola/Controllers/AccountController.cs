using ASP.NETSkola.Models;
using ASP.NETSkola.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NETSkola.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            LoginVM login = new LoginVM();
            login.ReturnUrl = returnUrl;
            return View(login);

        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login (LoginVM login)
        {
            if (ModelState.IsValid)
            {
                var appUser = await userManager.FindByNameAsync(login.Username);
                if (appUser != null)
                {
                    var signInResult = await signInManager.PasswordSignInAsync(appUser, login.Password, login.Remember, false);
                    if (signInResult.Succeeded)
                    {
                        return Redirect(login.ReturnUrl ?? "/");
                    }
                }
                ModelState.AddModelError("", "Login failed: Invalid username or password");
            }
            return View(login);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}
