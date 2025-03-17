public class CrearRestauranteDTO
{
    public string Nombre { get; set; }
    public string Direccion { get; set; }
    public string Telefono { get; set; }
    public string Horario { get; set; }
    public string Imagen { get; set; }
    public string Descripcion { get; set; }
    public int UsuarioId { get; set; }
}

public class RestauranteDTO
{
    public int Id { get; set; }
    public string RestauranteNombre { get; set; }
    public string Direccion { get; set; }
    public string Telefono { get; set; }
    public string Horario { get; set; }
    public string ImagenUrl { get; set; }
    public string Descripcion { get; set; }
    public int UsuarioId { get; set; }
}