using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ob.IBusinessLogic;
using ob.WebApi.Controllers;
using ob.WebApi.DTOs;
using ob.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Enums;

namespace ob.Tests.WebApi.Controllers
{
    [TestClass]
    public class MantenimientoControllerTests
    {
        private Mock<IMantenimientoService> _mantenimientoServiceMock;
        private Mock<ISessionService> _sessionServiceMock;
        private Mock<ISolicitudService> _solicitudServiceMock;
        private MantenimientoController _controller;
        private Solicitud solicitud;
        private Mantenimiento mant;

        [TestInitialize]
        public void Setup()
        {
            _mantenimientoServiceMock = new Mock<IMantenimientoService>();
            _sessionServiceMock = new Mock<ISessionService>();
            _solicitudServiceMock = new Mock<ISolicitudService>();
            _controller = new MantenimientoController(_sessionServiceMock.Object, _mantenimientoServiceMock.Object);
            mant = new Mantenimiento("Alber", "Zik", "a@a.com", "Contra1233");
            solicitud = new Solicitud("des", new Depto(1, 1, null, 11, 2, true, "ed", "dir"), new Categoria("cat"), DateTime.Now) { PerMan = mant };
            
            _sessionServiceMock.Setup(s => s.GetCurrentUser(It.IsAny<Guid>())).Returns(mant);
            _solicitudServiceMock.Setup(s => s.GetSolicitudById(It.IsAny<Guid>())).Returns(solicitud);  
            _mantenimientoServiceMock.Setup(m => m.GetMantenimientoByEmail(It.IsAny<string>())).Returns(mant);
            var guid = Guid.NewGuid();
            HttpContext httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = guid.ToString();

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext,
            };
        }

   

        [TestMethod]
        public void AtenderSolicitud_ReturnsOk()
        {
            // Arrange
            var solicitudId = Guid.NewGuid();
            

            // Act
            var result = _controller.AtenderSolicitud(solicitudId) as OkResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            _mantenimientoServiceMock.Verify(s => s.AtenderSolicitud(solicitudId, mant.Email), Times.Once);
        }

        [TestMethod]
        public void CompletarSolicitud_ReturnsOk()
        {
            // Arrange
            var solicitudId = Guid.NewGuid();
            solicitud.Estado = EstadoSolicitud.Atendiendo;


            // Act
            var result = _controller.CompletarSolicitud(solicitudId) as OkResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            _mantenimientoServiceMock.Verify(s => s.CompletarSolicitud(solicitudId, mant.Email), Times.Once);
        }

        
    }
}