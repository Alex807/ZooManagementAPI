using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema;
using backend.Enums; 

namespace backend.Models
{
    [Table("Role")]
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        public RoleName Name { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; } = string.Empty;

        //list with all users with this role
        public virtual ICollection<UserAccount> UserAccounts { get; set; } = new List<UserAccount>();

        // Many-to-many relationship with users through UserRole
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}