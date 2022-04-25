using DAL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1_Test.Models;

namespace WebApplication1_Test.Controllers
{
    public class AccountController : BaseController
    {
        private const string displayNameRegexPattern = "^[a-zA-Z0-9_.]{5,100}$";

        public AccountController(AppDbContext context,
            IHtmlLocalizer<SharedResource> sharedLocalizer)
            : base(context, sharedLocalizer)
        {
        }

        private async Task Authenticate(string name, string? displayName = null, params Role[] roles)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.GivenName, string.IsNullOrEmpty(displayName) ? name : displayName)
            };

            foreach (Role role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            ClaimsIdentity ci = new ClaimsIdentity(claims, "ApplicationCookie");

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(ci),
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddYears(1),
                    IsPersistent = true
                });
        }

        #region Sign in
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            #region Валидация
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            User? user = context.Users
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.Email == model.EmailOrName || u.DisplayName == model.EmailOrName);

            if (user == null || string.IsNullOrEmpty(user.Password) || !Hasher.Check(user.Password, model.Password))
            {
                ModelState.AddModelError("", sharedLocalizer["EmailOrPasswordInvalid"].Value);
                return View(model);
            }
            #endregion

            await Authenticate(user.Email, user.DisplayName, user.Roles.ToArray());

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Sign up
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            #region Валидация
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (context.Users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("", string.Format(sharedLocalizer["UserAlreadyExists"].Value, model.Email));
                return View(model);
            }
            #endregion Валидация

            User user = new User()
            {
                Email = model.Email,
                DisplayName = model.Email,
                Password = Hasher.Hash(model.Password),
                RegistrationDateTime = DateTime.UtcNow,
                EmailConfirmed = true
            };

            if (!Hasher.Check(user.Password, model.Password))
            {
                throw new Exception($"Hasher не работает. value: {model.Password}, hash: {user.Password}.");
            }

            context.Users.Add(user);
            await context.SaveChangesAsync();
            await Authenticate(model.Email);

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Sign out
        public async Task<IActionResult> SignOff()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
        #endregion

        [Authorize]
        public IActionResult Index()
        {
            User user = context.Users.First(u => u.Email == User.Identity!.Name);

            AccountModel model = new AccountModel
            {
                DisplayName = user.DisplayName,
                EmailConfirmed = user.EmailConfirmed
            };

            return View(model);
        }
    }
}
