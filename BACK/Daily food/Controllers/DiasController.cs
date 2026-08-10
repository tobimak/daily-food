using Aplicacion.DTOs;
using Aplicacion.Interfaces;
using Dominio.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Daily_food.Controllers;

[Route("api/dias")]
public class DiasController : BaseApiController
{
    private readonly IDiaService _service;

    public DiasController(IDiaService service) => _service = service;

    [HttpGet("mes")]
    public async Task<ActionResult<IEnumerable<DiaResponse>>> ListarMes([FromQuery] int anio, [FromQuery] int mes)
        => Ok(await _service.ListarMesAsync(anio, mes, IdUsuario));

    [HttpGet("fecha")]
    public async Task<ActionResult<DiaResponse>> ObtenerPorFecha([FromQuery] DateTime fecha)
        => Ok(await _service.ObtenerPorFechaAsync(fecha, IdUsuario));

    [HttpPost("plato")]
    public async Task<ActionResult<DiaResponse>> AnadirPlato([FromBody] AnadirPlatoADiaRequest request)
        => Ok(await _service.AnadirPlatoAsync(request, IdUsuario));

    [HttpDelete("plato")]
    public async Task<IActionResult> QuitarPlato([FromQuery] DateTime fecha, [FromQuery] int idPlato, [FromQuery] TipoComida tipo)
        => await _service.QuitarPlatoAsync(fecha, idPlato, tipo, IdUsuario) ? NoContent() : NotFound();

    [HttpPut("nota")]
    public async Task<ActionResult<DiaResponse>> GuardarNota([FromBody] GuardarNotaRequest request)
        => Ok(await _service.GuardarNotaAsync(request.Fecha, request.Nota, IdUsuario));

    // 🧠 3 sugerencias (almuerzo/cena) para una fecha
    [HttpGet("sugerencia")]
    public async Task<ActionResult<IEnumerable<SugerenciaResponse>>> Sugerencia([FromQuery] DateTime fecha)
        => Ok(await _service.SeleccionOptimaDeMenuAsync(IdUsuario, fecha));
}