using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore; 
using backend.Enums;

namespace backend.Models
{
    //feeding schedule for an animal. (tracks what, when, and how much an animal is fed)
    [Table("Feeding_Schedule")]
    public class FeedingSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int AnimalId { get; set; }

        public int? StaffId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FoodType { get; set; } = string.Empty;

        [Required]
        [Precision(10, 2)]
        public decimal QuantityInKg { get; set; }

        [Required]
        [Column(TypeName = "DateTime")] 
        public DateTime FeedingTime { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        public FeedingStatus Status { get; set; }

        [Column(TypeName = "text")]
        public string? Notes { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        
        [Column(TypeName = "datetime")]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("AnimalId")]
        public virtual Animal Animal { get; set; } = null!;

        [ForeignKey("StaffId")]
        public virtual Staff? Staff { get; set; } 
    }
}