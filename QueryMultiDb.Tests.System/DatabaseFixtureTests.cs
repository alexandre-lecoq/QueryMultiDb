﻿using System.Data.SqlClient;
using Xunit;

namespace QueryMultiDb.Tests.System
{
    public class DatabaseFixtureTests
    {
        [Fact]
        public void WorkingIntegratedConnectionConnection()
        {
            using (var connection = new SqlConnection(DatabaseFixture.ConnectionString))
            {
                connection.Open();
                connection.Close();
            }
        }
    }
}
