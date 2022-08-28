using WebApp.Ovserver.Models;
using WebApp.Ovserver.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Diagnostics;
using WebApp.Observer.DTO;
using System.Linq;
using WebApp.Observer.Observer;

namespace WebApp.Ovserver.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserObserverSubject _userObserverSubject;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, UserObserverSubject userObserverSubject)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userObserverSubject = userObserverSubject;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe)
        {
            var hasUser = await _userManager.FindByEmailAsync(email);
            if(hasUser == null)
            {
                return View();
            }
            var signInResult = await _signInManager.PasswordSignInAsync(hasUser,password,rememberMe,false); //3. parametre cookie'de kaydedilsin mi ?
            if (!signInResult.Succeeded)
            {
                return View();
            }
            return RedirectToAction(nameof(HomeController.Index),"Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(UserCreateDTO user)
        {
            var appUser = new AppUser() { UserName = user.Username, Email = user.Email };
            var identityResult = await _userManager.CreateAsync(appUser, user.Password);
            if (identityResult.Succeeded)
            {
                _userObserverSubject.NotifyObservers(appUser);
                ViewBag.message = "Üyelik işlemi başarıyla gerçekleşti.";
            }
            else
            {
                ViewBag.message = identityResult.Errors.ToList().First().Description;
            }
            return View();
        }
    }
}
