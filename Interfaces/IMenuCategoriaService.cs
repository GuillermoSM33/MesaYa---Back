using MesaYa.DTOs;
using MesaYa.Models;

namespace MesaYa.Interfaces
{
    public interface IMenuCategoriaService
    {

        public List<MenuCategoria> GetCategorias();
        public Task<MenuCategoria> UpdateCategoria(int id ,MenuCategoria menuCategoria);
        public Task<MenuCategoria>CreateCategoria(MenuCategoria menuCategoria);

        Task<List<MenuCategoria>> CrearCategoriasMultiples(CrearCategoriasMultiplesDTO dto);
    }
}
