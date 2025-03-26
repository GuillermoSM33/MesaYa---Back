using MesaYa.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MesaYa.Models
{


public class RestaurantesFavoritos
{
    [ForeignKey("Restaurante")]
    public int RestauranteId { get; set; }
    public Restaurante Restaurante { get; set; }

    [ForeignKey("Usuario")]
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

    public bool IsDeleted { get; set; } = false;
}
}