using Enums;
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

namespace ob.Tests
{
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
        public void CrearSolicitud_CreatesSolicitud()
        {
            // Arrange
            var categoria = new Categoria("Categoria1");
            var depto = new Depto(1, 101, null, 4, 3, true, "ed", "di");
            var solicitud = new Solicitud("Descripcion1", depto, categoria, EstadoSolicitud.Abierto, DateTime.Now);

            // Act
            _solicitudService.CrearSolicitud(solicitud);

            // Assert
            _mockRepository.Verify(repo => repo.Insert(solicitud), Times.Once);
            _mockRepository.Verify(repo => repo.Save(), Times.Once);
        }

        [TestMethod]
        public void EditarSolicitud_UpdatesSolicitud()
        {
            // Arrange
            var categoria = new Categoria("Categoria1");
            var depto = new Depto(1, 101, null, 4, 3, true, "ed", "di");
            var solicitud = new Solicitud("Descripcion1", depto, categoria, EstadoSolicitud.Abierto, DateTime.Now);

            // Act
            _solicitudService.EditarSolicitud(solicitud);

            // Assert
            _mockRepository.Verify(repo => repo.Update(solicitud), Times.Once);
            _mockRepository.Verify(repo => repo.Save(), Times.Once);
        }

        [TestMethod]
        public void GetSolicitudById_Exists_ReturnsSolicitud()
        {
            // Arrange
            var categoria = new Categoria("Categoria1");
            var depto = new Depto(1, 101, null, 4, 3, true, "ed", "di");
            var solicitud = new Solicitud("Descripcion1", depto, categoria, EstadoSolicitud.Abierto, DateTime.Now);
            solicitud.Id = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Solicitud, bool>>>(), It.IsAny<List<string>>())).Returns(solicitud);

            // Act
            var result = _solicitudService.GetSolicitudById(solicitud.Id);

            // Assert
            Assert.AreEqual(solicitud, result);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException), "No se encontr� la solicitud.")]
        public void GetSolicitudById_DoesNotExist_ThrowsException()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Solicitud, bool>>>(), It.IsAny<List<string>>())).Returns((Solicitud)null);

            // Act
            _solicitudService.GetSolicitudById(Guid.NewGuid());
        }

        [TestMethod]
        public void GetSolicitudesByEdificio_ReturnsSolicitudesInEdificio()
        {
            // Arrange
            var depto = new Depto(1, 101, null, 4, 3, true, "ed", "di");
            var edificio = new Edificio("ed", "di", "ubi", new Constructora("const"), 1000, new List<Depto> { depto });
            var categoria = new Categoria("Categoria1");
            var solicitud = new Solicitud("Descripcion1", depto, categoria, EstadoSolicitud.Abierto, DateTime.Now);
            var solicitudes = new List<Solicitud> { solicitud };
            _mockRepository.Setup(repo => repo.GetAll(It.IsAny<Func<Solicitud, bool>>(), It.IsAny<List<string>>())).Returns(solicitudes);

            // Act
            var result = _solicitudService.GetSolicitudesByEdificio(edificio);

            // Assert
            CollectionAssert.Contains(result, solicitud);
        }

        [TestMethod]
        public void GetSolicitudes_ReturnsAllSolicitudes()
        {
            // Arrange
            var categoria = new Categoria("Categoria1");
            var depto = new Depto(1, 101, null, 4, 3, true, "ed", "di");
            var solicitudes = new List<Solicitud> { new Solicitud("Descripcion1", depto, categoria, EstadoSolicitud.Abierto, DateTime.Now) };
            _mockRepository.Setup(repo => repo.GetAll(It.IsAny<Func<Solicitud, bool>>(), It.IsAny<List<string>>())).Returns(solicitudes);

            // Act
            var result = _solicitudService.GetSolicitudes();

            // Assert
            Assert.AreEqual(solicitudes.Count, result.Count());
        }

        [TestMethod]
        public void GetSolicitudesByCategoria_ReturnsSolicitudesByCategoria()
        {
            // Arrange
            var categoria = new Categoria("Categoria1");
            var depto = new Depto(1, 101, null, 4, 3, true, "ed", "di");
            var solicitud = new Solicitud("Descripcion1", depto, categoria, EstadoSolicitud.Abierto, DateTime.Now);
            var solicitudes = new List<Solicitud> { solicitud };
            _mockRepository.Setup(repo => repo.GetAll(It.IsAny<Func<Solicitud, bool>>(), It.IsAny<List<string>>())).Returns(solicitudes);

            // Act
            var result = _solicitudService.GetSolicitudesByCategoria(categoria);

            // Assert
            CollectionAssert.Contains(result, solicitud);
        }

        [TestMethod]
        public void GetSolicitudesByMantenimiento_ReturnsSolicitudesByMantenimiento()
        {
            // Arrange
            var categoria = new Categoria("Categoria1");
            var depto = new Depto(1, 101, null, 4, 3, true, "ed", "di");
            var mantenimiento = new Mantenimiento("Nombre", "Apellido", "email@example.com", "password");
            var solicitud = new Solicitud(mantenimiento, "Descripcion1", depto, categoria, EstadoSolicitud.Abierto, DateTime.Now);
            var solicitudes = new List<Solicitud> { solicitud };
            _mockRepository.Setup(repo => repo.GetAll(It.IsAny<Func<Solicitud, bool>>(), It.IsAny<List<string>>())).Returns(solicitudes);

            // Act
            var result = _solicitudService.GetSolicitudesByMantenimiento(mantenimiento);

            // Assert
            CollectionAssert.Contains(result, solicitud);
        }

        [TestMethod]
        public void CrearSolicitud_ConPerMan_CreatesSolicitud()
        {
            // Arrange
            var categoria = new Categoria("Categoria1");
            var depto = new Depto(1, 101, null, 4, 3, true, "ed", "di");
            var perMan = new Mantenimiento("Nombre", "Apellido", "email@example.com", "password");
            var solicitud = new Solicitud(perMan, "Descripcion1", depto, categoria, EstadoSolicitud.Abierto, DateTime.Now);

            // Act
            _solicitudService.CrearSolicitud(solicitud);

            // Assert
            _mockRepository.Verify(repo => repo.Insert(solicitud), Times.Once);
            _mockRepository.Verify(repo => repo.Save(), Times.Once);
        }

        [TestMethod]
        public void EditarSolicitud_ConPerMan_UpdatesSolicitud()
        {
            // Arrange
            var categoria = new Categoria("Categoria1");
            var depto = new Depto(1, 101, null, 4, 3, true, "ed", "di");
            var perMan = new Mantenimiento("Nombre", "Apellido", "email@example.com", "password");
            var solicitud = new Solicitud(perMan, "Descripcion1", depto, categoria, EstadoSolicitud.Abierto, DateTime.Now);

            // Act
            _solicitudService.EditarSolicitud(solicitud);

            // Assert
            _mockRepository.Verify(repo => repo.Update(solicitud), Times.Once);
            _mockRepository.Verify(repo => repo.Save(), Times.Once);
        }

        [TestMethod]
        public void SolicitudExists_ReturnsTrueIfSolicitudExists()
        {
            // Arrange
            var categoria = new Categoria("Categoria1");
            var depto = new Depto(1, 101, null, 4, 3, true, "ed", "di");
            var solicitudId = Guid.NewGuid();
            var solicitud = new Solicitud("Descripcion1", depto, categoria, EstadoSolicitud.Abierto, DateTime.Now) { Id = solicitudId };
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Solicitud, bool>>>(), null)).Returns(solicitud);

            // Act
            var result = _solicitudService.SolicitudExists(solicitudId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SolicitudExists_ReturnsFalseIfSolicitudDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Solicitud, bool>>>(), null)).Returns((Solicitud)null);

            // Act
            var result = _solicitudService.SolicitudExists(Guid.NewGuid());

            // Assert
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void SolicitudExists_ReturnsFalse()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Solicitud, bool>>>(), null)).Returns((Solicitud)null);

            // Act
            var result = _solicitudService.SolicitudExists(Guid.NewGuid());

            // Assert
            Assert.IsFalse(result);
        }

    }
}