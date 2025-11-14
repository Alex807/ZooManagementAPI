using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    [Table("Staff")]
    public class Staff
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserAccountId { get; set; } 

        [Required]
        [Column(TypeName = "date")] 
        public DateTime HireDate { get; set; }

        [MaxLength(100)]
        public string Department { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Position { get; set; } = string.Empty;

        [Required]
        [Precision(10, 2)]
        public decimal Salary { get; set; }

        [ForeignKey("UserAccountId")]
        public virtual UserAccount UserAccount { get; set; } = null!;

        public virtual ICollection<FeedingSchedule> FeedingSchedules { get; set; } = new List<FeedingSchedule>();
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
        public virtual ICollection<StaffAnimalAssignment> StaffAnimalAssignments { get; set; } = new List<StaffAnimalAssignment>();
    }
}