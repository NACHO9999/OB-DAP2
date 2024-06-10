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
public class ConstructoraController : ControllerBase
{
    private IConstructoraService _constructoraService;

    public ConstructoraController(IConstructoraService constructorasService)
    {
        _constructoraService = constructorasService;
    }

    [HttpGet]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
    public IActionResult GetConstructoras()
    {
        return Ok(_constructoraService.GetAllConstructoras().Select(c => new ConstructoraDTO(c)).ToList());
    }

    [HttpGet("{nombre}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
    public IActionResult GetConstructoraByNombre([FromRoute] string nombre)
    {
        ConstructoraDTO constructora = new ConstructoraDTO(_constructoraService.GetConstructoraByNombre(nombre));
        return Ok(constructora);
    }



}