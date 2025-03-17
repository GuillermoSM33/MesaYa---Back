using MesaYa.Models;

namespace MesaYa.Interfaces
{
    public interface IMesaService
    {

        public List<Mesa> GetMesas();

        public Task<CrearMesaDTO> CreateMesa(CrearMesaDTO crearmesaDTO);

        Task<UpdateMesaDTO> UpdateMesa(int id, UpdateMesaDTO updateMesaDTO);

    }
}
