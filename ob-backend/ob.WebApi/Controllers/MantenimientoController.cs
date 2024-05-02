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
public class MantenimientoController : ControllerBase
{
    private IMantenimientoService _mantenimientoService;

    public MantenimientoController(IMantenimientoService mantenimientosService)
    {
        _mantenimientoService = mantenimientosService;
    }
    [HttpPost("atender/{solicitudId}")]
    public IActionResult AtenderSolicitud([FromRoute] Guid solicitudId)
    {
       
            _mantenimientoService.AtenderSolicitud(solicitudId);
            return Ok();
        
    }

    [HttpPost("completar/{solicitudId}")]
    public IActionResult CompletarSolicitud([FromRoute] Guid solicitudId)
    {
        
            _mantenimientoService.CompletarSolicitud(solicitudId);
            return Ok();
       
    }
}