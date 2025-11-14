using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Enums;

namespace backend.Models
{
    [Table("UserDetails")]
    public class UserDetails
    {
        /// PK and FK to UserAccount
        [Key]
        [ForeignKey("UserAccount")]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Column(TypeName = "date")]
        public DateOnly? BirthDate { get; set; }

        [Column(TypeName = "varchar(10)")]
        public Gender? Gender { get; set; }

        [MaxLength(15)]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(255)]
        public string HomeAddress { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public virtual UserAccount UserAccount { get; set; } = null!; 

    }
}