using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    public class PersonController
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

    }
}
