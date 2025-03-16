using Microsoft.AspNetCore.Mvc;

namespace YourProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() // /
        {
            return Content("Главная страница");
        }

        public IActionResult About() // /home/about
        {
            return Content("Страница 'О нас'");
        }

        public IActionResult Contact() // /home/contact
        {
            return Content("Страница 'Контакты'");
        }
    }
}
