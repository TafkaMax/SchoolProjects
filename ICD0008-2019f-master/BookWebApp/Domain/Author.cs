using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Author
    {
        public int AuthorId { get; set; }

        [MaxLength(128)] public string FirstName { get; set; } = default!;

        [MaxLength(128)] public string LastName { get; set; } = default!;

        public int BirthYear { get; set; }

        public ICollection<BookAuthor>? AuthorsBooks  { get; set; }

        public string FirstLastName => FirstName + " " + LastName;
        
        public string LastFirstName => LastName + " " + FirstName;
    }
}