using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace EntityFrameworkMultipleProvidersWorkflow.EntityFramework.LocalSetup
{
    /// <summary>
    /// This class handle keeping the connection open for the lifetime of the object and closing it when the object is disposed. 
    /// By default SqlLite in memory will delete the database after the last connection closes. This prevent data loss between calls
    /// until the application restarts.
    /// </summary>
    public class SqlLiteDatabaseConnectionPersistor : IDisposable
    {
        private readonly SqliteConnection _connection;

        public SqlLiteDatabaseConnectionPersistor(IConfiguration configuration)
        {
            _connection = new SqliteConnection("DataSource=file::memory:?cache=shared");
            _connection.Open();
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}
