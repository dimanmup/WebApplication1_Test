using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using System.Net;

namespace WebApplication1_Test.Controllers
{
    [IgnoreAntiforgeryToken]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorController : BaseController
    {
        public ErrorController(AppDbContext context, IHtmlLocalizer<SharedResource> sharedLocalizer) 
            : base(context, sharedLocalizer)
        {
        }

        public IActionResult Page401(string returnUrl)
        {
            return RedirectToAction("Index", new { status_code = HttpStatusCode.Unauthorized, uri = returnUrl });
        }
        
        public IActionResult Page403(string returnUrl)
        {
            return RedirectToAction("Index", new { status_code = HttpStatusCode.Forbidden, uri = returnUrl });
        }
    }
}
