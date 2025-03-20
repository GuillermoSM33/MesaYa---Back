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
        //Obtener Restaurantes
        public List<RestauranteDTO> GetRestaurante()
        {
            try
            {
                List<RestauranteDTO> result = _context.Restaurantes
                    .Include(t => t.Usuario) // Carga la relación con Usuario
                    .Select(t => new RestauranteDTO
                    {
                        Id = t.RestauranteId,
                        RestauranteNombre = t.RestauranteNombre,
                        Direccion = t.Direccion,
                        Telefono = t.Telefono,
                        Horario = t.Horario,
                        ImagenUrl = t.ImagenUrl,
                        Descripcion = t.Descripcion,
                        UserName = t.Usuario.Username // Aquí traemos el nombre del usuario
                    })
                    .ToList();

                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }



        public async Task<RestauranteDTO> GetRestauranteById(int id)
        {
            var restaurante = await _context.Restaurantes
                .Include(t => t.Usuario) // Incluye la relación con Usuario
                .FirstOrDefaultAsync(t => t.RestauranteId == id);

            if (restaurante == null)
            {
                throw new KeyNotFoundException("El restaurante no existe.");
            }

            // Mapeo de Restaurante a RestauranteDTO
            var restauranteDTO = new RestauranteDTO
            {
                Id = restaurante.RestauranteId,
                RestauranteNombre = restaurante.RestauranteNombre,
                Direccion = restaurante.Direccion,
                Telefono = restaurante.Telefono,
                Horario = restaurante.Horario,
                ImagenUrl = restaurante.ImagenUrl,
                Descripcion = restaurante.Descripcion,
                UserName = restaurante.Usuario.Username // Devuelve el nombre del usuario
            };

            return restauranteDTO;
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
                    UserName = restaurante.UsuarioId.ToString() // Aquí traemos el nombre del usuario
                };

                return restauranteDTO;  // Devuelve el DTO
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public async Task<RestauranteDTO> UpdateRestaurante(int id, UpdateRestauranteDTO updateRestauranteDTO)
        {

            var restaurante = await _context.Restaurantes
                .FirstOrDefaultAsync(t => t.RestauranteId == id);

            if (restaurante == null)
            {
                throw new KeyNotFoundException("El restaurante no existe.");
            }


            try
            {
                restaurante.RestauranteNombre = updateRestauranteDTO.RestauranteNombre;
                restaurante.Direccion = updateRestauranteDTO.Direccion;
                restaurante.Telefono = updateRestauranteDTO.Telefono;
                restaurante.Horario = updateRestauranteDTO.Horario;
                restaurante.ImagenUrl = updateRestauranteDTO.ImagenUrl;
                restaurante.Descripcion = updateRestauranteDTO.Descripcion;
                restaurante.UsuarioId = updateRestauranteDTO.UserId;
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
                    UserName = restaurante.UsuarioId.ToString() // Aquí traemos el nombre del usuario
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
