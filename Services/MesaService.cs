
using MesaYa.Data;
using MesaYa.DTOs;
using MesaYa.Interfaces;
using MesaYa.Models;
using Microsoft.EntityFrameworkCore;


namespace MesaYa.Services
{
    public class MesaService : IMesaService
    {

        public readonly ApplicationDbContext _context;

        public MesaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<MesaDTO> GetMesas()
        {
            try
            {
                List<MesaDTO> result = _context.Mesa
                    .Include(t=>t.Restaurante)
                    .Select(t=> new MesaDTO
                    {
                        MesaId = t.MesaId,
                        MesaNumero = t.MesaNumero,
                        RestauranteId=t.RestauranteId,
                        RestauranteNombre =t.Restaurante.RestauranteNombre,
                        Capacidad = t.Capacidad,
                        Disponible =t.Disponible,
                        IsDeleted= t.IsDeleted,
                    })
                    .ToList();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CrearMesaDTO> CreateMesa(CrearMesaDTO crearmesaDTO)
        {
            var mesa = new Mesa
            {
                MesaNumero = crearmesaDTO.MesaNumero,
                RestauranteId = crearmesaDTO.RestauranteId,
                Capacidad = crearmesaDTO.Capacidad,
                Disponible = crearmesaDTO.Disponible
            };

            // FALTABA ESTO: agregar la mesa al contexto
            _context.Mesa.Add(mesa);

            await _context.SaveChangesAsync();

            return crearmesaDTO;
        }


        public async Task<UpdateMesaDTO> UpdateMesa(int id, UpdateMesaDTO updateMesaDTO)
        {
            // Buscar la mesa existente por su ID
            var mesa = await _context.Mesa
                .FirstOrDefaultAsync(m => m.MesaId == id);

            if (mesa == null)
            {
                throw new KeyNotFoundException("La mesa no existe.");
            }

            try
            {
                // Actualizar los campos de la mesa con los valores del DTO
                mesa.MesaNumero = updateMesaDTO.MesaNumero;
                mesa.Capacidad = updateMesaDTO.Capacidad;
                mesa.Disponible = updateMesaDTO.Disponible;

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                // Devolver el DTO actualizado
                return updateMesaDTO;
            }
            catch (Exception e)
            {
                throw new Exception("Error al actualizar la mesa.", e);
            }
        }

        public async Task<bool> RestoreMesa(int mesaId)
        {
            try
            {
                // Verificar si la mesa existe y está eliminada
                var mesa = await _context.Mesa.FirstOrDefaultAsync(m => m.MesaId == mesaId && m.IsDeleted);
                if (mesa == null)
                {
                    throw new KeyNotFoundException("La mesa no existe o ya está activa.");
                }

                // Validar si la mesa tiene alguna reserva activa en la tabla pivote
                bool tieneReservas = await _context.ReservaAsMesas
                    .Include(rm => rm.Reserva)
                    .AnyAsync(rm => rm.MesaId == mesaId && !rm.Reserva.IsDeleted);

                if (tieneReservas)
                {
                    throw new InvalidOperationException("No se puede restaurar la mesa porque está asignada a reservas activas.");
                }

                // Restaurar la mesa
                mesa.IsDeleted = false;
                _context.Mesa.Update(mesa);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Error al restaurar la mesa.", e);
            }
        }


        public async Task<bool> SoftDeleteMesa(int mesaId)
        {
            try
            {
                // Verificar si la mesa existe
                var mesa = await _context.Mesa.FirstOrDefaultAsync(m => m.MesaId == mesaId);
                if (mesa == null)
                {
                    throw new KeyNotFoundException("La mesa no existe.");
                }

                // Validar si la mesa está en alguna reserva activa (no eliminada)
                bool tieneReservas = await _context.ReservaAsMesas
                    .Include(rm => rm.Reserva)
                    .AnyAsync(rm => rm.MesaId == mesaId && !rm.Reserva.IsDeleted);

                if (tieneReservas)
                {
                    throw new InvalidOperationException("No se puede eliminar la mesa porque tiene reservas activas.");
                }

                // Marcar la mesa como eliminada
                mesa.IsDeleted = true;
                _context.Mesa.Update(mesa);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Error al marcar la mesa como eliminada.", e);
            }
        }


        public async Task<List<MesaDTO>> GetMesasPorRestauranteId(int restauranteId)
        {
            try
            {
                var result = await _context.Mesa
                    .Include(t => t.Restaurante)
                    .Where(t => t.RestauranteId == restauranteId)
                    .Select(t => new MesaDTO
                    {
                        MesaId = t.MesaId,
                        MesaNumero = t.MesaNumero,
                        RestauranteId=t.RestauranteId,
                        RestauranteNombre = t.Restaurante.RestauranteNombre,
                        Capacidad = t.Capacidad,
                        Disponible = t.Disponible,
                        IsDeleted = t.IsDeleted
                    })
                    .ToListAsync();

                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Error al obtener las mesas del restaurante.", e);
            }
        }

        public async Task<List<MesaDTO>> GetMesasActivasPorRestauranteId(int restauranteId)
        {
            try
            {
                var result = await _context.Mesa
                    .Include(t => t.Restaurante)
                    .Where(t => t.RestauranteId == restauranteId && t.IsDeleted == false)
                    .Select(t => new MesaDTO
                    {
                        MesaId = t.MesaId,
                        MesaNumero = t.MesaNumero,
                        RestauranteNombre = t.Restaurante.RestauranteNombre,
                        Capacidad = t.Capacidad,
                        Disponible = t.Disponible,
                        IsDeleted = t.IsDeleted
                    })
                    .ToListAsync();

                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Error al obtener las mesas activas del restaurante.", e);
            }
        }




    }
}
