using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Construction.Models.Models
{
    [Table("StoredFiles")]
    public class StoredFile
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string OriginalFileName { get; set; }

        [Required]
        public string StoredFileName { get; set; }

        [Required]
        public string ContentType { get; set; }

        [Required]
        public long Size { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int WorkId { get; set; }

        [ForeignKey("WorkId")]
        public Work? Work { get; set; }
    }
}
