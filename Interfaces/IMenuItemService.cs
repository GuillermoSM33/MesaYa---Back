using MesaYa.DTOs;
using MesaYa.Models;

namespace MesaYa.Interfaces
{
    public interface IMenuItemService
    {
        public List<MenuItemDTO>GetMenus();
        Task<MenuItem> UpdateMenuItem(int id, CrearMenuItemDTO dto);
        public Task<MenuItem> CreateMenuItem(CrearMenuItemDTO dto);
        List<MenuItemDTO> GetMenusByRestauranteId(int restauranteId);
        List<MenuItemDTO> GetMenusActivosByRestauranteId(int restauranteId);
        Task<bool> RestaurarMenuItem(int id);

        Task<bool> SoftDeleteMenuItem(int id);

    }
}
