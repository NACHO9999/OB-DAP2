using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using ob.DataAccess;
using ob.Domain;
using System;
using System.Linq;

namespace ob.DataAccess.Tests
{
    [TestClass]
    public class SolicitudRepositoryTests
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
        public void CanInsertSolicitudIntoDatabase()
        {
            using (var context = new TestAppContext(_options))
            {
                var repository = new SolicitudRepository(context);
                var categoria = new Categoria("Categoria2");
                var depto = new Depto(1, 101, null, 4, 3, true, "ed", "di");

                // Ensure the category and depto are not already added
                if (!context.Categorias.Any(c => c.Nombre == "Categoria1"))
                {
                    context.Categorias.Add(categoria);
                }

                if (!context.Deptos.Any(d => d.Numero == 101 && d.EdificioNombre == "ed" && d.EdificioDireccion == "di"))
                {
                    context.Deptos.Add(depto);
                }

                context.SaveChanges();

                var solicitud = new Solicitud("Descripcion1", depto, categoria, Enums.EstadoSolicitud.Abierto, DateTime.Now);
                repository.Insert(solicitud);
                repository.Save();
            }

            using (var context = new TestAppContext(_options))
            {
                Assert.AreEqual(1, context.Solicitudes.Count());
                var insertedSolicitud = context.Solicitudes.Include(s => s.Depto).Include(s => s.Categoria).Single();
                Assert.AreEqual("Descripcion1", insertedSolicitud.Descripcion);
                Assert.AreEqual("Categoria2", insertedSolicitud.Categoria.Nombre);
                Assert.AreEqual(101, insertedSolicitud.Depto.Numero);
            }
        }
    }
}
