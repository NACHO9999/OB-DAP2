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
public class MantenimientoController : BaseController
{
    private IMantenimientoService _mantenimientoService;

    public MantenimientoController(ISessionService sessionService, IMantenimientoService mantenimientosService) : base(sessionService)
    {
        _mantenimientoService = mantenimientosService;
    }
    [HttpPost("atender/{solicitudId}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Mantenimiento) })]
    public IActionResult AtenderSolicitud([FromRoute] Guid solicitudId)
    {

        _mantenimientoService.AtenderSolicitud(solicitudId, GetCurrentUser().Email);
        return Ok();

    }

    [HttpPost("completar/{solicitudId}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Mantenimiento) })]
    public IActionResult CompletarSolicitud([FromRoute] Guid solicitudId)
    {

        _mantenimientoService.CompletarSolicitud(solicitudId, GetCurrentUser().Email);
        return Ok();

    }
}