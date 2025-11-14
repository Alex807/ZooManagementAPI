using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("UserRole")]
    public class UserRole
    {
        //here we use composite PK (UserAccountId - RoleId)
        [Required]
        public int UserAccountId { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime AssignedAt { get; set; }

        [ForeignKey("UserAccountId")]
        public virtual UserAccount UserAccount { get; set; } = null!;

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; } = null!;
    }
}
