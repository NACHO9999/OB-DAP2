namespace ob.Domain.Tests;

[TestClass]
public class AdministradorTest
{
    [TestMethod]
    public void NuevoAdministrador()
    {
        //Arrange & Act
        Administrador nuevoAdministrador = new("Juan", "Sosa", "jsosa@gmail.com", "Contra12345");

        //Assert
        Assert.AreEqual("Juan", nuevoAdministrador.Nombre);
        Assert.AreEqual("Sosa", nuevoAdministrador.Apellido);
        Assert.AreEqual("jsosa@gmail.com", nuevoAdministrador.Email);
        Assert.AreEqual("Contra12345", nuevoAdministrador.Contrasena);
    }

    [TestMethod]
    public void AltaAdministrador()
    {
        //Arrange
        Administrador nuevoAdministrador = new();

        //Act
        Administrador administrador = nuevoAdministrador.AltaAdministrador(
            "Juan",
            "Sosa",
            "jsosa@gmail.com",
            "Contra12345"
        );

        //Assert
        Assert.AreEqual("Juan", administrador.Nombre);
        Assert.AreEqual("Sosa", administrador.Apellido);
        Assert.AreEqual("jsosa@gmail.com", administrador.Email);
        Assert.AreEqual("Contra12345", administrador.Contrasena);
    }

    [TestMethod]
    public void AltaCategoria()
    {
        //Arrange
        Administrador nuevoAdministrador = new();

        //Act
        Categoria categoria = nuevoAdministrador.AltaCategoria("Limpieza");

        //Assert
        Assert.AreEqual("Limpieza", categoria.Nombre);
    }
}
