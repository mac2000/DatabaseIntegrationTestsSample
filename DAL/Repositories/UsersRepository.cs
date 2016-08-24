using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dapper.FastCrud;
using DAL.Models;

namespace DAL.Repositories
{
    public class UsersRepository
    {
        private readonly ConnectionStringSettings _connectionStringSettings;

        private SqlConnection SqlConnection => new SqlConnection(_connectionStringSettings.ConnectionString);

        public UsersRepository(ConnectionStringSettings connectionStringSettings)
        {
            _connectionStringSettings = connectionStringSettings;
        }

        public int Total()
        {
            return SqlConnection.Query<int>("SELECT COUNT(*) FROM Users").FirstOrDefault();
        }

        private void Insert(User user)
        {
            SqlConnection.Insert(user);
        }

        private void Update(User user)
        {
            SqlConnection.Update(user);
        }

        public void Save(User user)
        {
            if (user.Id == 0)
            {
                Insert(user);
            }
            else
            {
                Update(user);
            }
        }

        public User Find(int id)
        {
            return SqlConnection.Get(new User { Id = id });
        }

        public IEnumerable<User> Find(string firstName)
        {
            return SqlConnection.Find<User>(statement => statement
            .Where($"{nameof(User.FirstName):C} = @FirstName")
            .OrderBy($"{nameof(User.FirstName):C} ASC")
            .WithParameters(new { FirstName = firstName }));
        }
    }
}
