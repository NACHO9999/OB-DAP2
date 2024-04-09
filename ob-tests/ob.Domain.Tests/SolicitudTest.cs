using Enums

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
        EstadoSolicitud estado = EstadoSolicitud.Abierto;
        DateTime fechaInicio = DateTime.Now;
        DateTime fechaFin = DateTime.Now;

    }
}