namespace ob.Domain.Tests;

[TestClass]
public class InvitacionTests
{
    [TestMethod]
    public void Email_SetValidValue_Success()
    {
        // Arrange
        Invitacion invitacion = new Invitacion("test@example.com", "John Doe", DateTime.Now.AddDays(7));

        // Act
        invitacion.Email = "another@example.com";

        // Assert
        Assert.AreEqual("another@example.com", invitacion.Email);
    }

    [TestMethod]
    public void Email_SetInvalidValue_ThrowsArgumentException()
    {
        // Arrange
        Invitacion invitacion = new Invitacion("test@example.com", "John Doe", DateTime.Now.AddDays(7));

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => invitacion.Email = "invalid-email");
    }

    [TestMethod]
    public void Nombre_SetValidValue_Success()
    {
        // Arrange
        Invitacion invitacion = new Invitacion("test@example.com", "John Doe", DateTime.Now.AddDays(7));

        // Act
        invitacion.Nombre = "Jane Smith";

        // Assert
        Assert.AreEqual("Jane Smith", invitacion.Nombre);
    }

    [TestMethod]
    public void Nombre_SetInvalidValue_ThrowsArgumentException()
    {
        // Arrange
        Invitacion invitacion = new Invitacion("test@example.com", "John Doe", DateTime.Now.AddDays(7));

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => invitacion.Nombre = "");
    }

    [TestMethod]
    public void FechaExpiracion_SetValidValue_Success()
    {
        // Arrange
        DateTime validDate = DateTime.Now.AddDays(7);
        Invitacion invitacion = new Invitacion("test@example.com", "John Doe", validDate);

        // Act
        invitacion.FechaExpiracion = validDate.AddDays(1);

        // Assert
        Assert.AreEqual(validDate.AddDays(1), invitacion.FechaExpiracion);
    }

    [TestMethod]
    public void FechaExpiracion_SetPastDate_ThrowsArgumentException()
    {
        // Arrange
        DateTime pastDate = DateTime.Now.AddDays(-1);
        Invitacion invitacion = new Invitacion("test@example.com", "John Doe", DateTime.Now.AddDays(7));

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => invitacion.FechaExpiracion = pastDate);
    }
}