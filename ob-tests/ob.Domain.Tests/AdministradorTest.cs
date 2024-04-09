namespace ob.Domain.Tests;

[TestClass]
public class AdministradorTest
{
    [TestMethod]
    public void NuevoAdministrador()
    {
        //Arrange & Act
        Administrador nuevoAdministrador = new Administrador("Juan", "Sosa", "jsosa@gmail.com", "Contra12345");

        //Assert
        Assert.AreEqual("Juan", nuevoAdministrador.Nombre);
        Assert.AreEqual("Sosa", nuevoAdministrador.Apellido);
        Assert.AreEqual("jsosa@gmail.com", nuevoAdministrador.Email);
        Assert.AreEqual("Contra12345", nuevoAdministrador.Contrasena);
    }
}
