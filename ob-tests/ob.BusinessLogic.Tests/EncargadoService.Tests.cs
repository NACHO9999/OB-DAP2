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

[TestClass]
public class EncargadoServiceTests
{
    private Mock<IUsuarioRepository> _mockRepository;
    private Mock<IEdificioService> _mockEdificioService;
    private Mock<IMantenimientoService> _mockMantenimientoService;
    private Mock<ISolicitudService> _mockSolicitudService;
    private IEncargadoService _encargadoService;

    [TestInitialize]
    public void Initialize()
    {
        _mockRepository = new Mock<IUsuarioRepository>();
        _mockEdificioService = new Mock<IEdificioService>();
        _mockMantenimientoService = new Mock<IMantenimientoService>();
        _mockSolicitudService = new Mock<ISolicitudService>();
        _encargadoService = new EncargadoService(_mockRepository.Object, _mockEdificioService.Object, _mockMantenimientoService.Object, _mockSolicitudService.Object);
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
    public void GetEncargadoByEmail_EmailDoesNotExist_ThrowsResourceNotFoundException()
    {
        // Arrange
        var email = "missing@example.com";
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns((Encargado)null);

        // Act & Assert
        Assert.ThrowsException<ResourceNotFoundException>(() => _encargadoService.GetEncargadoByEmail(email));
    }

    [TestMethod]
    public void CrearEdificio_ValidEncargadoAndEdificio_CreatesAndAddsEdificio()
    {
        // Arrange
        var encargado = new Encargado("test@example.com", "Test User", "Password123");
        encargado.Edificios = new List<Edificio>();
        var edificio = new Edificio("Edificio Central", "123 Main St", "Centro", new Constructora("Construcciones S.A."), 20000, new List<Depto>());
        _mockEdificioService.Setup(s => s.CrearEdificio(edificio)).Verifiable();

        // Act
        _encargadoService.CrearEdificio(encargado.Email, edificio);

        // Assert
        _mockEdificioService.Verify(s => s.CrearEdificio(edificio), Times.Once);
        Assert.IsTrue(encargado.Edificios.Contains(edificio));
        _mockRepository.Verify(repo => repo.Save(), Times.Once);
    }

   
   
}
