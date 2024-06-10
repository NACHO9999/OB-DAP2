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
public class EdificioServiceTests
{
    private Mock<IGenericRepository<Edificio>> _mockRepository;
    private Mock<IConstructoraService> _mockConstructoraService;
    private Mock<IDeptoService> _mockDeptoService;
    private IEdificioService _edificioService;

    [TestInitialize]
    public void Initialize()
    {
        _mockRepository = new Mock<IGenericRepository<Edificio>>();
        _mockConstructoraService = new Mock<IConstructoraService>();
        _mockDeptoService = new Mock<IDeptoService>();
        _edificioService = new EdificioService(_mockRepository.Object, _mockConstructoraService.Object, _mockDeptoService.Object);
    }

    [TestMethod]
    public void GetAllEdificios_ReturnsListOfEdificios()
    {
        // Arrange
        var edificios = new List<Edificio>
        {
            new Edificio("Edificio A", "Calle 1", "Zona 1", new Constructora("Constructora 1"), 10000M, new List<Depto>()),
            new Edificio("Edificio B", "Calle 2", "Zona 2", new Constructora("Constructora 2"), 20000M, new List<Depto>())
        };

        _mockRepository.Setup(repo => repo.GetAll(It.IsAny<Func<Edificio, bool>>(), It.IsAny<List<string>>()))
            .Returns((Func<Edificio, bool> predicate, List<string> includes) => edificios.Where(predicate).ToList());

        // Act
        var result = _edificioService.GetAllEdificios();

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("Edificio A", result[0].Nombre);
    }

