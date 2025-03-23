using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace TestApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResponsesController : ControllerBase
    {
        [HttpGet("html")]
        public ContentResult GetHtml()
        {
            return new ContentResult
            {
                ContentType = "text/html",
                Content = "<html><body><h1>test text!</h1></body></html>"
            };
        }

        [HttpGet("json")]
        public IActionResult GetJson()
        {
            var data = new { Name = "Kirill", Age = 19 };
            return Ok(data);
        }

        [HttpGet("file")]
        public IActionResult GetFile()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "example.txt");
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "text/plain", "example.txt");
        }
    }
}