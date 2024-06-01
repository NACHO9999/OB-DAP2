using ob.WebApi.DTOs;
using System.Collections.Generic;
using ob.Exceptions.BusinessLogicExceptions;
using ob.WebApi.Filters;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ob.IBusinessLogic;
using ob.Domain;
using Enums;

namespace ob.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[ExceptionFilter]
public class AdministradorController : ControllerBase
{
    private IAdminService _adminService;

    public AdministradorController(IAdminService adminService)
    {
        _adminService = adminService;
    }
    [HttpPost]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Administrador) })]
    public IActionResult CrearAdmin([FromBody] UsuarioCreateModel adminDTO)
    {
        var admin = new Administrador(adminDTO.Nombre, adminDTO.Apellido, adminDTO.Email, adminDTO.Contrasena);
        _adminService.CrearAdmin(admin);
        return Ok();


    }

    [HttpGet("{email}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Administrador) })]
    public IActionResult GetAdminByEmail([FromRoute] string email)
    {

        var admin = _adminService.GetAdminByEmail(email);
        return Ok(admin);


    }

    [HttpPost("invitar")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Administrador) })]
    public IActionResult Invitar([FromBody] InvitacionDTO invitacion)
    {
        _adminService.Invitar(invitacion.Email, invitacion.Nombre, invitacion.FechaExpiracion, invitacion.Rol);
        return Ok();
    }

    [HttpDelete("invitar/{email}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Administrador) })]
    public IActionResult EliminarInvitacion([FromRoute] string email)
    {
        _adminService.EliminarInvitacion(email);
        return Ok();
    }

    [HttpPost("categoria")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Administrador) })]
    public IActionResult AltaCategoria([FromBody] Categoria categoria)
    {
        _adminService.AltaCategoria(categoria);
        return Ok();
    }
}