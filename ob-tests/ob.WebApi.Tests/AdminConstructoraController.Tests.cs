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
public class AdminConstructoraControllerTests
{
    private Mock<ISessionService> _sessionServiceMock;
    private Mock<IAdminConstructoraService> _adminConstructoraServiceMock;
    private Mock<IEdificioService> _edificioServiceMock;
    private Mock<IConstructoraService> _constructoraServiceMock;
    private Mock<IImporterLogic> _importerLogicServiceMock;
    private AdminConstructoraController _controller;
    private Constructora constructora = new Constructora("Constructora1");
    private AdminConstructora admin;
    private Edificio edificio;

    [TestInitialize]
    public void Setup()
    {
        _sessionServiceMock = new Mock<ISessionService>();
        _constructoraServiceMock  = new Mock<IConstructoraService>();
        _edificioServiceMock = new Mock<IEdificioService>();
        _adminConstructoraServiceMock = new Mock<IAdminConstructoraService>();
        _importerLogicServiceMock = new Mock<IImporterLogic>();
        _controller = new AdminConstructoraController(_sessionServiceMock.Object, _adminConstructoraServiceMock.Object, _importerLogicServiceMock.Object);
        Constructora constructora = new Constructora("Constructora1");
        admin = new AdminConstructora("jo", "test@test.com", "Contra1234") { Constructora=constructora};
        edificio = new Edificio("Edificio1", "Direccion1", "ubi1", constructora, 1000, new List<Depto>());
        HttpContext httpContext = new DefaultHttpContext();
        var guid = Guid.NewGuid();
        httpContext.Request.Headers["Authorization"] = guid.ToString();

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext,
        };

