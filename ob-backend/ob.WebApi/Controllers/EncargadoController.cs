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

    [HttpPost("solicitud/{email}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) }, OwnUserAction = true)]
    public IActionResult CrearSolicitud([FromBody] SolicitudDTO solicitud, [FromRoute]string email)
    {
        
            _encargadoService.CrearSolicitud(solicitud.ToEntity(),email);
            return Ok("Solicitud creada exitosamente.");
        
    }

    [HttpPut("asignar/{solicitud}/{emailMantenimiento}/{email}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) }, OwnUserAction = true)]
    public IActionResult AsignarSolicitud([FromRoute] Guid solicitudId, [FromRoute]string emailMantenimiento, [FromRoute] string email)
    {
            _encargadoService.AsignarSolicitud(solicitudId, emailMantenimiento,email);

            return Ok("Solicitud asignada exitosamente.");
    }
    [HttpGet("solicitudes/edificio/{nombre}/{direccion}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
    public IActionResult GetSolicitudByEdificio([FromRoute] string nombre, [FromRoute] string direccion)
    {
        int[] result = _encargadoService.GetSolicitudByEdificio(nombre, direccion);
        return Ok(result);
    }

    [HttpGet("solicitudes/mantenimiento/{emailMantenimiento}/{email}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) }, OwnUserAction = true)]
    public IActionResult GetSolicitudByMantenimiento([FromRoute] string emailMantenimiento, [FromRoute] string email)
    {
        int[] result = _encargadoService.GetSolicitudByMantenimiento(emailMantenimiento, email);
        return Ok(result);
    }

    [HttpGet("tiempo-promedio-atencion/{email}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) }, OwnUserAction = true)]
    public IActionResult GetTiempoPromedioAtencion([FromRoute] string email)
    {
        TimeSpan? result = _encargadoService.TiempoPromedioAtencion(email);
        return result != null ? Ok(result) : Ok("No completo ninguna solicitud");
    }

    [HttpDelete("borrar-edificio/{nombre}/{ Direccion}/{email}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) }, OwnUserAction = true)]
    public IActionResult BorrarEdificio([FromRoute] string email, string nombre, string direccion)
    {
       
            _encargadoService.BorrarEdificio(nombre,direccion,email);
            return Ok("Edificio borrado exitosamente.");
        
    }
    [HttpGet("get-edificio/{nombre}/{ Direccion}/{email}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) }, OwnUserAction = true)]
    public IActionResult GetEdificio([FromRoute] string email, string nombre, string direccion)
    {

        EdificioDTO edificio = new EdificioDTO(_encargadoService.GetEdificioByNombreYDireccion(nombre, direccion, email));
        return Ok(edificio);

    }

    [HttpGet("get-edificiosbyencargado/{email}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) }, OwnUserAction = true)]
    public IActionResult GetEdificiosByEncargado([FromRoute] string email)
    {

        var lista = _encargadoService.GetEncargadoByEmail(email).Edificios;
        var retorno = lista.Select(e => new EdificioDTO(e)).ToList();
        return Ok(retorno);

    }

    [HttpPut("editar-edificio/{email}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) },OwnUserAction = true)]
    public IActionResult EditarEdificio([FromBody] EdificioDTO request, [FromRoute] string email)
    {
        _encargadoService.EditarEdificio(email,request.ToEntity());
        return Ok("Edificio editado exitosamente.");
    }


    [HttpPost("Crear-edificio/{email}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) }, OwnUserAction = true)]
    public IActionResult CrearEdificio([FromBody] EdificioDTO edificioDTO, [FromRoute] string email)
    {
        
            _encargadoService.CrearEdificio(email, edificioDTO.ToEntity());
            return Ok("Edificio creado exitosamente.");
        
        
    }

    [HttpPost("Crear-Depto/{email}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) }, OwnUserAction = true)]
    public IActionResult CrearDepto([FromBody] DeptoDTO deptoDTO, [FromRoute] string email)
    {

        _encargadoService.CrearDepto(email, deptoDTO.ToEntity());
        return Ok("Depto creado exitosamente.");

    }

    [HttpGet("get-depto/{numero}/{nombre}/{direccion}/{email}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) }, OwnUserAction = true)]
    public IActionResult GetDepto([FromRoute] string email, string nombre, string direccion, int numero)
    {

        DeptoDTO depto = new DeptoDTO(_encargadoService.GetDepto(numero, nombre, direccion, email));
        return Ok(depto);

    }

    [HttpPut("editar-depto/{email}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) }, OwnUserAction = true)]
    public IActionResult EditarDepto([FromBody] DeptoDTO request, [FromRoute] string email)
    {
        _encargadoService.EditarDepto(email, request.ToEntity());
        return Ok("Depto editado exitosamente.");
    }

}