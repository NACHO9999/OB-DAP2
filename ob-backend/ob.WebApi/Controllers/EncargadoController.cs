using Microsoft.AspNetCore.Mvc;
using ob.Domain;
using ob.IBusinessLogic;
using ob.WebApi.DTOs;
using System.Collections.Generic;
using ob.Exceptions.BusinessLogicExceptions;
using ob.WebApi.Filters;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ob.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[ExceptionFilter]
public class EncargadoController : ControllerBase
{
    private IEncargadoService _encargadoService;

    public EncargadoController(IEncargadoService encargadosService)
    {
        _encargadoService = encargadosService;
    }


    [HttpGet("{email}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
    public IActionResult GetEncargadoByEmail([FromRoute] string email)
    {
        var encargado = _encargadoService.GetEncargadoByEmail(email);
        UsuarioCreateModel encargadoDTO = new UsuarioCreateModel() {Email = encargado.Email, Apellido = encargado.Apellido, Nombre = encargado.Nombre, Contrasena = encargado.Contrasena };
        return Ok(encargadoDTO);
    }

    [HttpPost("mantenimiento")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
    public IActionResult CrearMantenimiento([FromBody] UsuarioCreateModel mantenimiento)
    {
        
            _encargadoService.CrearMantenimiento(new Mantenimiento(mantenimiento.Nombre, mantenimiento.Apellido, mantenimiento.Email, mantenimiento.Contrasena));
             return Ok("Mantenimiento creado exitosamente.");


    }

    [HttpPost("solicitud")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
    public IActionResult CrearSolicitud([FromBody] SolicitudDTO solicitud)
    {
        
            _encargadoService.CrearSolicitud(solicitud.ToEntity());
            return Ok("Solicitud creada exitosamente.");
        
    }

    [HttpPut("asignar/{solicitud}/{email}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
    public IActionResult AsignarSolicitud([FromRoute] Guid solicitudId, [FromRoute] string email)
    {
            _encargadoService.AsignarSolicitud(solicitudId, email);

            return Ok("Solicitud asignada exitosamente.");
    }
    [HttpGet("solicitudes/edificio/{nombre}/{direccion}")]
    public IActionResult GetSolicitudByEdificio([FromRoute] string nombre, [FromRoute] string direccion)
    {
        int[] result = _encargadoService.GetSolicitudByEdificio(nombre, direccion);
        return Ok(result);
    }

    [HttpGet("solicitudes/mantenimiento/{email}")]
    public IActionResult GetSolicitudByMantenimiento([FromRoute] string email)
    {
        int[] result = _encargadoService.GetSolicitudByMantenimiento(email);
        return Ok(result);
    }

    [HttpGet("tiempo-promedio-atencion/{email}")]
    public IActionResult GetTiempoPromedioAtencion([FromRoute] string email)
    {
        TimeSpan? result = _encargadoService.TiempoPromedioAtencion(email);
        return result != null ? Ok(result) : Ok("No completo ninguna solicitud");
    }





}