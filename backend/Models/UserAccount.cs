using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("User_Account")]
    public class UserAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)] 
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string PasswordHash { get; set; } = string.Empty;

        // Current active role for this session (at least is Visitor)
        [Required]
        public int CurrentRoleId { get; set; }

        [ForeignKey("CurrentRoleId")]
        public virtual Role CurrentRole { get; set; } = null!;

        // One-to-one relationship with UserDetails and Staff
        public virtual UserDetails UserDetails { get; set; } = null!;

        public virtual Staff? Staff { get; set; } //(optional) an user account as ADMIN may not have a staff record

        // Many-to-many relationship with roles through UserRole (additional roles)
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    }
}