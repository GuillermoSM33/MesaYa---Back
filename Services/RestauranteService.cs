using MesaYa.Data;

using MesaYa.Interfaces;
using MesaYa.Models;
using Microsoft.EntityFrameworkCore;

namespace MesaYa.Services
{
    public class RestauranteService : IRestauranteService
    {

        public readonly ApplicationDbContext _context;
        public RestauranteService(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Restaurante> GetRestaurante()
        {
            try
            {
                List<Restaurante> result = _context.Restaurantes
                    .Include(t=>t.Usuario)
                    .ToList();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);

            }
        }

        public async Task<Restaurante> GetRestauranteById(int id)
        {
            var restaurante = await _context.Restaurantes
     .Include(t => t.Usuario)
     .FirstOrDefaultAsync(t => t.RestauranteId == id);
            if (restaurante == null)
            {
                throw new KeyNotFoundException("El restaurante no existe.");
            }
            return restaurante;
        }

        //Crear Restaurante
        public async Task<RestauranteDTO> CreateRestaurante(CrearRestauranteDTO crearRestauranteDTO)
        {
            //Validación del campo nombre
            var restauranteExistente = await _context.Restaurantes
                .FirstOrDefaultAsync(t =>t.RestauranteNombre == crearRestauranteDTO.Nombre);
            if (restauranteExistente != null)
            {
                throw new Exception("El restaurante ya existe");
            }
            //Validación si el usuario tiene el rol correcto paraser hostes del restaurante
            var usuarioTieneRolHostess = await _context.UsuarioAsRoles
       .AnyAsync(t => t.UsuarioId == crearRestauranteDTO.UsuarioId && t.RoleId == 3);

            if (!usuarioTieneRolHostess)
            {
                throw new InvalidOperationException("El usuario no tiene el rol de hostess.");
            }
            var restaurante = new Restaurante
            {
                RestauranteNombre = crearRestauranteDTO.Nombre,
                Direccion = crearRestauranteDTO.Direccion,
                Telefono = crearRestauranteDTO.Telefono,
                Horario = crearRestauranteDTO.Horario,
                ImagenUrl = crearRestauranteDTO.Imagen,
                Descripcion = crearRestauranteDTO.Descripcion,
                UsuarioId = crearRestauranteDTO.UsuarioId
            };

            try
            {
                _context.Restaurantes.Add(restaurante);
                await _context.SaveChangesAsync();

                
                var restauranteDTO = new RestauranteDTO
                {
                    Id = restaurante.RestauranteId,
                    RestauranteNombre = restaurante.RestauranteNombre,
                    Direccion = restaurante.Direccion,
                    Telefono = restaurante.Telefono,
                    Horario = restaurante.Horario,
                    ImagenUrl = restaurante.ImagenUrl,
                    Descripcion = restaurante.Descripcion,
                    UsuarioId = restaurante.UsuarioId
                };

                return restauranteDTO;  // Devuelve el DTO
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> SoftDeleteRestaurante(int id)
        {
            
            var restaurante = await _context.Restaurantes
                .FirstOrDefaultAsync(t => t.RestauranteId == id);

            if (restaurante == null)
            {
                throw new KeyNotFoundException("El restaurante no existe.");
            }

            try
            {
                
                restaurante.IsDeleted = true;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Error al marcar el restaurante como eliminado.", e);
            }
        }


    }
}
