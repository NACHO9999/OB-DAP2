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
public class CategoriaController : ControllerBase
{
    private ICategoriaService _categoriaService;

    public CategoriaController(ICategoriaService categoriasService)
    {
        _categoriaService = categoriasService;
    }

    [HttpGet]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
    public IActionResult GetCatgorias()
    {
        return Ok(_categoriaService.GetAllCategorias().Select(c => new CategoriaDTO(c)).ToList());
    }

    [HttpGet("{nombre}")]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
    public IActionResult GetCategoriaByNombre([FromRoute] string nombre)
    {
        CategoriaDTO cat = new CategoriaDTO(_categoriaService.GetCategoriaByNombre(nombre));
        return Ok(cat);
    }


    [HttpPost]
    [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Administrador) })]
    public IActionResult InsertCategoria([FromBody] CategoriaDTO newCategoria)
    {
        _categoriaService.CrearCategoria(new Categoria(newCategoria.Nombre));
        return Ok();
    }

}