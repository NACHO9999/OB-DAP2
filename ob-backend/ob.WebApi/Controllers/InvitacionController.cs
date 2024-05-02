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



    [HttpPost("{email}/{contrasena}")]
    
    public IActionResult InvitacionAccepted([FromRoute]string email, [FromRoute] string contrasena )
    {
        
        Invitacion invitacion = _invitacionService.GetInvitacionByEmail(email);

        _invitacionService.InvitacionAceptada(invitacion, contrasena);


        return Ok();
    }

}
