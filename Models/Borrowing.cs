using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asp_mvc_crud.Models
{
    // Borrowing Model
    public class Borrowing
    {
        [Key]
        public int BorrowingId { get; set; }

        [Required]
        public int MemberId { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public DateTime BorrowDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Borrowed"; // Borrowed, Returned, Overdue

        [Column(TypeName = "decimal(10,2)")]
        [Range(0, double.MaxValue)]
        public decimal FineAmount { get; set; } = 0;

        // Foreign Key Properties
        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; } = null!;

        [ForeignKey("BookId")]
        public virtual Book Book { get; set; } = null!;

        // Computed Properties
        [NotMapped]
        public bool IsOverdue => !ReturnDate.HasValue && DateTime.Now > DueDate;

        [NotMapped]
        public int DaysOverdue => IsOverdue ? (DateTime.Now - DueDate).Days : 0;

        [NotMapped]
        public bool IsReturned => ReturnDate.HasValue && Status == "Returned";
    }
}
