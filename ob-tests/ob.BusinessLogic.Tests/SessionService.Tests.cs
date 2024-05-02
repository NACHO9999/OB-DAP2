using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ob.BusinessLogic;
using ob.IBusinessLogic;
using ob.Domain;
using ob.Exceptions.BusinessLogicExceptions;
using ob.IDataAccess;
using System;
using System.Collections.Generic;

[TestClass]
public class SessionServiceTests
{
    private Mock<IUsuarioRepository> _mockUsuarioRepository;
    private Mock<IGenericRepository<Session>> _mockSessionRepository;
    private ISessionService _sessionService;

    [TestInitialize]
    public void Initialize()
    {
        _mockUsuarioRepository = new Mock<IUsuarioRepository>();
        _mockSessionRepository = new Mock<IGenericRepository<Session>>();
        _sessionService = new SessionService(_mockUsuarioRepository.Object, _mockSessionRepository.Object);
    }

    [TestMethod]
    public void Authenticate_ValidCredentials_ReturnsAuthToken()
    {
        // Arrange
        var email = "test@example.com";
        var password = "correctpassword";
        var user = new Usuario { Email = email, Contrasena = password };
        var session = new Session { Usuario = user, AuthToken = Guid.NewGuid() };

        _mockUsuarioRepository.Setup(repo => repo.Get(It.IsAny<Func<Usuario, bool>>())).Returns(user);
        _mockSessionRepository.Setup(repo => repo.Insert(session)).Verifiable();
        _mockSessionRepository.Setup(repo => repo.Save()).Verifiable();

        // Act
        var result = _sessionService.Authenticate(email, password);

        // Assert
        Assert.AreEqual(session.AuthToken, result);
        _mockSessionRepository.Verify(repo => repo.Insert(It.IsAny<Session>()), Times.Once());
        _mockSessionRepository.Verify(repo => repo.Save(), Times.Once());
    }

    [TestMethod]
    public void Authenticate_InvalidCredentials_ThrowsInvalidCredentialException()
    {
        // Arrange
        var email = "wrong@example.com";
        var password = "wrongpassword";

        _mockUsuarioRepository.Setup(repo => repo.Get(It.IsAny<Func<Usuario, bool>>())).Returns((Usuario)null);

        // Act & Assert
        Assert.ThrowsException<InvalidCredentialException>(() => _sessionService.Authenticate(email, password));
    }

    [TestMethod]
    public void GetCurrentUser_WithAuthToken_ReturnsCurrentUser()
    {
        // Arrange
        var authToken = Guid.NewGuid();
        var user = new Usuario { Email = "user@example.com" };
        var session = new Session { Usuario = user, AuthToken = authToken };

        _mockSessionRepository.Setup(repo => repo.Get(It.IsAny<Func<Session, bool>>(), It.IsAny<List<string>>())).Returns(session);

        // Act
        var result = _sessionService.GetCurrentUser(authToken);

        // Assert
        Assert.AreEqual(user, result);
    }

    [TestMethod]
    public void GetCurrentUser_NoAuthTokenProvided_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => _sessionService.GetCurrentUser());
    }

    [TestMethod]
    public void GetCurrentUser_NoSessionFound_ReturnsNull()
    {
        // Arrange
        var authToken = Guid.NewGuid();

        _mockSessionRepository.Setup(repo => repo.Get(It.IsAny<Func<Session, bool>>(), It.IsAny<List<string>>())).Returns((Session)null);

        // Act
        var result = _sessionService.GetCurrentUser(authToken);

        // Assert
        Assert.IsNull(result);
    }
}
