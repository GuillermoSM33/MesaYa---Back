using MesaYa.DTOs;
using MesaYa.Models;

namespace MesaYa.Interfaces
{
    public interface IMesaService
    {

        public List<MesaDTO> GetMesas();
        public Task<CrearMesaDTO> CreateMesa(CrearMesaDTO crearmesaDTO);
        Task<UpdateMesaDTO> UpdateMesa(int id, UpdateMesaDTO updateMesaDTO);
        Task<bool> RestoreMesa(int mesaId);
        Task<bool> SoftDeleteMesa(int mesaId);
        Task<List<MesaDTO>> GetMesasPorRestauranteId(int restauranteId);
        Task<List<MesaDTO>> GetMesasActivasPorRestauranteId(int restauranteId);

    }
}
