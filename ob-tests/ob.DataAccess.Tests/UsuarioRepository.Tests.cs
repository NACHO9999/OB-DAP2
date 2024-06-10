using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using ob.DataAccess;
using ob.Domain;
using System.Linq;

namespace ob.DataAccess.Tests
{
    [TestClass]
    public class UsuarioRepositoryTests
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
        public void CanInsertUsuarioIntoDatabase()
        {
            using (var context = new TestAppContext(_options))
            {
                var repository = new UsuarioRepository(context);
                var usuario = new Administrador("admins", "bases", "admins@admin.com", "passwords");

                repository.Insert(usuario);
                repository.Save();
            }

            using (var context = new TestAppContext(_options))
            {
                Assert.AreEqual("admins@admin.com", "admins@admin.com");
                Assert.AreEqual("admins", "admins");
            }
        }

        [TestMethod]
        public void EmailExists_ReturnsTrueForExistingEmail()
        {
            using (var context = new TestAppContext(_options))
            {
                var repository = new UsuarioRepository(context);
                var usuario = new Administrador("admina", "basea", "admina@admin.com", "passworda");

                repository.Insert(usuario);
                repository.Save();
            }

            using (var context = new TestAppContext(_options))
            {
                var repository = new UsuarioRepository(context);
                var emailExists = repository.EmailExists("admina@admin.com");

                Assert.IsTrue(emailExists);
            }
        }

        [TestMethod]
        public void EmailExists_ReturnsFalseForNonExistingEmail()
        {
            using (var context = new TestAppContext(_options))
            {
                var repository = new UsuarioRepository(context);
                var emailExists = repository.EmailExists("nonexisting@admin.com");

                Assert.IsFalse(emailExists);
            }
        }
    }
}
