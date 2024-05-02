using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace ob.Domain.Tests;




[TestClass]
public class DuenoTests
{
    [TestMethod]
    public void Nombre_SetValidValue_Success()
    {
        // Arrange
        Dueno dueno = new Dueno("John", "Doe", "john.doe@example.com");

        // Act
        dueno.Nombre = "Jane";

        // Assert
        Assert.AreEqual("Jane", dueno.Nombre);
    }

    [TestMethod]
    public void Nombre_SetInvalidValue_ThrowsArgumentException()
    {
        // Arrange
        Dueno dueno = new Dueno("John", "Doe", "john.doe@example.com");

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => dueno.Nombre = "");
    }

    [TestMethod]
    public void Apellido_SetValidValue_Success()
    {
        // Arrange
        Dueno dueno = new Dueno("John", "Doe", "john.doe@example.com");

        // Act
        dueno.Apellido = "Smith";

        // Assert
        Assert.AreEqual("Smith", dueno.Apellido);
    }

    [TestMethod]
    public void Apellido_SetInvalidValue_ThrowsArgumentException()
    {
        // Arrange
        Dueno dueno = new Dueno("John", "Doe", "john.doe@example.com");

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => dueno.Apellido = "");
    }

    [TestMethod]
    public void Email_SetValidValue_Success()
    {
        // Arrange
        Dueno dueno = new Dueno("John", "Doe", "john.doe@example.com");

        // Act
        dueno.Email = "jane.smith@example.com";

        // Assert
        Assert.AreEqual("jane.smith@example.com", dueno.Email);
    }

    [TestMethod]
    public void Email_SetInvalidValue_ThrowsArgumentException()
    {
        // Arrange
        Dueno dueno = new Dueno("John", "Doe", "john.doe@example.com");

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => dueno.Email = "invalid-email");
    }
}