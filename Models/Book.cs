using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asp_mvc_crud.Models
{
    // Book Model
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(20)]
        public string? ISBN { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Range(1000, 2100)]
        public int? PublicationYear { get; set; }

        [StringLength(100)]
        public string? Publisher { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Total copies must be at least 1")]
        public int TotalCopies { get; set; } = 1;

        [Required]
        [Range(0, int.MaxValue)]
        public int AvailableCopies { get; set; } = 1;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Available"; // Available, Unavailable, Maintenance

        // Foreign Key Properties
        [ForeignKey("AuthorId")]
        public virtual Author Author { get; set; } = null!;

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; } = null!;

        // Navigation Properties
        public virtual ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();

        // Computed Properties
        [NotMapped]
        public bool IsAvailable => AvailableCopies > 0 && Status == "Available";

        [NotMapped]
        public int BorrowedCopies => TotalCopies - AvailableCopies;
    }
}
