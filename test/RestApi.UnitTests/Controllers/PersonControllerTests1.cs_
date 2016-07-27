using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using RestApi.Controllers;
using RestApi.Models;
using Xunit;

namespace RestApi.UnitTests.Controllers
{
    public class PersonControllerTests
    {
        [Fact]
        public void CanCreateInstanceOfPersonController()
        {
            var repo = new Mock<IPersonRepository>();

            //act
            var controller = new PersonController(repo.Object);

            //assert
            Assert.NotNull(controller);
        }

        [Fact]
        public void ConstructorGuardsNullRepository()
        {
            Assert.Throws<ArgumentNullException>(() => new PersonController(null));
        }

        [Fact]
        public void CanGetListOfPerson()
        {
            var repo = new Mock<IPersonRepository>();
            repo.Setup(o => o.GetListOfPersons())
                .Returns(new List<Person>()
                {
                    new Person(Guid.NewGuid(), "Person A"),
                    new Person(Guid.NewGuid(), "Person B"),
                });

            PersonController controller = new PersonController(repo.Object);

            var result = controller.GetListOfPersons();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());

        }
    }
}
