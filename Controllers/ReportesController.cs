using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ReportesController : ControllerBase
{
    private readonly ReporteService _reporteService;

    public ReportesController(ReporteService reporteService)
    {
        _reporteService = reporteService;
    }

    [HttpGet("restaurantes-mas-reservados")]
    public IActionResult GetRestaurantesMasReservados([FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin, [FromQuery] string? nombreRestaurante)
    {
        var data = _reporteService.GetRestaurantesMasReservados(fechaInicio, fechaFin, nombreRestaurante);
        return Ok(data);
    }

    [HttpGet("platos-mas-pedidos")]
    public IActionResult GetPlatosMasPedidos([FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin, [FromQuery] string? nombreRestaurante)
    {
        var data = _reporteService.GetPlatosMasPedidos(fechaInicio, fechaFin, nombreRestaurante);
        return Ok(data);
    }
}
