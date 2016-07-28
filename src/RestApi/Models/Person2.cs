using System;

namespace RestApi.Models
{

    public class Person
    {
        public Person(Guid id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(name));

            Id = id;
            Name = name;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
    }
}
