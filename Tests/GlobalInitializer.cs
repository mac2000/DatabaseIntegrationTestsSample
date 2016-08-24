using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GlobalInitializer
    {
        public static string Server = "(localdb)\\MSSQLLocalDB";
        public static string DatabaseName = "DatabaseIntegrationTestsSample";
        public static string SmoConnectionString = $"Server={Server}";
        public static string SqlConnectionString = $"Server={Server};Database={DatabaseName}";
        public static ConnectionStringSettings ConnectionStringSettings = new ConnectionStringSettings("Default", SqlConnectionString);

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {
            var server = new Server(new ServerConnection(new SqlConnection(SmoConnectionString)));

            if (server.Databases.Contains(DatabaseName)) return;

            server.Databases["master"].ExecuteNonQuery($"CREATE DATABASE {DatabaseName}");
            server.Databases[DatabaseName].ExecuteNonQuery(File.ReadAllText("Mini.sql"));
        }
    }
}
