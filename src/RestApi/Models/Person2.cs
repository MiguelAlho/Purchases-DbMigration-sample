using System;

namespace RestApi.Models
{

    public class Person
    {
        public Person(Guid id, string firstname, string lastname)
        {
            if (string.IsNullOrWhiteSpace(firstname))
                throw new ArgumentException(nameof(firstname));
            if (string.IsNullOrWhiteSpace(lastname))
                throw new ArgumentException(nameof(lastname));

            Id = id;
            FirstName = firstname;
            LastName = lastname;
        }

        public Guid Id { get; private set; }
        public string Name { get { return $"{FirstName} {LastName}"; } }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

    }
}
