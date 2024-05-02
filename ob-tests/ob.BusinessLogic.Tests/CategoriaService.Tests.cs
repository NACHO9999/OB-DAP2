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
public class CategoriaServiceTests
{
    private Mock<IGenericRepository<Categoria>> _mockRepository;
    private ICategoriaService _categoriaService;

    [TestInitialize]
    public void Initialize()
    {
        _mockRepository = new Mock<IGenericRepository<Categoria>>();
        _categoriaService = new CategoriaService(_mockRepository.Object);
    }

    [TestMethod]
    public void CrearCategoria()
    {
        // Arrange
        var existingCategoria = new Categoria("Test");

        // Setup mock repository to return true, indicating existing categoria
        _mockRepository.Setup(repo => repo.Get(
            It.IsAny<Expression<Func<Categoria, bool>>>(), null
        )).Returns(existingCategoria);
    }

   
    [TestMethod]
    public void GetCategoriaByNombre_ReturnsCategoria()
    {
        // Arrange
        var catString = "Test";
        var existingCat = new Categoria(catString);
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Categoria, bool>>>(), null)).Returns(existingCat);

        // Act
        var result = _categoriaService.GetCategoriaByNombre(catString);

        // Assert
        Assert.AreEqual(existingCat, result);
    }
    [TestMethod]
    public void GetCategoriaByNombre_CategoriaDoesNotExist_ThrowsResourceNotFoundException()
    {
        // Arrange
        var nonExistingNombre = "NonExistingCategoria";
        _mockRepository.Setup(repo => repo.Get(c => c.Nombre == nonExistingNombre, null)).Returns((Categoria)null);

        // Act & Assert
        Assert.ThrowsException<KeyNotFoundException>(() => _categoriaService.GetCategoriaByNombre(nonExistingNombre));
    }

   
}