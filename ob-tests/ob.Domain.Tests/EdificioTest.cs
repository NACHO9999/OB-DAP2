namespace ob.Domain.Tests;



[TestClass]
public class EdificioTests
{
    [TestMethod]
    public void Nombre_SetValidValue_Success()
    {
        // Arrange
        Edificio edificio = new Edificio("Edificio A", "123 Main St", "City", new Constructora("Gomez"), 1000, new List<Depto>());

        // Act
        edificio.Nombre = "Edificio B";

        // Assert
        Assert.AreEqual("Edificio B", edificio.Nombre);
    }

    [TestMethod]
    public void Nombre_SetInvalidValue_ThrowsArgumentException()
    {
        // Arrange
        Edificio edificio = new Edificio("Edificio A", "123 Main St", "City", new Constructora("Gomez"), 1000, new List<Depto>());

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => edificio.Nombre = "");
    }

    [TestMethod]
    public void Dirección_SetValidValue_Success()
    {
        // Arrange
        Edificio edificio = new Edificio("Edificio A", "123 Main St", "City", new Constructora("Gomez"), 1000, new List<Depto>());

        // Act
        edificio.Dirección = "456 Elm St";

        // Assert
        Assert.AreEqual("456 Elm St", edificio.Dirección);
    }

    [TestMethod]
    public void Dirección_SetInvalidValue_ThrowsArgumentException()
    {
        // Arrange
        Edificio edificio = new Edificio("Edificio A", "123 Main St", "City", new Constructora("Gomez"), 1000, new List<Depto>());

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => edificio.Dirección = "");
    }

    [TestMethod]
    public void Ubicación_SetValidValue_Success()
    {
        // Arrange
        Edificio edificio = new Edificio("Edificio A", "123 Main St", "City", new Constructora("Gomez"), 1000, new List<Depto>());

        // Act
        edificio.Ubicación = "Downtown";

        // Assert
        Assert.AreEqual("Downtown", edificio.Ubicación);
    }

    [TestMethod]
    public void Ubicación_SetInvalidValue_ThrowsArgumentException()
    {
        // Arrange
        Edificio edificio = new Edificio("Edificio A", "123 Main St", "City", new Constructora("Rodriguez"), 1000, new List<Depto>());

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => edificio.Ubicación = "");
    }

    [TestMethod]
    public void EmpresaConstructora_SetValidValue_Success()
    {
        // Arrange
        Constructora constructora = new Constructora("Perez Construcciones");
        Edificio edificio = new Edificio("Edificio A", "123 Main St", "City", constructora, 1000, new List<Depto>());

        // Act
        Constructora newConstructora = new Constructora("Gomez Construcciones");
        edificio.EmpresaConstructora = newConstructora;

        // Assert
        Assert.AreEqual(newConstructora, edificio.EmpresaConstructora);
    }

    [TestMethod]
    public void EmpresaConstructora_SetNullValue_ThrowsArgumentNullException()
    {
        // Arrange
        Edificio edificio = new Edificio("Edificio A", "123 Main St", "City", new Constructora("Gomez"), 1000, new List<Depto>());

        // Act & Assert
        Assert.ThrowsException<ArgumentNullException>(() => edificio.EmpresaConstructora = null);
    }

    [TestMethod]
    public void GastosComunes_SetValidValue_Success()
    {
        // Arrange
        Edificio edificio = new Edificio("Edificio A", "123 Main St", "City", new Constructora("gomez"), 1000, new List<Depto>());

        // Act
        edificio.GastosComunes = 1200;

        // Assert
        Assert.AreEqual(1200, edificio.GastosComunes);
    }

    [TestMethod]
    public void GastosComunes_SetInvalidValue_ThrowsArgumentException()
    {
        // Arrange
        Edificio edificio = new Edificio("Edificio A", "123 Main St", "City", new Constructora("Gomez"), 1000, new List<Depto>());

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => edificio.GastosComunes = -500);
    }

    [TestMethod]
    public void Deptos_SetValidValue_Success()
    {
        // Arrange
        List<Depto> deptos = new List<Depto>();
        deptos.Add(new Depto(1, 101, null, 2, 1, false));
        Edificio edificio = new Edificio("Edificio A", "123 Main St", "City", new Constructora("Gomez"), 1000, deptos);

        // Act
        List<Depto> newDeptos = new List<Depto>();
        newDeptos.Add(new Depto(2, 102, null, 3, 2, true));
        edificio.Deptos = newDeptos;

        // Assert
        CollectionAssert.AreEqual(newDeptos, edificio.Deptos);
    }

    [TestMethod]
    public void Deptos_SetNullValue_ThrowsArgumentNullException()
    {
        // Arrange
        Edificio edificio = new Edificio("Edificio A", "123 Main St", "City", new Constructora("Gomez"), 1000, new List<Depto>());

        // Act & Assert
        Assert.ThrowsException<ArgumentNullException>(() => edificio.Deptos = null);
    }
}