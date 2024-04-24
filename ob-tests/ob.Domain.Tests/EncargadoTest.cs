namespace ob.Domain.Tests;



using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class EncargadoTests
{
    [TestMethod]
    public void Nombre_SetValidValue_Success()
    {
        // Arrange
        Encargado usuario = new Encargado("John",  "john.doe@example.com", "password");

        // Act
        usuario.Nombre = "Jane";

        // Assert
        Assert.AreEqual("Jane", usuario.Nombre);
    }

    [TestMethod]
    public void Nombre_SetInvalidValue_ThrowsException()
    {
        // Arrange
        Encargado usuario = new Encargado("John",  "john.doe@example.com", "password");

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => usuario.Nombre = "");
    }

    [TestMethod]
    public void Apellido_SetValidValue_Success()
    {
        // Arrange
        Encargado usuario = new Encargado("John",  "john.doe@example.com", "password");

        // Act
        usuario.Apellido = "Smith";

        // Assert
        Assert.AreEqual("Smith", usuario.Apellido);
    }

    [TestMethod]
    public void Apellido_SetInvalidValue_ThrowsException()
    {
        // Arrange
        Encargado usuario = new Encargado("John",  "john.doe@example.com", "password");

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => usuario.Apellido = "");
    }

    [TestMethod]
    public void Email_SetValidValue_Success()
    {
        // Arrange
        Encargado usuario = new Encargado("John",  "john.doe@example.com", "password");

        // Act
        usuario.Email = "jane.smith@example.com";

        // Assert
        Assert.AreEqual("jane.smith@example.com", usuario.Email);
    }

    [TestMethod]
    public void Email_SetInvalidValue_ThrowsException()
    {
        // Arrange
        Encargado usuario = new Encargado("John",  "john.doe@example.com", "password");

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => usuario.Email = "invalid-email");
    }
    
    
}