using MesaYa.Data;
using MesaYa.DTOs;
using MesaYa.Interfaces;
using MesaYa.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace MesaYa.Services
{
    public class MenuCategoriaService : IMenuCategoriaService
    {
        public readonly ApplicationDbContext _context;
        public MenuCategoriaService(ApplicationDbContext context)
        {
            _context = context;
        }


        public List<MenuCategoria> GetCategorias()
        {
            try
            {
                return _context.MenuCategorias.ToList(); // Esto obtiene las categorías de la base de datos
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<MenuCategoria> CreateCategoria(MenuCategoria menuCategoria)
        {
            var categoria = new MenuCategoria
            {
                Nombre = menuCategoria.Nombre
            };

            _context.MenuCategorias.Add(categoria); // ✅ Agregar al contexto antes de guardar cambios
            await _context.SaveChangesAsync();

            return categoria;
        }

        public async Task<MenuCategoria> UpdateCategoria(int id, MenuCategoria menuCategoria)
        {
            var categoria = await _context.MenuCategorias
                .FirstOrDefaultAsync(x => x.CategoriaId == id);
            if (categoria == null)
            {
                throw new KeyNotFoundException("La categoria no existe");
            }
            try
            {
                categoria.Nombre = menuCategoria.Nombre;

                await _context.SaveChangesAsync();
                return categoria;
            }
            catch (Exception e)
            {
                throw new Exception("Error al actualizar", e);
            }

        }

        public async Task<List<MenuCategoria>> CrearCategoriasMultiples(CrearCategoriasMultiplesDTO dto)
        {
            if (dto.Nombres == null || !dto.Nombres.Any())
            {
                throw new ArgumentException("Debe proporcionar al menos un nombre de categoría.");
            }

            var categorias = dto.Nombres.Select(nombre => new MenuCategoria { Nombre = nombre }).ToList();

            await _context.MenuCategorias.AddRangeAsync(categorias);
            await _context.SaveChangesAsync();

            return categorias;
        }

    }
}
