using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Review
    {
        public int ReviewId { get; set; }
        
        [MaxLength(1024)]
        public string ReviewText { get; set; } = default!;

        public int BookId { get; set; }
        public Book? Book { get; set; }
    }
}