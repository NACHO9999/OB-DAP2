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
public class SolicitudController : ControllerBase
{
    private ISolicitudService _solicitudService;

    public SolicitudController(ISolicitudService solicitudsService)
    {
        _solicitudService = solicitudsService;
    }



    [HttpPost]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
    public IActionResult InsertSolicitud([FromBody] SolicitudDTO newSolicitud)
    {
        _solicitudService.CrearSolicitud(newSolicitud.ToEntity());
        return Ok();
    }



}
