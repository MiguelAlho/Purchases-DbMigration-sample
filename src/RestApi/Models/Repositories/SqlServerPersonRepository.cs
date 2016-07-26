using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Models.Repositories
{
    public class SqlServerPersonRepository : IPersonRepository
    {
        readonly string _connectionString;

        public SqlServerPersonRepository(string conString)
        {
            if (string.IsNullOrWhiteSpace(conString))
                throw new ArgumentException(nameof(conString));

            _connectionString = conString;
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
