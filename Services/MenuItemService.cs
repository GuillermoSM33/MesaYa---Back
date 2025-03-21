using MesaYa.Data;
using MesaYa.DTOs;
using MesaYa.Interfaces;
using MesaYa.Models;
using Microsoft.EntityFrameworkCore;

namespace MesaYa.Services
{
    public class MenuItemService : IMenuItemService

    {
        public readonly ApplicationDbContext _context;

        public MenuItemService(ApplicationDbContext context)
        {
            _context = context;
        }


        public List<MenuItemDTO> GetMenus()
        {
            try
            {
                List<MenuItemDTO> result = _context.MenuItems
                    .Include(x => x.MenuCategoria)
                    .Select(x => new MenuItemDTO
                    {
                        Id = x.ItemId,
                        NombreItem = x.Nombre,
                        Descripcion = x.Descripcion,
                        Precio = x.Precio,
                        CategoriaNombre = x.MenuCategoria.Nombre,
                        Imagen = x.ImagenUrl,
                        Disponible = x.Disponible,
                        isDeleted = x.IsDeleted

                    })
                    .ToList();

                return result;
            }
            catch (Exception ex) 
            
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<MenuItem> CreateMenuItem(CrearMenuItemDTO dto)
        {
            var nuevoItem = new MenuItem
            {
                Nombre = dto.NombreItem,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                ImagenUrl = dto.Imagen,
                Disponible = dto.Disponible,
                IsDeleted = dto.isDeleted,
                CategoriaId = dto.CategoriaId
            };

            // Guardamos el MenuItem en la base de datos
            _context.MenuItems.Add(nuevoItem);
            await _context.SaveChangesAsync();

            // Asociamos el menú al restaurante en la tabla pivote
            var restauranteMenuItem = new ItemAsRestaurante
            {
                RestauranteId = dto.RestauranteId,
                ItemId = nuevoItem.ItemId
            };

            _context.ItemAsRestaurantes.Add(restauranteMenuItem);
            await _context.SaveChangesAsync();

            return nuevoItem;
        }

        public async Task<MenuItem> UpdateMenuItem(int id, CrearMenuItemDTO dto)
        {
            var menuItem = await _context.MenuItems.FirstOrDefaultAsync(mi => mi.ItemId == id);

            if (menuItem == null)
            {
                throw new KeyNotFoundException("El menú no existe");
            }

            // Actualizar solo los campos normales sin tocar la tabla pivote
            menuItem.Nombre = dto.NombreItem;
            menuItem.Descripcion = dto.Descripcion;
            menuItem.Precio = dto.Precio;
            menuItem.ImagenUrl = dto.Imagen;
            menuItem.Disponible = dto.Disponible;
            menuItem.IsDeleted = dto.isDeleted;
            menuItem.CategoriaId = dto.CategoriaId;

            await _context.SaveChangesAsync();
            return menuItem;
        }



    }
}
