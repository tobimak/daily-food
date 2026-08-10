using Aplicacion.DTOs;
using Dominio.Entities;

namespace Aplicacion.Interfaces;

public interface IDiaService
{
    Task<DiaResponse> ObtenerPorFechaAsync(DateTime fecha, int idUsuario);
    Task<IEnumerable<DiaResponse>> ListarMesAsync(int anio, int mes, int idUsuario);
    Task<DiaResponse> AnadirPlatoAsync(AnadirPlatoADiaRequest request, int idUsuario);
    Task<bool> QuitarPlatoAsync(DateTime fecha, int idPlato, TipoComida tipo, int idUsuario);
    Task<DiaResponse> GuardarNotaAsync(DateTime fecha, string nota, int idUsuario);
    Task<IEnumerable<SugerenciaResponse>> SeleccionOptimaDeMenuAsync(int idUsuario, DateTime fecha);
}
