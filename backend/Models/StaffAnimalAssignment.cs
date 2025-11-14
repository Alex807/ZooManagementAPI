using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    // tracks which staff members are responsible for which animals

    [Table("StaffAnimalAssignment")]
    public class StaffAnimalAssignment
    {
        [Required]
        public int StaffId { get; set; }

        [Required]
        public int AnimalId { get; set; }

        [MaxLength(200)]
        public string Observations { get; set; } = string.Empty;

        [Column(TypeName = "date")]
        public DateTime? CreatedAt { get; set; }

        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; } = null!;

        [ForeignKey("AnimalId")]
        public virtual Animal Animal { get; set; } = null!;
    }
}