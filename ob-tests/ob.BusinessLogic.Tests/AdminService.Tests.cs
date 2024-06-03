using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using ob.Domain;
using ob.Exceptions.BusinessLogicExceptions;
using ob.BusinessLogic;
using ob.IBusinessLogic;
using ob.IDataAccess;
using System.Linq.Expressions;
using Enums;

[TestClass]
public class AdminServiceTests
{
    private Mock<IUsuarioRepository> _mockUsuarioRepository;
    private Mock<ICategoriaService> _mockCategoriaService;
    private Mock<IInvitacionService> _mockInvitacionService;
    private IAdminService _adminService;

    [TestInitialize]
    public void Initialize()
    {
        _mockUsuarioRepository = new Mock<IUsuarioRepository>();
        _mockCategoriaService = new Mock<ICategoriaService>();
        _mockInvitacionService = new Mock<IInvitacionService>();

        _adminService = new AdminService(
            _mockUsuarioRepository.Object,
            _mockCategoriaService.Object,
            _mockInvitacionService.Object);
    }

    [TestMethod]
    public void CrearAdmin_EmailExists_ThrowsAlreadyExistsException()
    {
        // Arrange
        var admin = new Administrador ("john", "doe", "jd@gmaial.com", "Abc12334");
        _mockUsuarioRepository.Setup(repo => repo.EmailExists(admin.Email)).Returns(true);

        // Act & Assert
        Assert.ThrowsException<AlreadyExistsException>(() => _adminService.CrearAdmin(admin));
    }

    [TestMethod]
    public void CrearAdmin_EmailDoesNotExist_InsertsAdminAndSaves()
    {
        // Arrange
        var admin = new Administrador("john", "doe", "johnd@gmaial.com", "Abc12334");
        _mockUsuarioRepository.Setup(repo => repo.EmailExists(admin.Email)).Returns(false);

        // Act
        _adminService.CrearAdmin(admin);

        // Assert
        _mockUsuarioRepository.Verify(repo => repo.Insert(admin), Times.Once);
        _mockUsuarioRepository.Verify(repo => repo.Save(), Times.Once);
    }

    [TestMethod]
    public void GetAdminByEmail_AdminExists_ReturnsAdmin()
    {
        // Arrange
        var adminEmail = "existingEmail@example.com";
        var existingAdmin = new Administrador("john", "doe", adminEmail, "Abc12334");
        _mockUsuarioRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Usuario, bool>>>(), null)).Returns(existingAdmin);

        // Act
        var result = _adminService.GetAdminByEmail(adminEmail);

        // Assert
        Assert.AreEqual(existingAdmin, result);
    }

    [TestMethod]
    public void InvitarEncargado_ValidParameters_CreatesInvitationAndSaves()
    {
        // Arrange
        var email = "test@example.com";
        var nombre = "John Doe";
        var fecha = DateTime.Now;
        _mockInvitacionService.Setup(repo => repo.CrearInvitacion(It.IsAny<Invitacion>()));

        // Act
        _adminService.Invitar(email, nombre, fecha,RolInvitaciion.Encargado);

        // Assert
        _mockInvitacionService.Verify(repo => repo.CrearInvitacion(It.IsAny<Invitacion>()), Times.Once);
        _mockUsuarioRepository.Verify(repo => repo.Save(), Times.Once);
    }

    [TestMethod]
    public void EliminarInvitacion_ValidEmail_DeletesInvitationAndSaves()
    {
        // Arrange
        var email = "test@example.com";
        _mockInvitacionService.Setup(repo => repo.EliminarInvitacion(email));

        // Act
        _adminService.EliminarInvitacion(email);

        // Assert
        _mockInvitacionService.Verify(repo => repo.EliminarInvitacion(email), Times.Once);
        _mockUsuarioRepository.Verify(repo => repo.Save(), Times.Once);
    }

    [TestMethod]
    public void AltaCategoria_ValidCategory_CreatesCategory()
    {
        // Arrange
        var categoria = new Categoria ("Test");
        _mockCategoriaService.Setup(repo => repo.CrearCategoria(categoria));

        // Act
        _adminService.AltaCategoria(categoria);

        // Assert
        _mockCategoriaService.Verify(repo => repo.CrearCategoria(categoria), Times.Once);
    }
    [TestMethod]
    public void CrearAdmin_DuplicateEmail_ThrowsAlreadyExistsException()
    {
        // Arrange
        var admin = new Administrador("John", "Doe", "john@example.com", "password");

        // Setup mock repository to return true, indicating duplicate email
        _mockUsuarioRepository.Setup(repo => repo.EmailExists(admin.Email)).Returns(true);

        // Act & Assert
        Assert.ThrowsException<AlreadyExistsException>(() => _adminService.CrearAdmin(admin));
    }
}

    // Write more test methods to cover other scenarios such as CrearAdmin, GetAdminByEmail, etc.
