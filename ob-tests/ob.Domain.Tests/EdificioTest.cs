namespace ob.Domain.Tests;

[TestClass]
public class EdificioTest
{
    [TestMethod]
    public void NuevoEdificio()
    {
        //Arrange
        List<Depto> departamentos = new List<Depto>();
        Constructora constructora = new Constructora("Constructora");

        //Act
        Edificio nuevoEdificio = new Edificio(
            "Empire State",
            "5th Avenue",
            "New York",
            constructora,
            100,
            departamentos
        );

        //Assert
        Assert.AreEqual("Empire State", nuevoEdificio.Nombre);
        Assert.AreEqual("5th Avenue", nuevoEdificio.Dirección);
        Assert.AreEqual("New York", nuevoEdificio.Ubicación);
        Assert.AreEqual(constructora, nuevoEdificio.EmpresaConstructora);
        Assert.AreEqual(100, nuevoEdificio.GastosComunes);
        Assert.AreEqual(departamentos, nuevoEdificio.Deptos);
    }
}
