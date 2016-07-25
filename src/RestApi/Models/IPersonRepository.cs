using System.Collections.Generic;

namespace RestApi.Models
{
    public interface IPersonRepository
    {
        IEnumerable<Person> GetListOfPersons();
    }
}