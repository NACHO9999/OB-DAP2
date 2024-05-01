using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ob.BusinessLogic;
using ob.Domain;
using ob.Exceptions.BusinessLogicExceptions;
using ob.IBusinessLogic;
using ob.IDataAccess;
using System;
using System.Linq.Expressions;

[TestClass]
public class InvitacionServiceTests
{
    private Mock<IGenericRepository<Invitacion>> _mockRepository;
    private IInvitacionService _invitacionService;

    [TestInitialize]
    public void Initialize()
    {
        _mockRepository = new Mock<IGenericRepository<Invitacion>>();
        _invitacionService = new InvitacionService(_mockRepository.Object);
    }

    [TestMethod]
    public void CrearInvitacion_InvitacionYaExiste_ThrowsAlreadyExistsException()
    {
        // Arrange
        var invitacion = new Invitacion("test@example.com", "Test User", DateTime.Now);
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Invitacion, bool>>>())).Returns(invitacion);

        // Act & Assert
        Assert.ThrowsException<AlreadyExistsException>(() => _invitacionService.CrearInvitacion(invitacion));
    }

    [TestMethod]
    public void CrearInvitacion_InvitacionNueva_InsertsInvitacion()
    {
        // Arrange
        var invitacion = new Invitacion("new@example.com", "New User", DateTime.Now);
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Invitacion, bool>>>())).Returns((Invitacion)null);
        _mockRepository.Setup(repo => repo.Insert(invitacion)).Verifiable();
        _mockRepository.Setup(repo => repo.Save()).Verifiable();

        // Act
        _invitacionService.CrearInvitacion(invitacion);

        // Assert
        _mockRepository.Verify(repo => repo.Insert(invitacion), Times.Once);
        _mockRepository.Verify(repo => repo.Save(), Times.Once);
    }

    [TestMethod]
    public void GetInvitacionByEmail_ExistingEmail_ReturnsInvitacion()
    {
        // Arrange
        var email = "test@example.com";
        var invitacion = new Invitacion(email, "Test User", DateTime.Now);
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Invitacion, bool>>>())).Returns(invitacion);

        // Act
        var result = _invitacionService.GetInvitacionByEmail(email);

        // Assert
        Assert.AreEqual(invitacion, result);
    }

    [TestMethod]
    public void EliminarInvitacion_NoExiste_ThrowsResourceNotFoundException()
    {
        // Arrange
        var email = "missing@example.com";
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Invitacion, bool>>>())).Returns((Invitacion)null);

        // Act & Assert
        Assert.ThrowsException<ResourceNotFoundException>(() => _invitacionService.EliminarInvitacion(email));
    }

    [TestMethod]
    public void EliminarInvitacion_ExistingEmail_DeletesInvitacion()
    {
        // Arrange
        var email = "test@example.com";
        var invitacion = new Invitacion(email, "Test User", DateTime.Now);
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Invitacion, bool>>>())).Returns(invitacion);
        _mockRepository.Setup(repo => repo.Delete(invitacion)).Verifiable();
        _mockRepository.Setup(repo => repo.Save()).Verifiable();

        // Act
        _invitacionService.EliminarInvitacion(email);

        // Assert
        _mockRepository.Verify(repo => repo.Delete(invitacion), Times.Once);
        _mockRepository.Verify(repo => repo.Save(), Times.Once);
    }

    [TestMethod]
    public void InvitacionAceptada_CreatesEncargadoAndDeletesInvitacion()
    {
        // Arrange
        var invitacion = new Invitacion("accept@example.com", "Accept User", DateTime.Now);
        var password = "SecurePassword123";
        _mockRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Invitacion, bool>>>())).Returns(invitacion);
        _mockRepository.Setup(repo => repo.Delete(invitacion)).Verifiable();
        _mockRepository.Setup(repo => repo.Save()).Verifiable();

        // Act
        var result = _invitacionService.InvitacionAceptada(invitacion, password);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(invitacion.Email, result.Email);
        _mockRepository.Verify(repo => repo.Delete(invitacion), Times.Once);
        _mockRepository.Verify(repo => repo.Save(), Times.Twice); // Once for delete and once for save
    }
}
