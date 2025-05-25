using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TestApp.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error")]
        public IActionResult General()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
            ViewBag.Message = exception?.Message ?? "Неизвестная ошибка";
            return View("General");
        }

        [Route("Error/404")]
        public IActionResult NotFoundPage()
        {
            return View("NotFound");
        }
    }
}
