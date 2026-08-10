// Daily food/Controllers/PlatosController.cs
using Aplicacion.DTOs;
using Aplicacion.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Daily_food.Controllers;

[Route("api/platos")]
public class PlatosController : BaseApiController
{
    private readonly IPlatoService _service;

    public PlatosController(IPlatoService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlatoResponse>>> Listar()
        => Ok(await _service.ListarAsync(IdUsuario));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PlatoResponse>> Obtener(int id)
        => Ok(await _service.ObtenerAsync(id, IdUsuario));

    [HttpPost]
    public async Task<ActionResult<PlatoResponse>> Crear([FromBody] CrearPlatoRequest request)
        => Ok(await _service.CrearAsync(request, IdUsuario));

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PlatoResponse>> Modificar(int id, [FromBody] ActualizarPlatoRequest request)
        => Ok(await _service.ModificarAsync(id, request, IdUsuario));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
        => await _service.EliminarAsync(id, IdUsuario) ? NoContent() : NotFound();
}