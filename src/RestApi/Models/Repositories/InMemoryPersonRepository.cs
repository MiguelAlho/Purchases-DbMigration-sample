using System;
using System.Collections.Generic;

namespace RestApi.Models.Repositories
{
    public class InMemoryPersonRepository : IPersonRepository
    {
        public IEnumerable<Person> GetListOfPersons()
        {
            return new[]
            {
                new Person(Guid.NewGuid(), "Miguel Alho"),
                new Person(Guid.NewGuid(), "Eduardo Piairo"),
            };
        }
    }
}