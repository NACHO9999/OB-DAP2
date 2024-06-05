using Microsoft.AspNetCore.Mvc;
using ob.Domain;
using ob.IBusinessLogic;
using ob.WebApi.DTOs;
using System.Collections.Generic;
using ob.Exceptions.BusinessLogicExceptions;
using ob.WebApi.Filters;
using Microsoft.AspNetCore.Mvc.Filters;
using ob.BusinessLogic;

namespace ob.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[ExceptionFilter]
public class EncargadoController : BaseController
{
    private IEncargadoService _encargadoService;

    public EncargadoController(ISessionService sessionService, IEncargadoService encargadosService) : base(sessionService)
    {
        _encargadoService = encargadosService;
    }

    [HttpGet()]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
    public IActionResult GetEncargados()
    {
        var encargados = _encargadoService.GetAllEncargados();
        var retorno = encargados.Select(e => new EncargadoDTO(e)).ToList();
        return Ok(retorno);
    }

    [HttpGet("{email}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
    public IActionResult GetEncargadoByEmail([FromRoute] string email)
    {
        var encargado = _encargadoService.GetEncargadoByEmail(email);
        EncargadoDTO encargadoDTO = new EncargadoDTO(encargado);
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
        var usuario = GetCurrentUser();
        _encargadoService.CrearSolicitud(solicitud.ToEntity(), usuario.Email);
        return Ok("Solicitud creada exitosamente.");

    }

    [HttpPut("asignar/{solicitud}/{emailMantenimiento}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
    public IActionResult AsignarSolicitud([FromRoute] Guid solicitudId, [FromRoute] string emailMantenimiento)
    {
        var usuario = GetCurrentUser();
        _encargadoService.AsignarSolicitud(solicitudId, emailMantenimiento, usuario.Email);

        return Ok("Solicitud asignada exitosamente.");
    }
    [HttpGet("solicitudes/edificio/{nombre}/{direccion}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
    public IActionResult GetSolicitudByEdificio([FromRoute] string nombre, [FromRoute] string direccion)
    {
        int[] result = _encargadoService.GetSolicitudByEdificio(nombre, direccion, GetCurrentUser().Email);
        return Ok(result);
    }

    [HttpGet("solicitudes/mantenimiento/{emailMantenimiento}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
    public IActionResult GetSolicitudByMantenimiento([FromRoute] string emailMantenimiento)
    {
        var usuario = GetCurrentUser();
        int[] result = _encargadoService.GetSolicitudByMantenimiento(emailMantenimiento, usuario.Email);
        return Ok(result);
    }

    [HttpGet("tiempo-promedio-atencion/{email}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
    public IActionResult GetTiempoPromedioAtencion([FromRoute] string email)
    {
        TimeSpan? result = _encargadoService.TiempoPromedioAtencion(email);
        return result != null ? Ok(result) : Ok("No completo ninguna solicitud");
    }

    [HttpGet("Dueno/{email}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
    public IActionResult GetDueno([FromRoute] string email)
    {
        Dueno dueno = _encargadoService.GetDueno(email);
        var result = new DuenoDTO(dueno);
        return Ok(result);
    }
    [HttpPut("asignar-dueno/{numero}/{edNombre}/{edDireccion}/{emailDueno}/{email}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
    public IActionResult AsignarDueno([FromRoute] int numero, [FromRoute] string edNombre, [FromRoute] string edDireccion, [FromRoute] string emailDueno)
    {
        _encargadoService.AsignarDueno(numero, edNombre, edDireccion, emailDueno, GetCurrentUser().Email);
        return Ok("Dueno asignado exitosamente.");
    }

}