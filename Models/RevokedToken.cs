using System.ComponentModel.DataAnnotations;

namespace MesaYa.Models
{
    public class RevokedToken
    {
        [Key]
        public int Id_Token { get; set; }

        [Required]
        public string Jti { get; set; }
        public DateTime RevokedAt { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
