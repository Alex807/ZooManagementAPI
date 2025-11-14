using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema; 
using backend.Enums; 

namespace backend.Models
{
    [Table("Animal")] 
    public class Animal
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get; set; } 

        [Required] 
        [MaxLength(100)]
        public string Name {get; set; } = string.Empty;

        [Required]
        [MaxLength(100)] 
        public string Species {get; set; } = string.Empty; 

        [Required]
        [MaxLength(255)]
        public string ImageUrl {get; set; } = string.Empty;

        [Column(TypeName = "date")] 
        public DateOnly? DateOfBirth {get; set; } 

        [Column(TypeName = "varchar(10)")] //for enum conversion to string
        public Gender? Gender {get; set; }

        [Column(TypeName = "date")] 
        public DateTime ArrivalDate {get; set; }

        [Required] 
        public int CategoryId {get; set; } //FK 

        [Required]
        public int EnclosureId {get; set; } //FK 

        public DateTime? CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; } = null!; 

        [ForeignKey("EnclosureId")]
        public virtual Enclosure Enclosure { get; set; } = null!; 

        // Collection of feeding schedules for this animal
        public virtual ICollection<FeedingSchedule> FeedingSchedules { get; set; } = new List<FeedingSchedule>();

        // Collection of medical records for this animal
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
    
        // Collection of staff assignments for this animal 
        public virtual ICollection<StaffAnimalAssignment> StaffAssignments { get; set; } = new List<StaffAnimalAssignment>();
    }
}