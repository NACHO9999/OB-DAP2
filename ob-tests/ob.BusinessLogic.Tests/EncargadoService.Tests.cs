using Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ob.BusinessLogic;
using ob.Domain;
using ob.Exceptions.BusinessLogicExceptions;
using ob.IBusinessLogic;
using ob.IDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

[TestClass]
public class EncargadoServiceTests
{
    private Mock<IUsuarioRepository> _mockRepository;
    private Mock<IEdificioService> _mockEdificioService;
    private Mock<IMantenimientoService> _mockMantenimientoService;
    private Mock<ISolicitudService> _mockSolicitudService;
    private Mock<IDeptoService> _mockDeptoService;
    private Mock<IDuenoService> _mockDuenoService;
    private IEncargadoService _encargadoService;

    [TestInitialize]
    public void Initialize()
    {
        _mockRepository = new Mock<IUsuarioRepository>();
        _mockEdificioService = new Mock<IEdificioService>();
        _mockMantenimientoService = new Mock<IMantenimientoService>();
        _mockSolicitudService = new Mock<ISolicitudService>();
        _mockDeptoService = new Mock<IDeptoService>();
        _mockDuenoService = new Mock<IDuenoService>();
        _encargadoService = new EncargadoService(_mockRepository.Object, _mockMantenimientoService.Object, _mockSolicitudService.Object, _mockEdificioService.Object, _mockDeptoService.Object, _mockDuenoService.Object);
    }

    [TestMethod]
    public void CrearEncargado_EmailAlreadyExists_ThrowsAlreadyExistsException()
    {
        // Arrange
        var encargado = new Encargado("test@example.com", "Test User", "Password123");
        _mockRepository.Setup(repo => repo.EmailExists(encargado.Email)).Returns(true);

        // Act & Assert
        Assert.ThrowsException<AlreadyExistsException>(() => _encargadoService.CrearEncargado(encargado));
    }

    [TestMethod]
    public void CrearEncargado_ValidEncargado_AddsEncargado()
    {
        // Arrange
        var encargado = new Encargado("new@example.com", "New User", "Password123");
        _mockRepository.Setup(repo => repo.EmailExists(encargado.Email)).Returns(false);
        _mockRepository.Setup(repo => repo.Insert(encargado)).Verifiable();

        // Act
        _encargadoService.CrearEncargado(encargado);

        // Assert
        _mockRepository.Verify(repo => repo.Insert(encargado), Times.Once);
        _mockRepository.Verify(repo => repo.Save(), Times.Once);
    }

    [TestMethod]
    public void GetEncargadoByEmail_ValidEmail_ReturnsEncargado()
    {
        // Arrange
        var email = "test@example.com";
        var expectedEncargado = new Encargado(email, "Test User", "Password123");
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(expectedEncargado);

        // Act
        var result = _encargadoService.GetEncargadoByEmail(email);

        // Assert
        Assert.AreEqual(expectedEncargado, result);
    }

    [TestMethod]
    public void GetEncargadoByEmail_EmailDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var email = "missing@example.com";
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns((Encargado)null);

