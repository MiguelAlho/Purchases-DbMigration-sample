using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestApi.Controllers;
using Xunit;

namespace RestApi.UnitTests.Controllers
{
    public class PersonControllerTests
    {
        [Fact]
        public void CanCreateInstanceOfPersonController()
        {
            //act
            var controller = new PersonController();

            //assert
            Assert.NotNull(controller);
        }
    }
}
