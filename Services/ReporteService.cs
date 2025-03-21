using MesaYa.Data;
using MesaYa.DTOs;
using Microsoft.EntityFrameworkCore;

public class ReporteService
{
    private readonly ApplicationDbContext _context;

    public ReporteService(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<RestauranteMasReservadoDTO> GetRestaurantesMasReservados(DateTime? fechaInicio, DateTime? fechaFin, string? nombreRestaurante)
    {
        var query = _context.Reservas
            //.Include(r => r.Mesa)
            //.ThenInclude(m => m.Restaurante)
            .AsQueryable();

        if (fechaInicio.HasValue)
        {
            query = query.Where(r => r.FechaReserva >= fechaInicio.Value);
        }

        if (fechaFin.HasValue)
        {
            query = query.Where(r => r.FechaReserva <= fechaFin.Value);
        }

        if (!string.IsNullOrEmpty(nombreRestaurante))
        {
            //query = query.Where(r => r.Mesa.Restaurante.RestauranteNombre.Contains(nombreRestaurante));
        }

        return query
            //.GroupBy(r => new { r.Mesa.RestauranteId, r.Mesa.Restaurante.RestauranteNombre })
            .Select(g => new RestauranteMasReservadoDTO
            {
               // RestauranteId = g.Key.RestauranteId,
               // RestauranteNombre = g.Key.RestauranteNombre,
               // TotalReservas = g.Count()
            })
            .OrderByDescending(r => r.TotalReservas)
        .ToList();
    }

    //public List<PlatoMasPedidoDTO> GetPlatosMasPedidos(DateTime? fechaInicio, DateTime? fechaFin, string? nombreRestaurante)
    //{
    //    var query = _context.ItemAsRestaurantes
    //        .Include(i => i.Restaurante)
    //        .Include(i => i.MenuItem)
    //        .AsQueryable();

    //    if (!string.IsNullOrEmpty(nombreRestaurante))
    //    {
    //        query = query.Where(i => i.Restaurante.RestauranteNombre.Contains(nombreRestaurante));
    //    }

    //    return query
    //        .GroupBy(i => new { i.ItemId, i.MenuItem.Nombre })
    //        .Select(g => new PlatoMasPedidoDTO
    //        {
    //            ItemId = g.Key.ItemId,
    //            Nombre = g.Key.Nombre,
    //            TotalPedidos = g.Count()
    //        })
    //        .OrderByDescending(p => p.TotalPedidos)
    //        .ToList();
    //}
    public List<PlatoMasPedidoDTO> GetPlatosMasPedidos(DateTime? fechaInicio, DateTime? fechaFin, string? nombreRestaurante)
{
    var query = _context.ItemAsRestaurantes
        .Include(i => i.Restaurante)
        .Include(i => i.MenuItem)
        .ThenInclude(m => m.MenuCategoria) // Incluye la categoría
        .AsQueryable();

    if (!string.IsNullOrEmpty(nombreRestaurante))
    {
        query = query.Where(i => i.Restaurante.RestauranteNombre.Contains(nombreRestaurante));
    }

        return query
        .GroupBy(i => new {
            i.ItemId,
            NombrePlato = i.MenuItem.Nombre, // Renombrar explícitamente
            NombreRestaurante = i.Restaurante.RestauranteNombre,
            NombreCategoria = i.MenuItem.MenuCategoria.Nombre // Renombrar para evitar conflicto
        })
        .Select(g => new PlatoMasPedidoDTO
        {
            ItemId = g.Key.ItemId,
            Nombre = g.Key.NombrePlato, // Usar alias definido en GroupBy
            RestauranteNombre = g.Key.NombreRestaurante,
            CategoriaNombre = g.Key.NombreCategoria,
            TotalPedidos = g.Count()
        })
        .OrderByDescending(p => p.TotalPedidos)
        .ToList();


    }

}
