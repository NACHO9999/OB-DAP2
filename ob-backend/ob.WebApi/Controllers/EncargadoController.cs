using Microsoft.AspNetCore.Mvc;
using ob.Domain;
using ob.IBusinessLogic;
using ob.WebApi.DTOs;
using System.Collections.Generic;
using ob.Exceptions.BusinessLogicExceptions;
using ob.WebApi.Filters;
using Microsoft.AspNetCore.Mvc.Filters;
using ob.BusinessLogic;

namespace ob.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ExceptionFilter]
    public class EncargadoController : BaseController
    {
        private IEncargadoService _encargadoService;

        public EncargadoController(ISessionService sessionService, IEncargadoService encargadosService) : base(sessionService)
        {
            _encargadoService = encargadosService;
        }

        [HttpGet()]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(AdminConstructora) })]
        public IActionResult GetEncargados()
        {
            try
            {
                var encargados = _encargadoService.GetAllEncargados();
                var retorno = encargados.Select(e => new EncargadoDTO(e)).ToList();
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"Error in GetEncargados: {ex.Message}");
                return StatusCode(500, new { message = "We encountered some issues, try again later" });
            }
        }

        [HttpGet("mantenimiento")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
        public IActionResult GetMantenimientos()
        {
            var mantenimientos = _encargadoService.GetAllMantenimiento();
            var retorno = mantenimientos.Select(m => new UsuarioCreateModel()
            {
                Email = m.Email,
                Apellido = m.Apellido,
                Contrasena = m.Contrasena,
                Nombre = m.Nombre
            }).ToList();
            return Ok(retorno);
        }

        [HttpGet("current-encargado")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
        public IActionResult GetEncargadoByEmail()
        {
            var encargado = _encargadoService.GetEncargadoByEmail(GetCurrentUser().Email);
            EncargadoDTO encargadoDTO = new EncargadoDTO(encargado);
            return Ok(encargadoDTO);
        }

        [HttpPost("mantenimiento")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
        public IActionResult CrearMantenimiento([FromBody] UsuarioCreateModel mantenimiento)
        {
            _encargadoService.CrearMantenimiento(new Mantenimiento(mantenimiento.Nombre, mantenimiento.Apellido, mantenimiento.Email, mantenimiento.Contrasena));
            return Ok(new { message = "Mantenimiento creado exitosamente." });
        }

        [HttpGet("solicitudes")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
        public IActionResult GetSolicitudesSinMantenimiento()
        {
            var usuario = GetCurrentUser();
            var solicitudes = _encargadoService.GetSolicitudesSinMantenimiento(usuario.Email);
            var retorno = solicitudes.Select(s => new SolicitudDTO(s)).ToList();
            return Ok(retorno);
        }

        [HttpPost("solicitud")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
        public IActionResult CrearSolicitud([FromBody] SolicitudDTO solicitud)
        {
            var usuario = GetCurrentUser();
            _encargadoService.CrearSolicitud(solicitud.ToEntity(), usuario.Email);
            return Ok(new { message = "Solicitud creada exitosamente." });
        }

        [HttpPut("asignar/{solicitudId}/{emailMantenimiento}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
        public IActionResult AsignarSolicitud([FromRoute] Guid solicitudId, [FromRoute] string emailMantenimiento)
        {
            var usuario = GetCurrentUser();
            _encargadoService.AsignarSolicitud(solicitudId, emailMantenimiento, usuario.Email);
            return Ok(new { message = "Solicitud asignada exitosamente." });
        }

        [HttpGet("solicitudes/edificio/{nombre}/{direccion}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
        public IActionResult GetSolicitudByEdificio([FromRoute] string nombre, [FromRoute] string direccion)
        {
            int[] result = _encargadoService.GetSolicitudByEdificio(nombre, direccion, GetCurrentUser().Email);
            return Ok(result);
        }

        [HttpGet("solicitudes/mantenimiento/{emailMantenimiento}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
        public IActionResult GetSolicitudByMantenimiento([FromRoute] string emailMantenimiento)
        {
            var usuario = GetCurrentUser();
            int[] result = _encargadoService.GetSolicitudByMantenimiento(emailMantenimiento, usuario.Email);
            return Ok(result);
        }

        [HttpGet("tiempo-promedio-atencion/{email}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
        public IActionResult GetTiempoPromedioAtencion([FromRoute] string email)
        {
            TimeSpan? result = _encargadoService.TiempoPromedioAtencion(email);
            return result != null ? Ok(result) : Ok(new { message = "No completo ninguna solicitud" });
        }

        [HttpGet("Dueno/{email}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
        public IActionResult GetDueno([FromRoute] string email)
        {
            Dueno dueno = _encargadoService.GetDueno(email);
            var result = new DuenoDTO(dueno);
            return Ok(result);
        }

        [HttpPut("asignar-dueno/{numero}/{edNombre}/{edDireccion}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
        public IActionResult AsignarDueno([FromRoute] int numero, [FromRoute] string edNombre, [FromRoute] string edDireccion, [FromBody] DuenoDTO dueno)
        {
            _encargadoService.AsignarDueno(numero, edNombre, edDireccion, new Dueno(dueno.Nombre, dueno.Apellido, dueno.Email), GetCurrentUser().Email);
            return Ok(new { message = "Dueno asignado exitosamente." });
        }

        [HttpPut("desasignar-dueno")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
        public IActionResult DesasignarDueno([FromBody] DeptoDTO depto)
        {
            _encargadoService.DesasignarDueno(depto.Numero, depto.EdificioNombre, depto.EdificioDireccion, GetCurrentUser().Email);
            return Ok(new { message = "Dueno desasignado exitosamente." });
        }
        [HttpGet("solicitudes/encargado")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(RoleNeeded = new Type[] { typeof(Encargado) })]
        public IActionResult GetAllEncargadoSolicitudes()
        {
            var usuario = GetCurrentUser();
            var solicitudes = _encargadoService.GetAllEncargadoSolicitudes(usuario.Email);
            var retorno = solicitudes.Select(s => new SolicitudDTO(s)).ToList();
            return Ok(retorno);
        }
    }
}