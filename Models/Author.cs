using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asp_mvc_crud.Models
{
    // Author Model
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Nationality { get; set; }

        public DateTime? BirthDate { get; set; }

        // Navigation Properties
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();

        // Computed Property
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}
