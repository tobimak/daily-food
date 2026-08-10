// Daily food/Controllers/AuthController.cs  (público: registro y login)
using Aplicacion.DTOs;
using Aplicacion.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Daily_food.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost("registro")]
    public async Task<ActionResult<AuthResponse>> Registro([FromBody] RegistroRequest request)
        => Ok(await _authService.RegistrarAsync(request));

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        => Ok(await _authService.LoginAsync(request));
}