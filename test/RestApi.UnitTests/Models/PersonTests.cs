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
            var firstname = "Mock";
            var lastname = "Name";
            
            var person = new Person(guid, firstname, lastname);

            Assert.NotNull(person);
            Assert.Equal(guid, person.Id);
            Assert.Equal(firstname, person.FirstName);
            Assert.Equal(lastname, person.LastName);
            Assert.Equal($"{firstname} {lastname}", person.Name);
        }

       

        [Fact]
        public void ConstructorGuardsNullFirstName()
        {
            Assert.Throws<ArgumentException>(() => new Person(Guid.NewGuid(), null, "Last"));
        }

        [Fact]
        public void ConstructorGuardsEmptyFirstName()
        {
            Assert.Throws<ArgumentException>(() => new Person(Guid.NewGuid(), " ", "Last"));
        }

        [Fact]
        public void ConstructorGuardsNullLastName()
        {
            Assert.Throws<ArgumentException>(() => new Person(Guid.NewGuid(), "First", null));
        }

        [Fact]
        public void ConstructorGuardsEmptyLastName()
        {
            Assert.Throws<ArgumentException>(() => new Person(Guid.NewGuid(), "First", " "));
        }
    }
}
