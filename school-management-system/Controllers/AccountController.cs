using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using school_management_system.DbModels.Enums;
using school_management_system.ViewModels;
using System.Security.Claims;

namespace school_management_system.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController (IAccountService accountService)
        {
            accountService = _accountService;
        }
        // GET: AccountController
        public IActionResult Login()
        {
            return View();
        }

        // GET: AccountController/Details/5
        public IActionResult Register(int id)
        {
            return View();
        }

        // GET: AccountController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) 
            { 
                return View(model);
            }

            var user = await _accountService.LoginAcc(model.Name, model.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Неверный логин или пароль");
                return View(model);
            }

            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var identity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return user.Role switch
            {
                UserRole.Admin => RedirectToAction("Index", "Admin"),
                UserRole.Student => RedirectToAction("Index", "Student"),
                UserRole.Teacher => RedirectToAction("Index", "Teacher"),

                _ => RedirectToAction("Index", "Home")
            };
        }

        // GET: AccountController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccountController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _accountService.RegistrationAcc(model.Name, model.Password);
            if (!result)
            {
                ModelState.AddModelError("Name", "Пользователь с таким именем уже существует");
                return View(model);
            }

            return RedirectToAction(nameof(Login));
        }

        // GET: AccountController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccountController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Login));
        }
    }
}
