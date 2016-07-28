using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DapperWrapper;
using Microsoft.Extensions.Options;
using RestApi.Models.Repositories;
using RestApi.SqlServer.IntegrationTests.Database;
using Xunit;

namespace RestApi.SqlServer.IntegrationTests
{
    public class SqlServerPersonRepositoryTests : IClassFixture<DbTestFixture>
    {
        readonly DbTestFixture _fixture;
        readonly IOptions<DbConfiguration> _config;

        Guid id1 = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
        Guid id2 = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2);
        string name1 = "Name One";
        string name2 = "Name Two";

        public SqlServerPersonRepositoryTests(DbTestFixture fixture)
        {
            _fixture = fixture;

            _config = new OptionsManager<DbConfiguration>(new[]
            {
                new ConfigureOptions<DbConfiguration>(configuration => 
                    configuration.ConnectionString = _fixture.ConnectionString),
            });
        }

        [Fact]
        public void CanReadPersonListFromDatabase()
        {
            //arrange
            _fixture.ClearRecords();
            AddPersonsToDatabase();

            var repo = new SqlServerPersonRepository(_config);

            var result = repo.GetListOfPersons();

            Assert.NotNull(result);
            Assert.NotEmpty(result);

            Assert.Equal(2, result.Count());

            var array = result.OrderBy(o => o.Id).ToArray();
            Assert.Equal(id1, array[0].Id);
            Assert.Equal(name1, array[0].Name);
        }

        private void AddPersonsToDatabase()
        {
            
            using (IDbExecutor exec = new SqlExecutor(_fixture.GetNewOpenConnection()))
            {
                var insert = "Insert Into Person (Id, Name) Values (@id, @name)";
                exec.Execute(insert, new { id = id1, name = name1 });
                exec.Execute(insert, new { id = id2, name = name2 });
            }
        }

        [Fact]
        public void CanReadEmptyPersonListWhenNoDataExists()
        {
            _fixture.ClearRecords();

            var repo = new SqlServerPersonRepository(_config);

            var result = repo.GetListOfPersons();

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
