using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RestApi.Models;

namespace RestApi.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    public class PersonController
    {
        private readonly IPersonRepository _repository;

        public PersonController(IPersonRepository repository)
        {
            if(repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<Person> GetListOfPersons()
        {
            return _repository.GetListOfPersons();
        }

    }
}
