using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Enums;

namespace backend.Models
{
    [Table("Medical_Record")]
    public class MedicalRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int AnimalId { get; set; }

        public int? StaffId { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        public HealthStatus Status { get; set; }

        [Required]
        [Column(TypeName = "DateTime")] 
        public DateTime Date { get; set; }

        [Column(TypeName = "text")]
        public string? Description { get; set; }

        [ForeignKey("AnimalId")]
        public virtual Animal Animal { get; set; } = null!;

        [ForeignKey("StaffId")]
        public virtual Staff? Staff { get; set; }
    }
}