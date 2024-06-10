using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using ob.DataAccess;
using ob.Domain;
using System.Linq;

namespace ob.DataAccess.Tests
{
    [TestClass]
    public class DuenoRepositoryTests
    {
        private DbContextOptions<AppContext> _duenoOptions;

        [TestInitialize]
        public void Initialize()
        {
            _duenoOptions = new DbContextOptionsBuilder<AppContext>()
                .UseInMemoryDatabase(databaseName: "DuenoTestDatabase")
                .Options;
        }

        [TestMethod]
        public void CanInsertDuenoIntoDatabase()
        {
            using (var context = new TestAppContext(_duenoOptions))
            {
                var repository = new DuenoRepository(context);
                var dueno = new Dueno("NombreTest", "ApellidoTest", "test@dueno.com");

                repository.Insert(dueno);
                repository.Save();
            }

            using (var context = new TestAppContext(_duenoOptions))
            {
                Assert.AreEqual(1, context.Duenos.Count());
                var insertedDueno = context.Duenos.Single();
                Assert.AreEqual("NombreTest", insertedDueno.Nombre);
                Assert.AreEqual("ApellidoTest", insertedDueno.Apellido);
                Assert.AreEqual("test@dueno.com", insertedDueno.Email);
            }
        }
    }
}
