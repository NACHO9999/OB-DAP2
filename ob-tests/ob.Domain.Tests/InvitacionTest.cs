namespace ob.Domain.Tests;

[TestClass]
public class InvitacionTest
{
    [TestMethod]
    public void NuevaInvitacion()
    {
        //Arrange

        string email = "jsosa@gmail.com";
        string nombre = "Juan";
        DateTime fechaExpiracion = DateTime.Now;

        //Act
        Invitacion nuevaInvitacion = new Invitacion
        {
            Email = email,
            Nombre = nombre,
            FechaExpiracion = fechaExpiracion
        };

        //Assert
        Assert.AreEqual(email, nuevaInvitacion.Email);
        Assert.AreEqual(nombre, nuevaInvitacion.Nombre);
        Assert.AreEqual(fechaExpiracion, nuevaInvitacion.FechaExpiracion);
    }
}