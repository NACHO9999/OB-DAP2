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
    public class InvitacionServiceTests
    {
        private Mock<IGenericRepository<Invitacion>> _mockRepository;
        private Mock<IEncargadoService> _mockEncargadoService;
        private Mock<IAdminConstructoraService> _mockAdminConstructoraService;
        private IInvitacionService _invitacionService;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IGenericRepository<Invitacion>>();
            _mockEncargadoService = new Mock<IEncargadoService>();
            _mockAdminConstructoraService = new Mock<IAdminConstructoraService>();
            _invitacionService = new InvitacionService(_mockRepository.Object, _mockEncargadoService.Object, _mockAdminConstructoraService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException), "La invitacion ya existe")]
        public void CrearInvitacion_AlreadyExists_ThrowsException()
        {
            // Arrange
            var invitacion = new Invitacion("test@example.com", "Test User", DateTime.Now.AddDays(1), RolInvitaciion.Encargado);
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Invitacion, bool>>>(), null)).Returns(invitacion);

            // Act
            _invitacionService.CrearInvitacion(invitacion);
        }

        [TestMethod]
        public void CrearInvitacion_DoesNotExist_CreatesInvitacion()
        {
            // Arrange
            var invitacion = new Invitacion("test@example.com", "Test User", DateTime.Now.AddDays(1), RolInvitaciion.Encargado);
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Invitacion, bool>>>(), null)).Returns((Invitacion)null);

            // Act
            _invitacionService.CrearInvitacion(invitacion);

            // Assert
            _mockRepository.Verify(repo => repo.Insert(invitacion), Times.Once);
            _mockRepository.Verify(repo => repo.Save(), Times.Once);
        }

        [TestMethod]
        public void GetInvitacionByEmail_ReturnsInvitacion()
        {
            // Arrange
            var invitacion = new Invitacion("test@example.com", "Test User", DateTime.Now.AddDays(1), RolInvitaciion.Encargado);
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Invitacion, bool>>>(), null)).Returns(invitacion);

            // Act
            var result = _invitacionService.GetInvitacionByEmail("test@example.com");

            // Assert
            Assert.AreEqual(invitacion, result);
        }

        [TestMethod]
        public void EliminarInvitacion_Exists_DeletesInvitacion()
        {
            // Arrange
            var invitacion = new Invitacion("test@example.com", "Test User", DateTime.Now.AddDays(1), RolInvitaciion.Encargado);
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Invitacion, bool>>>(), null)).Returns(invitacion);

            // Act
            _invitacionService.EliminarInvitacion("test@example.com");

            // Assert
            _mockRepository.Verify(repo => repo.Delete(invitacion), Times.Once);
            _mockRepository.Verify(repo => repo.Save(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException), "No se encontró la invitacion.")]
        public void EliminarInvitacion_DoesNotExist_ThrowsException()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Invitacion, bool>>>(), null)).Returns((Invitacion)null);

            // Act
            _invitacionService.EliminarInvitacion("test@example.com");
        }

        [TestMethod]
        public void InvitacionAceptada_Encargado_CreatesEncargado()
        {
            // Arrange
            var invitacion = new Invitacion("test@example.com", "Test User", DateTime.Now.AddDays(1), RolInvitaciion.Encargado);
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Invitacion, bool>>>(), null)).Returns(invitacion);

            // Act
            _invitacionService.InvitacionAceptada(invitacion.Email, "password");

            // Assert
            _mockEncargadoService.Verify(s => s.CrearEncargado(It.IsAny<Encargado>()), Times.Once);
            _mockRepository.Verify(repo => repo.Delete(invitacion), Times.Once);
            _mockRepository.Verify(repo => repo.Save(), Times.Once);
        }

        [TestMethod]
        public void InvitacionAceptada_AdminConstructora_CreatesAdminConstructora()
        {
            // Arrange
            var invitacion = new Invitacion("test@example.com", "Test User", DateTime.Now.AddDays(1), RolInvitaciion.AdminConstruccion);
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Invitacion, bool>>>(), null)).Returns(invitacion);

            // Act
            _invitacionService.InvitacionAceptada(invitacion.Email, "password");

            // Assert
            _mockAdminConstructoraService.Verify(s => s.CrearAdminConstructora(It.IsAny<AdminConstructora>()), Times.Once);
            _mockRepository.Verify(repo => repo.Delete(invitacion), Times.Once);
            _mockRepository.Verify(repo => repo.Save(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "La invitacion ha expirado.")]
        public void InvitacionAceptada_Expired_ThrowsException()
        {
            // Arrange
            var invitacion = new Invitacion("test@example.com", "Test User", DateTime.Now.AddDays(-1), RolInvitaciion.Encargado);
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Invitacion, bool>>>(), null)).Returns(invitacion);

            // Act
            _invitacionService.InvitacionAceptada(invitacion.Email, "password");
        }

        [TestMethod]
        public void InvitacionExiste_ReturnsTrueIfExists()
        {
            // Arrange
            var invitacion = new Invitacion("test@example.com", "Test User", DateTime.Now.AddDays(1), RolInvitaciion.Encargado);
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Invitacion, bool>>>(), null)).Returns(invitacion);

            // Act
            var result = _invitacionService.InvitacionExiste("test@example.com");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void InvitacionExiste_ReturnsFalseIfNotExists()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Invitacion, bool>>>(), null)).Returns((Invitacion)null);

            // Act
            var result = _invitacionService.InvitacionExiste("test@example.com");

            // Assert
            Assert.IsFalse(result);
        }
    }
}