using Microsoft.AspNetCore.Mvc;
using System;

namespace StateManagementDemo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.SessionData = HttpContext.Session.GetString("SessionData");

            Request.Cookies.TryGetValue("CookieData", out var cookieData);
            ViewBag.CookieData = cookieData;
            
            return View();
        }

        [HttpPost]
        public IActionResult SaveData(string data)
        {
            HttpContext.Session.SetString("SessionData", data);

            Response.Cookies.Append("CookieData", data, new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1),
                HttpOnly = true
            });
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult SaveTempData(string data)
        {
            TempData["TempDataValue"] = data;
            return RedirectToAction("Index");
        }

        public IActionResult GetJsonData()
        {
            var data = new {
                ServerTime = DateTime.Now.ToString(),
                Message = "Data from server"
            };
            return Json(data);
        }
    }
}