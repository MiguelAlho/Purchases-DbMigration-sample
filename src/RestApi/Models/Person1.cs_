using System;

namespace RestApi.Models
{

    public class Person
    {
        public Person(Guid id, String fullName)
        {
            if(string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException(nameof(fullName));

            Id = id;
            Name = fullName;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
    }

    
}
