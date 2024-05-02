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
public class DeptoController : ControllerBase
{
    private IDeptoService _deptoService;

    public DeptoController(IDeptoService deptosService)
    {
        _deptoService = deptosService;
    }


    [HttpGet("{numero}/{edificioNombre}/{edificioDireccion}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
    public IActionResult GetDeptoByEmail([FromRoute] int numero, [FromRoute] string edificioNombre, [FromRoute] string edificioDireccion)
    {
        DeptoDTO depto = new DeptoDTO(_deptoService.GetDepto(numero,edificioNombre,edificioDireccion));
        return Ok(depto);
    }

    [HttpDelete("{numero}/{edificioNombre}/{edificioDireccion}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
    public IActionResult DeleteDeptoByEmail([FromRoute] int numero, [FromRoute] string edificioNombre, [FromRoute] string edificioDireccion)
    {
        _deptoService.BorrarDepto(_deptoService.GetDepto(numero, edificioNombre, edificioDireccion));
        return Ok();
    }


    [HttpPost]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
    public IActionResult InsertDepto([FromBody] DeptoDTO newDepto)
    {
        _deptoService.CrearDepto(new Depto(newDepto.Edificio.ToEntity(), newDepto.Piso, newDepto.Numero, new Dueno(newDepto.Dueno.Nombre,newDepto.Dueno.Apellido, newDepto.Dueno.Email), newDepto.CantidadCuartos, newDepto.CantidadBanos,newDepto.ConTerraza));
        return Ok();
    }
   

}