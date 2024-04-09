namespace ob.Domain.Tests;

[TestClass]
public class MantenimientoTest
{
    [TestMethod]
    public void NuevoMantenimiento()
    {
        //Arrange & Act
        Mantenimiento nuevoMantenimiento = new Mantenimiento(
            "Juan",
            "Sosa",
            "jsosa@gmail.com",
            "Contra12345"
        );

        //Assert
        Assert.AreEqual("Juan", nuevoMantenimiento.Nombre);
        Assert.AreEqual("Sosa", nuevoMantenimiento.Apellido);
        Assert.AreEqual("jsosa@gmail.com", nuevoMantenimiento.Email);
        Assert.AreEqual("Contra12345", nuevoMantenimiento.Contrasena);
    }
}
