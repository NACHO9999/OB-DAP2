using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using ob.DataAccess;
using ob.Domain;
using System.Linq;

namespace ob.DataAccess.Tests
{
    [TestClass]
    public class InvitacionRepositoryTests
    {
        private DbContextOptions<AppContext> _invitacionOptions;

        [TestInitialize]
        public void Initialize()
        {
            _invitacionOptions = new DbContextOptionsBuilder<AppContext>()
                .UseInMemoryDatabase(databaseName: "InvitacionTestDatabase")
                .Options;
        }

        [TestMethod]
        public void CanInsertInvitacionIntoDatabase()
        {
            using (var context = new TestAppContext(_invitacionOptions))
            {
                var repository = new InvitacionRepository(context);
                var invitacion = new Invitacion("test@example.com", "Este es un mensaje de prueba.", DateTime.Now, Enums.RolInvitaciion.Encargado);
                repository.Insert(invitacion);
                repository.Save();
            }

            using (var context = new TestAppContext(_invitacionOptions))
            {
                Assert.AreEqual(1, context.Invitaciones.Count());
                var insertedInvitacion = context.Invitaciones.Single();
                Assert.AreEqual("test@example.com", insertedInvitacion.Email);
                Assert.AreEqual("Este es un mensaje de prueba.", "Este es un mensaje de prueba.");
                Assert.IsTrue(DateTime.Now <= DateTime.Now);
            }
        }
    }
}
