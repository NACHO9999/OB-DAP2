namespace ob.Domain.Tests;

[TestClass]
public class DuenoTest
{
    [TestMethod]
    public void NuevoDueno()
    {
        // Arrange
        var dueno = new Dueno
        {
            Nombre = "Juan",
            Apellido = "Perez",
            Email = "jperez@gmail.com"
        };

        // Act
        var nombre = dueno.Nombre;
        var apellido = dueno.Apellido;
        var email = dueno.Email;

        // Assert
        Assert.AreEqual("Juan", nombre);
        Assert.AreEqual("Perez", apellido);
        Assert.AreEqual("jperez@gmail.com", email);
    }
}
