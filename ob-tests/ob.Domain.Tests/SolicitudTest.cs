using Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ob.Domain.Tests;

[TestClass]
public class SolicitudTests
{
    [TestMethod]
    public void PerMan_SetValidValue_Success()
    {
        // Arrange
        Mantenimiento mantenimiento = new Mantenimiento("John", "Doe", "john.doe@example.com", "password");
        Solicitud solicitud = new Solicitud(mantenimiento, "Test Description", new Depto(1, 101, null, 2, 2, false), EstadoSolicitud.Atendiendo, DateTime.Now);

        // Act
        Mantenimiento newMantenimiento = new Mantenimiento("Jane", "Smith", "jane.smith@example.com", "password");
        solicitud.PerMan = newMantenimiento;

        // Assert
        Assert.AreEqual(newMantenimiento, solicitud.PerMan);
    }

    [TestMethod]
    public void Descripcion_SetValidValue_Success()
    {
        // Arrange
        Solicitud solicitud = new Solicitud(new Mantenimiento("John", "Doe", "john.doe@example.com", "password"), "Test Description", new Depto(1, 101, null, 2, 2, false), EstadoSolicitud.Atendiendo, DateTime.Now);

        // Act
        solicitud.Descripcion = "Updated Description";

        // Assert
        Assert.AreEqual("Updated Description", solicitud.Descripcion);
    }

    [TestMethod]
    public void Depto_SetValidValue_Success()
    {
        // Arrange
        Depto depto = new Depto(1, 101, null, 2, 2, false);
        Solicitud solicitud = new Solicitud(new Mantenimiento("John", "Doe", "john.doe@example.com", "password"), "Test Description", depto, EstadoSolicitud.Atendiendo, DateTime.Now);

        // Act
        solicitud.Depto = new Depto(2, 102, null, 3, 3, true);

        // Assert
        Assert.IsNotNull(solicitud.Depto);
    }

    [TestMethod]
    public void Estado_SetValidValue_Success()
    {
        // Arrange
        Solicitud solicitud = new Solicitud(new Mantenimiento("John", "Doe", "john.doe@example.com", "password"), "Test Description", new Depto(1, 101, null, 2, 2, false), EstadoSolicitud.Atendiendo, DateTime.Now);

        // Act
        solicitud.Estado = EstadoSolicitud.Abierto;

        // Assert
        Assert.AreEqual(EstadoSolicitud.Abierto, solicitud.Estado);
    }

    [TestMethod]
    public void FechaInicio_SetValidValue_Success()
    {
        // Arrange
        DateTime validDate = DateTime.Now.AddDays(-1);
        Solicitud solicitud = new Solicitud(new Mantenimiento("John", "Doe", "john.doe@example.com", "password"), "Test Description", new Depto(1, 101, null, 2, 2, false), EstadoSolicitud.Atendiendo, DateTime.Now);

        // Act
        solicitud.FechaInicio = validDate;

        // Assert
        Assert.AreEqual(validDate, solicitud.FechaInicio);
    }
    [TestMethod]
    public void PerMan_SetInvalidValue_ThrowsArgumentException()
    {
        // Arrange
        Solicitud solicitud = new Solicitud(new Mantenimiento("John", "Doe", "john.doe@example.com", "password"), "Test Description", new Depto(1, 101, null, 2, 2, false), EstadoSolicitud.Atendiendo, DateTime.Now);

        // Act & Assert
        Assert.ThrowsException<ArgumentNullException>(() => solicitud.PerMan = null);
    }

    [TestMethod]
    public void Descripcion_SetInvalidValue_ThrowsArgumentException()
    {
        // Arrange
        Solicitud solicitud = new Solicitud(new Mantenimiento("John", "Doe", "john.doe@example.com", "password"), "Test Description", new Depto(1, 101, null, 2, 2, false), EstadoSolicitud.Atendiendo, DateTime.Now);

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => solicitud.Descripcion = "");
    }

    [TestMethod]
    public void Depto_SetInvalidValue_ThrowsArgumentException()
    {
        // Arrange
        Solicitud solicitud = new Solicitud(new Mantenimiento("John", "Doe", "john.doe@example.com", "password"), "Test Description", new Depto(1, 101, null, 2, 2, false), EstadoSolicitud.Atendiendo, DateTime.Now);

        // Act & Assert
        Assert.ThrowsException<ArgumentNullException>(() => solicitud.Depto = null);
    }


}