        // Act & Assert
        Assert.ThrowsException<KeyNotFoundException>(() => _encargadoService.GetEncargadoByEmail(email));
    }

    [TestMethod]
    public void CrearSolicitud_EncargadoNotInChargeOfBuilding_ThrowsInvalidOperationException()
    {
        // Arrange
        var email = "test@example.com";
        var encargado = new Encargado(email, "Test User", "Password123");
        encargado.Edificios = new List<Edificio>();
        var solicitud = new Solicitud("Desc", new Depto(1,101, null,2,2,true, "edNombre","edDireccion"),new Categoria("categoria"), DateTime.Now) ;
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(encargado);

        // Act & Assert
        Assert.ThrowsException<InvalidOperationException>(() => _encargadoService.CrearSolicitud(solicitud, email));
    }

    [TestMethod]
    public void CrearSolicitud_ValidEncargado_CreatesSolicitud()
    {
        // Arrange
        var email = "test@example.com";
        var encargado = new Encargado(email, "Test User", "Password123");
        var depto = new Depto(1, 101, null, 2, 2, true, "edNombre", "edDireccion");
        var edificio = new Edificio("edNombre", "edDireccion", "ubi",new Constructora("Const"),1000, new List<Depto>());
        edificio.Deptos = new List<Depto> { depto };
        encargado.Edificios = new List<Edificio> { edificio };
        var solicitud = new Solicitud("Desc", depto, new Categoria("categoria"), DateTime.Now);
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(encargado);

        // Act
        _encargadoService.CrearSolicitud(solicitud, email);

        // Assert
        _mockSolicitudService.Verify(s => s.CrearSolicitud(solicitud), Times.Once);
    }

    [TestMethod]
    public void AsignarSolicitud_SolicitudOrMantenimientoNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var solicitudId = Guid.NewGuid();
        var email = "mantenimiento@example.com";
        var emailEncargado = "encargado@example.com";
        var encargado = new Encargado(emailEncargado, "Encargado User", "Password123");
        encargado.Edificios = new List<Edificio>();
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(encargado);
        _mockSolicitudService.Setup(s => s.GetSolicitudById(solicitudId)).Returns((Solicitud)null);
        _mockMantenimientoService.Setup(m => m.GetMantenimientoByEmail(email)).Returns((Mantenimiento)null);

        // Act & Assert
        Assert.ThrowsException<KeyNotFoundException>(() => _encargadoService.AsignarSolicitud(solicitudId, email, emailEncargado));
    }

    [TestMethod]
    public void AsignarSolicitud_EncargadoNotInChargeOfBuilding_ThrowsInvalidOperationException()
    {
        // Arrange
        var solicitudId = Guid.NewGuid();
        var email = "mantenimiento@example.com";
        var emailEncargado = "encargado@example.com";
        var encargado = new Encargado(emailEncargado, "Encargado User", "Password123");
        encargado.Edificios = new List<Edificio>();
        var solicitud = new Solicitud("Desc", new Depto(1, 101, null, 2, 2, true, "edNombre", "edDireccion"), new Categoria("categoria"), DateTime.Now);
        var perMan = new Mantenimiento ("rob","sape",email,"jadasd$3Dkas");
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(encargado);
        _mockSolicitudService.Setup(s => s.GetSolicitudById(solicitudId)).Returns(solicitud);
        _mockMantenimientoService.Setup(m => m.GetMantenimientoByEmail(email)).Returns(perMan);

        // Act & Assert
        Assert.ThrowsException<InvalidOperationException>(() => _encargadoService.AsignarSolicitud(solicitudId, email, emailEncargado));
    }

    [TestMethod]
    public void AsignarSolicitud_ValidSolicitudAndEncargado_AssignsSolicitud()
    {
        // Arrange
        var solicitudId = Guid.NewGuid();
        var email = "mantenimiento@example.com";
        var emailEncargado = "encargado@example.com";
        var encargado = new Encargado(emailEncargado, "Encargado User", "Password123");
        var depto = new Depto(1, 101, null, 2, 2, true, "edNombre", "edDireccion");
        var edificio = new Edificio("edNombre","edDireccion", "ubi", new Constructora("con"),100,new List<Depto> { depto });
        
        encargado.Edificios = new List<Edificio> { edificio };
        var solicitud = new Solicitud ("Desc", depto, new Categoria("categoria"), DateTime.Now);
        var perMan = new Mantenimiento("Mar","so",email,"dasas1SSd");
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(encargado);
        _mockSolicitudService.Setup(s => s.GetSolicitudById(solicitudId)).Returns(solicitud);
        _mockMantenimientoService.Setup(m => m.GetMantenimientoByEmail(email)).Returns(perMan);

        // Act
        _encargadoService.AsignarSolicitud(solicitudId, email, emailEncargado);

        // Assert
        Assert.AreEqual(perMan, solicitud.PerMan);
    }

    [TestMethod]
    public void GetSolicitudByEdificio_EncargadoNotInChargeOfBuilding_ThrowsInvalidOperationException()
    {
        // Arrange
        var nombre = "Edificio Central";
        var direccion = "123 Main St";
        var email = "encargado@example.com";
        var encargado = new Encargado(email, "Encargado User", "Password123");
        encargado.Edificios = new List<Edificio>();
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(encargado);
        var edificio = new Edificio(nombre, direccion, "Centro", new Constructora("Construcciones S.A."), 20000, new List<Depto>());
        _mockEdificioService.Setup(e => e.GetEdificioByNombreYDireccion(nombre, direccion)).Returns(edificio);

        // Act & Assert
        Assert.ThrowsException<InvalidOperationException>(() => _encargadoService.GetSolicitudByEdificio(nombre, direccion, email));
    }

    [TestMethod]
    public void GetSolicitudByEdificio_ValidEncargado_ReturnsSolicitudCounts()
    {
        // Arrange
        var nombre = "Edificio Central";
        var direccion = "123 Main St";
        var email = "encargado@example.com";
        var encargado = new Encargado(email, "Encargado User", "Password123");
        var depto = new Depto(1, 101, null, 2, 2, true, nombre, direccion);
        var edificio = new Edificio(nombre, direccion, "Centro", new Constructora("Construcciones S.A."), 20000, new List<Depto> { depto });
        encargado.Edificios = new List<Edificio> { edificio };
        var solicitudes = new List<Solicitud>
        {
            new Solicitud("desc",depto,new Categoria("cat"),DateTime.Now),
            new Solicitud ("desc", depto, new Categoria("cat"), EstadoSolicitud.Atendiendo,DateTime.Now),
            new Solicitud ("desc", depto, new Categoria("cat"), EstadoSolicitud.Cerrado, DateTime.Now),
        };
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(encargado);
        _mockEdificioService.Setup(e => e.GetEdificioByNombreYDireccion(nombre, direccion)).Returns(edificio);
        _mockSolicitudService.Setup(s => s.GetSolicitudesByEdificio(edificio)).Returns(solicitudes);

        // Act
        var result = _encargadoService.GetSolicitudByEdificio(nombre, direccion, email);

        // Assert
        Assert.AreEqual(1, result[0]);
        Assert.AreEqual(1, result[1]);
        Assert.AreEqual(1, result[2]);
    }

    [TestMethod]
    public void GetSolicitudByMantenimiento_ValidMantenimientoAndEncargado_ReturnsSolicitudCounts()
    {
        // Arrange
        var nombre = "Edificio Central";
        var direccion = "123 Main St";
        var email = "mantenimiento@example.com";
        var emailEncargado = "encargado@example.com";
        var encargado = new Encargado(emailEncargado, "Encargado User", "Password123");
        var depto = new Depto(1, 101, null, 2, 2, true, nombre, direccion);
        var edificio = new Edificio(nombre, direccion, "Centro", new Constructora("Construcciones S.A."), 20000, new List<Depto> { depto });
        edificio.Deptos = new List<Depto> { depto };
        encargado.Edificios = new List<Edificio> { edificio };
        var mantenimiento = new Mantenimiento ("Juan","kjasn", "a@a.com", "Hola12231");
        var solicitudes = new List<Solicitud>
        {
            new Solicitud("desc",depto,new Categoria("cat"),DateTime.Now){ PerMan = mantenimiento},
            new Solicitud ("desc", depto, new Categoria("cat"), EstadoSolicitud.Atendiendo,DateTime.Now){ PerMan = mantenimiento},
            new Solicitud ("desc", depto, new Categoria("cat"), EstadoSolicitud.Cerrado, DateTime.Now) { PerMan = mantenimiento },
        };
        _mockMantenimientoService.Setup(m => m.GetMantenimientoByEmail(email)).Returns(mantenimiento);
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(encargado);
        _mockSolicitudService.Setup(s => s.GetSolicitudesByMantenimiento(mantenimiento)).Returns(solicitudes);

        // Act
        var result = _encargadoService.GetSolicitudByMantenimiento(email, emailEncargado);

        // Assert
        Assert.AreEqual(1, result[0]);
        Assert.AreEqual(1, result[1]);
        Assert.AreEqual(1, result[2]);
    }

    [TestMethod]
    public void TiempoPromedioAtencion_NoClosedSolicitudes_ReturnsNull()
    {
        // Arrange
        var nombre = "Edificio Central";
        var direccion = "123 Main St";
        var depto = new Depto(1, 101, null, 2, 2, true, nombre, direccion);
        var mantenimiento = new Mantenimiento("Juan", "kjasn", "a@a.com", "Hola12231");
        var solicitudes = new List<Solicitud>
        {
            new Solicitud("desc",depto,new Categoria("cat"),DateTime.Now){ PerMan = mantenimiento},
            new Solicitud ("desc", depto, new Categoria("cat"), EstadoSolicitud.Atendiendo,DateTime.Now){ PerMan = mantenimiento},
        };
        _mockMantenimientoService.Setup(m => m.GetMantenimientoByEmail("a@a.com")).Returns(mantenimiento);
        _mockSolicitudService.Setup(s => s.GetSolicitudesByMantenimiento(mantenimiento)).Returns(solicitudes);

        // Act
        var result = _encargadoService.TiempoPromedioAtencion("a@a.com");

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void TiempoPromedioAtencion_ClosedSolicitudes_ReturnsAverageTime()
    {
        // Arrange
        var email = "mantenimiento@example.com";
        var mantenimiento = new Mantenimiento ("Juan", "kjasn", "a@a.com", "Hola12231");
        var depto = new Depto(1, 101, null, 2, 2, true, "edNombre", "edDireccion");
        var time = DateTime.Now;
        var solicitudes = new List<Solicitud>
        {
            new Solicitud("desc",depto,new Categoria("cat"),EstadoSolicitud.Cerrado, time){ PerMan = mantenimiento,  FechaFin = time.AddDays(3)},
            new Solicitud ("desc", depto, new Categoria("cat"), EstadoSolicitud.Cerrado,time){ PerMan = mantenimiento, FechaFin = time.AddDays(3)},
        };
        _mockMantenimientoService.Setup(m => m.GetMantenimientoByEmail(email)).Returns(mantenimiento);
        _mockSolicitudService.Setup(s => s.GetSolicitudesByMantenimiento(mantenimiento)).Returns(solicitudes);

        // Act
        var result = _encargadoService.TiempoPromedioAtencion(email);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(TimeSpan.FromDays(3), result);
    }
}