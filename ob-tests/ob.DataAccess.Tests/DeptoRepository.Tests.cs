using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using ob.DataAccess;
using ob.Domain;
using System.Linq;

namespace ob.DataAccess.Tests
{
    [TestClass]
    public class DeptoRepositoryTests
    {
        private DbContextOptions<AppContext> _deptoOptions;

        [TestInitialize]
        public void Initialize()
        {
            _deptoOptions = new DbContextOptionsBuilder<AppContext>()
                .UseInMemoryDatabase(databaseName: "DeptoTestDatabase")
                .Options;
        }

        [TestMethod]
        public void CanInsertDeptoIntoDatabase()
        {
            using (var context = new TestAppContext(_deptoOptions))
            {
                var repository = new DeptoRepository(context);
                
                // Utilizando reflexión para crear instancia de Depto si el constructor no es accesible
                var deptoType = typeof(Depto);
                var deptoConstructor = deptoType.GetConstructor(new[] 
                {
                    typeof(int), typeof(int), typeof(Dueno), typeof(int), typeof(int), 
                    typeof(bool), typeof(string), typeof(string)
                });

                var depto = (Depto)deptoConstructor.Invoke(new object[] 
                {
                    1, 101, null, 3, 2, true, "Edificio1", "Direccion1"
                });

                repository.Insert(depto);
                repository.Save();
            }

            using (var context = new TestAppContext(_deptoOptions))
            {
                Assert.AreEqual(1, context.Deptos.Count());
                var insertedDepto = context.Deptos.Single();
                Assert.AreEqual(101, insertedDepto.Numero);
                Assert.AreEqual("Edificio1", insertedDepto.EdificioNombre);
            }
        }
    }
}
