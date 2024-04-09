namespace ob.Domain.Tests;

[TestClass]
public class ConstructoraTest
{
    [TestMethod]
    public void NuevaConstructora()
    {
        //Arrange & Act
        Constructora nuevaConstructora = new Constructora("Constructora");

        //Assert
        Assert.AreEqual("Constructora", nuevaConstructora.Nombre);
    }
}