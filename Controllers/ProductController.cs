using Microsoft.AspNetCore.Mvc;

namespace YourProject.Controllers
{
    [Route("products")] // Базовый маршрут для контроллера
    public class ProductsController : Controller
    {
        [HttpGet("")] // /products
        public IActionResult Index()
        {
            return Content("Страница продуктов");
        }

        [HttpGet("details/{id:int}")] // /products/details/5. Ограничение: id должен быть числом
        public IActionResult Details(int id)
        {
            return Content($"Страница деталей продукта с ID: {id}");
        }

        [HttpGet("search/{query}")] // /products/search/somequery
        public IActionResult Search(string query)
        {
            return Content($"Результаты поиска для запроса: {query}");
        }

        public IActionResult Contact() // /home/contact
        {
            return Content("Страница 'Контакты'");
        }
    }
}