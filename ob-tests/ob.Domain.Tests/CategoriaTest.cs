namespace ob.Domain.Tests;

[TestClass]
public class CategoriaTest
{
    [TestMethod]
    public void NuevaCategoria()
    {
        //Arrange & Act
        Categoria nuevaCategoria = new("Limpieza");

        //Assert
        Assert.AreEqual("Limpieza", nuevaCategoria.Nombre);
    }

    [TestMethod]
    public void CambiarNombreCategoria()
    {
        //Arrange
        Categoria nuevaCategoria = new("Limpieza");

        //Act
        nuevaCategoria.CambiarNombre("Seguridad");

        //Assert
        Assert.AreEqual("Seguridad", nuevaCategoria.Nombre);
    }
}