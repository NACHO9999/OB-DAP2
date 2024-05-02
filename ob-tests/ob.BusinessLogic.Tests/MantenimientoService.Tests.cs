using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ob.BusinessLogic;
using ob.IBusinessLogic;
using ob.Domain;
using ob.Exceptions.BusinessLogicExceptions;
using ob.IDataAccess;
using System;

[TestClass]
public class MantenimientoServiceTests
{
    private Mock<IUsuarioRepository> _mockRepository;
    private Mock<ISolicitudService> _mockSolicitudService;
    private IMantenimientoService _mantenimientoService;

    [TestInitialize]
    public void Initialize()
    {
        _mockRepository = new Mock<IUsuarioRepository>();
        _mockSolicitudService = new Mock<ISolicitudService>();
        _mantenimientoService = new MantenimientoService(_mockRepository.Object, _mockSolicitudService.Object);
    }

    [TestMethod]
    public void CrearMantenimiento_EmailExists_ThrowsAlreadyExistsException()
    {
        // Arrange
        var mantenimiento = new Mantenimiento { Email = "test@example.com" };
        _mockRepository.Setup(repo => repo.EmailExists(mantenimiento.Email)).Returns(true);

        // Act & Assert
        Assert.ThrowsException<AlreadyExistsException>(() => _mantenimientoService.CrearMantenimiento(mantenimiento));
    }

    [TestMethod]
    public void CrearMantenimiento_ValidMantenimiento_InsertsMantenimiento()
    {
        // Arrange
        var mantenimiento = new Mantenimiento { Email = "new@example.com" };
        _mockRepository.Setup(repo => repo.EmailExists(mantenimiento.Email)).Returns(false);
        _mockRepository.Setup(repo => repo.Insert(mantenimiento)).Verifiable();
        _mockRepository.Setup(repo => repo.Save()).Verifiable();

        // Act
        _mantenimientoService.CrearMantenimiento(mantenimiento);

        // Assert
        _mockRepository.Verify(repo => repo.Insert(mantenimiento), Times.Once());
        _mockRepository.Verify(repo => repo.Save(), Times.Once());
    }

    [TestMethod]
    public void GetMantenimientoByEmail_ValidEmail_ReturnsMantenimiento()
    {
        // Arrange
        var email = "test@example.com";
        var mantenimiento = new Mantenimiento { Email = email };
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Func<Usuario, bool>>())).Returns(mantenimiento);

        // Act
        var result = _mantenimientoService.GetMantenimientoByEmail(email);

        // Assert
        Assert.AreEqual(mantenimiento, result);
    }

    [TestMethod]
    public void GetMantenimientoByEmail_NoMantenimientoFound_ThrowsResourceNotFoundException()
    {
        // Arrange
        var email = "missing@example.com";
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Func<Usuario, bool>>())).Returns((Usuario)null);

        // Act & Assert
        Assert.ThrowsException<ResourceNotFoundException>(() => _mantenimientoService.GetMantenimientoByEmail(email));
    }

    [TestMethod]
    public void AtenderSolicitud_Abierto_CambiaEstado()
    {
        // Arrange
        var solicitud = new Solicitud { Estado = EstadoSolicitud.Abierto };
        _mockSolicitudService.Setup(s => s.EditarSolicitud(solicitud)).Verifiable();

        // Act
        _mantenimientoService.AtenderSolicitud(solicitud);

        // Assert
        Assert.AreEqual(EstadoSolicitud.Atendiendo, solicitud.Estado);
        _mockSolicitudService.Verify(s => s.EditarSolicitud(solicitud), Times.Once());
    }

    [TestMethod]
    public void CompletarSolicitud_Atendiendo_CierraSolicitud()
    {
        // Arrange
        var solicitud = new Solicitud { Estado = EstadoSolicitud.Atendiendo };
        _mockSolicitudService.Setup(s => s.EditarSolicitud(solicitud)).Verifiable();

        // Act
        _mantenimientoService.CompletarSolicitud(solicitud);

        // Assert
        Assert.AreEqual(EstadoSolicitud.Cerrado, solicitud.Estado);
        _mockSolicitudService.Verify(s => s.EditarSolicitud(solicitud), Times.Once());
    }
}
