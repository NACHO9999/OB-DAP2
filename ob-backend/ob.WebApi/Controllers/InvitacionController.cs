using ob.WebApi.DTOs;
using System.Collections.Generic;
using ob.Exceptions.BusinessLogicExceptions;
using ob.WebApi.Filters;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ob.Domain;
using ob.IBusinessLogic;

namespace ob.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[ExceptionFilter]
public class InvitacionController : ControllerBase
{
    private IInvitacionService _invitacionService;

    public InvitacionController(IInvitacionService invitacionsService)
    {
        _invitacionService = invitacionsService;
    }


    [HttpGet("{email}")]
    public IActionResult GetInvitacionByEmail([FromRoute] string email)
    {
        InvitacionDTO invitacion = new InvitacionDTO(_invitacionService.GetInvitacionByEmail(email));
        return Ok(invitacion);
    }

    [HttpDelete("{email}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Administrador) })]
    public IActionResult DeleteInvitacionByEmail([FromRoute] string email)
    {
        _invitacionService.EliminarInvitacion(email);
        return Ok();
    }


    [HttpPost]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Administrador) })]
    public IActionResult InsertInvitacion([FromBody] InvitacionDTO newInvitacion)
    {
        _invitacionService.CrearInvitacion(new Invitacion(newInvitacion.Email, newInvitacion.Nombre, newInvitacion.FechaExpiracion));
        return Ok();
    }

    [HttpPost("{email}/{contrasena}")]
    
    public IActionResult InvitacionAccepted([FromRoute]string email, [FromRoute] string contrasena )
    {
        
        Invitacion invitacion = _invitacionService.GetInvitacionByEmail(email);

        _invitacionService.InvitacionAceptada(invitacion, contrasena);


        return Ok();
    }

}
