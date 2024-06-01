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
    public class AdminConstructoraServiceTests
    {
        private Mock<IUsuarioRepository> _mockRepository;
        private Mock<IEdificioService> _mockEdificioService;
        private Mock<IConstructoraService> _mockConstructoraService;
        private Mock<IEncargadoService> _mockEncargadoService;
        private Mock<IDeptoService> _mockDeptoService;
        private IAdminConstructoraService _adminConstructoraService;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IUsuarioRepository>();
            _mockEdificioService = new Mock<IEdificioService>();
            _mockConstructoraService = new Mock<IConstructoraService>();
            _mockEncargadoService = new Mock<IEncargadoService>();
            _mockDeptoService = new Mock<IDeptoService>();
            _adminConstructoraService = new AdminConstructoraService(_mockRepository.Object, _mockEdificioService.Object, _mockConstructoraService.Object, _mockEncargadoService.Object, _mockDeptoService.Object);
        }

        [TestMethod]
        public void CrearAdminConstructora_CreatesAdminConstructora()
        {
            // Arrange
            var admin = new AdminConstructora("Admin", "admin@example.com", "password");

            // Act
            _adminConstructoraService.CrearAdminConstructora(admin);

            // Assert
            _mockRepository.Verify(repo => repo.Insert(admin), Times.Once);
            _mockRepository.Verify(repo => repo.Save(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException), "El administrador de constructora ya existe.")]
        public void CrearAdminConstructora_AlreadyExists_ThrowsException()
        {
            // Arrange
            var admin = new AdminConstructora("Admin", "admin@example.com", "password");
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(admin);

            // Act
            _adminConstructoraService.CrearAdminConstructora(admin);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException), "No se encontró la constructora.")]
        public void CrearAdminConstructora_ConstructoraNotFound_ThrowsException()
        {
            // Arrange
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = new Constructora("Constructora1")
            };
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns((Usuario)null);
            _mockConstructoraService.Setup(service => service.GetConstructoraByNombre(It.IsAny<string>())).Returns((Constructora)null);

            // Act
            _adminConstructoraService.CrearAdminConstructora(admin);
        }

        [TestMethod]
        public void GetAdminConstructoraByEmail_Exists_ReturnsAdminConstructora()
        {
            // Arrange
            var admin = new AdminConstructora("Admin", "admin@example.com", "password");
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(admin);

            // Act
            var result = _adminConstructoraService.GetAdminConstructoraByEmail("admin@example.com");

            // Assert
            Assert.AreEqual(admin, result);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException), "El administrador de constructora no existe.")]
        public void GetAdminConstructoraByEmail_DoesNotExist_ThrowsException()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns((Usuario)null);

            // Act
            _adminConstructoraService.GetAdminConstructoraByEmail("nonexistent@example.com");
        }

        [TestMethod]
        public void CrearConstructora_CreatesConstructora()
        {
            // Arrange
            var admin = new AdminConstructora("Admin", "admin@example.com", "password");
            var constructora = new Constructora("Constructora1");

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(admin);

            // Act
            _adminConstructoraService.CrearConstructora(constructora, "admin@example.com");

            // Assert
            _mockConstructoraService.Verify(service => service.CrearConstructora(constructora), Times.Once);
            _mockRepository.Verify(repo => repo.Save(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException), "El usuario ya tiene constructora.")]
        public void CrearConstructora_UserAlreadyHasConstructora_ThrowsException()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(admin);

            // Act
            _adminConstructoraService.CrearConstructora(constructora, "admin@example.com");
        }

        [TestMethod]
        public void CrearEdificio_CreatesEdificio()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };
            var edificio = new Edificio("Edificio1", "Dirección1", "Ubicación1", constructora, 1000, new List<Depto>());

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(admin);

            // Act
            _adminConstructoraService.CrearEdificio(edificio, "admin@example.com");

            // Assert
            _mockEdificioService.Verify(service => service.CrearEdificio(edificio), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException), "El usuario no tiene constructora.")]
        public void CrearEdificio_UserHasNoConstructora_ThrowsException()
        {
            // Arrange
            var admin = new AdminConstructora("Admin", "admin@example.com", "password");
            var edificio = new Edificio("Edificio1", "Dirección1", "Ubicación1", null, 1000, new List<Depto>());

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(admin);

            // Act
            _adminConstructoraService.CrearEdificio(edificio, "admin@example.com");
        }

        [TestMethod]
        public void BorrarEdificio_DeletesEdificio()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };
            var edificio = new Edificio("Edificio1", "Dirección1", "Ubicación1", constructora, 1000, new List<Depto>());

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(admin);
            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion("Edificio1", "Dirección1")).Returns(edificio);

            // Act
            _adminConstructoraService.BorrarEdificio("Edificio1", "Dirección1", "admin@example.com");

            // Assert
            _mockEdificioService.Verify(service => service.BorrarEdificio(edificio), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "El edificio no pertenece a la constructora del usuario.")]
        public void BorrarEdificio_EdificioDoesNotBelongToUser_ThrowsException()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var otraConstructora = new Constructora("OtraConstructora");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };
            var edificio = new Edificio("Edificio1", "Dirección1", "Ubicación1", otraConstructora, 1000, new List<Depto>());

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(admin);
            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion("Edificio1", "Dirección1")).Returns(edificio);

            // Act
            _adminConstructoraService.BorrarEdificio("Edificio1", "Dirección1", "admin@example.com");
        }

        [TestMethod]
        public void EditarEdificio_EditsEdificio()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };
            var edificio = new Edificio("Edificio1", "Dirección1", "Ubicación1", constructora, 1000, new List<Depto>());

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(admin);

            // Act
            _adminConstructoraService.EditarEdificio(edificio, "admin@example.com");

            // Assert
            _mockEdificioService.Verify(service => service.EditarEdificio(edificio), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "El edificio no pertenece a la constructora del usuario.")]
        public void EditarEdificio_EdificioDoesNotBelongToUser_ThrowsException()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var otraConstructora = new Constructora("OtraConstructora");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };
            var edificio = new Edificio("Edificio1", "Dirección1", "Ubicación1", otraConstructora, 1000, new List<Depto>());

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(admin);

            // Act
            _adminConstructoraService.EditarEdificio(edificio, "admin@example.com");
        }

        [TestMethod]
        public void GetEdificioByNombreYDireccion_ReturnsEdificio()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };
            var edificio = new Edificio("Edificio1", "Dirección1", "Ubicación1", constructora, 1000, new List<Depto>());

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(admin);
            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion("Edificio1", "Dirección1")).Returns(edificio);

            // Act
            var result = _adminConstructoraService.GetEdificioByNombreYDireccion("Edificio1", "Dirección1", "admin@example.com");

            // Assert
            Assert.AreEqual(edificio, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "El edificio no pertenece a la constructora del usuario.")]
        public void GetEdificioByNombreYDireccion_EdificioDoesNotBelongToUser_ThrowsException()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var otraConstructora = new Constructora("OtraConstructora");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };
            var edificio = new Edificio("Edificio1", "Dirección1", "Ubicación1", otraConstructora, 1000, new List<Depto>());

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(admin);
            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion("Edificio1", "Dirección1")).Returns(edificio);

            // Act
            _adminConstructoraService.GetEdificioByNombreYDireccion("Edificio1", "Dirección1", "admin@example.com");
        }

        [TestMethod]
        public void CrearDepto_CreatesDepto()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };
            var edificio = new Edificio("Edificio1", "Dirección1", "Ubicación1", constructora, 1000, new List<Depto>());
            var depto = new Depto(1, 101, null, 2, 2, true, "Edificio1", "Dirección1");

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(admin);
            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion("Edificio1", "Dirección1")).Returns(edificio);

            // Act
            _adminConstructoraService.CrearDepto("admin@example.com", depto);

            // Assert
            _mockDeptoService.Verify(service => service.CrearDepto(depto), Times.Once);
            _mockEdificioService.Verify(service => service.EditarEdificio(edificio), Times.Once);
            _mockRepository.Verify(repo => repo.Save(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "El edificio no pertenece a la constructora del usuario.")]
        public void CrearDepto_EdificioDoesNotBelongToUser_ThrowsException()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var otraConstructora = new Constructora("OtraConstructora");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };
            var edificio = new Edificio("Edificio1", "Dirección1", "Ubicación1", otraConstructora, 1000, new List<Depto>());
            var depto = new Depto(1, 101, null, 2, 2, true, "Edificio1", "Dirección1");

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(admin);
            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion("Edificio1", "Dirección1")).Returns(edificio);

            // Act
            _adminConstructoraService.CrearDepto("admin@example.com", depto);
        }

        [TestMethod]
        public void EditarDepto_EditsDepto()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };
            var edificio = new Edificio("Edificio1", "Dirección1", "Ubicación1", constructora, 1000, new List<Depto>());
            var depto = new Depto(1, 101, null, 2, 2, true, "Edificio1", "Dirección1");

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(admin);
            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion("Edificio1", "Dirección1")).Returns(edificio);

            // Act
            _adminConstructoraService.EditarDepto("admin@example.com", depto);

            // Assert
            _mockDeptoService.Verify(service => service.EditarDepto(depto), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "El edificio no pertenece a la constructora del usuario.")]
        public void EditarDepto_EdificioDoesNotBelongToUser_ThrowsException()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var otraConstructora = new Constructora("OtraConstructora");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };
            var edificio = new Edificio("Edificio1", "Dirección1", "Ubicación1", otraConstructora, 1000, new List<Depto>());
            var depto = new Depto(1, 101, null, 2, 2, true, "Edificio1", "Dirección1");

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(admin);
            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion("Edificio1", "Dirección1")).Returns(edificio);

            // Act
            _adminConstructoraService.EditarDepto("admin@example.com", depto);
        }

        [TestMethod]
        public void BorrarDepto_DeletesDepto()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };
            var edificio = new Edificio("Edificio1", "Dirección1", "Ubicación1", constructora, 1000, new List<Depto>());
            var depto = new Depto(1, 101, null, 2, 2, true, "Edificio1", "Dirección1");

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(admin);
            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion("Edificio1", "Dirección1")).Returns(edificio);
            _mockDeptoService.Setup(service => service.GetDepto(101, "Edificio1", "Dirección1")).Returns(depto);

            // Act
            _adminConstructoraService.BorrarDepto("admin@example.com", 101, "Edificio1", "Dirección1");

            // Assert
            _mockDeptoService.Verify(service => service.BorrarDepto(depto), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "El edificio no pertenece a la constructora del usuario.")]
        public void BorrarDepto_EdificioDoesNotBelongToUser_ThrowsException()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var otraConstructora = new Constructora("OtraConstructora");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };
            var edificio = new Edificio("Edificio1", "Dirección1", "Ubicación1", otraConstructora, 1000, new List<Depto>());
            var depto = new Depto(1, 101, null, 2, 2, true, "Edificio1", "Dirección1");

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(admin);
            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion("Edificio1", "Dirección1")).Returns(edificio);

            // Act
            _adminConstructoraService.BorrarDepto("admin@example.com", 101, "Edificio1", "Dirección1");
        }

        [TestMethod]
        public void GetDepto_ReturnsDepto()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };
            var edificio = new Edificio("Edificio1", "Dirección1", "Ubicación1", constructora, 1000, new List<Depto>());
            var depto = new Depto(1, 101, null, 2, 2, true, "Edificio1", "Dirección1");

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(admin);
            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion("Edificio1", "Dirección1")).Returns(edificio);
            _mockDeptoService.Setup(service => service.GetDepto(101, "Edificio1", "Dirección1")).Returns(depto);

            // Act
            var result = _adminConstructoraService.GetDepto(101, "Edificio1", "Dirección1", "admin@example.com");

            // Assert
            Assert.AreEqual(depto, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "El edificio no pertenece a la constructora del usuario.")]
        public void GetDepto_EdificioDoesNotBelongToUser_ThrowsException()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var otraConstructora = new Constructora("OtraConstructora");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };
            var edificio = new Edificio("Edificio1", "Dirección1", "Ubicación1", otraConstructora, 1000, new List<Depto>());
            var depto = new Depto(1, 101, null, 2, 2, true, "Edificio1", "Dirección1");

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(admin);
            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion("Edificio1", "Dirección1")).Returns(edificio);

            // Act
            _adminConstructoraService.GetDepto(101, "Edificio1", "Dirección1", "admin@example.com");
        }

        [TestMethod]
        public void EditarConstructora_EditsConstructora()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(admin);

            // Act
            _adminConstructoraService.EditarConstructora(constructora, "admin@example.com");

            // Assert
            _mockConstructoraService.Verify(service => service.EditarConstructora(constructora), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException), "El usuario no tiene constructora.")]
        public void EditarConstructora_UserHasNoConstructora_ThrowsException()
        {
            // Arrange
            var admin = new AdminConstructora("Admin", "admin@example.com", "password");

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(admin);

            // Act
            _adminConstructoraService.EditarConstructora(new Constructora("Constructora1"), "admin@example.com");
        }
        // Test method to verify GetConstructoraByAdmin method
        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException), "El administrador de constructora ya existe.")]
        public void CrearAdminConstructora_AdminAlreadyExists_ThrowsException()
        {
            // Arrange
            var admin = new AdminConstructora("Admin", "admin@example.com", "password");

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns(admin);

            // Act
            _adminConstructoraService.CrearAdminConstructora(admin);
        }



        [TestMethod]
        public void CrearAdminConstructora_ValidAdmin_Success()
        {
            // Arrange
            var admin = new AdminConstructora("Admin", "admin@example.com", "password");

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns((Usuario)null);

            // Act
            _adminConstructoraService.CrearAdminConstructora(admin);

            // Assert
            _mockRepository.Verify(repo => repo.Insert(admin), Times.Once);
            _mockRepository.Verify(repo => repo.Save(), Times.Once);
        }

        [TestMethod]
        public void CrearConstructora_ValidConstructora_Success()
        {
            // Arrange
            var admin = new AdminConstructora("Admin", "admin@example.com", "password");

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns(admin);

            // Act
            _adminConstructoraService.CrearConstructora(new Constructora("Constructora1"), "admin@example.com");

            // Assert
            _mockConstructoraService.Verify(service => service.CrearConstructora(It.IsAny<Constructora>()), Times.Once);
            _mockRepository.Verify(repo => repo.Save(), Times.Once);
        }


        [TestMethod]
        public void CrearEdificio_ValidEdificio_Success()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };

            var edificio = new Edificio("Edificio1", "Direccion1", "Ubicacion1", null, 1000, new List<Depto>());

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns(admin);

            // Act
            _adminConstructoraService.CrearEdificio(edificio, "admin@example.com");

            // Assert
            Assert.AreEqual(constructora, edificio.EmpresaConstructora);
            _mockEdificioService.Verify(service => service.CrearEdificio(edificio), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "El edificio no pertenece a la constructora del usuario.")]
        public void BorrarEdificio_EdificioDoesNotBelongToConstructora_ThrowsException()
        {
            // Arrange
            var constructora1 = new Constructora("Constructora1");
            var constructora2 = new Constructora("Constructora2");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora1
            };

            var edificio = new Edificio("Edificio1", "Direccion1", "Ubicacion1", constructora2, 1000, new List<Depto>());

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns(admin);

            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion(It.IsAny<string>(), It.IsAny<string>()))
                                .Returns(edificio);

            // Act
            _adminConstructoraService.BorrarEdificio("Edificio1", "Direccion1", "admin@example.com");
        }

        [TestMethod]
        public void BorrarEdificio_ValidEdificio_Success()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };

            var edificio = new Edificio("Edificio1", "Direccion1", "Ubicacion1", constructora, 1000, new List<Depto>());

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns(admin);

            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion(It.IsAny<string>(), It.IsAny<string>()))
                                .Returns(edificio);

            // Act
            _adminConstructoraService.BorrarEdificio("Edificio1", "Direccion1", "admin@example.com");

            // Assert
            _mockEdificioService.Verify(service => service.BorrarEdificio(edificio), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "El edificio no pertenece a la constructora del usuario.")]
        public void EditarEdificio_EdificioDoesNotBelongToConstructora_ThrowsException()
        {
            // Arrange
            var constructora1 = new Constructora("Constructora1");
            var constructora2 = new Constructora("Constructora2");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora1
            };

            var edificio = new Edificio("Edificio1", "Direccion1", "Ubicacion1", constructora2, 1000, new List<Depto>());

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns(admin);

            // Act
            _adminConstructoraService.EditarEdificio(edificio, "admin@example.com");
        }

        [TestMethod]
        public void EditarEdificio_ValidEdificio_Success()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };

            var edificio = new Edificio("Edificio1", "Direccion1", "Ubicacion1", constructora, 1000, new List<Depto>());

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns(admin);

            // Act
            _adminConstructoraService.EditarEdificio(edificio, "admin@example.com");

            // Assert
            _mockEdificioService.Verify(service => service.EditarEdificio(edificio), Times.Once);
        }

        [TestMethod]
        public void GetEdificioByNombreYDireccion_ValidEdificio_ReturnsEdificio()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };

            var edificio = new Edificio("Edificio1", "Direccion1", "Ubicacion1", constructora, 1000, new List<Depto>());

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns(admin);

            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion(It.IsAny<string>(), It.IsAny<string>()))
                                .Returns(edificio);

            // Act
            var result = _adminConstructoraService.GetEdificioByNombreYDireccion("Edificio1", "Direccion1", "admin@example.com");

            // Assert
            Assert.AreEqual(edificio, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "El edificio no pertenece a la constructora del usuario.")]
        public void GetEdificioByNombreYDireccion_EdificioDoesNotBelongToConstructora_ThrowsException()
        {
            // Arrange
            var constructora1 = new Constructora("Constructora1");
            var constructora2 = new Constructora("Constructora2");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora1
            };

            var edificio = new Edificio("Edificio1", "Direccion1", "Ubicacion1", constructora2, 1000, new List<Depto>());

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns(admin);

            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion(It.IsAny<string>(), It.IsAny<string>()))
                                .Returns(edificio);

            // Act
            _adminConstructoraService.GetEdificioByNombreYDireccion("Edificio1", "Direccion1", "admin@example.com");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "El edificio no pertenece a la constructora del usuario.")]
        public void CrearDepto_EdificioDoesNotBelongToConstructora_ThrowsException()
        {
            // Arrange
            var constructora1 = new Constructora("Constructora1");
            var constructora2 = new Constructora("Constructora2");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora1
            };

            var edificio = new Edificio("Edificio1", "Direccion1", "Ubicacion1", constructora2, 1000, new List<Depto>());
            var depto = new Depto(1, 101, null, 3, 2, true, "Edificio1", "Direccion1");
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns(admin);

            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion(It.IsAny<string>(), It.IsAny<string>()))
                                .Returns(edificio);

            // Act
            _adminConstructoraService.CrearDepto("admin@example.com", depto);
        }

        [TestMethod]
        public void CrearDepto_ValidDepto_Success()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };

            var edificio = new Edificio("Edificio1", "Direccion1", "Ubicacion1", constructora, 1000, new List<Depto>());
            var depto = new Depto(1, 101, null, 3, 2, true, "Edificio1", "Direccion1");
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns(admin);

            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion(It.IsAny<string>(), It.IsAny<string>()))
                                .Returns(edificio);

            // Act
            _adminConstructoraService.CrearDepto("admin@example.com", depto);

            // Assert
            _mockDeptoService.Verify(service => service.CrearDepto(depto), Times.Once);
            _mockEdificioService.Verify(service => service.EditarEdificio(edificio), Times.Once);
            _mockRepository.Verify(repo => repo.Save(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "El edificio no pertenece a la constructora del usuario.")]
        public void EditarDepto_EdificioDoesNotBelongToConstructora_ThrowsException()
        {
            // Arrange
            var constructora1 = new Constructora("Constructora1");
            var constructora2 = new Constructora("Constructora2");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora1
            };

            var edificio = new Edificio("Edificio1", "Direccion1", "Ubicacion1", constructora2, 1000, new List<Depto>());
            var depto = new Depto(1, 101, null, 3, 2, true, "Edificio1", "Direccion1");
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns(admin);

            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion(It.IsAny<string>(), It.IsAny<string>()))
                                .Returns(edificio);

            // Act
            _adminConstructoraService.EditarDepto("admin@example.com", depto);
        }

        [TestMethod]
        public void EditarDepto_ValidDepto_Success()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };

            var edificio = new Edificio("Edificio1", "Direccion1", "Ubicacion1", constructora, 1000, new List<Depto>());
            var depto = new Depto(1, 101, null, 3, 2, true, "Edificio1", "Direccion1");

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns(admin);

            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion(It.IsAny<string>(), It.IsAny<string>()))
                                .Returns(edificio);

            // Act
            _adminConstructoraService.EditarDepto("admin@example.com", depto);

            // Assert
            _mockDeptoService.Verify(service => service.EditarDepto(depto), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "El edificio no pertenece a la constructora del usuario.")]
        public void BorrarDepto_EdificioDoesNotBelongToConstructora_ThrowsException()
        {
            // Arrange
            var constructora1 = new Constructora("Constructora1");
            var constructora2 = new Constructora("Constructora2");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora1
            };

            var edificio = new Edificio("Edificio1", "Direccion1", "Ubicacion1", constructora2, 1000, new List<Depto>());
            var depto = new Depto(1, 101, null, 3, 2, true, "Edificio1", "Direccion1");

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns(admin);

            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion(It.IsAny<string>(), It.IsAny<string>()))
                                .Returns(edificio);

            _mockDeptoService.Setup(service => service.GetDepto(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                             .Returns(depto);

            // Act
            _adminConstructoraService.BorrarDepto("admin@example.com", 101, "Edificio1", "Direccion1");
        }

        [TestMethod]
        public void BorrarDepto_ValidDepto_Success()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };

            var edificio = new Edificio("Edificio1", "Direccion1", "Ubicacion1", constructora, 1000, new List<Depto>());
            var depto = new Depto(1, 101, null, 3, 2, true, "Edificio1", "Direccion1"); ;

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns(admin);

            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion(It.IsAny<string>(), It.IsAny<string>()))
                                .Returns(edificio);

            _mockDeptoService.Setup(service => service.GetDepto(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                             .Returns(depto);

            // Act
            _adminConstructoraService.BorrarDepto("admin@example.com", 101, "Edificio1", "Direccion1");

            // Assert
            _mockDeptoService.Verify(service => service.BorrarDepto(depto), Times.Once);
        }

        [TestMethod]
        public void GetDepto_ValidDepto_ReturnsDepto()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };

            var edificio = new Edificio("Edificio1", "Direccion1", "Ubicacion1", constructora, 1000, new List<Depto>());
            var depto = new Depto(1, 101, null, 3, 2, true, "Edificio1", "Direccion1"); // Create a new Depto with the specified constructor

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns(admin);

            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion(It.IsAny<string>(), It.IsAny<string>()))
                                .Returns(edificio);

            _mockDeptoService.Setup(service => service.GetDepto(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                             .Returns(depto);

            // Act
            var result = _adminConstructoraService.GetDepto(101, "Edificio1", "Direccion1", "admin@example.com");

            // Assert
            Assert.AreEqual(depto, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "El edificio no pertenece a la constructora del usuario.")]
        public void GetDepto_EdificioDoesNotBelongToConstructora_ThrowsException()
        {
            // Arrange
            var constructora1 = new Constructora("Constructora1");
            var constructora2 = new Constructora("Constructora2");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora1
            };

            var edificio = new Edificio("Edificio1", "Direccion1", "Ubicacion1", constructora2, 1000, new List<Depto>());
            var depto = new Depto(1, 101, null, 4, 3, true, "Edificio1", "Direccion1");

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns(admin);

            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion(It.IsAny<string>(), It.IsAny<string>()))
                                .Returns(edificio);

            _mockDeptoService.Setup(service => service.GetDepto(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                             .Returns(depto);

            // Act
            _adminConstructoraService.GetDepto(101, "Edificio1", "Direccion1", "admin@example.com");
        }

        [TestMethod]
        public void AsignarConstructora_ValidConstructora_Success()
        {
            // Arrange
            var admin = new AdminConstructora("Admin", "admin@example.com", "password");

            var constructora = new Constructora("Constructora1");

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns(admin);

            _mockConstructoraService.Setup(service => service.GetConstructoraByNombre(It.IsAny<string>()))
                                    .Returns(constructora);

            // Act
            _adminConstructoraService.AsignarConstructora("admin@example.com", "Constructora1");

            // Assert
            Assert.AreEqual(constructora, admin.Constructora);
            _mockRepository.Verify(repo => repo.Update(admin), Times.Once);
            _mockRepository.Verify(repo => repo.Save(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException), "No se encontró la constructora.")]
        public void AsignarConstructora_ConstructoraNotFound_ThrowsException()
        {
            // Arrange
            var admin = new AdminConstructora("Admin", "admin@example.com", "password");

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns(admin);

            _mockConstructoraService.Setup(service => service.GetConstructoraByNombre(It.IsAny<string>()))
                                    .Returns<Constructora>(null);

            // Act
            _adminConstructoraService.AsignarConstructora("admin@example.com", "Constructora1");
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException), "El usuario ya tiene constructora.")]
        public void AsignarConstructora_UserAlreadyHasConstructora_ThrowsException()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var constructora2 = new Constructora("Constructora2");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };
            _mockConstructoraService.Setup(service => service.GetConstructoraByNombre(It.IsAny<string>()))
                                    .Returns(constructora2);
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns(admin);

            // Act
            _adminConstructoraService.AsignarConstructora("admin@example.com", "Constructora2");
        }

        [TestMethod]
        public void AsignarEncargado_ValidEncargado_Success()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };

            var edificio = new Edificio("Edificio1", "Direccion1", "Ubicacion1", constructora, 1000, new List<Depto>());
            var encargado = new Encargado("Encargado1", "encargado@example.com", "password")
            {
                Edificios = new List<Edificio>()
            };

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns(admin);

            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion(It.IsAny<string>(), It.IsAny<string>()))
                                .Returns(edificio);

            _mockEncargadoService.Setup(service => service.GetEncargadoByEmail(It.IsAny<string>()))
                                 .Returns(encargado);

            _mockEncargadoService.Setup(service => service.GetAllEncargados())
                                 .Returns(new List<Encargado> { encargado });

            // Act
            _adminConstructoraService.AsignarEncargado("admin@example.com", "encargado@example.com", "Edificio1", "Direccion1");

            // Assert
            CollectionAssert.Contains(encargado.Edificios, edificio);
            _mockRepository.Verify(repo => repo.Update(encargado), Times.Once);
            _mockRepository.Verify(repo => repo.Save(), Times.Once);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "El edificio no pertenece a la constructora del usuario.")]
        public void AsignarEncargado_EdificioDoesNotBelongToConstructora_ThrowsException()
        {
            // Arrange
            var constructora1 = new Constructora("Constructora1");
            var constructora2 = new Constructora("Constructora2");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora1
            };

            var edificio = new Edificio("Edificio1", "Direccion1", "Ubicacion1", constructora2, 1000, new List<Depto>());
            var encargado = new Encargado("Encargado1", "encargado@example.com", "password")
            {
                Edificios = new List<Edificio>()
            };

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns(admin);

            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion(It.IsAny<string>(), It.IsAny<string>()))
                                .Returns(edificio);

            _mockEncargadoService.Setup(service => service.GetEncargadoByEmail(It.IsAny<string>()))
                                 .Returns(encargado);

            _mockEncargadoService.Setup(service => service.GetAllEncargados())
                                 .Returns(new List<Encargado> { encargado });

            // Act
            _adminConstructoraService.AsignarEncargado("admin@example.com", "encargado@example.com", "Edificio1", "Direccion1");
        }

        [TestMethod]
        public void GetEdificiosPorAdmin_ValidAdmin_ReturnsEdificios()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora
            };

            var edificios = new List<Edificio>
    {
        new Edificio("Edificio1", "Direccion1", "Ubicacion1", constructora, 1000, new List<Depto>()),
        new Edificio("Edificio2", "Direccion2", "Ubicacion2", constructora, 1000, new List<Depto>())
    };

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns(admin);

            _mockEdificioService.Setup(service => service.GetAllEdificios())
                                .Returns(edificios);

            // Act
            var result = _adminConstructoraService.GetEdificiosPorAdmin("admin@example.com");

            // Assert
            CollectionAssert.AreEqual(edificios, result);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException), "No se encontró el administrador.")]
        public void GetEdificiosPorAdmin_AdminNotFound_ThrowsException()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns<AdminConstructora>(null);

            // Act
            _adminConstructoraService.GetEdificiosPorAdmin("admin@example.com");
        }



        [TestMethod]
        public void GetEdificioPorNombreYDireccion_ValidEdificio_ReturnsEdificio()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var admin = new AdminConstructora("Admin", "a@a.com", "Contra123") { Constructora = constructora };
            var edificio = new Edificio("Edificio1", "Direccion1", "Ubicacion1", constructora, 1000, new List<Depto>());
            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>())).Returns(admin);

            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion(It.IsAny<string>(), It.IsAny<string>()))
                                .Returns(edificio);

            // Act
            var result = _adminConstructoraService.GetEdificioByNombreYDireccion("Edificio1", "Direccion1", "admin@example.com");

            // Assert
            Assert.AreEqual(edificio, result);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException), "No se encontró el edificio.")]
        public void GetEdificioPorNombreYDireccion_EdificioNotFound_ThrowsException()
        {
            // Arrange
            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion(It.IsAny<string>(), It.IsAny<string>()))
                                .Returns<Edificio>(null);

            // Act
            _adminConstructoraService.GetEdificioByNombreYDireccion("Edificio1", "Direccion1", "admin@example.com");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "El edificio no pertenece a la constructora del usuario.")]
        public void GetEdificioPorNombreYDireccion_EdificioDoesNotBelongToConstructora_ThrowsException()
        {
            // Arrange
            var constructora1 = new Constructora("Constructora1");
            var constructora2 = new Constructora("Constructora2");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password")
            {
                Constructora = constructora1
            };

            var edificio = new Edificio("Edificio1", "Direccion1", "Ubicacion1", constructora2, 1000, new List<Depto>());

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns(admin);

            _mockEdificioService.Setup(service => service.GetEdificioByNombreYDireccion(It.IsAny<string>(), It.IsAny<string>()))
                                .Returns(edificio);

            // Act
            _adminConstructoraService.GetEdificioByNombreYDireccion("Edificio1", "Direccion1", "admin@example.com");
        }


        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException), "El usuario no tiene constructora.")]
        public void GetEdificiosPorAdmin_AdminWithoutConstructora_ThrowsException()
        {
            // Arrange
            var admin = new AdminConstructora("Admin", "admin@example.com", "password");

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns(admin);

            // Act
            _adminConstructoraService.GetEdificiosPorAdmin("admin@example.com");
        }

        [TestMethod]
        public void ListarEdificiosSinEncargado_ValidAdmin_ReturnsEdificios()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password") { Constructora = constructora };
            var edificio1 = new Edificio("Edificio1", "Direccion1", "Ubicacion1", constructora, 1000, new List<Depto>());
            var edificio2 = new Edificio("Edificio2", "Direccion2", "Ubicacion2", constructora, 2000, new List<Depto>());
            var edificio3 = new Edificio("Edificio3", "Direccion3", "Ubicacion3", constructora, 3000, new List<Depto>());
            var edificios = new List<Edificio> { edificio1, edificio2, edificio3 };
            var encargado = new Encargado("Encargado1", "encargado@example.com", "Contra1213") { Edificios = new List<Edificio> { edificio1 } };

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns(admin);
            _mockEdificioService.Setup(service => service.GetAllEdificios())
                                .Returns(edificios);
            _mockEncargadoService.Setup(service => service.GetAllEncargados())
                                 .Returns(new List<Encargado> { encargado });

            // Act
            var result = _adminConstructoraService.GetEdificiosSinEncargado("admin@example.com");

            // Assert
            CollectionAssert.AreEqual(new List<Edificio> { edificio2, edificio3 }, result);
        }

        [TestMethod]
        public void ListarEdificiosConEncargado_ValidAdmin_ReturnsEdificios()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var admin = new AdminConstructora("Admin", "admin@example.com", "password") { Constructora = constructora };
            var edificio1 = new Edificio("Edificio1", "Direccion1", "Ubicacion1", constructora, 1000, new List<Depto>());
            var edificio2 = new Edificio("Edificio2", "Direccion2", "Ubicacion2", constructora, 2000, new List<Depto>());
            var edificio3 = new Edificio("Edificio3", "Direccion3", "Ubicacion3", constructora, 3000, new List<Depto>());
            var edificios = new List<Edificio> { edificio1, edificio2, edificio3 };
            var encargado = new Encargado("Encargado1", "encargado@example.com", "Contra1213") { Edificios = new List<Edificio> { edificio1 } };

            _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<List<string>>()))
                           .Returns(admin);
            _mockEdificioService.Setup(service => service.GetAllEdificios())
                                .Returns(edificios);
            _mockEncargadoService.Setup(service => service.GetAllEncargados())
                                 .Returns(new List<Encargado> { encargado });

            // Act
            var result = _adminConstructoraService.GetEdificiosConEncargado("admin@example.com");

            // Assert
            CollectionAssert.AreEqual(new List<Edificio> { edificio1 }, result);
        }

        [TestMethod]
        public void FiltrarPorNmobreDeEdificio_ValidNombre_ReturnsEdificios()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var edificio1 = new Edificio("Edificio1", "Direccion1", "Ubicacion1", constructora, 1000, new List<Depto>());
            var edificio2 = new Edificio("Edificio2", "Direccion2", "Ubicacion2", constructora, 2000, new List<Depto>());
            var edificios = new List<Edificio> { edificio1, edificio2 };

            // Act
            var result = _adminConstructoraService.FiltrarPorNombreDeEdificio(edificios, "Edificio1");

            // Assert
            CollectionAssert.AreEqual(new List<Edificio> { edificio1 }, result);
        }

        [TestMethod]
        public void FiltrarPorNombreDeEncargado_ValidNombre_ReturnsEdificios()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var edificio1 = new Edificio("Edificio1", "Direccion1", "Ubicacion1", constructora, 1000, new List<Depto>());
            var edificio2 = new Edificio("Edificio2", "Direccion2", "Ubicacion2", constructora, 2000, new List<Depto>());
            var edificios = new List<Edificio> { edificio1, edificio2 };
            var encargado = new Encargado("Encargado1", "encargado@example.com", "Contra1213") { Edificios = new List<Edificio> { edificio1 } };

            _mockEncargadoService.Setup(service => service.GetAllEncargados())
                                 .Returns(new List<Encargado> { encargado });

            // Act
            var result = _adminConstructoraService.FiltrarPorNombreDeEncargado(edificios, "Encargado1");

            // Assert
            CollectionAssert.AreEqual(new List<Edificio> { edificio1 }, result);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException), "No se encontró el encargado.")]
        public void FiltrarPorNombreDeEncargado_InvalidNombre_ThrowsException()
        {
            // Arrange
            var constructora = new Constructora("Constructora1");
            var edificio1 = new Edificio("Edificio1", "Direccion1", "Ubicacion1", constructora, 1000, new List<Depto>());
            var edificio2 = new Edificio("Edificio2", "Direccion2", "Ubicacion2", constructora, 2000, new List<Depto>());
            var edificios = new List<Edificio> { edificio1, edificio2 };

            _mockEncargadoService.Setup(service => service.GetAllEncargados())
                                 .Returns(new List<Encargado>());

            // Act
            _adminConstructoraService.FiltrarPorNombreDeEncargado(edificios, "EncargadoInexistente");
        }
    }
}