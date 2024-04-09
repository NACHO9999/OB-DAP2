namespace ob.Domain.Tests;

[TestClass]
public class EncargadoTest
{
    [TestMethod]
    public void NuevoEncargado()
    {
        // Arrange
        Encargado encargado = new Encargado("Juan", "Perez", "jperez@gmail.com", "Contra12345");

        // Act
        string nombre = encargado.Nombre;
        string apellido = encargado.Apellido;
        string email = encargado.Email;
        string contrasena = encargado.Contrasena;
        
        // Assert
        Assert.AreEqual("Juan", nombre);
        Assert.AreEqual("Perez", apellido);
        Assert.AreEqual("jperez@gmail.com", email);
        Assert.AreEqual("Contra12345", contrasena);
    }
}