namespace ob.DataAccess.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ob.DataAccess;
using ob.Domain;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

[TestClass]
public class AppContextTests
{
    private DbContextOptions<AppContext> _options;

    [TestInitialize]
    public void Initialize()
    {
        _options = new DbContextOptionsBuilder<AppContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
    }

    [TestMethod]
    public async Task CanInsertUserIntoDatabase()
    {
        using (var context = new AppContext(_options))
        {
            var user = new Administrador("admin", "base", "abc@abc.com", "Hola1234");
            context.Usuarios.Add(user);
            await context.SaveChangesAsync();
        }

        using (var context = new AppContext(_options))
        {
            Assert.AreEqual(1, context.Usuarios.Count());
            Assert.AreEqual("abc@abc.com", context.Usuarios.Single().Email);
        }
    }

    [TestMethod]
    public async Task CanInitializeDatabaseWithAdminUser()
    {
        using (var context = new AppContext(_options))
        {
            await context.InitializeAsync();
        }

        using (var context = new AppContext(_options))
        {
            Assert.AreEqual(1, context.Usuarios.Count());
            Assert.AreEqual("abc@abc.com", context.Usuarios.Single().Email);
        }
    }

    [TestMethod]
    public void Configuration_IsLoadedCorrectly()
    {
        var configBuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configBuilder.GetConnectionString("DBTest");
        Assert.IsFalse(string.IsNullOrEmpty(connectionString));
    }

    [TestMethod]
    public void Model_Creates_Keys_And_Relations()
    {
        using (var context = new AppContext(_options))
        {
            var model = context.Model;

            var categoria = model.FindEntityType(typeof(Categoria));
            Assert.IsNotNull(categoria.FindPrimaryKey().Properties.SingleOrDefault(p => p.Name == "Nombre"));

            var constructora = model.FindEntityType(typeof(Constructora));
            Assert.IsNotNull(constructora.FindPrimaryKey().Properties.SingleOrDefault(p => p.Name == "Id"));

            var depto = model.FindEntityType(typeof(Depto));
            Assert.IsNotNull(depto.FindPrimaryKey().Properties.SingleOrDefault(p => p.Name == "Numero"));
            Assert.IsNotNull(depto.FindPrimaryKey().Properties.SingleOrDefault(p => p.Name == "EdificioNombre"));
            Assert.IsNotNull(depto.FindPrimaryKey().Properties.SingleOrDefault(p => p.Name == "EdificioDireccion"));

            // Add checks for other entities as needed
        }
    }
}
