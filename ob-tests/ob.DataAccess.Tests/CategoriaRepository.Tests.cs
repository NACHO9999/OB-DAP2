using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using ob.DataAccess;
using ob.Domain;
using System.Linq;

namespace ob.DataAccess.Tests
{
    [TestClass]
    public class CategoriaRepositoryTests
    {
        private DbContextOptions<AppContext> options;

        [TestInitialize]
        public void Initialize()
        {
            options = new DbContextOptionsBuilder<AppContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [TestMethod]
        public void CanInsertCategoriaIntoDatabase()
        {
            using (var context = new TestAppContext(options))
            {
                var repository = new CategoriaRepository(context);
                var categoria = new Categoria("Categoria1");
                repository.Insert(categoria);
                repository.Save();
            }

            using (var context = new TestAppContext(options))
            {
                Assert.AreEqual(1, context.Categorias.Count());
                Assert.AreEqual("Categoria1", context.Categorias.Single().Nombre);
            }
        }
    }
}
