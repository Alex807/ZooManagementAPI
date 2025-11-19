using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    // Represents a category/classification for animals (e.g., Mammals, Birds, Reptiles) 

    [Table("Category")] 
    public class Category
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get; set; }

        [Required] 
        [MaxLength(50)] 
        public string Name {get; set;} = string.Empty; 

        [Column(TypeName = "text")] //use this type to not limit the size 
        public string? Description {get; set; } 

        [Required]
        [MaxLength(2048)] //increased length for image URL
        public string ImageUrl {get; set; } = string.Empty;

        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        
        [Column(TypeName = "datetime")]
        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<Animal> Animals {get; set; } = new List<Animal>();
    }
}