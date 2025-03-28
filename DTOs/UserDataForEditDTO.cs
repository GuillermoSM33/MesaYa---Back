namespace MesaYa.DTOs
{
    public class UserDataForEditDTO
    {
        public int UsuarioId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsDeleted { get; set; }
        public List<int> RoleIds { get; set; }
    }
}
