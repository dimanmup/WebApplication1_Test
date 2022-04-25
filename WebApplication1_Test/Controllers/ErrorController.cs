using DAL;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using System.Net;
using WebApplication1_Test.Models;

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

        public async Task<IActionResult> Index(HttpStatusCode status_code, string uri)
        {
            ErrorModel model = new ErrorModel();

            switch (status_code)
            {
                case HttpStatusCode.NotFound:
                    model.Title = sharedLocalizer["Error"].Value;
                    model.Message = sharedLocalizer["Error404Message"].Value;
                    model.StatusCode = status_code;
                    break;

                case HttpStatusCode.Unauthorized:
                    model.Title = sharedLocalizer["AccessDenied"].Value;
                    model.Message = sharedLocalizer["Error401Message"].Value;
                    model.StatusCode = status_code;
                    break;

                case HttpStatusCode.Forbidden:
                    model.Title = sharedLocalizer["AccessDenied"].Value;
                    model.Message = sharedLocalizer["Error403Message"].Value;
                    model.StatusCode = status_code;
                    break;

                case HttpStatusCode.RequestTimeout:
                    model.Title = sharedLocalizer["Error"].Value;
                    model.Message = sharedLocalizer["Error408Message"].Value;
                    model.StatusCode = status_code;
                    break;

                default:
                    model.Title = sharedLocalizer["Error"].Value;
                    model.Message = sharedLocalizer["ErrorInternalServerMessage"].Value;
                    model.StatusCode = HttpStatusCode.InternalServerError;
                    break;
            }

            if (model.StatusCode != HttpStatusCode.InternalServerError)
            {
                string message = $"HTTP ERROR\nCODE={model.StatusCode}\nURI=\"{uri}\"";

                // Логика...

                return View(model);
            }

            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature != null)
            {
                // Отправка на почту...
            }

            return View(model);
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
