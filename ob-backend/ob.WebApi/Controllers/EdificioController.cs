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
public class EdificioController : ControllerBase
{
    private IEdificioService _edificioService;

    public EdificioController(IEdificioService edificiosService)
    {
        _edificioService = edificiosService;
    }


    [HttpGet("{nombre}/{direccion}")]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
    [ServiceFilter(typeof(AuthenticationFilter))]
    public IActionResult GetEdificioByEmail([FromRoute]  string nombre, [FromRoute] string direccion)
    {
        EdificioDTO edificio = new EdificioDTO(_edificioService.GetEdificioByNombreYDireccion( nombre, direccion));
        return Ok(edificio);
    }

    [HttpDelete("{Nombre}/{Direccion}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
    public IActionResult DeleteEdificioByEmail([FromRoute] string nombre, [FromRoute] string direccion)
    {
        _edificioService.BorrarEdificio(_edificioService.GetEdificioByNombreYDireccion( nombre, direccion));
        return Ok();
    }


    [HttpPost]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
    public IActionResult InsertEdificio([FromBody] EdificioDTO newEdificio)
    {
        _edificioService.CrearEdificio(newEdificio.ToEntity());
        return Ok();
    }

    [HttpPut("{nombre}/{direccion}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
    public IActionResult PutEdificio([FromRoute] string nombre, [FromRoute] string direccion, [FromBody] EdificioDTO updatedEdificio)
    {
        Edificio edificio = _edificioService.GetEdificioByNombreYDireccion(nombre, direccion);
        
        edificio.Deptos = updatedEdificio.Deptos.Select(depto => depto.ToEntity()).ToList(); ;
        edificio.GastosComunes = updatedEdificio.GastosComunes;
        _edificioService.EditarEdificio(edificio);
        EdificioDTO edificioDTO = new EdificioDTO(edificio);
        return Ok(edificioDTO);
    }

}
