// Daily food/Controllers/BaseApiController.cs
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Daily_food.Controllers;

[ApiController]
[Authorize] // 🔒 todo lo que herede de aquí requiere JWT
public abstract class BaseApiController : ControllerBase
{
    // El id del usuario viene del claim "sub" del token
    protected int IdUsuario =>
        int.Parse(User.FindFirstValue("sub")
            ?? User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("Token sin id de usuario."));
}