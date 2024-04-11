using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ob.Domain.Tests
{
    [TestClass]
    public class CategoriaTests
    {
        [TestMethod]
        public void Constructor_ValidNombre_SetNombre()
        {
            // Arrange
            string nombre = "TestCategory";

            // Act
            Categoria categoria = new Categoria(nombre);

            // Assert
            Assert.AreEqual(nombre, categoria.Nombre);
        }

        [TestMethod]
        public void Nombre_SetValidValue_Success()
        {
            // Arrange
            Categoria categoria = new Categoria("TestCategory");

            // Act
            categoria.Nombre = "UpdatedName";

            // Assert
            Assert.AreEqual("UpdatedName", categoria.Nombre);
        }

        [TestMethod]
        public void Nombre_SetInvalidValue_ThrowsException()
        {
            // Arrange
            Categoria categoria = new Categoria("TestCategory");

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => categoria.Nombre = "");
        }
    }
}