using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestApi.Models;
using Xunit;

namespace RestApi.UnitTests.ViewModels
{
    public class PersonTests
    {
        [Fact]
        public void CanCreateInstanceOfPerson()
        {
            var guid = Guid.NewGuid();
            var name = "Mock Name";
            
            var person = new Person(guid, name);

            Assert.NotNull(person);
            Assert.Equal(guid, person.Id);
            Assert.Equal(name, person.Name);
        }

       

        [Fact]
        public void ConstructorGuardsNullName()
        {
            Assert.Throws<ArgumentException>(() => new Person(Guid.NewGuid(), null));
        }

        [Fact]
        public void ConstructorGuardsEmptyName()
        {
            Assert.Throws<ArgumentException>(() => new Person(Guid.NewGuid(), " "));
        }
    }
}
