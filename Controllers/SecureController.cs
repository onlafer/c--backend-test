using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/secure")]
[ApiController]
[Authorize]
public class SecureController : ControllerBase
{
    [HttpGet("user")]
    public IActionResult UserEndpoint()
    {
        return Ok(new { Message = "Сообщение доступное любому авторизованному пользователю" });
    }

    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public IActionResult AdminEndpoint()
    {
        return Ok(new { Message = "Сообщение доступное админу" });
    }

    [HttpGet("manager")]
    [Authorize(Roles = "Manager")]
    public IActionResult ManagerEndpoint()
    {
        return Ok(new { Message = "Сообщение доступное менеджеру" });
    }
}
