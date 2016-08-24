using System.Linq;
using DAL.Models;
using DAL.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class UsersRepositoryTests : AbstractRepositoryTests<UsersRepository>
    {
        public override UsersRepository InitializeRepository()
        {
            return new UsersRepository(GlobalInitializer.ConnectionStringSettings);
        }

        [TestMethod]
        public void AtStartWeWeHaveOneUser()
        {
            Assert.AreEqual(1, Repository.Total());

            var user = Repository.Find(1);
            Assert.AreEqual("foo", user.FirstName);
        }

        [TestMethod]
        public void SaveMethodSetIdPropertyAndSavesRecordToDatabase()
        {
            var total = Repository.Total();
            Assert.AreEqual(1, total);

            var user = new User { FirstName = "foo", LastName = "bar" };
            Repository.Save(user);

            Assert.AreNotEqual(0, user.Id);
            Assert.AreEqual(total + 1, Repository.Total());
        }

        [TestMethod]
        public void SaveMethodUpdatesExistingRecord()
        {
            var user = new User { FirstName = "foo", LastName = "bar" };
            Repository.Save(user);
            Assert.AreNotEqual(0, user.Id);
            user.FirstName = "Hello";
            user.LastName = "World";
            Repository.Save(user);
            user = Repository.Find(user.Id);
            Assert.AreEqual("Hello", user.FirstName);
            Assert.AreEqual(2, Repository.Total());
        }

        [TestMethod]
        public void CanFindUsers()
        {
            var users = Repository.Find("foo").ToList();
            Assert.AreEqual(1, users.Count());
            Assert.AreEqual("foo", users.FirstOrDefault()?.FirstName);
        }
    }
}
