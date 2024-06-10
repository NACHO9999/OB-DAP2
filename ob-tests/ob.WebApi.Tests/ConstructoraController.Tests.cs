using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ob.Domain;
using ob.IBusinessLogic;
using ob.WebApi.Controllers;
using ob.WebApi.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ob.Tests.WebApi.Controllers
{
    [TestClass]
    public class ConstructoraControllerTests
    {
        private Mock<IConstructoraService> _constructoraServiceMock;
        private ConstructoraController _controller;

        [TestInitialize]
        public void Setup()
        {
            _constructoraServiceMock = new Mock<IConstructoraService>();
            _controller = new ConstructoraController(_constructoraServiceMock.Object);
        }

        [TestMethod]
        public void GetConstructoras_ReturnsOkWithConstructoras()
        {
            // Arrange
            var constructoras = new List<Constructora>
            {
                new Constructora ("Constructora1"),
                new Constructora ("Constructora2")
            };

            _constructoraServiceMock.Setup(s => s.GetAllConstructoras()).Returns(constructoras);

            // Act
            var result = _controller.GetConstructoras() as OkObjectResult;
            var value = result.Value as List<ConstructoraDTO>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(value);
            Assert.AreEqual(2, value.Count);
            Assert.AreEqual("Constructora1", value[0].Nombre);
            Assert.AreEqual("Constructora2", value[1].Nombre);
        }

        [TestMethod]
        public void GetConstructoraByNombre_ReturnsOkWithConstructora()
        {
            // Arrange
            var constructora = new Constructora ("Constructora1");

            _constructoraServiceMock.Setup(s => s.GetConstructoraByNombre(It.IsAny<string>())).Returns(constructora);

            // Act
            var result = _controller.GetConstructoraByNombre("Constructora1") as OkObjectResult;
            var value = result.Value as ConstructoraDTO;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(value);
            Assert.AreEqual("Constructora1", value.Nombre);
        }
    }
}