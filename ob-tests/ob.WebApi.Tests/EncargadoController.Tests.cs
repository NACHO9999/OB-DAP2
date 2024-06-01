using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ob.IBusinessLogic;
using ob.WebApi.Controllers;
using ob.WebApi.DTOs;
using ob.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace ob.Tests.WebApi.Controllers
{
    [TestClass]
    public class EncargadoControllerTests
    {
        private Mock<IEncargadoService> _encargadoServiceMock;
        private Mock<ISessionService> _sessionServiceMock;
        private Mock<IMantenimientoService> _mantenimientoServiceMock;
        private Mock<IDuenoService> _duenoServiceMock;
        private Mock<IDeptoService> _deptoServiceMock;
        private EncargadoController _controller;
        private Encargado encargado = new Encargado("Encargado Name", "encargado@example.com", "passwordD1");

        [TestInitialize]
        public void Setup()
        {
            _encargadoServiceMock = new Mock<IEncargadoService>();
            _sessionServiceMock = new Mock<ISessionService>();
            _mantenimientoServiceMock = new Mock<IMantenimientoService>();
            _duenoServiceMock = new Mock<IDuenoService>();
            _deptoServiceMock = new Mock<IDeptoService>();
            _controller = new EncargadoController(_sessionServiceMock.Object, _encargadoServiceMock.Object);
            HttpContext httpContext = new DefaultHttpContext();
            var guid = Guid.NewGuid();
            httpContext.Request.Headers["Authorization"] = guid.ToString();

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext,
            };

            _sessionServiceMock.Setup(s => s.GetCurrentUser(It.IsAny<Guid>()))
                .Returns(encargado);
            _encargadoServiceMock.Setup(s => s.GetEncargadoByEmail(It.IsAny<string>()))
                .Returns(encargado);
        }

        [TestMethod]
        public void GetEncargadoByEmail_ReturnsOk()
        {
            // Arrange
            var email = "test@example.com";
            var encargado = new Encargado("John", email, "password");
            _encargadoServiceMock.Setup(s => s.GetEncargadoByEmail(email)).Returns(encargado);

            // Act
            var result = _controller.GetEncargadoByEmail(email);

            // Assert
            Assert.IsNotNull(result, "Expected non-null result.");
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Expected result to be OkObjectResult.");
            var encargadoDTO = okResult.Value as EncargadoDTO;
            Assert.IsNotNull(encargadoDTO, "Expected value to be EncargadoDTO.");
            Assert.AreEqual(email, encargadoDTO.Email);
        }
        [TestMethod]
        public void CrearMantenimiento_ReturnsOk()
        {
            // Arrange
            var mantenimiento = new UsuarioCreateModel { Nombre = "John", Apellido = "Doe", Email = "john.doe@example.com", Contrasena = "password" };

            // Act
            var result = _controller.CrearMantenimiento(mantenimiento);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Mantenimiento creado exitosamente.", okResult.Value);
            _encargadoServiceMock.Verify(s => s.CrearMantenimiento(It.IsAny<Mantenimiento>()), Times.Once);
        }
        [TestMethod]
        public void CrearSolicitud_ReturnsOk()
        {
            // Arrange
            var solicitud = new Solicitud("desc", new Depto(1, 1, null, 2, 2, true, "ed", "dir"), new Categoria("cat"), DateTime.Now);
            var solicitudDTO = new SolicitudDTO(solicitud);
            var currentUser = _sessionServiceMock.Object.GetCurrentUser(Guid.NewGuid() );

            // Act
            var result = _controller.CrearSolicitud(solicitudDTO);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Solicitud creada exitosamente.", okResult.Value);
            _encargadoServiceMock.Verify(s => s.CrearSolicitud(It.IsAny<Solicitud>(), currentUser.Email), Times.Once);
        }
        [TestMethod]
        public void AsignarSolicitud_ReturnsOk()
        {
            // Arrange
            var solicitudId = Guid.NewGuid();
            var emailMantenimiento = "mantenimiento@example.com";
            var currentUser = _sessionServiceMock.Object.GetCurrentUser(Guid.NewGuid());
            _mantenimientoServiceMock.Setup(s => s.GetMantenimientoByEmail(emailMantenimiento)).Returns(new Mantenimiento("Mantenimiento Name", "Mantenimiento Surname", emailMantenimiento, "passwordD1"));

            // Act
            var result = _controller.AsignarSolicitud(solicitudId, emailMantenimiento);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Solicitud asignada exitosamente.", okResult.Value);
            _encargadoServiceMock.Verify(s => s.AsignarSolicitud(solicitudId, emailMantenimiento, currentUser.Email), Times.Once);
        }
        [TestMethod]
        public void GetSolicitudByEdificio_ReturnsOk()
        {
            // Arrange
            var nombre = "Edificio A";
            var direccion = "123 Main St";
            var currentUser = _sessionServiceMock.Object.GetCurrentUser(Guid.NewGuid());
            var solicitudes = new int[] { 1, 2, 3 };
            _encargadoServiceMock.Setup(s => s.GetSolicitudByEdificio(nombre, direccion, currentUser.Email)).Returns(solicitudes);

            // Act
            var result = _controller.GetSolicitudByEdificio(nombre, direccion);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(solicitudes, okResult.Value);
        }
        [TestMethod]
        public void GetSolicitudByMantenimiento_ReturnsOk()
        {
            // Arrange
            var emailMantenimiento = "mantenimiento@example.com";
            var currentUser = _sessionServiceMock.Object.GetCurrentUser(Guid.NewGuid());
            var solicitudes = new int[] { 1, 2, 3 };
            _encargadoServiceMock.Setup(s => s.GetSolicitudByMantenimiento(emailMantenimiento, currentUser.Email)).Returns(solicitudes);

            // Act
            var result = _controller.GetSolicitudByMantenimiento(emailMantenimiento);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(solicitudes, okResult.Value);
        }
        [TestMethod]
        public void GetTiempoPromedioAtencion_ReturnsOk()
        {
            // Arrange
            var email = "encargado@example.com";
            var tiempoPromedio = TimeSpan.FromHours(5);
            _encargadoServiceMock.Setup(s => s.TiempoPromedioAtencion(email)).Returns(tiempoPromedio);

            // Act
            var result = _controller.GetTiempoPromedioAtencion(email);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(tiempoPromedio, okResult.Value);
        }
        [TestMethod]
        public void GetDueno_ReturnsOk()
        {
            // Arrange
            var email = "dueno@example.com";
            var dueno = new Dueno("Jane", "Doe", email);
            _encargadoServiceMock.Setup(s => s.GetDueno(email)).Returns(dueno);

            // Act
            var result = _controller.GetDueno(email);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var duenoDTO = okResult.Value as DuenoDTO;
            Assert.IsNotNull(duenoDTO);
            Assert.AreEqual(email, duenoDTO.Email);
        }
        [TestMethod]
        public void AsignarDueno_ReturnsOk()
        {
            // Arrange
            var numero = 1;
            var depto = new Depto(1, 1, null, 1, 1, false, "Edificio A", "123 Main St");
            var emailDueno = "dueno@example.com";
            var currentUser = _sessionServiceMock.Object.GetCurrentUser(Guid.NewGuid());
            _duenoServiceMock.Setup(s => s.GetDuenoByEmail(emailDueno)).Returns(new Dueno("Jane", "Doe", emailDueno));
            _deptoServiceMock.Setup(s => s.GetDepto(numero, depto.EdificioNombre, depto.EdificioDireccion)).Returns(depto);


            // Act
            var result = _controller.AsignarDueno(numero, depto.EdificioNombre, depto.EdificioDireccion, emailDueno);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Dueno asignado exitosamente.", okResult.Value);
            _encargadoServiceMock.Verify(s => s.AsignarDueno(numero, depto.EdificioNombre, depto.EdificioDireccion, emailDueno, currentUser.Email), Times.Once);
        }


    }
}