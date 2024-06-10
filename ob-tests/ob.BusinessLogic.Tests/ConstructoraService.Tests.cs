using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ob.BusinessLogic;
using ob.Domain;
using ob.Exceptions.BusinessLogicExceptions;
using ob.IBusinessLogic;
using ob.IDataAccess;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

[TestClass]
public class ConstructoraServiceTests
{
    private Mock<IGenericRepository<Constructora>> _mockRepository;
    private IConstructoraService _constructoraService;

    [TestInitialize]
    public void Initialize()
    {
        _mockRepository = new Mock<IGenericRepository<Constructora>>();
        _constructoraService = new ConstructoraService(_mockRepository.Object);
    }

    [TestMethod]
    public void CrearConstructora_ConstructoraNoExiste_ConstructoraCreada()
    {
        // Arrange
        var nuevaConstructora = new Constructora("NuevaConstructora");
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Constructora, bool>>>(), null)).Returns((Constructora)null);
        _mockRepository.Setup(repo => repo.Insert(It.IsAny<Constructora>()));
        _mockRepository.Setup(repo => repo.Save());

        // Act
        _constructoraService.CrearConstructora("NuevaConstructora");

        // Assert
        _mockRepository.Verify(repo => repo.Insert(It.Is<Constructora>(c => c.Nombre == "NuevaConstructora")), Times.Once);
        _mockRepository.Verify(repo => repo.Save(), Times.Once);
    }

    [TestMethod]
    public void CrearConstructora_ConstructoraYaExiste_LanzaAlreadyExistsException()
    {
        // Arrange
        var existingConstructora = new Constructora("ConstructoraExistente");
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Constructora, bool>>>(), null)).Returns(existingConstructora);

        // Act & Assert
        Assert.ThrowsException<AlreadyExistsException>(() => _constructoraService.CrearConstructora("ConstructoraExistente"));
    }

    [TestMethod]
    public void GetConstructoraByNombre_ConstructoraExiste_ReturnsConstructora()
    {
        // Arrange
        var constructoraName = "Test";
        var existingConstructora = new Constructora(constructoraName);
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Constructora, bool>>>(), null)).Returns(existingConstructora);

        // Act
        var result = _constructoraService.GetConstructoraByNombre(constructoraName);

        // Assert
        Assert.AreEqual(existingConstructora, result);
    }

    [TestMethod]
    public void GetConstructoraByNombre_ConstructoraNoExiste_LanzaKeyNotFoundException()
    {
        // Arrange
        var nonExistingNombre = "NonExistingConstructora";
        _mockRepository.Setup(repo => repo.Get(c => c.Nombre == nonExistingNombre, null)).Returns((Constructora)null);

        // Act & Assert
        Assert.ThrowsException<KeyNotFoundException>(() => _constructoraService.GetConstructoraByNombre(nonExistingNombre));
    }

    [TestMethod]
    public void GetAllConstructoras_ReturnsAllConstructoras()
    {
        // Arrange
        var constructoras = new List<Constructora> { new Constructora("Constructora1"), new Constructora("Constructora2") };
        _mockRepository.Setup(repo => repo.GetAll<Constructora>()).Returns(constructoras);

        // Act
        var result = _constructoraService.GetAllConstructoras();

        // Assert
        CollectionAssert.AreEqual(constructoras, (System.Collections.ICollection)result);
    }

    [TestMethod]
    public void GetAllConstructoras_NoConstructoras_ReturnsEmptyList()
    {
        // Arrange
        var constructoras = new List<Constructora>();
        _mockRepository.Setup(repo => repo.GetAll<Constructora>()).Returns(constructoras);

        // Act
        var result = _constructoraService.GetAllConstructoras();

        // Assert
        Assert.AreEqual(0, result.Count());
    }
    
    [TestMethod]
    public void ConstructoraExists_ConstructoraExiste_ReturnsTrue()
    {
        // Arrange
        var nombre = "TestConstructora";
        var existingConstructora = new Constructora(nombre);
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Constructora, bool>>>(), null)).Returns(existingConstructora);

        // Act
        var result = _constructoraService.ConstructoraExists(nombre);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void ConstructoraExists_ConstructoraNoExiste_ReturnsFalse()
    {
        // Arrange
        var nombre = "NonExistingConstructora";
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Constructora, bool>>>(), null)).Returns((Constructora)null);

        // Act
        var result = _constructoraService.ConstructoraExists(nombre);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void EditarConstructora_ConstructoraExiste_ConstructoraEditada()
    {
        // Arrange
        var existingConstructora = new Constructora("ExistingConstructora");
        var updatedConstructora = new Constructora("UpdatedConstructora");
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Constructora, bool>>>(), null)).Returns(existingConstructora);
        _mockRepository.Setup(repo => repo.Update(It.IsAny<Constructora>()));
        _mockRepository.Setup(repo => repo.Save());

        // Act
        _constructoraService.EditarConstructora(updatedConstructora);

        // Assert
        _mockRepository.Verify(repo => repo.Update(existingConstructora), Times.Once);
        _mockRepository.Verify(repo => repo.Save(), Times.Once);
        Assert.AreEqual("UpdatedConstructora", existingConstructora.Nombre);
    }

    [TestMethod]
    public void EditarConstructora_ConstructoraNoExiste_LanzaKeyNotFoundException()
    {
        // Arrange
        var updatedConstructora = new Constructora("UpdatedConstructora");
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Constructora, bool>>>(), null)).Returns((Constructora)null);

        // Act & Assert
        Assert.ThrowsException<KeyNotFoundException>(() => _constructoraService.EditarConstructora(updatedConstructora));
    }
}
