using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using ob.DataAccess;
using ob.Domain;
using System.Linq;

namespace ob.DataAccess.Tests
{
    [TestClass]
    public class EdificioRepositoryTests
    {
        private DbContextOptions<AppContext> _edificioOptions;

        [TestInitialize]
        public void Initialize()
        {
            _edificioOptions = new DbContextOptionsBuilder<AppContext>()
                .UseInMemoryDatabase(databaseName: "EdificioTestDatabase")
                .Options;
        }

        [TestMethod]
        public void CanInsertEdificioIntoDatabase()
        {
            using (var context = new TestAppContext(_edificioOptions))
            {
                var repository = new EdificioRepository(context);
                var constructora = new Constructora("Constructora1");
                var edificio = new Edificio("Edificio1", "Direccion1", "Ubicacion1", constructora, 1000m, null);

                context.Constructoras.Add(constructora); // Add constructora to the context
                context.SaveChanges(); // Save changes to ensure the constructora is stored in the database

                repository.Insert(edificio);
                repository.Save();
            }

            using (var context = new TestAppContext(_edificioOptions))
            {
                Assert.AreEqual(1, context.Edificios.Count());
                var insertedEdificio = context.Edificios.Include(e => e.EmpresaConstructora).Single();
                Assert.AreEqual("Edificio1", insertedEdificio.Nombre);
                Assert.AreEqual("Direccion1", insertedEdificio.Direccion);
                Assert.AreEqual("Ubicacion1", insertedEdificio.Ubicación);
                Assert.AreEqual(1000m, insertedEdificio.GastosComunes);
                Assert.IsNotNull(insertedEdificio.EmpresaConstructora);
                Assert.AreEqual("Constructora1", insertedEdificio.EmpresaConstructora.Nombre);
            }
        }
    }
}
