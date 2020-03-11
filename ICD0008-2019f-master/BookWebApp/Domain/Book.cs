using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Book
    {
        public int BookId { get; set; }

        [MaxLength(128)] public string Title { get; set; } = default!;

        public int PublishingYear { get; set; }

        public int AuthoredYear { get; set; }

        public int WordCount { get; set; }

        [MaxLength(1024)] public string? Summary { get; set; }

        public int LanguageId { get; set; }
        public Language? Language { get; set; }

        public int PublisherId { get; set; }
        public Publisher? Publisher { get; set; }


        public ICollection<Review>? Reviews { get; set; }
        public ICollection<BookAuthor>? BookAuthors { get; set; }
    }
}