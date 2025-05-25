using Microsoft.AspNetCore.Mvc;
using System;

namespace TestApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ThrowException()
        {
            throw new Exception("Простое исключение");
        }

        public IActionResult DbException()
        {
            throw new InvalidOperationException("Исключение базы данных");
        }

        public IActionResult NotFoundError()
        {
            return NotFound();
        }
    }
}