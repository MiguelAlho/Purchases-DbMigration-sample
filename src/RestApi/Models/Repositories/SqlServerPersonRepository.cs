using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Options;


namespace RestApi.Models.Repositories
{
    public class SqlServerPersonRepository : IPersonRepository
    {
        readonly string _connectionString;

        public SqlServerPersonRepository(IOptions<DbConfiguration> dbOptions)
        {
            if (dbOptions == null)
                throw new ArgumentException(nameof(dbOptions));
            if (string.IsNullOrWhiteSpace(dbOptions.Value.ConnectionString))
                throw new ArgumentException("connectionsString");

            _connectionString = dbOptions.Value.ConnectionString;
        }



        public IEnumerable<Person> GetListOfPersons()
        {
            List<Person> results = new List<Person>();
            var queryText = "Select * From Person";

            using (var connection = new SqlConnection(_connectionString))
            using (var query = new SqlCommand(queryText, connection))
            {
                connection.Open();

                using (var reader = query.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Person p = new Person(
                                Guid.Parse(reader["Id"].ToString()), 
                                reader["Name"].ToString()
                            );    

                            results.Add(p);
                        }
                    }
                }

            }

            return results;
        }
    }
}
