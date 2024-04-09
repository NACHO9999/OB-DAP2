using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ob.Domain;

namespace ob.Domain.Tests
{
    [TestClass]
    public class DeptoTest
    {

        [TestMethod]
        public void NuevoDepto(){
            //Arrange
            Dueno dueno = new Dueno();
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
        [ExpectedException(typeof(ArgumentException))]
        public void NuevoDepto_InvalidPiso()
        {
            // Arrange
            int expectedPiso = -1; // Invalid piso value
            int expectedNumero = 1;
            Dueno expectedDueno = new Dueno();
            int expectedCantidadCuartos = 1;
            int expectedCantidadBanos = 1;
            bool expectedConTerraza = true;

            // Act
            Depto depto = new Depto(expectedPiso, expectedNumero, expectedDueno, expectedCantidadCuartos, expectedCantidadBanos, expectedConTerraza)
            {
                Piso = expectedPiso,
                Numero = expectedNumero,
                Dueno = expectedDueno,
                CantidadCuartos = expectedCantidadCuartos,
                CantidadBanos = expectedCantidadBanos,
                ConTerraza = expectedConTerraza
            };

            // Assert
            // Exception is expected to be thrown
        
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NuevoDepto_InvalidNumero_ExceptionThrown()
        {
            // Arrange
            int expectedPiso = 0;
            int expectedNumero = 0; // Invalid numero value
            Dueno expectedDueno = new Dueno();
            int expectedCantidadCuartos = 1;
            int expectedCantidadBanos = 1;
            bool expectedConTerraza = true;

            // Act
            Depto depto = new Depto(expectedPiso, expectedNumero, expectedDueno, expectedCantidadCuartos, expectedCantidadBanos, expectedConTerraza)
            {
                Piso = expectedPiso,
                Numero = expectedNumero,
                Dueno = expectedDueno,
                CantidadCuartos = expectedCantidadCuartos,
                CantidadBanos = expectedCantidadBanos,
                ConTerraza = expectedConTerraza
            };

            // Assert
            // Exception is expected to be thrown
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NuevoDepto_InvalidCantidadCuartos_ExceptionThrown()
        {
            // Arrange
            int expectedPiso = 0;
            int expectedNumero = 1;
            Dueno expectedDueno = new Dueno();
            int expectedCantidadCuartos = 0; // Invalid cantidad cuartos value
            int expectedCantidadBanos = 1;
            bool expectedConTerraza = true;

            // Act
            Depto depto = new Depto(expectedPiso, expectedNumero, expectedDueno, expectedCantidadCuartos, expectedCantidadBanos, expectedConTerraza)
            {
                Piso = expectedPiso,
                Numero = expectedNumero,
                Dueno = expectedDueno,
                CantidadCuartos = expectedCantidadCuartos,
                CantidadBanos = expectedCantidadBanos,
                ConTerraza = expectedConTerraza
            };

            // Assert
            // Exception is expected to be thrown
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NuevoDepto_InvalidCantidadBanos_ExceptionThrown()
        {
            // Arrange
            int expectedPiso = 0;
            int expectedNumero = 1;
            Dueno expectedDueno = new Dueno();
            int expectedCantidadCuartos = 1;
            int expectedCantidadBanos = 0; // Invalid cantidad banos value
            bool expectedConTerraza = true;

            // Act
            Depto depto = new Depto(expectedPiso, expectedNumero, expectedDueno, expectedCantidadCuartos, expectedCantidadBanos, expectedConTerraza)
            {
                Piso = expectedPiso,
                Numero = expectedNumero,
                Dueno = expectedDueno,
                CantidadCuartos = expectedCantidadCuartos,
                CantidadBanos = expectedCantidadBanos,
                ConTerraza = expectedConTerraza
            };

            // Assert
            // Exception is expected to be thrown
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NuevoDepto_NullDueno_ExceptionThrown()
        {
            // Arrange
            int expectedPiso = 0;
            int expectedNumero = 1;
            Dueno expectedDueno = null; // Invalid null Dueno value
            int expectedCantidadCuartos = 1;
            int expectedCantidadBanos = 1;
            bool expectedConTerraza = true;

            // Act
            Depto depto = new Depto(expectedPiso, expectedNumero, expectedDueno, expectedCantidadCuartos, expectedCantidadBanos, expectedConTerraza)
            {
                Piso = expectedPiso,
                Numero = expectedNumero,
                Dueno = expectedDueno,
                CantidadCuartos = expectedCantidadCuartos,
                CantidadBanos = expectedCantidadBanos,
                ConTerraza = expectedConTerraza
            };

            // Assert
            // Exception is expected to be thrown
        }

        [TestMethod]
        public void NuevoDepto_ValidAttributes_NoExceptionThrown()
        {
            // Arrange
            int expectedPiso = 1;
            int expectedNumero = 1;
            Dueno expectedDueno = new Dueno();
            int expectedCantidadCuartos = 1;
            int expectedCantidadBanos = 1;
            bool expectedConTerraza = true;

            // Act
            Depto depto = new Depto(expectedPiso, expectedNumero, expectedDueno, expectedCantidadCuartos, expectedCantidadBanos, expectedConTerraza)
            {
                Piso = expectedPiso,
                Numero = expectedNumero,
                Dueno = expectedDueno,
                CantidadCuartos = expectedCantidadCuartos,
                CantidadBanos = expectedCantidadBanos,
                ConTerraza = expectedConTerraza
            };

            // Assert
            // No exception is expected to be thrown
        }
    }
}
