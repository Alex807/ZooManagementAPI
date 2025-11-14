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

        [Column(TypeName = "date")]
        public DateOnly? BirthDate { get; set; }

        [Column(TypeName = "varchar(10)")]
        public Gender? Gender { get; set; }

        [MaxLength(255)]
        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public virtual UserAccount UserAccount { get; set; } = null!; 

        // (moved) many-to-many relationship with roles is exposed on UserAccount via UserRole

        // feeding schedules created by this user (if they're a zookeeper)
        public virtual ICollection<FeedingSchedule> FeedingSchedules { get; set; } = new List<FeedingSchedule>();

        // medical records created by this user (if they're a veterinarian)
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

        // animal assignments for this staff member
        public virtual ICollection<StaffAnimalAssignment> AnimalAssignments { get; set; } = new List<StuffAnimalAssignment>();
    }
}