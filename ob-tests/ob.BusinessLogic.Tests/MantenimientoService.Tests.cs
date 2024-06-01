using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ob.BusinessLogic;
using ob.Domain;
using ob.Exceptions.BusinessLogicExceptions;
using Enums;
using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using ob.IBusinessLogic;

namespace YourNamespace.Tests
{
    [TestClass]
    public class MantenimientoServiceTests
    {
        private Mock<IUsuarioRepository> _mockRepository;
        private Mock<ISolicitudService> _mockSolicitudService;
        private MantenimientoService _mantenimientoService;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<IUsuarioRepository>();
            _mockSolicitudService = new Mock<ISolicitudService>();
            _mantenimientoService = new MantenimientoService(_mockRepository.Object, _mockSolicitudService.Object);
        }

        [TestMethod]
        public void CrearMantenimiento_NewMantenimiento_CallsInsertAndSave()
        {
            // Arrange
            var mantenimiento = new Mantenimiento("Nombre", "Apellido", "email@example.com", "password");

            // Act
            _mantenimientoService.CrearMantenimiento(mantenimiento);

            // Assert
            _mockRepository.Verify(repo => repo.Insert(mantenimiento), Times.Once);
            _mockRepository.Verify(repo => repo.Save(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException), "El mantenimiento ya existe")]
        public void CrearMantenimiento_ExistingMantenimiento_ThrowsException()
        {
            // Arrange
            var existingMantenimiento = new Mantenimiento("Existing", "Mantenimiento", "existing@example.com", "password");
            _mockRepository.Setup(repo => repo.EmailExists("existing@example.com")).Returns(true);

            // Act
            _mantenimientoService.CrearMantenimiento(existingMantenimiento);
        }