    [TestMethod]
    public void CrearEdificio_SavesEdificio()
    {
        // Arrange
        var constructora = new Constructora("Constructora Principal");
        var deptos = new List<Depto>();
        var newEdificio = new Edificio("Edificio Central", "Calle Principal 123", "Centro", constructora, 50000M, deptos);

        _mockConstructoraService.Setup(s => s.GetConstructoraByNombre(It.IsAny<string>())).Returns(constructora);
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Edificio, bool>>>(), null)).Returns((Edificio)null);

        // Act
        _edificioService.CrearEdificio(newEdificio);

        // Assert
        _mockRepository.Verify(repo => repo.Insert(It.IsAny<Edificio>()), Times.Once);
        _mockRepository.Verify(repo => repo.Save(), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception), "El edificio ya existe")]
    public void CrearEdificio_AlreadyExists_ThrowsException()
    {
        // Arrange
        var constructora = new Constructora("Constructora Principal");
        var deptos = new List<Depto>();
        var newEdificio = new Edificio("Edificio Central", "Calle Principal 123", "Centro", constructora, 50000M, deptos);

        _mockConstructoraService.Setup(s => s.GetConstructoraByNombre(It.IsAny<string>())).Returns(constructora);
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Edificio, bool>>>(), It.IsAny<List<string>>())).Returns(newEdificio);

        // Act
        _edificioService.CrearEdificio(newEdificio);
    }

    [TestMethod]
    public void CrearEdificioConDatos_CreatesAndSavesEdificio()
    {
        // Arrange
        var constructora = new Constructora("Constructora Principal");
        var deptos = new List<Depto>();
        _mockConstructoraService.Setup(s => s.GetConstructoraByNombre(It.IsAny<string>())).Returns(constructora);
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Edificio, bool>>>(), null)).Returns((Edificio)null);

        // Act
        _edificioService.CrearEdificioConDatos("Edificio Central", "Calle Principal 123", "Centro", "Constructora Principal", 50000M, deptos);

        // Assert
        _mockRepository.Verify(repo => repo.Insert(It.IsAny<Edificio>()), Times.Once);
        _mockRepository.Verify(repo => repo.Save(), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException), "No se encontró la empresa constructora.")]
    public void CrearEdificioConDatos_ConstructoraNotFound_ThrowsException()
    {
        // Arrange
        var deptos = new List<Depto>();
        _mockConstructoraService.Setup(s => s.GetConstructoraByNombre(It.IsAny<string>())).Returns((Constructora)null);

        // Act
        _edificioService.CrearEdificioConDatos("Edificio Central", "Calle Principal 123", "Centro", "Constructora Principal", 50000M, deptos);
    }

    [TestMethod]
    public void EditarEdificio_UpdatesEdificio()
    {
        // Arrange
        var constructora = new Constructora("Constructora Principal");
        var deptos = new List<Depto>();
        var existingEdificio = new Edificio("Edificio Central", "Calle Principal 123", "Centro", constructora, 50000M, deptos);

        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Edificio, bool>>>(), It.IsAny<List<string>>())).Returns(existingEdificio);
        _mockConstructoraService.Setup(s => s.GetConstructoraByNombre(It.IsAny<string>())).Returns(constructora);

        // Act
        _edificioService.EditarEdificio(existingEdificio);

        // Assert
        _mockRepository.Verify(repo => repo.Update(It.IsAny<Edificio>()), Times.Once);
        _mockRepository.Verify(repo => repo.Save(), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException), "No se encontró el edificio.")]
    public void EditarEdificio_NotFound_ThrowsException()
    {
        // Arrange
        var constructora = new Constructora("Constructora Principal");
        var deptos = new List<Depto>();
        var newEdificio = new Edificio("Edificio Central", "Calle Principal 123", "Centro", constructora, 50000M, deptos);

        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Edificio, bool>>>(), It.IsAny<List<string>>())).Returns((Edificio)null);

        // Act
        _edificioService.EditarEdificio(newEdificio);
    }

    [TestMethod]
    public void BorrarEdificio_DeletesEdificio()
    {
        // Arrange
        var constructora = new Constructora("Constructora Principal");
        var deptos = new List<Depto>();
        var edificioToDelete = new Edificio("Edificio Central", "Calle Principal 123", "Centro", constructora, 50000M, deptos);

        // Act
        _edificioService.BorrarEdificio(edificioToDelete);

        // Assert
        _mockRepository.Verify(repo => repo.Delete(It.IsAny<Edificio>()), Times.Once);
        _mockRepository.Verify(repo => repo.Save(), Times.Once);
    }

    [TestMethod]
    public void GetEdificioByNombreYDireccion_ReturnsEdificio()
    {
        // Arrange
        var constructora = new Constructora("Constructora Principal");
        var deptos = new List<Depto>();
        var existingEdificio = new Edificio("Edificio Central", "Calle Principal 123", "Centro", constructora, 50000M, deptos);

        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Edificio, bool>>>(), It.IsAny<List<string>>())).Returns(existingEdificio);

        // Act
        var result = _edificioService.GetEdificioByNombreYDireccion("Edificio Central", "Calle Principal 123");

        // Assert
        Assert.AreEqual(existingEdificio, result);
    }

    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException), "No se encontró el edificio.")]
    public void GetEdificioByNombreYDireccion_NotFound_ThrowsException()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Edificio, bool>>>(), It.IsAny<List<string>>())).Returns((Edificio)null);

        // Act
        _edificioService.GetEdificioByNombreYDireccion("NonExisting", "NonExisting");
    }

    [TestMethod]
    public void EdificioExists_ReturnsTrue()
    {
        // Arrange
        var constructora = new Constructora("Constructora Principal");
        var deptos = new List<Depto>();
        var existingEdificio = new Edificio("Edificio Central", "Calle Principal 123", "Centro", constructora, 50000M, deptos);

        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Edificio, bool>>>(), null)).Returns(existingEdificio);

        // Act
        var result = _edificioService.EdificioExists("Edificio Central", "Calle Principal 123");

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void EdificioExists_ReturnsFalse()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Edificio, bool>>>(), null)).Returns((Edificio)null);

        // Act
        var result = _edificioService.EdificioExists("NonExisting", "NonExisting");

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void AgregarDepto_AddsDeptoToEdificio()
    {
        // Arrange
        var constructora = new Constructora("Constructora Principal");
        var deptos = new List<Depto>();
        var existingEdificio = new Edificio("Edificio Central", "Calle Principal 123", "Centro", constructora, 50000M, deptos);
        var newDepto = new Depto(1, 101, null, 2, 1, true, "Edificio Central", "Calle Principal 123");

        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Edificio, bool>>>(), It.IsAny<List<string>>())).Returns(existingEdificio);
        _mockDeptoService.Setup(s => s.CrearDepto(It.IsAny<Depto>()));

        // Act
        _edificioService.AgregarDepto(existingEdificio, newDepto);

        // Assert
        Assert.AreEqual(1, existingEdificio.Deptos.Count);
        Assert.AreEqual(newDepto, existingEdificio.Deptos.First());
        _mockDeptoService.Verify(s => s.CrearDepto(newDepto), Times.Once);
    }

    [TestMethod]
    public void CrearEdificio_SinConstructora_ThrowsException()
    {
        // Arrange
        var constructora = new Constructora("Constructora Principal");
        var deptos = new List<Depto>();
        var newEdificio = new Edificio("Edificio Central", "Calle Principal 123", "Centro", constructora, 50000M, deptos);

        _mockConstructoraService.Setup(s => s.GetConstructoraByNombre(It.IsAny<string>())).Returns((Constructora)null);

        // Act & Assert
        Assert.ThrowsException<KeyNotFoundException>(() => _edificioService.CrearEdificio(newEdificio));
    }

    [TestMethod]
    public void EditarEdificio_ActualizaDeptos()
    {
        // Arrange
        var constructora = new Constructora("Constructora Principal");
        var deptos = new List<Depto>
        {
            new Depto(1, 101, null, 2, 1, true, "Edificio Central", "Calle Principal 123"),
            new Depto(2, 102, null, 3, 2, false, "Edificio Central", "Calle Principal 123")
        };
        var existingEdificio = new Edificio("Edificio Central", "Calle Principal 123", "Centro", constructora, 50000M, deptos);

        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Edificio, bool>>>(), It.IsAny<List<string>>())).Returns(existingEdificio);
        _mockConstructoraService.Setup(s => s.GetConstructoraByNombre(It.IsAny<string>())).Returns(constructora);

        var updatedEdificio = new Edificio("Edificio Central", "Calle Principal 123", "Centro", constructora, 60000M, new List<Depto>
        {
            new Depto(1, 101, null, 2, 2, true, "Edificio Central", "Calle Principal 123")
        });

        // Act
        _edificioService.EditarEdificio(updatedEdificio);

        // Assert
        _mockRepository.Verify(repo => repo.Update(existingEdificio), Times.Once);
        _mockRepository.Verify(repo => repo.Save(), Times.Once);
        Assert.AreEqual(1, existingEdificio.Deptos.Count);
        Assert.AreEqual(2, existingEdificio.Deptos.First().CantidadBanos);
    }

    [TestMethod]
    public void AgregarDepto_DeptoExiste_ActualizaDepto()
    {
        // Arrange
        var constructora = new Constructora("Constructora Principal");
        var deptos = new List<Depto>
        {
            new Depto(1, 101, null, 2, 1, true, "Edificio Central", "Calle Principal 123")
        };
        var existingEdificio = new Edificio("Edificio Central", "Calle Principal 123", "Centro", constructora, 50000M, deptos);

        var existingDepto = new Depto(1, 101, null, 2, 1, true, "Edificio Central", "Calle Principal 123");
        _mockDeptoService.Setup(s => s.GetDepto(101, "Edificio Central", "Calle Principal 123")).Returns(existingDepto);

        var newDepto = new Depto(1, 101, null, 3, 2, true, "Edificio Central", "Calle Principal 123");

        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Edificio, bool>>>(), It.IsAny<List<string>>())).Returns(existingEdificio);

        // Act
        _edificioService.AgregarDepto(existingEdificio, newDepto);

        // Assert
        Assert.AreEqual(1, existingEdificio.Deptos.Count);
        Assert.AreEqual(3, existingEdificio.Deptos.First().CantidadCuartos);
        _mockDeptoService.Verify(s => s.EditarDepto(existingDepto), Times.Once);
    }

}
