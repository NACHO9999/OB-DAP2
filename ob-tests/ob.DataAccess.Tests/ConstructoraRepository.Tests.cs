using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using ob.DataAccess;
using ob.Domain;
using System.Linq;

namespace ob.DataAccess.Tests
{
    [TestClass]
    public class ConstructoraRepositoryTests
    {
        private DbContextOptions<AppContext> _constructoraOptions;

        [TestInitialize]
        public void Initialize()
        {
            _constructoraOptions = new DbContextOptionsBuilder<AppContext>()
                .UseInMemoryDatabase(databaseName: "ConstructoraTestDatabase")
                .Options;
        }

        [TestMethod]
        public void CanInsertConstructoraIntoDatabase()
        {
            using (var context = new TestAppContext(_constructoraOptions))
            {
                var repository = new ConstructoraRepository(context);
                var constructora = new Constructora("Constructora1");
                repository.Insert(constructora);
                repository.Save();
            }

            using (var context = new TestAppContext(_constructoraOptions))
            {
                Assert.AreEqual(1, context.Constructoras.Count());
                Assert.AreEqual("Constructora1", context.Constructoras.Single().Nombre);
            }
        }
    }
}
