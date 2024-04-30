using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ob.Domain;
using ob.IDataAccess;
using ob.IBusinessLogic;

namespace ob.BusinessLogic.Tests;

[TestClass]
public class AdminServiceTests
{
    private Mock<IUsuarioRepository> _mockRepository;
    private Mock<ICategoriaService> _mockCategoriaService;
    private Mock<IInvitacionService> _mockInvitacionService;
    private AdminService _adminService;

    [TestInitialize]
    public void InitTest()
    {
        _mockRepository = new Mock<IUsuarioRepository>(MockBehavior.Strict);
        _mockCategoriaService = new Mock<ICategoriaService>(MockBehavior.Strict);
        _mockInvitacionService = new Mock<IInvitacionService>(MockBehavior.Strict);
        _adminService = new AdminService(_mockRepository.Object, _mockCategoriaService.Object, _mockInvitacionService.Object);
    }

    [TestMethod]
    public void GetAllEncargados_ReturnsEncargados()
    {
        var expectedAdministradors = new List<Administrador> { new Administrador() };
        _mockRepository.Setup(r => r.GetAll<Administrador>()).Returns(expectedAdministradors);

        var result = _adminService.GetAllEncargados();

        Assert.IsTrue(expectedAdministradors.SequenceEqual(result));
        _mockRepository.Verify(r => r.GetAll<Administrador>(), Times.Once);
    }

    [TestMethod]
    public void CrearAdmin_CorrectlyInsertsAdmin()
    {
        var admin = new Administrador { Email = "newadmin@example.com" };
        _mockRepository.Setup(repo => repo.EmailExists(admin.Email)).Returns(false);
        _mockRepository.Setup(repo => repo.Insert(admin));
        _mockRepository.Setup(repo => repo.Save());

        _adminService.CrearAdmin(admin);

        _mockRepository.VerifyAll();
    }

    [ExpectedException(typeof(Exception), "El email ya existe")]
    [TestMethod]
    public void CrearAdmin_ThrowsExceptionWhenEmailExists()
    {
        var admin = new Administrador { Email = "existingadmin@example.com" };
        _mockRepository.Setup(repo => repo.EmailExists(admin.Email)).Returns(true);

        _adminService.CrearAdmin(admin);
    }

    [TestMethod]
    public void GetAdminByEmail_ReturnsAdmin()
    {
        var email = "admin@example.com";
        var expectedAdmin = new Administrador { Email = email };
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Func<Usuario, bool>>())).Returns(expectedAdmin);

        var result = _adminService.GetAdminByEmail(email);

        Assert.AreEqual(expectedAdmin, result);
        _mockRepository.Verify(repo => repo.Get(It.IsAny<Func<Usuario, bool>>()), Times.Once);
    }

    [ExpectedException(typeof(Exception), "No Administrador found with the specified email.")]
    [TestMethod]
    public void GetAdminByEmail_ThrowsWhenAdminNotFound()
    {
        var email = "nonexistent@example.com";
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Func<Usuario, bool>>())).Returns((Administrador)null);

        _adminService.GetAdminByEmail(email);
    }

    [TestMethod]
    public void InvitarEncargado_CreatesInvitation()
    {
        var email = "newinvite@example.com";
        var name = "New Invitee";
        var date = DateTime.Now;
        _mockInvitacionService.Setup(service => service.CrearInvitacion(It.IsAny<Invitacion>()));
        _mockRepository.Setup(repo => repo.Save());

        _adminService.InvitarEncargado(email, name, date);

        _mockInvitacionService.Verify(service => service.CrearInvitacion(It.IsAny<Invitacion>()), Times.Once);
        _mockRepository.Verify(repo => repo.Save(), Times.Once);
    }

    [TestMethod]
    public void EliminarInvitacion_DeletesInvitation()
    {
        var email = "deleteinvite@example.com";
        _mockInvitacionService.Setup(service => service.EliminarInvitacion(email));
        _mockRepository.Setup(repo => repo.Save());

        _adminService.EliminarInvitacion(email);

        _mockInvitacionService.Verify(service => service.EliminarInvitacion(email), Times.Once);
        _mockRepository.Verify(repo => repo.Save(), Times.Once);
    }

    [TestMethod]
    public void AltaCategoria_CreatesCategory()
    {
        var categoria = new Categoria { Nombre = "New Category" };
        _mockCategoriaService.Setup(service => service.CrearCategoria(categoria));

        _adminService.AltaCategoria(categoria);

        _mockCategoriaService.Verify(service => service.CrearCategoria(categoria), Times.Once);
    }
}
