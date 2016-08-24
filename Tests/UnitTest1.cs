using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using DAL;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using User = DAL.Models.User;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        private const string ConnectionString = "Server=(localdb)\\MSSQLLocalDB";
        private TransactionScope _trans;
        private UsersRepository _usersRepository;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {
            var server = new Server(new ServerConnection(new SqlConnection(ConnectionString)));
            if (!server.Databases.Contains("DatabaseIntegrationTestsSample"))
            {
                server.Databases["master"].ExecuteNonQuery(@"
                    CREATE DATABASE DatabaseIntegrationTestsSample;
                    GO
                    USE DatabaseIntegrationTestsSample;
                    GO
                    CREATE TABLE UsersRepository (
	                    Id INT NOT NULL IDENTITY(1, 1),
	                    FirstName NVARCHAR(50) NOT NULL,
	                    LastName NVARCHAR(50) NOT NULL
                    );
                    GO
                    SET IDENTITY_INSERT dbo.UsersRepository ON;  
                    INSERT INTO UsersRepository (Id, FirstName, LastName) VALUES (1, 'foo', 'bar');
                    SET IDENTITY_INSERT dbo.UsersRepository OFF;  
                    ");
            }
        }

        [TestInitialize]
        public void InitializeTest()
        {
            _usersRepository = new UsersRepository(new ConnectionStringSettings("Default", $"{ConnectionString};Database=DatabaseIntegrationTestsSample"));
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
