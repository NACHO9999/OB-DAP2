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
        string descripcion = "Reparar puerta";
        Depto depto = new Depto();
        EstadoSolicitud estado = new EstadoSolicitud("Abierto");

    }
}