using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ob.IBusinessLogic;
using ob.WebApi.Controllers;
using ob.Exceptions;
using ob.Domain;
using Enums;

namespace ob.Tests.WebApi.Controllers
{
    [TestClass]
    public class InvitacionControllerTests
    {
        private Mock<IInvitacionService> _invitacionServiceMock;
        private InvitacionController _controller;

        [TestInitialize]
        public void Setup()
        {
            _invitacionServiceMock = new Mock<IInvitacionService>();
            _controller = new InvitacionController(_invitacionServiceMock.Object);
        }

        [TestMethod]
        public void InvitacionAccepted_ReturnsOk()
        {
            string email = "test@example.com";
            string contrasena = "password123";
            var invitacion = new Invitacion(email, "null", DateTime.Now.AddDays(2), RolInvitaciion.Encargado);
            
            _invitacionServiceMock.Setup(s => s.GetInvitacionByEmail(email)).Returns(invitacion);
            // Act
            var result = _controller.InvitacionAccepted(email, contrasena) as OkResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            _invitacionServiceMock.Verify(s => s.InvitacionAceptada(email, contrasena), Times.Once);
        }
    }
}