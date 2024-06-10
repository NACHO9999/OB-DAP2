namespace ob.DataAccess.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using ob.DataAccess;
    using ob.Domain;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class TestAppContext : AppContext
    {
        public TestAppContext(DbContextOptions<AppContext> options, IConfiguration config = null)
            : base(options)
        {
            Config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (Config == null)
            {
                optionsBuilder.UseInMemoryDatabase("TestDatabase");
            }
            else
            {
                base.OnConfiguring(optionsBuilder);
            }
        }
    }

    [TestClass]
    public class AppContextTests
    {
        private DbContextOptions<AppContext> _options;
        private IConfiguration _configuration;

        [TestInitialize]
        public void Initialize()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            _configuration = configBuilder;

            _options = new DbContextOptionsBuilder<AppContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [TestMethod]
        public async Task CanInsertUserIntoDatabase()
        {
            using (var context = new TestAppContext(_options))
            {
                var user = new Administrador("admin", "base", "abc@abc.com", "Hola1234");
                context.Usuarios.Add(user);
                await context.SaveChangesAsync();
            }

            using (var context = new TestAppContext(_options))
            {
                Assert.AreEqual(1, context.Usuarios.Count());
                Assert.AreEqual("abc@abc.com", context.Usuarios.Single().Email);
            }
        }

        [TestMethod]
        public async Task CanInitializeDatabaseWithAdminUser()
        {
            using (var context = new TestAppContext(_options))
            {
                await context.InitializeAsync();
            }

            using (var context = new TestAppContext(_options))
            {
                Assert.AreEqual(1, context.Usuarios.Count());
                Assert.AreEqual("abc@abc.com", context.Usuarios.Single().Email);
            }
        }

        [TestMethod]
        public void Model_Creates_Keys_And_Relations()
        {
            using (var context = new TestAppContext(_options))
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

        [TestMethod]
        public async Task InitializeAsync_AddsAdminIfNotExists()
        {
            using (var context = new TestAppContext(_options))
            {
                // Act
                await context.InitializeAsync();

                // Assert
                var admin = context.Usuarios.OfType<Administrador>().FirstOrDefault();
                Assert.IsNotNull(admin);
                Assert.AreEqual("abc@abc.com", admin.Email);
            }
        }
    }
}