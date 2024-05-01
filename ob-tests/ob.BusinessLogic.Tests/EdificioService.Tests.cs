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
    public void CrearEdificio()
    {
        // Arrange
        var constructoraMock = new Constructora("Constructora Principal");
        var deptosMock = new List<Depto>();
        var newEdificio = new Edificio("Edificio Central", "Calle Principal 123", "Centro", constructoraMock, 50000M, deptosMock);
        
        _mockRepository.Setup(repo => repo.Get(
            It.IsAny<Expression<Func<Edificio, bool>>>(), null
        )).Returns(newEdificio);

        // Act
        _edificioService.CrearEdificio(newEdificio);

        // Assert
        _mockRepository.Verify(repo => repo.Add(It.IsAny<Edificio>()), Times.Once);
    }

    [TestMethod]
    public void GetEdificioByNombre_ReturnsEdificio()
    {
        // Arrange
        var constructoraMock = new Constructora("Constructora Principal");
        var deptosMock = new List<Depto>();
        var nombreEdificio = "Edificio Central";
        var existingEdificio = new Edificio(nombreEdificio, "Calle Principal 123", "Centro", constructoraMock, 50000M, deptosMock);
        _mockRepository.Setup(repo => repo.Get(e => e.Nombre == nombreEdificio, null)).Returns(existingEdificio);

        // Act
        var result = _edificioService.GetEdificioByNombre(nombreEdificio);

        // Assert
        Assert.AreEqual(existingEdificio, result);
    }

    [TestMethod]
    public void GetEdificioByNombre_EdificioDoesNotExist_ThrowsResourceNotFoundException()
    {
        // Arrange
        var nonExistingNombre = "NonExistingEdificio";
        _mockRepository.Setup(repo => repo.Get(e => e.Nombre == nonExistingNombre, null)).Returns((Edificio)null);

        // Act & Assert
        Assert.ThrowsException<ResourceNotFoundException>(() => _edificioService.GetEdificioByNombre(nonExistingNombre));
    }
}
