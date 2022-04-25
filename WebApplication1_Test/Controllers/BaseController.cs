using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace WebApplication1_Test.Controllers
{
    public class BaseController : Controller
    {
        protected const string authCookieName = ".AspNetCore.Cookies";

        protected readonly AppDbContext context;
        protected readonly IHtmlLocalizer<SharedResource> sharedLocalizer;
        protected User? user;

#pragma warning disable CS8618
        public BaseController(AppDbContext context)
#pragma warning restore CS8618
        {
            this.context = context;
        }

        public BaseController(AppDbContext context, IHtmlLocalizer<SharedResource> sharedLocalizer)
        {
            this.context = context;
            this.sharedLocalizer = sharedLocalizer;
        }

        public override void OnActionExecuting(ActionExecutingContext aeContext)
        {
            base.OnActionExecuting(aeContext);

            #region Выброс
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                user = context.Users.Include(u => u.Roles).FirstOrDefault(u => u.Email == User.Identity.Name);

                if (user == null)
                {
                    Response.Cookies.Delete(authCookieName);
                    aeContext.Result = new RedirectResult("/");

                    return;
                }

                ViewBag.DisplayName = user.DisplayName;

                string[] cookieRoles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(r => r.Value).ToArray();

                if (cookieRoles.Length == 0)
                {
                    return;
                }

                string[] dbRoles = user.Roles.Select(r => r.Name).ToArray();

                foreach (string cookieRole in cookieRoles)
                {
                    if (!dbRoles.Contains(cookieRole))
                    {
                        Response.Cookies.Delete(authCookieName);
                        aeContext.Result = new RedirectResult("/");

                        return;
                    }
                }
            }
            #endregion
        }
    }
}
