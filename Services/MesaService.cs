
using MesaYa.Data;
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

        public List<Mesa> GetMesas()
        {
            try
            {
                List<Mesa> result = _context.Mesa
                    .Include(t=>t.Restaurante)
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


    }
}
