using Enums;

namespace ob.Domain.Tests;

[TestClass]
public class SolicitudTest
{
    [TestMethod]
    public void NuevaSolicitud()
    {
        //Arrange
        Mantenimiento perMan = new Mantenimiento(
            "Juan",
            "Sosa",
            "jsosa@gmail.com",
            "Contra12345"
        );
        Dueno dueno = new Dueno();
        string descripcion = "Reparar puerta";
        Depto depto = new Depto(1, 1, dueno, 1, 1, true);

        //Act
        Solicitud solicitud = new Solicitud(perMan, descripcion, depto);

        //Assert
        Assert.AreEqual(perMan, solicitud.PerMan);
        Assert.AreEqual(descripcion, solicitud.Descripcion);
        Assert.AreEqual(depto, solicitud.Depto);
        Assert.AreEqual(EstadoSolicitud.Abierto, solicitud.Estado);
    }
}