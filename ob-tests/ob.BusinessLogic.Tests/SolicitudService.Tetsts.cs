using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ob.BusinessLogic;
using ob.IBusinessLogic;
using ob.Domain;
using ob.IDataAccess;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

[TestClass]
public class SolicitudServiceTests
{
    private Mock<IGenericRepository<Solicitud>> _mockRepository;
    private ISolicitudService _solicitudService;

    [TestInitialize]
    public void Initialize()
    {
        _mockRepository = new Mock<IGenericRepository<Solicitud>>();
        _solicitudService = new SolicitudService(_mockRepository.Object);
    }

    [TestMethod]
    public void CrearSolicitud_InsertsSolicitud()
    {
        // Arrange
        var categoria = new Categoria(); // Assuming you have an empty constructor or relevant data
        var depto = new Depto(); // Assuming you have an empty constructor or relevant data
        var solicitud = new Solicitud("Fix a leak", depto, categoria, Enums.EstadoSolicitud.Abierto, DateTime.Now);
        _mockRepository.Setup(repo => repo.Insert(solicitud)).Verifiable();
        _mockRepository.Setup(repo => repo.Save()).Verifiable();

        // Act
        _solicitudService.CrearSolicitud(solicitud);

        // Assert
        _mockRepository.Verify(repo => repo.Insert(solicitud), Times.Once());
        _mockRepository.Verify(repo => repo.Save(), Times.Once());
    }

    [TestMethod]
    public void GetSolicitudById_SolicitudExists_ReturnsSolicitud()
    {
        // Arrange
        var id = Guid.NewGuid();
        var depto = new Depto(); // Assuming you have an empty constructor or relevant data
        var categoria = new Categoria(); // Assuming you have an empty constructor or relevant data
        var solicitud = new Solicitud("Service HVAC", depto, categoria, Enums.EstadoSolicitud.Abierto, DateTime.Now) { Id = id };
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Solicitud, bool>>>())).Returns(solicitud);

        // Act
        var result = _solicitudService.GetSolicitudById(id);

        // Assert
        Assert.AreEqual(solicitud, result);
    }

    [TestMethod]
    public void GetSolicitudById_SolicitudDoesNotExist_ThrowsResourceNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Solicitud, bool>>>())).Returns((Solicitud)null);

        // Act & Assert
        Assert.ThrowsException<ResourceNotFoundException>(() => _solicitudService.GetSolicitudById(id));
    }
}
