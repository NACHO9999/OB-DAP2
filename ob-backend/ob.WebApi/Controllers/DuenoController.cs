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
public class DuenoController : ControllerBase
{
    private IDuenoService _duenoService;

    public DuenoController(IDuenoService duenosService)
    {
        _duenoService = duenosService;
    }


    [HttpGet("{email}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
    public IActionResult GetDuenoByEmail([FromRoute] string email)
    {
        DuenoDTO dueno = new DuenoDTO(_duenoService.GetDuenoByEmail(email));
        return Ok(dueno);
    }


    [HttpPost]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado), typeof(Administrador) })]
    public IActionResult InsertDueno([FromBody] DuenoDTO newDueno)
    {
        _duenoService.CrearDueno(new Dueno(newDueno.Nombre,newDueno.Apellido, newDueno.Email));
        return Ok();
    }

}