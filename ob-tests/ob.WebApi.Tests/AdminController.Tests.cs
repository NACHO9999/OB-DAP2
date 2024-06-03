using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ob.IBusinessLogic;
using ob.WebApi.Controllers;
using ob.WebApi.DTOs;
using ob.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using Enums;
namespace ob.WebApi.Tests.Controllers
{
    [TestClass]
    public class AdministradorControllerTests
    {
        private Mock<IAdminService> _mockAdminService;
        private AdministradorController _adminController;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockAdminService = new Mock<IAdminService>();
            _adminController = new AdministradorController(_mockAdminService.Object);
        }

        [TestMethod]
        public void CrearAdmin_ValidAdmin_ReturnsOk()
        {
            // Arrange
            var adminDTO = new UsuarioCreateModel
            {
                Nombre = "Test",
                Apellido = "User",
                Email = "testuser@example.com",
                Contrasena = "password"
            };

            // Act
            var result = _adminController.CrearAdmin(adminDTO) as OkResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            _mockAdminService.Verify(service => service.CrearAdmin(It.IsAny<Administrador>()), Times.Once);
        }

        [TestMethod]
        public void GetAdminByEmail_ValidEmail_ReturnsOkWithAdmin()
        {
            // Arrange
            var email = "testuser@example.com";
            var admin = new Administrador("Test", "User", email, "password");
            _mockAdminService.Setup(service => service.GetAdminByEmail(email)).Returns(admin);

            // Act
            var result = _adminController.GetAdminByEmail(email) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(admin, result.Value);
            _mockAdminService.Verify(service => service.GetAdminByEmail(email), Times.Once);
        }

        [TestMethod]
        public void Invitar_ValidInvitacion_ReturnsOk()
        {
            // Arrange
            var invitacionDTO = new InvitacionDTO
            {
                Email = "invitee@example.com",
                Nombre = "Invitee",
                FechaExpiracion = DateTime.Now.AddDays(1),
                Rol = RolInvitaciion.Encargado
            };

            // Act
            var result = _adminController.Invitar(invitacionDTO) as OkResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            _mockAdminService.Verify(service => service.Invitar(invitacionDTO.Email, invitacionDTO.Nombre, invitacionDTO.FechaExpiracion, invitacionDTO.Rol), Times.Once);
        }

        [TestMethod]
        public void EliminarInvitacion_ValidEmail_ReturnsOk()
        {
            // Arrange
            var email = "invitee@example.com";

            // Act
            var result = _adminController.EliminarInvitacion(email) as OkResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            _mockAdminService.Verify(service => service.EliminarInvitacion(email), Times.Once);
        }

        [TestMethod]
        public void AltaCategoria_ValidCategoria_ReturnsOk()
        {
            // Arrange
            var categoria = new Categoria("Cat");

            // Act
            var result = _adminController.AltaCategoria(categoria) as OkResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            _mockAdminService.Verify(service => service.AltaCategoria(categoria), Times.Once);
        }
    }
}