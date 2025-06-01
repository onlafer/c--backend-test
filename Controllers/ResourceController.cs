using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ResourceController : ControllerBase
{
    [HttpGet("public")]
    [AllowAnonymous]
    public IActionResult GetPublicResource()
    {
        return Ok(new { Message = "This is a public resource" });
    }

    [HttpGet("user")]
    [Authorize(Roles = "User,Manager,Admin")]
    public IActionResult GetUserResource()
    {
        return Ok(new { Message = "This is a user resource" });
    }

    [HttpGet("manager")]
    [Authorize(Roles = "Manager,Admin")]
    public IActionResult GetManagerResource()
    {
        return Ok(new { Message = "This is a manager resource" });
    }

    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetAdminResource()
    {
        return Ok(new { Message = "This is an admin resource" });
    }
}
