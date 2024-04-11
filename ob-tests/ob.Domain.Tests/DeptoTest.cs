using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ob.Domain.Tests
{
    [TestClass]
    public class DeptoTests
    {

        [TestMethod]
        public void NuevoDepto(){
            //Arrange
            int piso = 1;
            int numero = 1;
            Dueno dueno1 = new Dueno();
            int cantidadCuartos = 1;
            int cantidadBanos = 1;
            bool conTerraza = true;

            //Act
            Depto depto = new Depto(piso, numero, dueno1, cantidadCuartos, cantidadBanos, conTerraza);

            //Assert
            Assert.AreEqual(piso, depto.Piso);
            Assert.AreEqual(numero, depto.Numero);
            Assert.AreEqual(dueno1, depto.Dueno);
            Assert.AreEqual(cantidadCuartos, depto.CantidadCuartos);
            Assert.AreEqual(cantidadBanos, depto.CantidadBanos);
            Assert.AreEqual(conTerraza, depto.ConTerraza);
        }

        [TestMethod]
        public void Constructor_ValidValues_SetProperties()
        {
            // Arrange
            int piso = 5;
            int numero = 101;
            Dueno dueno = new Dueno("John", "Doe", "john.doe@example.com");
            int cantidadCuartos = 3;
            int cantidadBanos = 2;
            bool conTerraza = true;

            // Act
            Depto depto = new Depto(piso, numero, dueno, cantidadCuartos, cantidadBanos, conTerraza);

            // Assert
            Assert.AreEqual(piso, depto.Piso);
            Assert.AreEqual(numero, depto.Numero);
            Assert.AreEqual(dueno, depto.Dueno);
            Assert.AreEqual(cantidadCuartos, depto.CantidadCuartos);
            Assert.AreEqual(cantidadBanos, depto.CantidadBanos);
            Assert.AreEqual(conTerraza, depto.ConTerraza);
        }

        [TestMethod]
        public void Piso_SetValidValue_Success()
        {
            // Arrange
            Depto depto = new Depto(5, 101, null, 3, 2, true);

            // Act
            depto.Piso = 10;

            // Assert
            Assert.AreEqual(10, depto.Piso);
        }

        [TestMethod]
        public void Piso_SetInvalidValue_ThrowsException()
        {
            // Arrange
            Depto depto = new Depto(5, 101, null, 3, 2, true);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => depto.Piso = -1);
        }

        [TestMethod]
        public void Numero_SetValidValue_Success()
        {
            // Arrange
            Depto depto = new Depto(5, 101, null, 3, 2, true);

            // Act
            depto.Numero = 102;

            // Assert
            Assert.AreEqual(102, depto.Numero);
        }

        [TestMethod]
        public void Numero_SetInvalidValue_ThrowsException()
        {
            // Arrange
            Depto depto = new Depto(5, 101, null, 3, 2, true);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => depto.Numero = -1);
        }

        [TestMethod]
        public void Dueno_SetValidValue_Success()
        {
            // Arrange
            Depto depto = new Depto(5, 101, null, 3, 2, true);
            Dueno newDueno = new Dueno("Jane", "Doe", "jane.doe@example.com");

            // Act
            depto.Dueno = newDueno;

            // Assert
            Assert.AreEqual(newDueno, depto.Dueno);
        }

        [TestMethod]
        public void CantidadCuartos_SetValidValue_Success()
        {
            // Arrange
            Depto depto = new Depto(5, 101, null, 3, 2, true);

            // Act
            depto.CantidadCuartos = 4;

            // Assert
            Assert.AreEqual(4, depto.CantidadCuartos);
        }

        [TestMethod]
        public void CantidadCuartos_SetInvalidValue_ThrowsException()
        {
            // Arrange
            Depto depto = new Depto(5, 101, null, 3, 2, true);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => depto.CantidadCuartos = 0);
        }

        [TestMethod]
        public void CantidadBanos_SetValidValue_Success()
        {
            // Arrange
            Depto depto = new Depto(5, 101, null, 3, 2, true);

            // Act
            depto.CantidadBanos = 3;

            // Assert
            Assert.AreEqual(3, depto.CantidadBanos);
        }

        [TestMethod]
        public void CantidadBanos_SetInvalidValue_ThrowsException()
        {
            // Arrange
            Depto depto = new Depto(5, 101, null, 3, 2, true);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => depto.CantidadBanos = 0);
        }

        [TestMethod]
        public void ConTerraza_SetValidValue_Success()
        {
            // Arrange
            Depto depto = new Depto(5, 101, null, 3, 2, true);

            // Act
            depto.ConTerraza = false;

            // Assert
            Assert.IsFalse(depto.ConTerraza);
        }
    }
}

