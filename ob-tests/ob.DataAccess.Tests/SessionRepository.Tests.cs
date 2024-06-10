using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using ob.DataAccess;
using ob.Domain;
using System;
using System.Linq;

namespace ob.DataAccess.Tests
{
    [TestClass]
    public class SessionRepositoryTests
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
        public void CanInsertSessionIntoDatabase()
        {
            using (var context = new TestAppContext(_options))
            {
                var repository = new SessionRepository(context);
                var usuario = new Administrador("admin", "apellido", "admin@example.com", "password123");
                var session = new Session
                {
                    Usuario = usuario,
                    AuthToken = Guid.NewGuid(),
                };

                context.Usuarios.Add(usuario);
                context.SaveChanges();

                repository.Insert(session);
                repository.Save();
            }

            using (var context = new TestAppContext(_options))
            {
                Assert.AreEqual(1, context.Sessions.Count());
                var insertedSession = context.Sessions.Include(s => s.Usuario).Single();
                Assert.AreEqual("admin@example.com", insertedSession.Usuario.Email);
                Assert.IsNotNull(insertedSession.AuthToken);
                Assert.IsTrue(DateTime.Now <= DateTime.Now);
            }
        }
    }
}
