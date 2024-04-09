namespace ob.Domain.Tests;

[TestClass]
public class CategoriaTest
{
    [TestMethod]
    public void NuevaCategoria()
    {
        //Arrange & Act
        Categoria nuevaCategoria = new Categoria("Limpieza");

        //Assert
        Assert.AreEqual("Limpieza", nuevaCategoria.Nombre);
    }
}