        [TestMethod]
        public void GetMantenimientoByEmail_ExistingEmail_ReturnsMantenimiento()
        {
            // Arrange
            var email = "mantenimiento@example.com";
            var mantenimiento = new Mantenimiento("Mantenimiento", "Example", email, "password");
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(),It.IsAny<List<string>>())).Returns(mantenimiento);

            // Act
            var result = _mantenimientoService.GetMantenimientoByEmail(email);

            // Assert
            Assert.AreEqual(mantenimiento, result);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException), "No Mantenimiento found with the specified email.")]
        public void GetMantenimientoByEmail_NonExistingEmail_ThrowsException()
        {
            // Arrange
            var nonExistingEmail = "nonexisting@example.com";
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns((Mantenimiento)null);

            // Act
            _mantenimientoService.GetMantenimientoByEmail(nonExistingEmail);
        }

        [TestMethod]
        public void AtenderSolicitud_ValidSolicitud_ChangesEstadoToAtendiendo()
        {
            // Arrange
            var mantenimiento = new Mantenimiento("Mantenimiento", "Example", "mantenimiento@example.com", "password");
            var solicitudId = Guid.NewGuid();
            var solicitud = new Solicitud(mantenimiento, "Descripción", new Depto(1,101,null,3,4,true,"ed","dir"), new Categoria("Cat"), EstadoSolicitud.Abierto, DateTime.Now);
            _mockSolicitudService.Setup(service => service.GetSolicitudById(solicitudId)).Returns(solicitud);
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(mantenimiento);

            // Act
            _mantenimientoService.AtenderSolicitud(solicitudId, mantenimiento.Email);

            // Assert
            Assert.AreEqual(EstadoSolicitud.Atendiendo, solicitud.Estado);
            Assert.IsNotNull(solicitud.FechaInicio);
            _mockSolicitudService.Verify(service => service.EditarSolicitud(solicitud), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "El mantenimiento no puede atender la solicitud")]
        public void AtenderSolicitud_InvalidSolicitudForMantenimiento_ThrowsException()
        {
            // Arrange
            var mantenimiento = new Mantenimiento("Mantenimiento", "Example", "mantenimiento@example.com", "password");
            var otherMantenimiento = new Mantenimiento("Other", "Mantenimiento", "other@example.com", "password");
            var solicitudId = Guid.NewGuid();
            var solicitud = new Solicitud(otherMantenimiento, "Descripción", new Depto(1,101,null,3,4,true,"ed","dir"), new Categoria("Cat"), EstadoSolicitud.Abierto, DateTime.Now);
            _mockSolicitudService.Setup(service => service.GetSolicitudById(solicitudId)).Returns(solicitud);
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(mantenimiento);

            // Act
            _mantenimientoService.AtenderSolicitud(solicitudId, mantenimiento.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "La solicitud ya fue atendida")]
        public void AtenderSolicitud_AlreadyAtendidaSolicitud_ThrowsException()
        {
            // Arrange
            var mantenimiento = new Mantenimiento("Mantenimiento", "Example", "mantenimiento@example.com", "password");
            var solicitudId = Guid.NewGuid();
            var solicitud = new Solicitud(mantenimiento, "Descripción", new Depto(1,101,null,3,4,true,"ed","dir"), new Categoria("Cat"), EstadoSolicitud.Atendiendo, DateTime.Now);
            _mockSolicitudService.Setup(service => service.GetSolicitudById(solicitudId)).Returns(solicitud);
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(mantenimiento);

            // Act
            _mantenimientoService.AtenderSolicitud(solicitudId, mantenimiento.Email);
        }

        [TestMethod]
        public void CompletarSolicitud_ValidSolicitud_ChangesEstadoToCerrado()
        {
            // Arrange
            var mantenimiento = new Mantenimiento("Mantenimiento", "Example", "mantenimiento@example.com", "password");
            var solicitudId = Guid.NewGuid();
            var solicitud = new Solicitud(mantenimiento, "Descripción", new Depto(1,101,null,3,4,true,"ed","dir"), new Categoria("Cat"), EstadoSolicitud.Atendiendo, DateTime.Now);
            _mockSolicitudService.Setup(service => service.GetSolicitudById(solicitudId)).Returns(solicitud);
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(mantenimiento);

            // Act
            _mantenimientoService.CompletarSolicitud(solicitudId, mantenimiento.Email);

            // Assert
            Assert.AreEqual(EstadoSolicitud.Cerrado, solicitud.Estado);
            Assert.IsNotNull(solicitud.FechaFin);
            _mockSolicitudService.Verify(service => service.EditarSolicitud(solicitud), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "El mantenimiento no puede atender la solicitud")]
        public void CompletarSolicitud_InvalidSolicitudForMantenimiento_ThrowsException()
        {
            // Arrange
            var mantenimiento = new Mantenimiento("Mantenimiento", "Example", "mantenimiento@example.com", "password");
            var otherMantenimiento = new Mantenimiento("Other", "Mantenimiento", "other@example.com", "password");
            var solicitudId = Guid.NewGuid();
            var solicitud = new Solicitud(otherMantenimiento, "Descripción", new Depto(1,101,null,3,4,true,"ed","dir"), new Categoria("Cat"), EstadoSolicitud.Atendiendo, DateTime.Now);
            _mockSolicitudService.Setup(service => service.GetSolicitudById(solicitudId)).Returns(solicitud);
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(mantenimiento);

            // Act
            _mantenimientoService.CompletarSolicitud(solicitudId, mantenimiento.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "La solicitud no esta en estado de atendiendo")]
        public void CompletarSolicitud_NotAtendiendoSolicitud_ThrowsException()
        {
            // Arrange
            var mantenimiento = new Mantenimiento("Mantenimiento", "Example", "mantenimiento@example.com", "password");
            var solicitudId = Guid.NewGuid();
            var solicitud = new Solicitud(mantenimiento, "Descripción", new Depto(1,101,null,3,4,true,"ed","dir"), new Categoria("Cat"), EstadoSolicitud.Abierto, DateTime.Now);
            _mockSolicitudService.Setup(service => service.GetSolicitudById(solicitudId)).Returns(solicitud);
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(mantenimiento);

            // Act
            _mantenimientoService.CompletarSolicitud(solicitudId, mantenimiento.Email);
        }
    }
}