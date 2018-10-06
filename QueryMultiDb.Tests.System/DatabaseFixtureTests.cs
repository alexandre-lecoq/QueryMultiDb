using System.Data.SqlClient;
using Xunit;

namespace QueryMultiDb.Tests.System
{
    public class DatabaseFixtureTests
    {
        [Fact]
        public void WorkingIntegrateConnectionConnection()
        {
            using (var connection = new SqlConnection(DatabaseFixture.IntegratedSecurityConnectionString))
            {
                connection.Open();
                connection.Close();
            }
        }

        [Fact]
        public void WorkingSaConnection()
        {
            using (var connection = new SqlConnection(DatabaseFixture.SaConnectionString))
            {
                connection.Open();
                connection.Close();
            }
        }
    }
}
