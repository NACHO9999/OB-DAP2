using ob.WebApi.DTOs;
using System.Collections.Generic;
using ob.Exceptions.BusinessLogicExceptions;
using ob.WebApi.Filters;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ob.IBusinessLogic;
using ob.Domain;
using Enums;
using ob.BusinessLogic;

namespace ob.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ExceptionFilter]
    public class AdminConstructoraController : BaseController
    {
        private IAdminConstructoraService _adminConstructoraService;

        public AdminConstructoraController(ISessionService sessionService, IAdminConstructoraService adminConstructoraService) : base(sessionService)
        {
            _adminConstructoraService = adminConstructoraService;
        }

        [HttpDelete("borrar-edificio/{nombre}/{direccion}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
        public IActionResult BorrarEdificio([FromRoute] string nombre, [FromRoute] string direccion)
        {
            var usuario = GetCurrentUser();
            _adminConstructoraService.BorrarEdificio(nombre, direccion, usuario.Email);
            return Ok(new { message = "Edificio borrado exitosamente." });
        }

        [HttpGet("get-edificio/{nombre}/{direccion}/{email}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
        public IActionResult GetEdificio([FromRoute] string nombre, [FromRoute] string direccion)
        {
            var usuario = GetCurrentUser();
            EdificioDTO edificio = new EdificioDTO(_adminConstructoraService.GetEdificioByNombreYDireccion(nombre, direccion, usuario.Email));
            return Ok(edificio);
        }

        [HttpGet("get-edificiosporadmin")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
        public IActionResult GetEdificiosPorAdmin()
        {
            var usuario = GetCurrentUser();
            var lista = _adminConstructoraService.GetEdificiosPorAdmin(usuario.Email);
            var retorno = lista.Select(e => new EdificioDTO(e)).ToList();
            return Ok(retorno);
        }

        [HttpPut("editar-edificio")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
        public IActionResult EditarEdificio([FromBody] EdificioDTO request)
        {
            _adminConstructoraService.EditarEdificio(request.ToEntity(), GetCurrentUser().Email);
            return Ok(new { message = "Edificio editado exitosamente." });
        }
        [HttpPut("editar-constructora/{nombre}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
        public IActionResult EditarConstructora([FromRoute] string nombre, [FromBody] ConstructoraDTO constructoraDTO)
        {
            var constructora = _adminConstructoraService.GetConstructora(GetCurrentUser().Email);
            constructora.Nombre = nombre;
            _adminConstructoraService.EditarConstructora(constructora, GetCurrentUser().Email);
            return Ok(new { message = "Constructora editado exitosamente." });
        }

        [HttpPost("Crear-edificio")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
        public IActionResult CrearEdificio([FromBody] EdificioCreateDTO edificioDTO)
        {
            AdminConstructora admin = (AdminConstructora)GetCurrentUser();
            var edificio = new Edificio(edificioDTO.Nombre, edificioDTO.Direccion, edificioDTO.Ubicacion, admin.Constructora, edificioDTO.GastosComunes, edificioDTO.Deptos.Select(depto => depto.ToEntity()).ToList());
            _adminConstructoraService.CrearEdificio(edificio, GetCurrentUser().Email);
            return Ok(new { message = "Edificio creado exitosamente." });
        }

        [HttpPost("Crear-Depto")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
        public IActionResult CrearDepto([FromBody] DeptoDTO deptoDTO)
        {
            _adminConstructoraService.CrearDepto(GetCurrentUser().Email, deptoDTO.ToEntity());
            return Ok(new { message = "Depto creado exitosamente." });
        }

        [HttpGet("get-depto/{numero}/{nombre}/{direccion}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
        public IActionResult GetDepto([FromRoute] string nombre, [FromRoute] string direccion, [FromRoute] int numero)
        {
            var email = GetCurrentUser().Email;
            DeptoDTO depto = new DeptoDTO(_adminConstructoraService.GetDepto(numero, nombre, direccion, GetCurrentUser().Email));
            return Ok(depto);
        }
        [HttpDelete("borrar-depto/{numero}/{nombre}/{direccion}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
        public IActionResult BorrarDepto([FromRoute] string nombre, [FromRoute] string direccion, [FromRoute] int numero)
        {
            _adminConstructoraService.BorrarDepto(GetCurrentUser().Email, numero, nombre, direccion);
            return Ok(new { message = "Depto borrado exitosamente." });
        }

        [HttpPut("editar-depto")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
        public IActionResult EditarDepto([FromBody] DeptoDTO request)
        {
            _adminConstructoraService.EditarDepto(GetCurrentUser().Email, request.ToEntity());
            return Ok(new { message = "Depto editado exitosamente." });
        }

        [HttpGet("get-edificios-con-encargados")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
        public IActionResult GetEdificiosConEncargados()
        {
            var lista = _adminConstructoraService.GetEdificiosConEncargado(GetCurrentUser().Email);
            var retorno = lista.Select(e => new EdificioDTO(e)).ToList();
            return Ok(retorno);
        }

        [HttpGet("get-edificios-sin-encargados")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
        public IActionResult GetEdificiosSinEncargados()
        {
            var lista = _adminConstructoraService.GetEdificiosSinEncargado(GetCurrentUser().Email);
            var retorno = lista.Select(e => new EdificioDTO(e)).ToList();
            return Ok(retorno);
        }

       

        [HttpGet("filtrar-por-nombre-encargado/{nombre}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
        public IActionResult FiltrarEdificiosPorNombreEncargado( [FromRoute] string nombre)
        {
            var filtro = _adminConstructoraService.FiltrarPorNombreDeEncargado(GetCurrentUser().Email, nombre);
            var retorno = filtro.Select(e => new EdificioDTO(e)).ToList();
            return Ok(retorno);
        }

        [HttpPut("asignar-encargado/{emailEncargado}/{edNombre}/{edDirecccion}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
        public IActionResult AsignarEncargado([FromRoute] string emailEncargado, [FromRoute] string edNombre, [FromRoute] string edDirecccion)
        {
            _adminConstructoraService.AsignarEncargado(GetCurrentUser().Email, emailEncargado, edNombre, edDirecccion);
            return Ok(new { message = "Encargado asignado exitosamente." });
        }

        [HttpPut("desasignar-encargado/{edNombre}/{edDirecccion}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
        public IActionResult DesasignarEncargado([FromRoute] string edNombre, [FromRoute] string edDirecccion)
        {
            _adminConstructoraService.DesasignarEncargado(GetCurrentUser().Email, edNombre, edDirecccion);
            return Ok(new { message = "Encargado desasignado exitosamente." });
        }

        [HttpPut("elegir-constructora/{nombre}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
        public IActionResult ElegirConstructora([FromRoute] string nombre)
        {
            _adminConstructoraService.ElegirConstructora(GetCurrentUser().Email, nombre);
            return Ok(new { message = "Constructora elegida exitosamente." });
        }

        [HttpGet("tiene-constructora")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
        public IActionResult TieneConstructora()
        {
            var tiene = _adminConstructoraService.TieneConstructora(GetCurrentUser().Email);
            return Ok(tiene);
        }

        [HttpGet("get-constructoras")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
        public IActionResult GetConstructoras()
        {
            var lista = _adminConstructoraService.GetConstructoras();
            var retorno = lista.Select(c => new ConstructoraDTO(c)).ToList();
            return Ok(retorno);
        }

        [HttpPost("crear-constructora/{nombre}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
        public IActionResult CrearConstructora([FromRoute] string nombre)
        {
            _adminConstructoraService.CrearConstructora(nombre, GetCurrentUser().Email);
            return Ok(new { message = "Constructora creada exitosamente." });
        }
    }
}
