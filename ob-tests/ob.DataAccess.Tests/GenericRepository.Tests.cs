using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using ob.DataAccess;
using ob.Domain;
using System.Collections.Generic;
using System.Linq;

namespace ob.DataAccess.Tests
{
    public class TestAppContext : AppContext
    {
        public TestAppContext(DbContextOptions<AppContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // No llamar al base.OnConfiguring para evitar cargar appsettings.json
        }
    }

    [TestClass]
    public class GenericRepositoryTests
    {
        private DbContextOptions<AppContext> _options;

        [TestInitialize]
        public void Initialize()
        {
            _options = new DbContextOptionsBuilder<AppContext>()
                .UseInMemoryDatabase(databaseName: "GenericTestDatabase")
                .Options;
        }

        [TestMethod]
        public void CanInsertEntityIntoDatabase()
        {
            using (var context = new TestAppContext(_options))
            {
                var repository = new GenericRepository<Categoria>(context);
                var categoria = new Categoria { Nombre = "Categoria1" };

                repository.Insert(categoria);
                repository.Save();
            }

            using (var context = new TestAppContext(_options))
            {
                Assert.AreEqual(1, context.Categorias.Count());
                var insertedCategoria = context.Categorias.Single();
                Assert.AreEqual("Categoria1", insertedCategoria.Nombre);
            }
        }

        [TestMethod]
        public void CanUpdateEntityInDatabase()
        {
            using (var context = new TestAppContext(_options))
            {
                var repository = new GenericRepository<Categoria>(context);
                var categoria = new Categoria { Nombre = "Categoria1" };

                repository.Insert(categoria);
                repository.Save();

                categoria.Nombre = "CategoriaUpdated";
                repository.Update(categoria);
                repository.Save();
            }

            using (var context = new TestAppContext(_options))
            {
                Assert.AreEqual(1, context.Categorias.Count());
                var updatedCategoria = context.Categorias.Single();
                Assert.AreEqual("CategoriaUpdated", updatedCategoria.Nombre);
            }
        }

        [TestMethod]
        public void CanDeleteEntityFromDatabase()
        {
            using (var context = new TestAppContext(_options))
            {
                var repository = new GenericRepository<Categoria>(context);
                var categoria = new Categoria { Nombre = "Categoria1" };

                repository.Insert(categoria);
                repository.Save();

                repository.Delete(categoria);
                repository.Save();
            }

            using (var context = new TestAppContext(_options))
            {
                Assert.AreEqual(0, context.Categorias.Count());
            }
        }

        [TestMethod]
        public void CanGetEntityFromDatabase()
        {
            using (var context = new TestAppContext(_options))
            {
                var repository = new GenericRepository<Categoria>(context);
                var categoria = new Categoria { Nombre = "Categoria1" };

                repository.Insert(categoria);
                repository.Save();

                var retrievedCategoria = repository.Get(c => c.Nombre == "Categoria1");

                Assert.IsNotNull(retrievedCategoria);
                Assert.AreEqual("Categoria1", retrievedCategoria.Nombre);
            }
        }

        [TestMethod]
        public void CanGetAllEntitiesFromDatabase()
        {
            using (var context = new TestAppContext(_options))
            {
                var repository = new GenericRepository<Categoria>(context);
                var categorias = new List<Categoria>
                {
                    new Categoria { Nombre = "Categoria1" },
                    new Categoria { Nombre = "Categoria2" }
                };

                foreach (var categoria in categorias)
                {
                    repository.Insert(categoria);
                }
                repository.Save();
            }

            using (var context = new TestAppContext(_options))
            {
                var repository = new GenericRepository<Categoria>(context);
                var allCategorias = repository.GetAll<Categoria>().ToList();

                Assert.AreEqual(2, allCategorias.Count);
                Assert.IsTrue(allCategorias.Any(c => c.Nombre == "Categoria1"));
                Assert.IsTrue(allCategorias.Any(c => c.Nombre == "Categoria2"));
            }
        }
    }
}
