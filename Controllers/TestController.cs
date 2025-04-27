using Microsoft.AspNetCore.Mvc;

namespace LoggingDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;

    public TestController(ILogger<TestController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogTrace("Это сообщение уровня Trace");
        _logger.LogDebug("Это сообщение уровня Debug");
        _logger.LogInformation("Это сообщение уровня Information");
        _logger.LogWarning("Это сообщение уровня Warning");
        _logger.LogError("Это сообщение уровня Error");
        _logger.LogCritical("Это сообщение уровня Critical");

        return Ok("Проверка логирования завершена");
    }

    [HttpGet("exception")]
    public IActionResult GenerateException()
    {
        try
        {
            throw new InvalidOperationException("Тестовая ошибка");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка при обработке запроса");
            return StatusCode(500, "Произошла ошибка");
        }
    }
}