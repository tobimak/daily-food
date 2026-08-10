// Daily food/Controllers/UsuarioController.cs  (perfil)
using Aplicacion.DTOs;
using Aplicacion.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Daily_food.Controllers;

[Route("api/usuario")]
public class UsuarioController : BaseApiController
{
    private readonly IUsuarioService _service;

    public UsuarioController(IUsuarioService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<UsuarioResponse>> Obtener()
        => Ok(await _service.ObtenerAsync(IdUsuario));

    [HttpPut]
    public async Task<ActionResult<UsuarioResponse>> Actualizar([FromBody] ActualizarUsuarioRequest request)
        => Ok(await _service.ActualizarAsync(IdUsuario, request));

    [HttpDelete]
    public async Task<IActionResult> Eliminar()
        => await _service.EliminarAsync(IdUsuario) ? NoContent() : NotFound();

    [HttpPost("foto")]
    public async Task<ActionResult<UsuarioResponse>> GuardarFoto([FromBody] GuardarFotoRequest request)
    => Ok(await _service.GuardarFotoAsync(IdUsuario, request.Foto));
}