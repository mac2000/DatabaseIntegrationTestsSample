using System.Configuration;
using System.Linq;
using System.Transactions;
using DAL;
using DAL.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using User = DAL.Models.User;

namespace Tests
{
    [TestClass]
    public class UsersRepositoryTests
    {
        private TransactionScope _trans;
        private UsersRepository _usersRepository;

        [TestInitialize]
        public void InitializeTest()
        {
            _usersRepository = new UsersRepository(new ConnectionStringSettings("Default", GlobalInitializer.SqlConnectionString));
            _trans = new TransactionScope();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _trans.Dispose();
        }

        [TestMethod]
        public void AtStartWeWeHaveOneUser()
        {
            Assert.AreEqual(1, _usersRepository.Total());
            var user = _usersRepository.Find(1);
            Assert.AreEqual("foo", user.FirstName);
        }

        [TestMethod]
        public void SaveMethodSetIdPropertyAndSavesRecordToDatabase()
        {
            Assert.AreEqual(1, _usersRepository.Total());
            var user = new User { FirstName = "foo", LastName = "bar" };
            _usersRepository.Save(user);
            Assert.AreNotEqual(0, user.Id);
            Assert.AreEqual(2, _usersRepository.Total());
        }

        [TestMethod]
        public void SaveMethodUpdatesExistingRecord()
        {
            var user = new User { FirstName = "foo", LastName = "bar" };
            _usersRepository.Save(user);
            Assert.AreNotEqual(0, user.Id);
            user.FirstName = "Hello";
            user.LastName = "World";
            _usersRepository.Save(user);
            user = _usersRepository.Find(user.Id);
            Assert.AreEqual("Hello", user.FirstName);
            Assert.AreEqual(2, _usersRepository.Total());
        }

        [TestMethod]
        public void CanFindUsers()
        {
            var users = _usersRepository.Find("foo").ToList();
            Assert.AreEqual(1, users.Count());
            Assert.AreEqual("foo", users.FirstOrDefault()?.FirstName);
        }
    }
}
