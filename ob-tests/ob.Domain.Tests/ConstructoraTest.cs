using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ob.Domain.Tests
{
    [TestClass]
    public class ConstructoraTests
    {
        [TestMethod]
        public void Constructor_ValidNombre_SetNombre()
        {
            // Arrange
            string nombre = "TestCategory";

            // Act
            Constructora constructora = new Constructora(nombre);

            // Assert
            Assert.AreEqual(nombre, constructora.Nombre);
        }

        [TestMethod]
        public void Nombre_SetValidValue_Success()
        {
            // Arrange
            Constructora constructora = new Constructora("TestCategory");

            // Act
            constructora.Nombre = "UpdatedName";

            // Assert
            Assert.AreEqual("UpdatedName", constructora.Nombre);
        }

        [TestMethod]
        public void Nombre_SetInvalidValue_ThrowsException()
        {
            // Arrange
            Constructora constructora = new Constructora("TestCategory");

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => constructora.Nombre = "");
        }
    }
}