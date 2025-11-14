using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    // Represents a physical enclosure/habitat where animals are kept
    
    [Table("Enclosure")]
    public class Enclosure
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Type { get; set; }

        [Required]
        public int Capacity { get; set; }

        [MaxLength(150)]
        public string? Location { get; set; }

        //one enclosure can house many animals
        public virtual ICollection<Animal> Animals { get; set; } = new List<Animal>();
    }
}