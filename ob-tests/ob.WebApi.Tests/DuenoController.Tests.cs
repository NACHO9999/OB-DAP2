using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ob.Domain;
using ob.IBusinessLogic;
using ob.WebApi.Controllers;
using ob.WebApi.DTOs;
using System;

namespace ob.Tests.WebApi.Controllers
{
    [TestClass]
    public class DuenoControllerTests
    {
        private Mock<IDuenoService> _duenoServiceMock;
        private DuenoController _controller;

        [TestInitialize]
        public void Setup()
        {
            _duenoServiceMock = new Mock<IDuenoService>();
            _controller = new DuenoController(_duenoServiceMock.Object);
        }

        [TestMethod]
        public void GetDuenoByEmail_ReturnsOkWithDueno()
        {
            // Arrange
            var dueno = new Dueno("John", "Doe", "john.doe@example.com");

            _duenoServiceMock.Setup(s => s.GetDuenoByEmail(It.IsAny<string>())).Returns(dueno);

            // Act
            var result = _controller.GetDuenoByEmail("john.doe@example.com") as OkObjectResult;
            var value = result.Value as DuenoDTO;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(value);
            Assert.AreEqual("John", value.Nombre);
            Assert.AreEqual("Doe", value.Apellido);
            Assert.AreEqual("john.doe@example.com", value.Email);
        }

        [TestMethod]
        public void InsertDueno_ReturnsOk()
        {
            // Arrange
            var duenoDTO = new DuenoDTO { Nombre = "John", Apellido = "Doe", Email = "john.doe@example.com" };

            // Act
            var result = _controller.InsertDueno(duenoDTO);

            // Assert
            Assert.IsNotNull(result, "Expected non-null result.");
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Expected result to be OkObjectResult.");
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual("Dueno creado exitosamente.", okResult.Value);

            _duenoServiceMock.Verify(s => s.CrearDueno(It.IsAny<Dueno>()), Times.Once);
        }
    }
}