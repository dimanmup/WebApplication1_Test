using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1_Test.Controllers
{
    public class InfoController : BaseController
    {

        public InfoController(AppDbContext context, IDataProtectionProvider provider) : base(context)
        {
        }

        /// <summary>
        /// Если проверяешь в режиме Debug, нажми Continue, чтобы увидеть страницу обработки ошибки.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="AggregateException"></exception>
        public IActionResult GenError500Tree()
        {
            throw new AggregateException(
                new IndexOutOfRangeException("ex 1"),
                new ApplicationException("ex 2"),
                new ArgumentNullException("ex 3",
                    new AggregateException(
                        new Exception("ex 4",
                            new ArgumentException("ex 5")
                            )
                        )
                    )
                );
        }

        [Authorize]
        public string ForAuthenticated()
        {
            return "Authenticated";
        }

        [Authorize(Roles = "Master")]
        public string ForAuthorized()
        {
            return "Authorized";
        }

        public JsonResult AllUsers() 
            => Json(context.Users
                .Include(u => u.Roles)
                .Select(u => new { 
                    Name = u.Email, 
                    Roles = u.Roles
                        .Select(r => r.Name) }));
    }
}
