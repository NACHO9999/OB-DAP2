using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ob.BusinessLogic;
using ob.Domain;
using ob.Exceptions.BusinessLogicExceptions;
using ob.IBusinessLogic;
using ob.IDataAccess;
using System;
using System.Linq.Expressions;

[TestClass]
public class DuenoServiceTests
{
    private Mock<IGenericRepository<Dueno>> _mockRepository;
    private IDuenoService _duenoService;

    [TestInitialize]
    public void Initialize()
    {
        _mockRepository = new Mock<IGenericRepository<Dueno>>();
        _duenoService = new DuenoService(_mockRepository.Object);
    }

    [TestMethod]
    public void CrearDueno_DuenoDoesNotExist_InsertsDuenoAndSaves()
    {
        // Arrange
        var newDueno = new Dueno("John", "Doe", "test@example.com");
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Dueno, bool>>>(), null)).Returns((Dueno)null);

        // Act
        _duenoService.CrearDueno(newDueno);

        // Assert
        _mockRepository.Verify(repo => repo.Insert(newDueno), Times.Once);
        _mockRepository.Verify(repo => repo.Save(), Times.Once);
    }

    [TestMethod]
    public void CrearDueno_DuenoExists_ThrowsAlreadyExistsException()
    {
        // Arrange
        var existingDueno = new Dueno("John", "Doe", "test@example.com");
        _mockRepository.Setup(repo => repo.Get(d => d.Email.ToLower() == existingDueno.Email.ToLower(), null)).Returns(existingDueno);

        // Set up the mock repository to return existingDueno when queried by email
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Dueno, bool>>>(), null)).Returns(existingDueno);
        // Act & Assert
        Assert.ThrowsException<AlreadyExistsException>(() => _duenoService.CrearDueno(existingDueno));
    }

    [TestMethod]
    public void BorrarDueno_ValidDueno_DeletesDuenoAndSaves()
    {
        // Arrange
        var existingDueno = new Dueno("John", "Doe", "test@example.com");

        // Act
        _duenoService.BorrarDueno(existingDueno);

        // Assert
        _mockRepository.Verify(repo => repo.Delete(existingDueno), Times.Once);
        _mockRepository.Verify(repo => repo.Save(), Times.Once);
    }

    [TestMethod]
    public void GetDuenoByEmail_DuenoExists_ReturnsDueno()
    {
        // Arrange
        var email = "test@example.com";
        var existingDueno = new Dueno("John", "Doe", email);
        _mockRepository.Setup(repo => repo.Get(d => d.Email.ToLower() == email.ToLower(), null)).Returns(existingDueno);

        // Act
        var result = _duenoService.GetDuenoByEmail(email);

        // Assert
        Assert.AreEqual(existingDueno, result);
    }

    [TestMethod]
    public void DuenoExists_DuenoExists_ReturnsTrue()
    {
        // Arrange
        var email = "test@example.com";
        var existingDueno = new Dueno("John", "Doe", email);
        _mockRepository.Setup(repo => repo.Get(d => d.Email.ToLower() == email.ToLower(), null)).Returns(existingDueno);

        // Act
        var result = _duenoService.DuenoExists(email);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void DuenoExists_DuenoDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var email = "nonexistent@example.com";
        _mockRepository.Setup(repo => repo.Get(d => d.Email.ToLower() == email.ToLower(), null)).Returns((Dueno)null);

        // Act
        var result = _duenoService.DuenoExists(email);

        // Assert
        Assert.IsFalse(result);
    }
}