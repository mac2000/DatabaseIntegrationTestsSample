using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class GlobalInitializer
    {
        public static string Server = "(localdb)\\MSSQLLocalDB";
        public static string DatabaseName = "DatabaseIntegrationTestsSample";
        public static string SmoConnectionString = $"Server={Server}";
        public static string SqlConnectionString = $"Server={Server};Database={DatabaseName}";

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {
            var server = new Server(new ServerConnection(new SqlConnection(SmoConnectionString)));

            if (server.Databases.Contains(DatabaseName)) return;

            server.Databases["master"].ExecuteNonQuery($"CREATE DATABASE {DatabaseName}");
            server.Databases[DatabaseName].ExecuteNonQuery(@"
                    CREATE TABLE Users (
	                    Id INT NOT NULL IDENTITY(1, 1),
	                    FirstName NVARCHAR(50) NOT NULL,
	                    LastName NVARCHAR(50) NOT NULL
                    );
                    GO
                    SET IDENTITY_INSERT dbo.Users ON;  
                    INSERT INTO Users (Id, FirstName, LastName) VALUES (1, 'foo', 'bar');
                    SET IDENTITY_INSERT dbo.Users OFF;
                    GO
                    CREATE TABLE Posts (
	                    Id INT NOT NULL IDENTITY(1, 1),
	                    Title NVARCHAR(50) NOT NULL,
	                    CreatedAt DATE NOT NULL
                    );
                    GO
                    SET IDENTITY_INSERT dbo.Posts ON;  
                    INSERT INTO Posts (Id, Title, CreatedAt) VALUES (1, 'foo', '2000-02-14');
                    SET IDENTITY_INSERT dbo.Posts OFF;  
                    ");
        }
    }
}
