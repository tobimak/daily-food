using Aplicacion.DTOs;
using Aplicacion.Exceptions;
using Aplicacion.Interfaces;
using Aplicacion.Mapper;
using Dominio.Entities;
using Dominio.Interfaces_repository.Command;
using Dominio.Interfaces_repository.Query;

namespace Aplicacion.Services;

public class DiaService : IDiaService
{
    private readonly IDiaCommandRepository _diaCommand;
    private readonly IDiaQueryRepository _diaQuery;
    private readonly IPlatoQueryRepository _platoQuery;

    public DiaService(IDiaCommandRepository diaCommand, IDiaQueryRepository diaQuery, IPlatoQueryRepository platoQuery)
    {
        _diaCommand = diaCommand;
        _diaQuery = diaQuery;
        _platoQuery = platoQuery;
    }

    private async Task<Dia> ObtenerOCrearDia(DateTime fecha, int idUsuario)
    {
        var dia = await _diaQuery.ObtenerPorFechaAsync(fecha, idUsuario);
        return dia ?? await _diaCommand.CrearAsync(new Dia { Fecha = fecha.Date, IdUsuario = idUsuario });
    }

    public async Task<DiaResponse> ObtenerPorFechaAsync(DateTime fecha, int idUsuario)
    {
        var dia = await _diaQuery.ObtenerPorFechaAsync(fecha, idUsuario);
        return dia is null
            ? new DiaResponse(0, fecha.Date, null, new List<PlatoDelDiaResponse>())
            : DiaMapper.ToResponse(dia);
    }

    public async Task<IEnumerable<DiaResponse>> ListarMesAsync(int anio, int mes, int idUsuario)
    {
        var dias = await _diaQuery.ListarPorMesAsync(anio, mes, idUsuario);
        return dias.Select(DiaMapper.ToResponse);
    }

    public async Task<DiaResponse> AnadirPlatoAsync(AnadirPlatoADiaRequest request, int idUsuario)
    {
        var plato = await _platoQuery.ObtenerPorIdAsync(request.IdPlato, idUsuario)
            ?? throw new NotFoundException("El plato no existe.");

        var dia = await ObtenerOCrearDia(request.Fecha, idUsuario);

        var ok = await _diaCommand.AnadirPlatoAsync(dia.Id, plato.Id, request.TipoComida);
        if (!ok) throw new BusinessException("Ese plato ya está en esa comida del día.");

        var actualizado = await _diaQuery.ObtenerPorFechaAsync(request.Fecha, idUsuario);
        return DiaMapper.ToResponse(actualizado!);
    }

    public async Task<bool> QuitarPlatoAsync(DateTime fecha, int idPlato, TipoComida tipo, int idUsuario)
    {
        var dia = await _diaQuery.ObtenerPorFechaAsync(fecha, idUsuario)
            ?? throw new NotFoundException("No hay menú para esa fecha.");
        return await _diaCommand.QuitarPlatoAsync(dia.Id, idPlato, tipo);
    }

    public async Task<DiaResponse> GuardarNotaAsync(DateTime fecha, string nota, int idUsuario)
    {
        var dia = await ObtenerOCrearDia(fecha, idUsuario);
        await _diaCommand.GuardarNotaAsync(dia.Id, idUsuario, nota);
        var actualizado = await _diaQuery.ObtenerPorFechaAsync(fecha, idUsuario);
        return DiaMapper.ToResponse(actualizado!);
    }

    // ============ ALGORITMO DE SELECCIÓN ÓPTIMA v2 (solo almuerzo y cena) ============
    // Devuelve las 3 MEJORES opciones para la fecha indicada:
    //  1) Cooldown 4 días: comido hace <4 días → −100 (tu regla de variedad)
    //  2) Repeticiones en 7 días: −3 por uso
    //  3) Ingredientes compartidos con ese día: +2
    //  4) Plato nunca usado: +5
    public async Task<IEnumerable<SugerenciaResponse>> SeleccionOptimaDeMenuAsync(int idUsuario, DateTime fecha)
    {
        var platos = (await _platoQuery.ListarPorUsuarioAsync(idUsuario)).ToList();
        if (platos.Count == 0)
            throw new BusinessException("Aún no tienes platos. Crea alguno primero.");

        fecha = fecha.Date;
        var hoy = DateTime.Today;

        var dia = await _diaQuery.ObtenerPorFechaAsync(fecha, idUsuario);

        // Historial: mes actual + anterior (el cooldown funciona a inicio de mes)
        var prev = fecha.AddMonths(-1);
        var historial = (await _diaQuery.ListarPorMesAsync(fecha.Year, fecha.Month, idUsuario))
            .Concat(await _diaQuery.ListarPorMesAsync(prev.Year, prev.Month, idUsuario))
            .SelectMany(d => d.DiasPlato, (d, dp) => new { d.Fecha, dp.IdPlato })
            .ToList();

        var ingredientesDia = (dia?.DiasPlato ?? Enumerable.Empty<DiaPlato>())
            .SelectMany(dp => dp.Plato.Ingredientes.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var tipoElegido = ElegirTipoComida(dia);

        var top = platos
            .Select(p =>
            {
                var usos = historial.Where(h => h.IdPlato == p.Id).ToList();
                var ultimoUso = usos.Count > 0 ? usos.Max(h => h.Fecha).Date : (DateTime?)null;
                var diasSinComerse = ultimoUso is null ? int.MaxValue : (hoy - ultimoUso.Value).Days;

                int score = 0;

                if (diasSinComerse < 4) score -= 100;                       // cooldown
                score -= usos.Count(h => h.Fecha >= hoy.AddDays(-7)) * 3;   // variedad semanal
                score += p.Ingredientes
                          .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                          .Count(i => ingredientesDia.Contains(i)) * 2;     // ingredientes
                if (ultimoUso is null) score += 5;                          // novedad

                return new { Plato = p, Score = score, DiasSinComerse = diasSinComerse };
            })
            .OrderByDescending(x => x.Score)
            .Take(3)
            .ToList();

        return top.Select(x => new SugerenciaResponse(x.Plato.Id, x.Plato.Nombre, tipoElegido, Motivo(x.DiasSinComerse)));
    }

    private static string Motivo(int diasSinComerse) => diasSinComerse switch
    {
        int.MaxValue => "Nunca lo has usado: ideal para variar.",
        >= 4 => $"Hace {diasSinComerse} días que no lo comes.",
        _ => "Lo comiste hace poco, pero es de lo menos repetido."
    };

    // Solo almuerzo o cena: antes de las 18h sugiere almuerzo; después, cena
    private static TipoComida TipoComidaSegunHora() =>
        DateTime.Now.Hour < 18 ? TipoComida.Comida : TipoComida.Cena;

    private static TipoComida ElegirTipoComida(Dia? dia)
    {
        var tipos = new[] { TipoComidaSegunHora(), TipoComida.Comida, TipoComida.Cena };
        return tipos.FirstOrDefault(t =>
            dia is null || !dia.DiasPlato.Any(dp => dp.TipoComida == t), TipoComidaSegunHora());
    }
}