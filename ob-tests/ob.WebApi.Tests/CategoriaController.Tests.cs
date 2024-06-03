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
    public class CategoriaControllerTests
    {
        private Mock<ICategoriaService> _categoriaServiceMock;
        private CategoriaController _controller;

        [TestInitialize]
        public void Setup()
        {
            _categoriaServiceMock = new Mock<ICategoriaService>();
            _controller = new CategoriaController(_categoriaServiceMock.Object);
        }

        [TestMethod]
        public void GetCategorias_ReturnsOkWithCategorias()
        {
            // Arrange
            var categorias = new List<Categoria>
            {
                new Categoria ("Categoria1"),
                new Categoria ("Categoria2")
            };

            _categoriaServiceMock.Setup(s => s.GetAllCategorias()).Returns(categorias);

            // Act
            var result = _controller.GetCategorias() as OkObjectResult;
            var value = result.Value as List<CategoriaDTO>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(value);
            Assert.AreEqual(2, value.Count);
            Assert.AreEqual("Categoria1", value[0].Nombre);
            Assert.AreEqual("Categoria2", value[1].Nombre);
        }

        [TestMethod]
        public void GetCategoriaByNombre_ReturnsOkWithCategoria()
        {
            // Arrange
            var categoria = new Categoria("Categoria1");

            _categoriaServiceMock.Setup(s => s.GetCategoriaByNombre(It.IsAny<string>())).Returns(categoria);

            // Act
            var result = _controller.GetCategoriaByNombre("Categoria1") as OkObjectResult;
            var value = result.Value as CategoriaDTO;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(value);
            Assert.AreEqual("Categoria1", value.Nombre);
        }
    }
}