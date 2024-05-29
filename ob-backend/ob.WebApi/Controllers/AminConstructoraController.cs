using ob.WebApi.DTOs;
using System.Collections.Generic;
using ob.Exceptions.BusinessLogicExceptions;
using ob.WebApi.Filters;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ob.IBusinessLogic;
using ob.Domain;
using Enums;
using ob.BusinessLogic;

namespace ob.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[ExceptionFilter]
public class AdminConstructoraController : BaseController
{
    private IAdminConstructoraService _adminConstructoraService;

    public AdminConstructoraController(ISessionService sessionService, IAdminConstructoraService adminConstructoraService) : base(sessionService)
    {
        _adminConstructoraService = adminConstructoraService;
    }

    [HttpDelete("borrar-edificio/{nombre}/{ Direccion}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
    public IActionResult BorrarEdificio([FromRoute] string nombre, [FromRoute] string direccion)
    {
        var usuario = GetCurrentUser();
        _adminConstructoraService.BorrarEdificio(nombre, direccion, usuario.Email);
        return Ok("Edificio borrado exitosamente.");

    }
    [HttpGet("get-edificio/{nombre}/{ Direccion}/{email}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
    public IActionResult GetEdificio([FromRoute] string nombre, [FromRoute] string direccion)
    {
        var usuario = GetCurrentUser();
        EdificioDTO edificio = new EdificioDTO(_adminConstructoraService.GetEdificioByNombreYDireccion(nombre, direccion, usuario.Email));
        return Ok(edificio);

    }

    [HttpGet("get-edificiosporadmin")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
    public IActionResult GetEdificiosPorAdmin()
    {
        var usuario = GetCurrentUser();
        var lista = _adminConstructoraService.GetEdificiosPorAdmin(usuario.Email);
        var retorno = lista.Select(e => new EdificioDTO(e)).ToList();
        return Ok(retorno);

    }

    [HttpPut("editar-edificio")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
    public IActionResult EditarEdificio([FromBody] EdificioDTO request)
    {
        _adminConstructoraService.EditarEdificio(request.ToEntity(), GetCurrentUser().Email);
        return Ok("Edificio editado exitosamente.");
    }


    [HttpPost("Crear-edificio")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
    public IActionResult CrearEdificio([FromBody] EdificioDTO edificioDTO)
    {

        _adminConstructoraService.CrearEdificio(edificioDTO.ToEntity(), GetCurrentUser().Email);
        return Ok("Edificio creado exitosamente.");


    }

    [HttpPost("Crear-Depto")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
    public IActionResult CrearDepto([FromBody] DeptoDTO deptoDTO)
    {

        _adminConstructoraService.CrearDepto(GetCurrentUser().Email, deptoDTO.ToEntity());
        return Ok("Depto creado exitosamente.");

    }

    [HttpGet("get-depto/{numero}/{nombre}/{direccion}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
    public IActionResult GetDepto([FromRoute] string nombre, [FromRoute] string direccion,[FromRoute] int numero)
    {
        var email = GetCurrentUser().Email;
        DeptoDTO depto = new DeptoDTO(_adminConstructoraService.GetDepto(numero, nombre, direccion, GetCurrentUser().Email));
        return Ok(depto);

    }

    [HttpPut("editar-depto")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
    public IActionResult EditarDepto([FromBody] DeptoDTO request)
    {
        _adminConstructoraService.EditarDepto(GetCurrentUser().Email, request.ToEntity());
        return Ok("Depto editado exitosamente.");
    }

}

