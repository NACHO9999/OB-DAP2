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
    public void CrearConstructora()
    {
        // Arrange
        var existingConstructora = new Constructora("Test");

        // Setup mock repository to return true, indicating existing constructora
        _mockRepository.Setup(repo => repo.Get(
            It.IsAny<Expression<Func<Constructora, bool>>>(), null
        )).Returns(existingConstructora);
    }

    [TestMethod]
    public void GetConstructoraByNombre_ConstructoraExists_ReturnsConstructora()
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
    public void GetConstructoraByNombre_ConstructoraDoesNotExist_ThrowsResourceNotFoundException()
    {
        // Arrange
        var nonExistingNombre = "NonExistingConstructora";
        _mockRepository.Setup(repo => repo.Get(c => c.Nombre == nonExistingNombre, null)).Returns((Constructora)null);

        // Act & Assert
        Assert.ThrowsException<KeyNotFoundException>(() => _constructoraService.GetConstructoraByNombre(nonExistingNombre));
    }

}