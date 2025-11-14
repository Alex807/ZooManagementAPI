using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("UserAccount")]
    public class UserAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)] 
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        // Current active role for this session
        [Required]
        public int CurrentRoleId { get; set; }

        // primary role of this user
        [ForeignKey("CurrentRoleId")]
        public virtual Role CurrentRole { get; set; } = null!;

        // One-to-one relationship with UserDetails
        public virtual UserDetails UserDetails { get; set; } = null!;
        
            // Many-to-many relationship with roles through UserRole (additional roles)
            public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    }
}