        // Setup the GetCurrentUser method to return a mock user
        _sessionServiceMock.Setup(s => s.GetCurrentUser(It.IsAny<Guid>())).Returns(admin);
        _adminConstructoraServiceMock.Setup(a => a.GetAdminConstructoraByEmail(It.IsAny<string>())).Returns(admin);
        _constructoraServiceMock.Setup(c => c.GetConstructoraByNombre(It.IsAny<string>())).Returns(constructora);
        _edificioServiceMock.Setup(e => e.GetEdificioByNombreYDireccion(It.IsAny<string>(), It.IsAny<string>())).Returns(edificio);
    }

        [TestMethod]
        public void BorrarEdificio_ReturnsOk()
        {
            // Act
            var result = _controller.BorrarEdificio("Edificio1", "Direccion1") as OkObjectResult;

            // Assert
            _adminConstructoraServiceMock.Verify(s => s.BorrarEdificio("Edificio1", "Direccion1", "test@test.com"), Times.Once);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual("Edificio borrado exitosamente.", result.Value);
        }

        [TestMethod]
        public void GetEdificio_ReturnsOkWithEdificio()
        {
            // Arrange
            
            _adminConstructoraServiceMock.Setup(s => s.GetEdificioByNombreYDireccion("Edificio1", "Direccion1", "test@test.com")).Returns(edificio);

            // Act
            var result = _controller.GetEdificio("Edificio1", "Direccion1") as OkObjectResult;
            var edificioDTO = result.Value as EdificioDTO;

            // Assert
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(edificio.Nombre, edificioDTO.Nombre);
            Assert.AreEqual(edificio.Direccion, edificioDTO.Direccion);
        }

        [TestMethod]
        public void GetEdificiosPorAdmin_ReturnsOkWithEdificios()
        {
            // Arrange
            var edificios = new List<Edificio> { edificio }; 
            _adminConstructoraServiceMock.Setup(s => s.GetEdificiosPorAdmin("test@test.com")).Returns(edificios);

            // Act
            var result = _controller.GetEdificiosPorAdmin() as OkObjectResult;
            var edificioDTOs = result.Value as List<EdificioDTO>;

            // Assert
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(1, edificioDTOs.Count);
            Assert.AreEqual(edificios[0].Nombre, edificioDTOs[0].Nombre);
            Assert.AreEqual(edificios[0].Direccion, edificioDTOs[0].Direccion);
        }

        [TestMethod]
        public void EditarEdificio_ReturnsOk()
        {
            // Arrange
            var edificioDTO = new EdificioDTO(edificio);

            // Act
            var result = _controller.EditarEdificio(edificioDTO) as OkObjectResult;

            // Assert
            _adminConstructoraServiceMock.Verify(s => s.EditarEdificio(It.IsAny<Edificio>(), "test@test.com"), Times.Once);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual("Edificio editado exitosamente.", result.Value);
        }

        [TestMethod]
        public void CrearEdificio_ReturnsOk()
        {
            // Arrange
            var edificioDTO = new EdificioCreateDTO();
            

            // Act
            var result = _controller.CrearEdificio(edificioDTO) as OkObjectResult;

            // Assert
            _adminConstructoraServiceMock.Verify(s => s.CrearEdificio(It.IsAny<Edificio>(), "test@test.com"), Times.Once);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual("Edificio creado exitosamente.", result.Value);
        }

        [TestMethod]
        public void CrearDepto_ReturnsOk()
        {
            // Arrange
            var depto = new Depto(1, 101, null, 2, 3, true, "Edificio1", "Direccion1");
            var deptoDTO = new DeptoDTO(depto);
            // Act
            var result = _controller.CrearDepto(deptoDTO) as OkObjectResult;

            // Assert
            _adminConstructoraServiceMock.Verify(s => s.CrearDepto("test@test.com", It.IsAny<Depto>()), Times.Once);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual("Depto creado exitosamente.", result.Value);
        }

        [TestMethod]
        public void GetDepto_ReturnsOkWithDepto()
        {
            // Arrange

            var depto = new Depto(1,101,null,2,3,true, "Edificio1", "Direccion1");
            edificio.Deptos.Add(depto);
            _adminConstructoraServiceMock.Setup(s => s.GetDepto(101, "Edificio1", "Direccion1", "test@test.com")).Returns(depto);

            // Act
            var result = _controller.GetDepto("Edificio1", "Direccion1", 101) as OkObjectResult;
            var deptoDTO = result.Value as DeptoDTO;

            // Assert
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(depto.Numero, deptoDTO.Numero);
        }

        [TestMethod]
        public void EditarDepto_ReturnsOk()
        {
            // Arrange
            var depto = new Depto(1, 101, null, 2, 3, true, "Edificio1", "Direccion1");
            var deptoDTO = new DeptoDTO(depto);

            // Act
            var result = _controller.EditarDepto(deptoDTO) as OkObjectResult;

            // Assert
            _adminConstructoraServiceMock.Verify(s => s.EditarDepto("test@test.com", It.IsAny<Depto>()), Times.Once);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual("Depto editado exitosamente.", result.Value);
        }


        [TestMethod]
        public void GetEdificiosConEncargados_ReturnsOkWithEdificios()
        {
            // Arrange
            var edificios = new List<Edificio> { edificio };
            _adminConstructoraServiceMock.Setup(s => s.GetEdificiosConEncargado("test@test.com")).Returns(edificios);

            // Act
            var result = _controller.GetEdificiosConEncargados() as OkObjectResult;
            var edificioDTOs = result.Value as List<EdificioDTO>;

            // Assert
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(1, edificioDTOs.Count);
            Assert.AreEqual(edificios[0].Nombre, edificioDTOs[0].Nombre);
        }

        [TestMethod]
        public void GetEdificiosSinEncargados_ReturnsOkWithEdificios()
        {
            // Arrange
            var edificios = new List<Edificio> { edificio };
            _adminConstructoraServiceMock.Setup(s => s.GetEdificiosSinEncargado("test@test.com")).Returns(edificios);

            // Act
            var result = _controller.GetEdificiosSinEncargados() as OkObjectResult;
            var edificioDTOs = result.Value as List<EdificioDTO>;

            // Assert
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(1, edificioDTOs.Count);
            Assert.AreEqual(edificios[0].Nombre, edificioDTOs[0].Nombre);
        }


        [TestMethod]
        public void FiltrarEdificiosPorNombreEncargado_ReturnsOkWithEdificios()
        {
            // Arrange
            var edificio = new Edificio("Edificio B", "Avenida Siempreviva 742", "Ubicación B", new Constructora("Constructora B"), 1500m, new List<Depto>());
            var encargado = new Encargado("John", "a@a.com", "Contraa1234");
            encargado.Edificios.Add(edificio);
            var edificios = new List<Edificio> { edificio };
            var edificioDTOs = edificios.Select(e => new EdificioDTO(e)).ToList();
            _adminConstructoraServiceMock.Setup(s => s.FiltrarPorNombreDeEncargado("EdificioB", "John")).Returns(edificios);

            // Act
            var result = _controller.FiltrarEdificiosPorNombreEncargado(encargado.Nombre) as OkObjectResult;
            var filteredEdificioDTOs = result?.Value as List<EdificioDTO>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(filteredEdificioDTOs);
            Assert.AreEqual(1, filteredEdificioDTOs.Count);
            Assert.AreEqual(edificios[0].Nombre, filteredEdificioDTOs[0].Nombre);
        }
    }
}