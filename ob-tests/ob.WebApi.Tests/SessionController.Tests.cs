using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ob.WebApi.Controllers;
using ob.IBusinessLogic;
using ob.WebApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;
using System.Security.Authentication;

namespace ob.Tests.WebApi.Controllers
{
    [TestClass]
    public class SessionControllerTests
    {
        private Mock<ISessionService> _sessionServiceMock;
        private SessionController _controller;

        [TestInitialize]
        public void Setup()
        {
            _sessionServiceMock = new Mock<ISessionService>();
            _controller = new SessionController(_sessionServiceMock.Object);
        }

        [TestMethod]
        public void Login_ValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var userLoginModel = new UserLoginModel
            {
                Email = "test@example.com",
                Contrasena = "password123"
            };
            var token = Guid.NewGuid();
            _sessionServiceMock.Setup(s => s.Authenticate(userLoginModel.Email, userLoginModel.Contrasena))
                .Returns(token);

            // Act
            var result = _controller.Login(userLoginModel) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(result.Value);
            var value = result.Value.GetType().GetProperty("token").GetValue(result.Value, null);
            Assert.AreEqual(token, value);
        }



        [TestMethod]
        public void Logout_ValidToken_ReturnsOk()
        {
            // Arrange
            var authToken = Guid.NewGuid();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = authToken.ToString();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = _controller.Logout() as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual("Logged out successfully.", result.Value);
            _sessionServiceMock.Verify(s => s.Logout(authToken), Times.Once);
        }


    }
}
