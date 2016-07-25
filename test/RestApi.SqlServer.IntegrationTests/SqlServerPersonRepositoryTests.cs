using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DapperWrapper;
using RestApi.Models.Repositories;
using RestApi.SqlServer.IntegrationTests.Database;
using Xunit;

namespace RestApi.SqlServer.IntegrationTests
{
    public class SqlServerPersonRepositoryTests : IClassFixture<DbTestFixture>
    {
        readonly DbTestFixture _fixture;

        public SqlServerPersonRepositoryTests(DbTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanReadPersonListFromDatabase()
        {
            //arrange
            _fixture.ClearRecords();
            AddPersonsToDatabase();

            var repo = new SqlServerPersonRepository(_fixture.ConnectionString);

            var result = repo.GetListOfPersons();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        private void AddPersonsToDatabase()
        {
            using (IDbExecutor exec = new SqlExecutor(_fixture.GetNewOpenConnection()))
            {
                var insert = "Insert Into Person (Id, Name) Values (@id, @name)";
                exec.Execute(insert, new { id = Guid.NewGuid(), name = "Name One" });
                exec.Execute(insert, new { id = Guid.NewGuid(), name = "Name Two" });
            }
        }

        [Fact]
        public void CanReadEmptyPersonListWhenNoDataExists()
        {
            var repo = new SqlServerPersonRepository(_fixture.ConnectionString);

            var result = repo.GetListOfPersons();

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
