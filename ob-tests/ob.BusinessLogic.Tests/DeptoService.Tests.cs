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
public class DeptoServiceTests
{
    private Mock<IGenericRepository<Depto>> _mockRepository;
    private Mock<IDuenoService> _mockDuenoService;
    private IDeptoService _deptoService;

    [TestInitialize]
    public void Initialize()
    {
        _mockRepository = new Mock<IGenericRepository<Depto>>();
        _mockDuenoService = new Mock<IDuenoService>();
        _deptoService = new DeptoService(_mockRepository.Object, _mockDuenoService.Object);
    }

    [TestMethod]
    public void CrearDepto_DeptoExists_ThrowsAlreadyExistsException()
    {
        // Arrange
        var existingDepto = new Depto(5, 101, null, 3, 2, false, "nom", "dir");
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Depto, bool>>>(), null)).Returns(existingDepto);

        // Act & Assert
        Assert.ThrowsException<AlreadyExistsException>(() => _deptoService.CrearDepto(existingDepto));
    }

    [TestMethod]
    public void CrearDepto_DeptoDoesNotExist_InsertsDeptoAndSaves()
    {
        // Arrange
        var newDepto = new Depto(5, 101, null, 3, 2, false, "nom", "dir");
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Depto, bool>>>(), null)).Returns((Depto)null);

        // Act
        _deptoService.CrearDepto(newDepto);

        // Assert
        _mockRepository.Verify(repo => repo.Insert(newDepto), Times.Once);
        _mockRepository.Verify(repo => repo.Save(), Times.Once);
    }

    [TestMethod]
    public void EditarDepto_UpdatesDeptoAndSaves()
    {
        // Arrange
        var existingDepto = new Depto(5, 101, null, 3, 2, false, "nom", "dir");
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Depto, bool>>>(), null)).Returns(existingDepto);
        _mockRepository.Setup(repo => repo.Update(existingDepto)).Verifiable();
        _mockRepository.Setup(repo => repo.Save()).Verifiable();

        // Act
        _deptoService.EditarDepto(existingDepto);

        // Assert
        _mockRepository.Verify(repo => repo.Update(existingDepto), Times.Once);
        _mockRepository.Verify(repo => repo.Save(), Times.Once);
    }
    [TestMethod]
    public void BorrarDepto_DeletesDeptoAndSaves()
    {
        // Arrange
        var existingDepto = new Depto(5, 101, null, 3, 2, false, "nom", "dir");

        // Act
        _deptoService.BorrarDepto(existingDepto);

        // Assert
        _mockRepository.Verify(repo => repo.Delete(existingDepto), Times.Once);
        _mockRepository.Verify(repo => repo.Save(), Times.Once);
    }

    [TestMethod]
    public void ExisteDepto_DeptoExists_ReturnsTrue()
    {
        // Arrange
        var existingDepto = new Depto(5, 101, null, 3, 2, false, "nom", "dir");
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Depto, bool>>>(), null)).Returns(existingDepto);

        // Act
        var result = _deptoService.ExisteDepto(existingDepto);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void ExisteDepto_DeptoDoesNotExist_ReturnsFalse()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Depto, bool>>>(), null)).Returns((Depto)null);

        // Act
        var result = _deptoService.ExisteDepto(new Depto(5, 101, null, 3, 2, false, "nom", "dir"));

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void GetDepto_DeptoExists_ReturnsDepto()
    {
        // Arrange
        var depto = new Depto(5, 101, null, 3, 2, false, "nom", "dir");
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Depto, bool>>>(), It.IsAny<List<string>>())).Returns(depto);

        // Act
        var result = _deptoService.GetDepto(101, "nom", "dir");

        // Assert
        Assert.AreEqual(depto, result);
    }

    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException), "Departamento no encontrado")]
    public void GetDepto_DeptoDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Depto, bool>>>(), It.IsAny<List<string>>())).Returns((Depto)null);

        // Act
        _deptoService.GetDepto(101, "nom", "dir");
    }
    [TestMethod]
    public void GetDeptosPorEdificio_ReturnsDeptos()
    {
        // Arrange
        var edificio = new Edificio("nom", "dir", "ubi", new Constructora("const"), 1000, new List<Depto>());
        var deptos = new List<Depto>
    {
        new Depto(5, 101, null, 3, 2, false, "nom", "dir"),
        new Depto(5, 102, null, 3, 2, false, "nom", "dir")
    };

        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Depto, bool>>>(), null)).Returns((Expression<Func<Depto, bool>> predicate, List<string> includes) =>
        {
            return deptos.Find(d => predicate.Compile().Invoke(d));
        });

        // Act
        var result = _deptoService.GetDeptosPorEdificio(edificio);

        // Assert
        Assert.AreEqual(2, result.Count());
        Assert.AreEqual(101, result.First().Numero);
        Assert.AreEqual(102, result.Last().Numero);
    }


}